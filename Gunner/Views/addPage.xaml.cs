using Gunner.Data;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls;
using SQLite;

// ��С�Ȫ����ǡ�� (namespace) ��Ф��ʢͧ˹���������� (addPage)
namespace Gunner.Views;

public partial class addPage : ContentPage
{
    private int count = 0;  // ��ǹѺ�ӹǹ�ѵ�شԺ
    private Models.Recipes currentRecipe;  // �红��������ٻѨ�غѹ�����ѧ���
    private byte[] dataimg;  // ��èآ������Ҿ��ٻẺ亵�
    private List<string> originalIngredients = new List<string>();  // ��¡���ѵ�شԺ���
    private List<string> Ingredients = new List<string>();  // ��¡���ѵ�شԺ�������͹�����
    Data.UserRecipes userRecipes = new Data.UserRecipes();  // ��ǨѴ��â��������٢ͧ�����

    // Constructor ˹��������������Ѻ���ҧ����
    public addPage()
    {
        InitializeComponent();
        SetupPage();
    }

    // Constructor ˹��������������Ѻ������ٷ��������
    public addPage(Models.Recipes recipe)
    {
        InitializeComponent();
        currentRecipe = recipe;
        SetupPage();
        PopulateFormData(recipe);
    }

    // ��駤��������鹢ͧ˹��
    private void SetupPage()
    {
        dataimg = new byte[0];
        Ingredients = new List<string>();
    }

    // ��Ŵ������������ҿ��������͵�ͧ������
    private void PopulateFormData(Models.Recipes recipe)
    {
        FoodName.Text = recipe.Name;
        Category.Text = recipe.Category;
        Details.Text = recipe.Description;

        if (recipe.ImagePath != null && recipe.ImagePath.Length > 0)
        {
            imgfood.Source = ImageSource.FromStream(() => new MemoryStream(recipe.ImagePath));
            dataimg = recipe.ImagePath; // Keep original image data
        }

        string[] ingredients = recipe.Ingredients.Split('\n');
        originalIngredients = ingredients.ToList();
        foreach (string ingredient in ingredients)
        {
            AddIngredientEntry(ingredient, false);
        }
    }

    // ����͡�������Ѻ˹����ѡ
    private async void homeButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new MainPage());
    }

    // ����͡��������˹���ʵ�ͧ�ѹ
    private async void menuButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new MyPostPage());
    }

    // ����͡�����������������
    private async void addButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new addPage());
    }

    // ����͡�������¡���ô
    private async void markButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new FavoritePage());
    }

    // ����͡������͡�ҡ�к�
    private async void logoutButton_Clicked(object sender, EventArgs e)
    {
         await Navigation.PushModalAsync(new AccountPage.LoginPage());

    }

    // ����͡����������ѵ�شԺ
    private void entryButton_Clicked(object sender, EventArgs e)
    {
        AddIngredientEntry("", true);
    }

    // �����ѵ�شԺŧ� UI
    private void AddIngredientEntry(string ingredient = "", bool isNew = true)
    {
        count++;
        Border border = new Border
        {
            Margin = new Thickness(15, 0, 15, 15),
            StrokeShape = new RoundRectangle { CornerRadius = 10 }
        };

        Entry newEntry = new Entry
        {
            Placeholder = "�ѵ�شԺ��� " + count,
            Text = ingredient,
            HeightRequest = 55,
            Margin = new Thickness(10, 0, 0, 0),
            FontFamily = "kanitLight",
        };

        border.Content = newEntry;

        if (inputContainer.Content is StackLayout stackLayout)
        {
            stackLayout.Children.Add(border);
        }

        newEntry.TextChanged += (s, e) =>
        {
            var entry = s as Entry;
            // Adjust the index calculation for newly added entries
            int index = isNew ? Ingredients.Count : Convert.ToInt32(entry.Placeholder.Split(' ')[1]) - 1;
            if (index < Ingredients.Count)
            {
                Ingredients[index] = entry.Text;
            }
            else
            {
                Ingredients.Add(entry.Text);
            }
        };
    }

    // ��駤����кѹ�֡����������
    private async void PostButton_Clicked(object sender, EventArgs e)
    {
        if (Ingredients.Count == 0)
        {
            Ingredients = originalIngredients; // �������ա�������ѵ�شԺ����������ѵ�شԺ���
        }

        if (string.IsNullOrWhiteSpace(FoodName.Text) || string.IsNullOrWhiteSpace(Details.Text))
        {
            await DisplayAlert("Warining", "Please fill in all fields", "OK");
            return;
        }

        string ingredientsString = string.Join("\n", Ingredients);
        string username = AccountPage.LoginPage.getusername();
        if (username == null || userRecipes == null)
        {
            await DisplayAlert("Error", "System not initialized properly.", "OK");
            return;
        }

        // �������ա���ѻ��Ŵ�ٻ����������ٻ���
        if (dataimg == null || dataimg.Length == 0)
        {
            if (currentRecipe != null && currentRecipe.ImagePath != null)
            {
                dataimg = currentRecipe.ImagePath;
            }
        }

        Models.Recipes recipeToSave = new Models.Recipes
        {
            Username = username,
            Name = FoodName.Text,
            Category = Category.Text,
            Ingredients = ingredientsString,
            Description = Details.Text,
            ImagePath = dataimg
        };

        if (currentRecipe != null && currentRecipe.Id != 0)
        {
            recipeToSave.Id = currentRecipe.Id;
            await userRecipes.UpdateRecipeAsync(recipeToSave);
            await DisplayAlert("Update Success", "Recipe updated successfully", "OK");
        }
        else
        {
            await userRecipes.SaveRecipesAsync(recipeToSave);
            await DisplayAlert("Post Success", "Recipe posted successfully", "OK");
        }
        await Navigation.PushModalAsync(new MyPostPage());
    }

    private async void PhotoButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            PermissionStatus status = await CheckAndRequestPhotosPermission();
            if (status == PermissionStatus.Granted || status == PermissionStatus.Limited)
            {
                var photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null)
                {
                    // ����ա�����͡�ٻ���� �ѻവ�ٻ�Ҿ��Т�����
                    string filePath = await LoadPhotoAsync(photo);
                    imgfood.Source = ImageSource.FromFile(filePath);
                    dataimg = await ConvertImageToByteArray(filePath);
                }
                // �������ա�����͡�ٻ���� ����ͧ����¹�ŧ�������ٻ�Ҿ
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async Task<byte[]> ConvertImageToByteArray(string imagePath)
    {
        try
        {
            byte[] result;
            using (var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await fileStream.CopyToAsync(memoryStream);
                    result = memoryStream.ToArray();
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error converting image to byte array: " + ex.Message);
            return null; // ����բ�ͼԴ��Ҵ��������칤�� null ���ͨѴ���Ẻ��蹵��������ͧ��âͧ�ͻ
        }
    }

    private async Task<string> LoadPhotoAsync(FileResult photo)
    {
        if (photo == null) return "";

        string newFilePath = System.IO.Path.Combine(FileSystem.CacheDirectory, photo.FileName);
        using (Stream sourceStream = await photo.OpenReadAsync())
        {
            using (FileStream fileStream = new FileStream(newFilePath, FileMode.Create, FileAccess.Write))
            {
                await sourceStream.CopyToAsync(fileStream);
            }
        }
        return newFilePath;
    }

    public async Task<PermissionStatus> CheckAndRequestPhotosPermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Photos>();
        if (status == PermissionStatus.Granted)
        {
            return status;
        }
        if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
        {
            return status;
        }
        if (Permissions.ShouldShowRationale<Permissions.Photos>())
        {
        }
        status = await Permissions.RequestAsync<Permissions.Photos>();
        return status;
    }

    private async void cancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new MyPostPage());
    }
}

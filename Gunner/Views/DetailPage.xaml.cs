using Gunner.Models;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

namespace Gunner.Views;

public partial class DetailPage : ContentPage
{
    // ��С�ȵ��������Ѻ��èѴ��â����ż����
    Data.UserRecipes userRecipes = new Data.UserRecipes();
    private const string StarStateKey = "StarState_";

    // ���ҧ����������Ǣ�ͧ�Ѻ�����������ͨѴ���ʶҹТͧ���
    private string RecipeNameKey => $"{StarStateKey}{nameText.Text}";

    // �кص��˹觷���������¡���ô
    private readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Favoritee.txt");

    // Constructor ����Ѻ˹���ʴ���������´
    public DetailPage()
    {
        InitializeComponent();
    }

    // Constructor �Ѻ����������ǡѺ�ٻ�Ҿ ���� �����������´�ͧ�ٵ�
    public DetailPage(ImageSource imagePath, string name, string raw, string description,string username)
    {
        InitializeComponent();
        starButton.Source = "star"; // ��駤���ٻ��������������
        selectedImage.Source = imagePath;
        nameText.Text = name;
        rawText.Text = raw;
        descriptionText.Text = description;
        UserName = username;
    }

    // ���ʹ����Ѻ��Ǩ�ͺʶҹд�Ƿ��ѹ�֡���
    private void LoadStarState()
    {
        bool isFavorite = Preferences.Get(RecipeNameKey, false);
        starButton.Source = isFavorite ? "starafter" : "star";
    }

    // ��á�˹���ȷҧ�˹����� � ���������衴
    private async void homeButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new MainPage());
    }

    private async void menuButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new MyPostPage());
    }

    private async void addButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new addPage());
    }

    private async void markButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new FavoritePage());
    }

    private async void logoutButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AccountPage.LoginPage());

    }

    // ������������ٵ������
    private async void shareButton_Clicked(object sender, EventArgs e)
    {
        var result = await DisplayActionSheet("Share", "cancel", null, "Share link", "Share message");

        switch (result)
        {
            case "Share link":
                await ShareUri("https://www.google.com");
                break;
            case "Share message":
                await ShareText($"�ѵ�شԺ \n{rawText.Text}\n�Ըշ� \n{descriptionText.Text}");
                break;
        }
    }

    // ���ʹ����Ѻ�ѹ�֡��¡���ô��ѧ���
    private async Task SaveDataToFile(string namefood, string username, string ingredients)
    {
        var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var filePath = Path.Combine(documentsPath, "Favoritee.txt");

        // �Ѵ�ٻẺ��������кѹ�֡
        string dataToSave = $"Name Food: {namefood}\nUsername: {username}\nIngredients: {ingredients}\n-------------\n";
        await File.AppendAllTextAsync(filePath, dataToSave);
    }

    // ���ʹ����Ѻ�Ѵ��û����������Ͷ١��
    private async void StarButton_Clicked(object sender, EventArgs e)
    {
        bool isFavorite = Preferences.Get(RecipeNameKey, false);

        if (isFavorite)
        {
            Preferences.Set(RecipeNameKey, false);
            starButton.Source = "star";
            await DisplayAlert("Delete from Favorite Recipes", "Click OK to acknowledge", "OK");

            // ź��¡���͡�ҡ�����¡���ô
            await RemoveRecipeFromFile(nameText.Text);
            string updatedContent = await ReadFileContent();
            await DisplayAlert("Favorite Recipes", updatedContent, "OK");
        }
        else
        {
            Preferences.Set(RecipeNameKey, true);
            starButton.Source = "starafter";
            await DisplayAlert("Add to Favorite Recipes", "Click OK to acknowledge", "OK");

            string nameuser = AccountPage.LoginPage.getusername();
            await SaveDataToFile(nameText.Text, nameuser, rawText.Text);

            // �ʴ������ҷ���ѻവ�ͧ���
            string fileContent = await ReadFileContent();
            await DisplayAlert("Favorite Recipes", fileContent, "OK");
        }
    }

    // ���ʹź��¡���͡�ҡ�����¡���ô
    private async Task RemoveRecipeFromFile(string recipeName)
    {
        var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var filePath = Path.Combine(documentsPath, "Favoritee.txt");

        if (File.Exists(filePath))
        {
            var lines = await File.ReadAllLinesAsync(filePath);
            var sb = new StringBuilder();
            bool isInRecipe = false;

            foreach (var line in lines)
            {
                if (line.StartsWith("Name Food: "))
                {
                    // ��Ǩ�ͺ��������
                    isInRecipe = line.Contains(recipeName);
                }

                if (!isInRecipe)
                {
                    sb.AppendLine(line);
                }

                // �礨ش����ش�ͧ��¡��
                if (line.StartsWith("-------------"))
                {
                    isInRecipe = false;
                }
            }

            await File.WriteAllTextAsync(filePath, sb.ToString());
        }
    }

    // ���ʹ����Ѻ��ҹ�����¡���ô������
    private async Task<string> ReadFileContent()
    {
        if (File.Exists(filePath))
        {
            return await File.ReadAllTextAsync(filePath);
        }
        return "Couldn't find favorite food item.";
    }

    // ���ʹ��� URL ��ҹ�к����ͧ�ػ�ó�
    public async Task ShareUri(string uri)
    {
        await Share.Default.RequestAsync
        (new ShareTextRequest
        {
            Uri = uri,
            Title = "Share link"
        });
    }

    // ���ʹ����ͤ���
    public async Task ShareText(string text)
    {
        await Share.Default.RequestAsync
        (new ShareTextRequest
        {
            Text = text,
            Title = "Share Text"
        });
    }

    // ���ʹ����Ѻ���ź�ʵ������ҧ�¼����
    private async void deleteButton_Clicked(object sender, EventArgs e)

    {
        string nameuser = AccountPage.LoginPage.getusername();
        bool confirmDelete = await DisplayAlert("Confirm deletion", "Are you sure you want to delete this item?", "Delete", "Cancel");
        if(nameuser == UserName)
        {
            if (confirmDelete)
            {
                await userRecipes.DeleteRecipeByNameAsync(nameText.Text);
                await DisplayAlert("Successfully deleted", "The item was successfully deleted.", "OK");

                await Navigation.PushModalAsync(new MyPostPage());
            }

        }
        else
        {
            await DisplayAlert("Error", "You cannot delete another user's post", "OK");
        }
    }

    // ���¡�����˹���ʴ����������
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadStarState();
    }

    public string UserName { get; set; }
}

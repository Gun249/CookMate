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
    // ประกาศตัวแปรสำหรับการจัดการข้อมูลผู้ใช้
    Data.UserRecipes userRecipes = new Data.UserRecipes();
    private const string StarStateKey = "StarState_";

    // สร้างคีย์ที่เกี่ยวข้องกับชื่อเมนูเพื่อจัดการสถานะของดาว
    private string RecipeNameKey => $"{StarStateKey}{nameText.Text}";

    // ระบุตำแหน่งที่เก็บไฟล์รายการโปรด
    private readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Favoritee.txt");

    // Constructor สำหรับหน้าแสดงรายละเอียด
    public DetailPage()
    {
        InitializeComponent();
    }

    // Constructor รับข้อมูลเกี่ยวกับรูปภาพ ชื่อ และรายละเอียดของสูตร
    public DetailPage(ImageSource imagePath, string name, string raw, string description,string username)
    {
        InitializeComponent();
        starButton.Source = "star"; // ตั้งค่ารูปปุ่มดาวเริ่มต้น
        selectedImage.Source = imagePath;
        nameText.Text = name;
        rawText.Text = raw;
        descriptionText.Text = description;
        UserName = username;
    }

    // เมธอดสำหรับตรวจสอบสถานะดาวที่บันทึกไว้
    private void LoadStarState()
    {
        bool isFavorite = Preferences.Get(RecipeNameKey, false);
        starButton.Source = isFavorite ? "starafter" : "star";
    }

    // การกำหนดทิศทางไปหน้าอื่น ๆ ตามปุ่มที่กด
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

    // การแชร์ข้อมูลสูตรอาหาร
    private async void shareButton_Clicked(object sender, EventArgs e)
    {
        var result = await DisplayActionSheet("Share", "cancel", null, "Share link", "Share message");

        switch (result)
        {
            case "Share link":
                await ShareUri("https://www.google.com");
                break;
            case "Share message":
                await ShareText($"วัตถุดิบ \n{rawText.Text}\nวิธีทำ \n{descriptionText.Text}");
                break;
        }
    }

    // เมธอดสำหรับบันทึกรายการโปรดไปยังไฟล์
    private async Task SaveDataToFile(string namefood, string username, string ingredients)
    {
        var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var filePath = Path.Combine(documentsPath, "Favoritee.txt");

        // จัดรูปแบบข้อมูลและบันทึก
        string dataToSave = $"Name Food: {namefood}\nUsername: {username}\nIngredients: {ingredients}\n-------------\n";
        await File.AppendAllTextAsync(filePath, dataToSave);
    }

    // เมธอดสำหรับจัดการปุ่มดาวเมื่อถูกกด
    private async void StarButton_Clicked(object sender, EventArgs e)
    {
        bool isFavorite = Preferences.Get(RecipeNameKey, false);

        if (isFavorite)
        {
            Preferences.Set(RecipeNameKey, false);
            starButton.Source = "star";
            await DisplayAlert("Delete from Favorite Recipes", "Click OK to acknowledge", "OK");

            // ลบรายการออกจากไฟล์รายการโปรด
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

            // แสดงเนื้อหาที่อัปเดตของไฟล์
            string fileContent = await ReadFileContent();
            await DisplayAlert("Favorite Recipes", fileContent, "OK");
        }
    }

    // เมธอดลบรายการออกจากไฟล์รายการโปรด
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
                    // ตรวจสอบชื่อเมนู
                    isInRecipe = line.Contains(recipeName);
                }

                if (!isInRecipe)
                {
                    sb.AppendLine(line);
                }

                // เช็คจุดสิ้นสุดของรายการ
                if (line.StartsWith("-------------"))
                {
                    isInRecipe = false;
                }
            }

            await File.WriteAllTextAsync(filePath, sb.ToString());
        }
    }

    // เมธอดสำหรับอ่านไฟล์รายการโปรดทั้งหมด
    private async Task<string> ReadFileContent()
    {
        if (File.Exists(filePath))
        {
            return await File.ReadAllTextAsync(filePath);
        }
        return "Couldn't find favorite food item.";
    }

    // เมธอดแชร์ URL ผ่านระบบแชร์ของอุปกรณ์
    public async Task ShareUri(string uri)
    {
        await Share.Default.RequestAsync
        (new ShareTextRequest
        {
            Uri = uri,
            Title = "Share link"
        });
    }

    // เมธอดแชร์ข้อความ
    public async Task ShareText(string text)
    {
        await Share.Default.RequestAsync
        (new ShareTextRequest
        {
            Text = text,
            Title = "Share Text"
        });
    }

    // เมธอดสำหรับการลบโพสต์ที่สร้างโดยผู้ใช้
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

    // เรียกเมื่อหน้าแสดงผลเสร็จสิ้น
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadStarState();
    }

    public string UserName { get; set; }
}

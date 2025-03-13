using Gunner.Models;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gunner.Views;

public partial class MyPostPage : ContentPage, INotifyPropertyChanged
{
    // ประกาศตัวแปรเพื่อเก็บข้อมูลโพสต์ของผู้ใช้
    private ObservableCollection<MyPostModel> _postModel;

    // สร้าง property เพื่อใช้เชื่อมโยงข้อมูลกับ UI
    public ObservableCollection<MyPostModel> postmodel
    {
        get => _postModel;
        set
        {
            if (_postModel != value)
            {
                _postModel = value;
                OnPropertyChanged(nameof(postmodel)); // แจ้งการเปลี่ยนแปลงข้อมูล
            }
        }
    }

    // Constructor สำหรับหน้าโพสต์ของฉัน
    public MyPostPage()
    {
        InitializeComponent(); // เริ่มต้น UI จาก XAML
        dataPost(); // โหลดข้อมูลโพสต์
        BindingContext = this; // กำหนดบริบทการเชื่อมโยงข้อมูล
    }

    // ประกาศตัวแปรสำหรับคลาสจัดการข้อมูลโพสต์
    Data.UserRecipes userRecipes = new Data.UserRecipes();

    // เมธอดสำหรับโหลดข้อมูลโพสต์ของผู้ใช้
    private async void dataPost()
    {
        var username = AccountPage.LoginPage.getusername(); // ดึงชื่อผู้ใช้ที่เข้าสู่ระบบ

        if (username != null)
        {
            // ค้นหาโพสต์ของผู้ใช้ตามชื่อ
            var dataList = await userRecipes.findusername(username);

            if (dataList != null && dataList.Any())
            {
                // สร้างสตริงเพื่อแสดงข้อมูล
                StringBuilder sb = new StringBuilder();
                foreach (var data in dataList)
                {
                    sb.AppendLine($"Category: {data.Category}, Name: {data.Name}, Ingredients: {data.Ingredients}, Description: {data.Description}");
                }

                // สร้างข้อความสำหรับการแจ้งเตือน
                string displayData = $"พบ {dataList.Count} โพสต์:\n{sb.ToString()}";
                await DisplayAlert("Data loaded successfully", displayData, "OK");

                // อัปเดตข้อมูลโพสต์ใน ObservableCollection เพื่อเชื่อมโยงกับ UI
                postmodel = new ObservableCollection<MyPostModel>(
                    dataList.Select(d => new MyPostModel(d.Category, d.ImagePath, d.Name, d.Ingredients, d.Description,d.Username)
                ));
            }
            else
            {
                await DisplayAlert("Information", "No posts found for this user.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Unable to determine username.", "OK");
        }
    }

    // เมธอดสำหรับจัดการเมื่อคลิกปุ่มเพื่อดูรายละเอียดของโพสต์
    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is MyPostModel myPost)
        {
            await Navigation.PushModalAsync(new DetailPage(myPost.ImagePath, myPost.Name, myPost.Raw, myPost.Description,myPost.nameuser));
        }
    }
    // เมธอดสำหรับจัดการปุ่มเพื่อแก้ไขโพสต์
    private async void editButton_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is MyPostModel myPost)
        {
            // ค้นหาโพสต์ตามชื่อเพื่อแก้ไข
            var recipe = await userRecipes.GetRecipeByNameAsync(myPost.Name);

            if (recipe != null)
            {
                await Navigation.PushModalAsync(new addPage(recipe)); // นำทางไปยังหน้าการเพิ่มโพสต์ใหม่พร้อมส่งข้อมูลโพสต์ที่เลือก
            }
            else
            {
                await DisplayAlert("Error", "Unable to find post.", "OK");
            }
        }
    }

    // เมธอดสำหรับจัดการปุ่มนำทางไปหน้าเข้าสู่ระบบ
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

}

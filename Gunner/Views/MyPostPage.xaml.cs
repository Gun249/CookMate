using Gunner.Models;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gunner.Views;

public partial class MyPostPage : ContentPage, INotifyPropertyChanged
{
    // ��С�ȵ���������红������ʵ�ͧ�����
    private ObservableCollection<MyPostModel> _postModel;

    // ���ҧ property ������������§�����šѺ UI
    public ObservableCollection<MyPostModel> postmodel
    {
        get => _postModel;
        set
        {
            if (_postModel != value)
            {
                _postModel = value;
                OnPropertyChanged(nameof(postmodel)); // �駡������¹�ŧ������
            }
        }
    }

    // Constructor ����Ѻ˹���ʵ�ͧ�ѹ
    public MyPostPage()
    {
        InitializeComponent(); // ������� UI �ҡ XAML
        dataPost(); // ��Ŵ�������ʵ�
        BindingContext = this; // ��˹���Ժ����������§������
    }

    // ��С�ȵ��������Ѻ���ʨѴ��â������ʵ�
    Data.UserRecipes userRecipes = new Data.UserRecipes();

    // ���ʹ����Ѻ��Ŵ�������ʵ�ͧ�����
    private async void dataPost()
    {
        var username = AccountPage.LoginPage.getusername(); // �֧���ͼ�������������к�

        if (username != null)
        {
            // �����ʵ�ͧ�����������
            var dataList = await userRecipes.findusername(username);

            if (dataList != null && dataList.Any())
            {
                // ���ҧʵ�ԧ�����ʴ�������
                StringBuilder sb = new StringBuilder();
                foreach (var data in dataList)
                {
                    sb.AppendLine($"Category: {data.Category}, Name: {data.Name}, Ingredients: {data.Ingredients}, Description: {data.Description}");
                }

                // ���ҧ��ͤ�������Ѻ�������͹
                string displayData = $"�� {dataList.Count} �ʵ�:\n{sb.ToString()}";
                await DisplayAlert("Data loaded successfully", displayData, "OK");

                // �ѻവ�������ʵ�� ObservableCollection ����������§�Ѻ UI
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

    // ���ʹ����Ѻ�Ѵ�������ͤ�ԡ�������ʹ���������´�ͧ�ʵ�
    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is MyPostModel myPost)
        {
            await Navigation.PushModalAsync(new DetailPage(myPost.ImagePath, myPost.Name, myPost.Raw, myPost.Description,myPost.nameuser));
        }
    }
    // ���ʹ����Ѻ�Ѵ��û�����������ʵ�
    private async void editButton_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is MyPostModel myPost)
        {
            // �����ʵ��������������
            var recipe = await userRecipes.GetRecipeByNameAsync(myPost.Name);

            if (recipe != null)
            {
                await Navigation.PushModalAsync(new addPage(recipe)); // �ӷҧ��ѧ˹�ҡ�������ʵ����������觢������ʵ������͡
            }
            else
            {
                await DisplayAlert("Error", "Unable to find post.", "OK");
            }
        }
    }

    // ���ʹ����Ѻ�Ѵ��û����ӷҧ�˹���������к�
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

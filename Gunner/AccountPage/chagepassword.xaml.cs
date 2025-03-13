using Gunner.Data;

namespace Gunner.AccountPage;

public partial class chagepassword : ContentPage
{
    DataLogin dataLogin = new DataLogin();
    public string user;
	public chagepassword()
	{
		InitializeComponent();
	}
    public chagepassword(string username)
    {
        InitializeComponent();
        user = username;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        string newPassword = NewPasswordEntry.Text;
        string confirmedPassword = ConfirmedPasswordEntry.Text;

        if (newPassword != confirmedPassword)
        {
            await DisplayAlert("Error", "New passwords do not match", "OK");
            return;
        }

        // �ѻവ���ʼ�ҹ㹰ҹ������
        await dataLogin.UpdatePasswordAsync(user, newPassword);

        // �ʴ���ͤ�������͹�������¹���ʼ�ҹ�������
        await DisplayAlert("Success", "Password has been changed", "OK");

        // ��͹��Ѻ��ѧ˹����͡�Թ��ѡ��ѧ�ҡ�������¹���ʼ�ҹ
        await Navigation.PushModalAsync(new LoginPage());
    }


    private async void backButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ForgotPasswordPage());
    }
}
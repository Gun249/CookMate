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

        // อัปเดตรหัสผ่านในฐานข้อมูล
        await dataLogin.UpdatePasswordAsync(user, newPassword);

        // แสดงข้อความแจ้งเตือนการเปลี่ยนรหัสผ่านเสร็จสิ้น
        await DisplayAlert("Success", "Password has been changed", "OK");

        // ย้อนกลับไปยังหน้าล็อกอินหลักหลังจากการเปลี่ยนรหัสผ่าน
        await Navigation.PushModalAsync(new LoginPage());
    }


    private async void backButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ForgotPasswordPage());
    }
}
namespace Gunner.AccountPage;

public partial class ForgotPasswordPage : ContentPage
{
    Data.DataLogin dataLogin = new Data.DataLogin();
	public ForgotPasswordPage()
	{
		InitializeComponent();
	}

    async private void backButton_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushModalAsync(new LoginPage());
    }

    async private void returnButton_Clicked(object sender, EventArgs e)
    {

        await Navigation.PushModalAsync(new LoginPage());
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var user = await dataLogin.findusername(Username.Text);
        if (user != null)
        {
             await Navigation.PushModalAsync(new chagepassword(Username.Text));

        }
        else if (user == null)
        {     
            await DisplayAlert("Error", "Username not found", "OK");
        }

    }
}
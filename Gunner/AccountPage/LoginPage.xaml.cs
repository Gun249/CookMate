namespace Gunner.AccountPage;

public partial class LoginPage : ContentPage
{
    // �Թ�ᵹ��ͧ���� DataLogin ����Ѻ�Ѵ��â����š���������к�
    Data.DataLogin dataLogin = new Data.DataLogin();

    // �����Ẻ�ᵵԡ�����纪��ͼ�������������к�
    private static string usern;

    // Constructor: ���ҧ˹��ྨ����������к�
    public LoginPage()
    {
        InitializeComponent(); // ������� UI �ҡ XAML
    }

    // ������ѧ˹����Ѥ���Ҫԡ
    async private void signupButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SignupPage());
    }

    // ������ѧ˹��������ʼ�ҹ
    async private void forgotButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ForgotPasswordPage());
    }

    // �Ѵ��û����������к�
    private async void loginButton_Clicked(object sender, EventArgs e)
    {
        // ���Ҽ����ҡ�ҹ������������ͼ����
        var user = await dataLogin.findusername(UserName.Text);

        // ��Ǩ�ͺ��Ҽ����������������ʼ�ҹ�١��ͧ�������
        if (user != null && user.Password == PassWord.Text)
        {
            // ��˹����ͼ�������������к�
            setusername(UserName.Text);

            // �ʴ���ͤ�������������к������
            await DisplayAlert("information", "login successful", "OK");

            // �Ӽ������ѧ˹����ѡ (MainPage)
            await Navigation.PushModalAsync(new Views.MainPage());
        }
        else
        {
            // �ʴ���ͤ�����Ҫ��ͼ�����������ʼ�ҹ���١��ͧ
            await DisplayAlert("Login failed", "Please enter the correct username or password", "OK");
        }
    }

    // �Ѻ���ͼ�������������к�
    public static string getusername()
    {
        return usern;
    }

    // ��˹����ͼ�������������к�
    public static void setusername(string username)
    {
        usern = username;
    }
}

namespace Gunner.AccountPage;

public partial class SignupPage : ContentPage
{
    // Constructor: ���ҧ˹��ྨ�����Ѥ���Ҫԡ
    public SignupPage()
    {
        InitializeComponent(); // ������� UI �ҡ XAML
    }

    // �Թ�ᵹ��ͧ���� DataLogin ���ͨѴ��â����š���������к�
    Data.DataLogin dataLogin = new Data.DataLogin();

    // ������ѧ˹���������к�
    async private void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new LoginPage());
    }

    // �Ѵ��û�����Ѥ���Ҫԡ
    async private void Button_Clicked(object sender, EventArgs e)
    {
        // ��Ǩ�ͺ��������Ң����ŷ������١��͹�ú��ǹ
        if (email.Text == null || FirstName.Text == null || LastName.Text == null || PassWord.Text == null || ConfirmPassword.Text == null || UserName.Text == null)
        {
            // �ʴ���ͤ�������͹����͡�����Ťú��ǹ
            await DisplayAlert("Warning", "Please fill in information completely", "OK");
        }
        // ��Ǩ�ͺ������ʼ�ҹ��С���׹�ѹ���ʼ�ҹ�ç�ѹ�������
        else if (PassWord.Text != ConfirmPassword.Text)
        {
            // �ʴ���ͤ�������͹������ʼ�ҹ���ç�ѹ
            await DisplayAlert("Passwords do not match", "Please enter passwords to match", "OK");
        }
        // ��Ǩ�ͺ������ʼ�ҹ�դ������ 8 ����ѡ���������
        else if (PassWord.Text.Length < 8)
        {
            // �ʴ���ͤ�������͹������ʼ�ҹ��ͧ�դ������ 8 ����ѡ��
            await DisplayAlert("Warning", "Please enter a password that is at least 8 characters long", "OK");
        }
        else
        {
            // ��Ǩ�ͺ��Ҫ��ͼ�����������������������
            if (await dataLogin.findusername(UserName.Text) == null)
            {
                // �ѹ�֡�����š����Ѥ���Ҫԡ
                await dataLogin.SaveLoginAsync(new Models.Login
                {
                    Email = email.Text,
                    name = FirstName.Text,
                    lastname = LastName.Text,
                    Password = PassWord.Text,
                    Username = UserName.Text
                });

                // �ʴ���ͤ��������Ѥ���Ҫԡ�������йӼ�����Ѻ��ѧ˹���������к�
                await DisplayAlert("Information", "Register successfully", "OK");
                await Navigation.PushModalAsync(new LoginPage());
            }
            else
            {
                // �ʴ���ͤ�������ժ��ͼ��������������
                await DisplayAlert("This username already exists", "Please enter a new username", "OK");
            }
        }
    }
}

namespace Gunner.AccountPage;

public partial class LoginPage : ContentPage
{
    // อินสแตนซ์ของคลาส DataLogin สำหรับจัดการข้อมูลการเข้าสู่ระบบ
    Data.DataLogin dataLogin = new Data.DataLogin();

    // ตัวแปรแบบสแตติกเพื่อเก็บชื่อผู้ใช้ที่เข้าสู่ระบบ
    private static string usern;

    // Constructor: สร้างหน้าเพจการเข้าสู่ระบบ
    public LoginPage()
    {
        InitializeComponent(); // เริ่มต้น UI จาก XAML
    }

    // ย้ายไปยังหน้าสมัครสมาชิก
    async private void signupButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SignupPage());
    }

    // ย้ายไปยังหน้าลืมรหัสผ่าน
    async private void forgotButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ForgotPasswordPage());
    }

    // จัดการปุ่มเข้าสู่ระบบ
    private async void loginButton_Clicked(object sender, EventArgs e)
    {
        // ค้นหาผู้ใช้จากฐานข้อมูลโดยใช้ชื่อผู้ใช้
        var user = await dataLogin.findusername(UserName.Text);

        // ตรวจสอบว่าผู้ใช้มีอยู่และรหัสผ่านถูกต้องหรือไม่
        if (user != null && user.Password == PassWord.Text)
        {
            // กำหนดชื่อผู้ใช้ที่เข้าสู่ระบบ
            setusername(UserName.Text);

            // แสดงข้อความว่าเข้าสู่ระบบสำเร็จ
            await DisplayAlert("information", "login successful", "OK");

            // นำผู้ใช้ไปยังหน้าหลัก (MainPage)
            await Navigation.PushModalAsync(new Views.MainPage());
        }
        else
        {
            // แสดงข้อความว่าชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง
            await DisplayAlert("Login failed", "Please enter the correct username or password", "OK");
        }
    }

    // รับชื่อผู้ใช้ที่เข้าสู่ระบบ
    public static string getusername()
    {
        return usern;
    }

    // กำหนดชื่อผู้ใช้ที่เข้าสู่ระบบ
    public static void setusername(string username)
    {
        usern = username;
    }
}

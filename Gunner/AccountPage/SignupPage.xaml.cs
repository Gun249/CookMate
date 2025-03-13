namespace Gunner.AccountPage;

public partial class SignupPage : ContentPage
{
    // Constructor: สร้างหน้าเพจการสมัครสมาชิก
    public SignupPage()
    {
        InitializeComponent(); // เริ่มต้น UI จาก XAML
    }

    // อินสแตนซ์ของคลาส DataLogin เพื่อจัดการข้อมูลการเข้าสู่ระบบ
    Data.DataLogin dataLogin = new Data.DataLogin();

    // ย้ายไปยังหน้าเข้าสู่ระบบ
    async private void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new LoginPage());
    }

    // จัดการปุ่มสมัครสมาชิก
    async private void Button_Clicked(object sender, EventArgs e)
    {
        // ตรวจสอบให้แน่ใจว่าข้อมูลทั้งหมดถูกป้อนครบถ้วน
        if (email.Text == null || FirstName.Text == null || LastName.Text == null || PassWord.Text == null || ConfirmPassword.Text == null || UserName.Text == null)
        {
            // แสดงข้อความแจ้งเตือนให้กรอกข้อมูลครบถ้วน
            await DisplayAlert("Warning", "Please fill in information completely", "OK");
        }
        // ตรวจสอบว่ารหัสผ่านและการยืนยันรหัสผ่านตรงกันหรือไม่
        else if (PassWord.Text != ConfirmPassword.Text)
        {
            // แสดงข้อความแจ้งเตือนว่ารหัสผ่านไม่ตรงกัน
            await DisplayAlert("Passwords do not match", "Please enter passwords to match", "OK");
        }
        // ตรวจสอบว่ารหัสผ่านมีความยาว 8 ตัวอักษรหรือไม่
        else if (PassWord.Text.Length < 8)
        {
            // แสดงข้อความแจ้งเตือนว่ารหัสผ่านต้องมีความยาว 8 ตัวอักษร
            await DisplayAlert("Warning", "Please enter a password that is at least 8 characters long", "OK");
        }
        else
        {
            // ตรวจสอบว่าชื่อผู้ใช้นี้มีอยู่แล้วหรือไม่
            if (await dataLogin.findusername(UserName.Text) == null)
            {
                // บันทึกข้อมูลการสมัครสมาชิก
                await dataLogin.SaveLoginAsync(new Models.Login
                {
                    Email = email.Text,
                    name = FirstName.Text,
                    lastname = LastName.Text,
                    Password = PassWord.Text,
                    Username = UserName.Text
                });

                // แสดงข้อความว่าสมัครสมาชิกสำเร็จและนำผู้ใช้กลับไปยังหน้าเข้าสู่ระบบ
                await DisplayAlert("Information", "Register successfully", "OK");
                await Navigation.PushModalAsync(new LoginPage());
            }
            else
            {
                // แสดงข้อความว่ามีชื่อผู้ใช้นี้อยู่แล้ว
                await DisplayAlert("This username already exists", "Please enter a new username", "OK");
            }
        }
    }
}

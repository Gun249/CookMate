using Gunner.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Gunner.Views
{
    public partial class FavoritePage : ContentPage
    {
        private const string FilePath = "Favoritee.txt";
        public ObservableCollection<FavoriteModel> FavoriteModels { get; set; }

        // Constructor: สร้างหน้าเพจและเรียกใช้ฟังก์ชัน LoadFavoritesFromFile เพื่อโหลดเมนูที่ชอบ
        public FavoritePage()
        {
            InitializeComponent();
            LoadFavoritesFromFile(); // โหลดเมนูที่ชอบจากไฟล์

            BindingContext = this; // กำหนดบริบทข้อมูลให้กับคลาสนี้
        }

        // ย้ายไปหน้าเข้าสู่ระบบ
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

        // อ่านเนื้อหาในไฟล์ "Favoritee.txt" และโหลดเมนูที่ตรงกับผู้ใช้ปัจจุบันลงในคอลเลกชัน FavoriteModels
        private void LoadFavoritesFromFile()
        {
            // สร้างคอลเลกชันใหม่
            FavoriteModels = new ObservableCollection<FavoriteModel>();

            // กำหนดเส้นทางไฟล์
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), FilePath);
            if (File.Exists(filePath))
            {
                string[] fileLines = File.ReadAllLines(filePath);
                string currentName = "";
                string currentUsername = "";
                ObservableCollection<IngredientItem> currentIngredients = new ObservableCollection<IngredientItem>();

                // ประมวลผลแต่ละบรรทัดเพื่อดึงข้อมูลเกี่ยวกับเมนูอาหาร
                foreach (string line in fileLines)
                {
                    // ถ้าบรรทัดเริ่มด้วย "Name Food:" แสดงว่าเป็นชื่อเมนูใหม่
                    if (line.StartsWith("Name Food: "))
                    {
                        // เพิ่มเมนูอาหารก่อนหน้า (ถ้ามี) ลงในคอลเลกชัน
                        AddCurrentDishToCollection(ref currentName, ref currentUsername, ref currentIngredients);

                        // เริ่มเมนูใหม่
                        currentName = line.Replace("Name Food: ", "").Trim();
                        currentUsername = "";
                        currentIngredients = new ObservableCollection<IngredientItem>();
                    }
                    // ดึงชื่อผู้ใช้ที่เกี่ยวข้องกับเมนูนี้
                    else if (line.StartsWith("Username: "))
                    {
                        currentUsername = line.Replace("Username: ", "").Trim();
                    }
                    // เริ่มอ่านวัตถุดิบ
                    else if (line.StartsWith("Ingredients: "))
                    {
                        string firstIngredient = line.Replace("Ingredients: ", "").Trim();
                        currentIngredients.Add(new IngredientItem { Ingredient = firstIngredient, IsSelected = false });
                    }
                    // ถ้าบรรทัดเริ่มด้วย "-----" แสดงว่าเป็นจุดสิ้นสุดของข้อมูลเมนู
                    else if (line.StartsWith("-----"))
                    {
                        AddCurrentDishToCollection(ref currentName, ref currentUsername, ref currentIngredients);
                    }
                    // อ่านวัตถุดิบเพิ่มเติมลงในรายการปัจจุบัน
                    else if (!string.IsNullOrWhiteSpace(line))
                    {
                        currentIngredients.Add(new IngredientItem { Ingredient = line.Trim(), IsSelected = false });
                    }
                }

                // เพิ่มเมนูสุดท้าย (ถ้ามี) ลงในคอลเลกชัน
                AddCurrentDishToCollection(ref currentName, ref currentUsername, ref currentIngredients);
            }
            else
            {
                // พิมพ์ข้อความแจ้งเตือนหากไม่พบไฟล์
                Console.WriteLine("File not found.");
            }
        }

        // เพิ่มเมนูอาหารลงในคอลเลกชันหากชื่อผู้ใช้ตรงกับผู้ใช้ที่เข้าสู่ระบบปัจจุบัน
        private void AddCurrentDishToCollection(ref string name, ref string username, ref ObservableCollection<IngredientItem> ingredients)
        {
            // ดึงชื่อผู้ใช้ปัจจุบันจากหน้าเข้าสู่ระบบ
            string userna = AccountPage.LoginPage.getusername();
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(username) && ingredients.Count > 0)
            {
                // เพิ่มเมนูอาหารเฉพาะถ้าชื่อผู้ใช้ตรงกับชื่อผู้ใช้ที่เข้าสู่ระบบ
                if (username == userna)
                {
                    FavoriteModels.Add(new FavoriteModel(name, username, ingredients));
                }
            }

            // รีเซ็ตตัวแปรเพื่อเตรียมการเมนูต่อไป
            name = "";
            username = "";
            ingredients = new ObservableCollection<IngredientItem>();
        }
    }
}

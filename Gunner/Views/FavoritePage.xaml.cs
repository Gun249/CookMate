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

        // Constructor: ���ҧ˹��ྨ������¡��ѧ��ѹ LoadFavoritesFromFile ������Ŵ���ٷ��ͺ
        public FavoritePage()
        {
            InitializeComponent();
            LoadFavoritesFromFile(); // ��Ŵ���ٷ��ͺ�ҡ���

            BindingContext = this; // ��˹���Ժ����������Ѻ���ʹ��
        }

        // �����˹���������к�
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

        // ��ҹ���������� "Favoritee.txt" �����Ŵ���ٷ��ç�Ѻ�����Ѩ�غѹŧ㹤���š�ѹ FavoriteModels
        private void LoadFavoritesFromFile()
        {
            // ���ҧ����š�ѹ����
            FavoriteModels = new ObservableCollection<FavoriteModel>();

            // ��˹���鹷ҧ���
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), FilePath);
            if (File.Exists(filePath))
            {
                string[] fileLines = File.ReadAllLines(filePath);
                string currentName = "";
                string currentUsername = "";
                ObservableCollection<IngredientItem> currentIngredients = new ObservableCollection<IngredientItem>();

                // �����ż����к�÷Ѵ���ʹ֧����������ǡѺ���������
                foreach (string line in fileLines)
                {
                    // ��Һ�÷Ѵ��������� "Name Food:" �ʴ�����繪�����������
                    if (line.StartsWith("Name Food: "))
                    {
                        // ������������á�͹˹�� (�����) ŧ㹤���š�ѹ
                        AddCurrentDishToCollection(ref currentName, ref currentUsername, ref currentIngredients);

                        // �������������
                        currentName = line.Replace("Name Food: ", "").Trim();
                        currentUsername = "";
                        currentIngredients = new ObservableCollection<IngredientItem>();
                    }
                    // �֧���ͼ����������Ǣ�ͧ�Ѻ���ٹ��
                    else if (line.StartsWith("Username: "))
                    {
                        currentUsername = line.Replace("Username: ", "").Trim();
                    }
                    // �������ҹ�ѵ�شԺ
                    else if (line.StartsWith("Ingredients: "))
                    {
                        string firstIngredient = line.Replace("Ingredients: ", "").Trim();
                        currentIngredients.Add(new IngredientItem { Ingredient = firstIngredient, IsSelected = false });
                    }
                    // ��Һ�÷Ѵ��������� "-----" �ʴ�����繨ش����ش�ͧ����������
                    else if (line.StartsWith("-----"))
                    {
                        AddCurrentDishToCollection(ref currentName, ref currentUsername, ref currentIngredients);
                    }
                    // ��ҹ�ѵ�شԺ�������ŧ���¡�ûѨ�غѹ
                    else if (!string.IsNullOrWhiteSpace(line))
                    {
                        currentIngredients.Add(new IngredientItem { Ingredient = line.Trim(), IsSelected = false });
                    }
                }

                // ���������ش���� (�����) ŧ㹤���š�ѹ
                AddCurrentDishToCollection(ref currentName, ref currentUsername, ref currentIngredients);
            }
            else
            {
                // ������ͤ�������͹�ҡ��辺���
                Console.WriteLine("File not found.");
            }
        }

        // �������������ŧ㹤���š�ѹ�ҡ���ͼ����ç�Ѻ��������������к��Ѩ�غѹ
        private void AddCurrentDishToCollection(ref string name, ref string username, ref ObservableCollection<IngredientItem> ingredients)
        {
            // �֧���ͼ����Ѩ�غѹ�ҡ˹���������к�
            string userna = AccountPage.LoginPage.getusername();
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(username) && ingredients.Count > 0)
            {
                // �������������੾�ж�Ҫ��ͼ����ç�Ѻ���ͼ�������������к�
                if (username == userna)
                {
                    FavoriteModels.Add(new FavoriteModel(name, username, ingredients));
                }
            }

            // ���絵�������������������ٵ���
            name = "";
            username = "";
            ingredients = new ObservableCollection<IngredientItem>();
        }
    }
}

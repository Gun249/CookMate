using System.Collections.ObjectModel;

namespace Gunner.Models
{
    public class FavoriteModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public ObservableCollection<IngredientItem> RawList { get; set; }

        public FavoriteModel(string name, string username, ObservableCollection<IngredientItem> rawList)
        {
            Name = name;
            Username = username;
            RawList = rawList;
        }
    }
}
using Microsoft.Maui.Controls;
using System.IO;

namespace Gunner.Models
{
    public class MyPostModel
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string nameuser { get; set; }
        public ImageSource ImagePath { get; set; }  // Assuming ImageSource for direct binding
        public string Name { get; set; }
        public string Raw { get; set; }
        public string Description { get; set; }

        // Constructor that accepts byte[] for image and converts it
        public MyPostModel(string category, byte[] imagePath, string name, string raw, string description,string username)
        {
            Category = category;
            ImagePath = ImageSource.FromStream(() => new MemoryStream(imagePath));
            Name = name;
            Raw = raw;
            Description = description;
            nameuser = username;
        }

    }
}

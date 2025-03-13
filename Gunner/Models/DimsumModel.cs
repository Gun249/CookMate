using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gunner.Models
{
    public class DimsumModel
    {
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public string Raw { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public DimsumModel(string imagePath, string name, string raw, string description, string username)
        {
            ImagePath = imagePath;
            Name = name;
            Raw = raw;
            Description = description;
            Username = username;
        }
    }
}

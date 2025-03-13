using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Gunner.Models
{
    [Table("Recipes")]
    public class Recipes  // Make the class public to ensure it can be accessed where needed
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("username")]
        public string Username { get; set; } // Consider using PascalCase for properties

        [Column("name")]
        public string Name { get; set; }

        [Column("ingredients")]
        public string Ingredients { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [Column("imagepath")]
        public byte[] ImagePath { get; set; }

    }
}

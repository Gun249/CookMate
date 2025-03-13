using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gunner.Models
{
    [Table("Login")]
    public class Login
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("name")]
        public string name { get; set; }

        [Column("lastname")]
        public string lastname { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        public string getusername()
        {
            return Username;
        }

        public void setusername(string username)
        {
            Username = username;
        }
    }
}

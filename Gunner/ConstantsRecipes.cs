﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gunner
{
    internal class ConstantsRecipes
    {
        // สร้างค่าคงที่สำหรับชื่อฐานข้อมูล

        private const string DatabaseFilename = Data.UserRecipes.namedb;


        public const SQLiteOpenFlags sQLiteOpenFlags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

        // สร้าง Property สำหรับเส้นทางฐานข้อมูลที่คำนวณได้
        public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        // หากต้องการใช้งาน Method เพื่อรับเส้นทางฐานข้อมูล, คุณสามารถใช้วิธีนี้
        public static string GetDatabasePath()
        {
            return Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
        }
    }
        
}


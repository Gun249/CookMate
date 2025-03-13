using SQLite;
using System;
using System.IO; // เพิ่มไลบรารีนี้เพื่อใช้งาน Path

namespace Gunner
{
    class ConstantsLogin
    {
        // สร้างค่าคงที่สำหรับชื่อฐานข้อมูล

        private const string DatabaseFilename = Data.DataLogin.namedb;


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

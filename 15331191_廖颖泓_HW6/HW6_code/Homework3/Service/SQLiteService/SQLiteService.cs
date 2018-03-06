using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.SQLiteService
{
    class SQLiteService
    {
        public static SQLiteConnection conn;

        /// <summary>
        /// 
        /// </summary>
        public static void LoadDatabase()
        {
            conn = new SQLiteConnection("SQLite.db");

            string sql = @"CREATE TABLE IF NOT EXISTS
                                TodoItem (id         VARCHAR( 36 ) PRIMARY KEY NOT NULL,
                                         title       VARCHAR( 140 ),
                                         description VARCHAR( 140 ),
                                         image       VARCHAR( 140 ),
                                         date        DATETIME
                            )";
            using (var statement = conn.Prepare(sql))
            {
                statement.Step();
            }
        }
    }
}

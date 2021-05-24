using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AppBD
{
    public static class DataManager
    {
        public static List<string> nameTables = DatabaseConnector.GetTables();
        public static List<string> Views;
        public static DataTable currentTable = new DataTable();
        public static DataTable Users = new DataTable();
        public static int indexUser;
    }
}

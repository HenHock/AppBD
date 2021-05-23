using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AppBD
{
    public class DataManager
    {
        public static List<string> nameTables = DatabaseConnector.GetTables();
        public static DataTable currentTable = new DataTable();
        public static List<int> indexDeleteRow = new List<int>();
        public static List<int> indexEditRow = new List<int>();
        public static DataTable Users = new DataTable();
        public static int indexUser;
    }
}

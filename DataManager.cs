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
    }
}

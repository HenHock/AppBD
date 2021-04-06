using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;

namespace AppBD
{
    public class DatabaseConnector
    {
        private static string ConnectionString = "Data Source=DESKTOP-UVLISFT;Initial Catalog=Seabattle1;Integrated Security=True";

        public static List<string> GetTables()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                DataTable schema = connection.GetSchema("Tables");
                List<string> TableNames = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    if(row[2].ToString().IndexOf("View") >= 0 || row[2].ToString().IndexOf("view") >= 0)
                    {

                    }
                    else if(row[2].ToString().IndexOf("diagram") >= 0 || row[2].ToString().IndexOf("Diagram") >= 0)
                    {

                    }
                    else
                        {
                        TableNames.Add(row[2].ToString());
                    }
                }
                return TableNames;
            }
        }

        public static void getInfoFromTable(string tableName)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    if (DataManager.currentTable != null)
                        DataManager.currentTable = new DataTable();

                    SqlCommand command = new SqlCommand("SELECT * FROM " + tableName, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(DataManager.currentTable);
                }
                catch
                {
                    MessageBox.Show("Error read info from table", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

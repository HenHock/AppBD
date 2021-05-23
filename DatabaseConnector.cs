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
        private static string сonnectionString = @"Data Source=LAB07_PC01\SQLEXPRESS;Initial Catalog=Seabattle;Integrated Security=True";

        public static List<string> GetTables()
        {
            using (SqlConnection connection = new SqlConnection(сonnectionString))
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
            using (SqlConnection connection = new SqlConnection(сonnectionString))
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

        public static void getInfoFromTable(string tableName, DataTable dataTable)
        {
            using (SqlConnection connection = new SqlConnection(сonnectionString))
            {
                try
                {
                    connection.Open();

                    if (DataManager.currentTable != null)
                        DataManager.currentTable = new DataTable();

                    SqlCommand command = new SqlCommand("SELECT * FROM " + tableName, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
                catch
                {
                    MessageBox.Show("Ошибка чтения данных из файла!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public static void UpdateBD(string tableName)
        {
            using (SqlConnection connection = new SqlConnection(сonnectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT * FROM " + tableName, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);

                    adapter.UpdateCommand = comandbuilder.GetUpdateCommand();
                    adapter.DeleteCommand = comandbuilder.GetDeleteCommand();

                    adapter.Update(DataManager.currentTable);

                    DataManager.currentTable.AcceptChanges();
                }
                catch
                {
                    MessageBox.Show("Не удалось обновить базу данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public static void UpdateBD(string tableName, DataTable dataTable)
        {
            using (SqlConnection connection = new SqlConnection(сonnectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT * FROM " + tableName, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);

                    adapter.UpdateCommand = comandbuilder.GetUpdateCommand();
                    adapter.DeleteCommand = comandbuilder.GetDeleteCommand();

                    adapter.Update(dataTable);

                    dataTable.AcceptChanges();
                }
                catch
                {
                    MessageBox.Show("Не удалось обновить базу данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

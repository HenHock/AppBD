using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppBD
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataGrid tableDataGrid;
        public MainWindow()
        {
            InitializeComponent();

            tableListBox.ItemsSource = DataManager.nameTables;
        }

        private void tableButton_Click(object sender, RoutedEventArgs e)
        {
            if (tableListBox.Visibility == Visibility.Collapsed)
                tableListBox.Visibility = Visibility.Visible;
            else tableListBox.Visibility = Visibility.Collapsed;
        }

        private void tableListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tableListBox.SelectedIndex != -1)
            {
                infoScrollViewer.Visibility = Visibility.Visible;
                tableDataGrid = new DataGrid();
                tableDataGrid.SelectionChanged += tableDataGrid_SelectionChange;
                tableDataGrid.Margin = new Thickness(10);
                tableDataGrid.AutoGenerateColumns = true;

                DatabaseConnector.getInfoFromTable(tableListBox.SelectedItem.ToString());
                tableDataGrid.ItemsSource = DataManager.currentTable.DefaultView;
                if (infoStackPanel.Children.Count > 0)
                    infoStackPanel.Children.Clear();
                infoStackPanel.Children.Add(tableDataGrid);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(tableDataGrid.SelectedIndex != -1)
            {
                DataManager.currentTable.Rows.RemoveAt(tableDataGrid.SelectedIndex);
                //update bd
            }
            else
                MessageBox.Show("Выделите строку, которую хотите удалить",
                        "Выделите строку",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
        }

        private void findTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (findTextBox.Text.Equals("Поиск..."))
            {
                findTextBox.Text = "";
            }
        }

        private void findTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (findTextBox.Text.Equals(""))
            {
                findTextBox.Text = "Поиск...";
            }
        }

        private void findTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tableDataGrid != null)
            {
                var filteredTable = DataManager.currentTable.Clone();

                foreach (DataRow row in DataManager.currentTable.Rows)
                {
                    if (findTextBox.Text.Equals(""))
                    {
                        filteredTable.Rows.Add(row.ItemArray);
                    }
                    else
                    {
                        for (int i = 0; i < DataManager.currentTable.Columns.Count; i++)
                        {
                            if (row[i].ToString().StartsWith(findTextBox.Text))
                            {
                                filteredTable.Rows.Add(row.ItemArray);
                            }
                        }
                    }
                }

                tableDataGrid.ItemsSource = filteredTable.DefaultView;
            }
            
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            DataRow newRow = DataManager.currentTable.NewRow();
            DataManager.currentTable.Rows.Add(newRow);
            tableDataGrid.SelectedIndex = tableDataGrid.Items.Count - 2;
            
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (tableDataGrid.SelectedIndex != -1)
            {
                if (editStackPanel.Visibility == Visibility.Collapsed)
                    editStackPanel.Visibility = Visibility.Visible;
                else editStackPanel.Visibility = Visibility.Collapsed;

                if (editStackPanel.Children.Count > 0)
                    editStackPanel.Children.Clear();

                TabPanel tabPanel;

                foreach (DataColumn column in DataManager.currentTable.Columns)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = column.ColumnName;

                    TextBox textBox = new TextBox();
                    //textBox.Name = column.ColumnName + "TextBox";
                    textBox.Margin = new Thickness(0);
                    foreach(DataRow row in DataManager.currentTable.Rows)
                    {
                        if(tableDataGrid.SelectedIndex == DataManager.currentTable.Rows.IndexOf(row))
                        {
                            textBox.Text = row[column.ColumnName].ToString();
                        }
                    }

                    tabPanel = new TabPanel();
                    tabPanel.Children.Add(textBlock);
                    tabPanel.Children.Add(textBox);
                    editStackPanel.Children.Add(tabPanel);
                }

                Button confirmButton = new Button();
                confirmButton.Content = "Ок";
                confirmButton.Margin = new Thickness(0);
                confirmButton.Click += confirmButton_Click;

                Button cancelButton = new Button();
                cancelButton.Content = "Отмена";
                cancelButton.Margin = new Thickness(0);
                cancelButton.Click += cancelButton_Click;

                tabPanel = new TabPanel();
                tabPanel.Margin = new Thickness(0, 5, 0, 5);
                tabPanel.Children.Add(confirmButton);
                tabPanel.Children.Add(cancelButton);

                editStackPanel.Children.Add(tabPanel);
            }
            else MessageBox.Show("Выберите строку, которую хотите отредактировать",
                "Выберите строку", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            editStackPanel.Children.Clear();
            editStackPanel.Visibility = Visibility.Collapsed;
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {

        }

        private void tableDataGrid_SelectionChange(object sender, EventArgs e)
        {
            if (editStackPanel.Visibility == Visibility.Visible)
            {
                if (editStackPanel.Children.Count > 0)
                    editStackPanel.Children.Clear();

                TabPanel tabPanel;

                foreach (DataColumn column in DataManager.currentTable.Columns)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = column.ColumnName;

                    TextBox textBox = new TextBox();

                    textBox.Margin = new Thickness(0);
                    foreach (DataRow row in DataManager.currentTable.Rows)
                    {
                        if (tableDataGrid.SelectedIndex == DataManager.currentTable.Rows.IndexOf(row))
                        {
                            textBox.Text = row[column.ColumnName].ToString();
                        }
                    }

                    tabPanel = new TabPanel();
                    tabPanel.Children.Add(textBlock);
                    tabPanel.Children.Add(textBox);
                    editStackPanel.Children.Add(tabPanel);
                }

                Button confirmButton = new Button();
                confirmButton.Content = "Ок";
                confirmButton.Margin = new Thickness(0);
                confirmButton.Click += confirmButton_Click;

                Button cancelButton = new Button();
                cancelButton.Content = "Отмена";
                cancelButton.Margin = new Thickness(0);
                cancelButton.Click += cancelButton_Click;

                tabPanel = new TabPanel();
                tabPanel.Margin = new Thickness(0, 5, 0, 5);
                tabPanel.Children.Add(confirmButton);
                tabPanel.Children.Add(cancelButton);

                editStackPanel.Children.Add(tabPanel);
            }
        }
    }
}

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
        private bool BDChange = false;
        public MainWindow()
        {
            InitializeComponent();

            tableListBox.ItemsSource = DataManager.nameTables;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Closed += MainWindow_Closed;
        }

        private void SaveBD()
        {
            if(BDChange)
            {
                MessageBoxResult result = MessageBox.Show("Сохранить изменения в базе данных?", "Сохранение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (tableListBox.SelectedIndex != -1)
                        DatabaseConnector.UpdateBD(nameTableTextBlock.Text);
                    BDChange = false;
                }
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            SaveBD();
            this.Close();
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
                if(BDChange)
                {
                    int index = -1, i = 0;
                    foreach (string nameTable in tableListBox.Items)
                    {
                        if (nameTable.Equals(nameTableTextBlock.Text))
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    if (index != tableListBox.SelectedIndex)
                    {
                        MessageBoxResult result = MessageBox.Show("Изменения не будут сохранены!\nВы действительно хотите продолжить?",
                        "Вопрос",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                        if (result == MessageBoxResult.No)
                        {
                            tableListBox.SelectedIndex = index;
                            e.Handled = true;
                            return;
                        }
                        else
                        {
                            BDChange = false;
                        }
                    }
                    else if (index == tableListBox.SelectedIndex)
                        return;
                }

                addStackPanel.Visibility = Visibility.Collapsed;
                editStackPanel.Visibility = Visibility.Collapsed;

                infoScrollViewer.Visibility = Visibility.Visible;
                tableDataGrid = new DataGrid();
                tableDataGrid.SelectionChanged += tableDataGrid_SelectionChange;
                //tableDataGrid.IsReadOnly = true;
                tableDataGrid.Margin = new Thickness(10);
                tableDataGrid.AutoGenerateColumns = true;

                Style headerStyle = new Style();
                headerStyle.Setters.Add(new Setter { Property = Control.BackgroundProperty, Value = Brushes.AliceBlue });
                headerStyle.Setters.Add(new Setter { Property = Control.ForegroundProperty, Value = Brushes.DarkGoldenrod });

                Style dataGridStyle = new Style();
                //dataGridStyle.Resources = Resources["borderStyle"];
                dataGridStyle.Setters.Add(new Setter { Property = DataGrid.BorderBrushProperty, Value = Brushes.DarkRed });
                dataGridStyle.Setters.Add(new Setter { Property = DataGrid.BorderThicknessProperty, Value = new Thickness(2) });
                dataGridStyle.Setters.Add(new Setter { Property = DataGrid.AlternatingRowBackgroundProperty, Value = Brushes.Black });
                dataGridStyle.Setters.Add(new Setter { Property = DataGrid.ForegroundProperty, Value = Brushes.Gray });
                dataGridStyle.Setters.Add(new Setter { Property = DataGrid.ColumnHeaderStyleProperty, Value = headerStyle });
                dataGridStyle.Setters.Add(new Setter { Property = DataGrid.RowHeaderStyleProperty, Value = headerStyle });
                tableDataGrid.Style = dataGridStyle;

                nameTableTextBlock.Text = tableListBox.SelectedItem.ToString();
                DatabaseConnector.getInfoFromTable(nameTableTextBlock.Text);
                tableDataGrid.ItemsSource = DataManager.currentTable.DefaultView;
                if (infoStackPanel.Children.Count > 0)
                    infoStackPanel.Children.Clear();
                infoStackPanel.Children.Add(nameTableTextBlock);
                infoStackPanel.Children.Add(tableDataGrid);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(tableDataGrid.SelectedIndex != -1)
            {
                DataManager.currentTable.Rows.RemoveAt(tableDataGrid.SelectedIndex);
            }
            else
                MessageBox.Show("Выделите строку, которую хотите удалить",
                        "Выделите строку",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
        }
        //Поиск
        #region findBlock
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
                    if (findTextBox.Text.Equals("") || findTextBox.Text.Equals("Поиск..."))
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
        #endregion
        //Кнопка добавить
        #region addBlock
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (addStackPanel.Visibility == Visibility.Collapsed)
                addStackPanel.Visibility = Visibility.Visible;
            else addStackPanel.Visibility = Visibility.Collapsed;

            if (addStackPanel.Children.Count > 0)
                addStackPanel.Children.Clear();

            TabPanel tabPanel;

            foreach (DataColumn column in DataManager.currentTable.Columns)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = column.ColumnName;

                TextBox textBox = new TextBox();
                textBox.Margin = new Thickness(0);

                tabPanel = new TabPanel();
                tabPanel.Children.Add(textBlock);
                tabPanel.Children.Add(textBox);
                addStackPanel.Children.Add(tabPanel);
            }

            Button confirmButton = new Button();
            confirmButton.Content = "Ок";
            confirmButton.Margin = new Thickness(0);
            confirmButton.Click += addConfirmButton_Click;

            Button cancelButton = new Button();
            cancelButton.Content = "Отмена";
            cancelButton.Margin = new Thickness(0);
            cancelButton.Click += addCancelButton_Click;

            tabPanel = new TabPanel();
            tabPanel.Margin = new Thickness(0, 5, 0, 5);
            tabPanel.Children.Add(confirmButton);
            tabPanel.Children.Add(cancelButton);

            addStackPanel.Children.Add(tabPanel);
        }

        private void addConfirmButton_Click(object sender, EventArgs e)
        {
            DataRow newRow = DataManager.currentTable.NewRow();
            int i = 0;

            foreach(TabPanel tabPanel in addStackPanel.Children)
            {
                foreach(UIElement el in tabPanel.Children)
                {
                    if(el.GetType().ToString().Equals("System.Windows.Controls.TextBox"))
                    {
                        TextBox textBox = el as TextBox;
                        newRow[i] = textBox.Text;
                        i++;
                    }
                }
            }

            DataManager.currentTable.Rows.Add(newRow);
            BDChange = true;

            foreach (TabPanel tabPanel in addStackPanel.Children)
            {
                foreach (UIElement el in tabPanel.Children)
                {
                    if (el.GetType().ToString().Equals("System.Windows.Controls.TextBox"))
                    {
                        TextBox textBox = el as TextBox;
                        textBox.Text = "";
                    }
                }
            }
        }

        private void addCancelButton_Click(object sender, EventArgs e)
        {
            addStackPanel.Children.Clear();
            addStackPanel.Visibility = Visibility.Collapsed;
        }
        #endregion
        //Кнопка редактирования
        #region editBlock
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
            if (editStackPanel.Visibility == Visibility.Visible)
            {
                editStackPanel.Children.Clear();
                editStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            DataRow updateRow = DataManager.currentTable.NewRow();
            int i = 0;
            foreach(TabPanel tabPanel in editStackPanel.Children)
            {
                foreach(UIElement uI in tabPanel.Children)
                {
                    if(uI.GetType().ToString().Equals("System.Windows.Controls.TextBox"))
                    {
                        TextBox textBox = uI as TextBox;
                        updateRow[i] = textBox.Text;
                        i++;
                    }
                }
            }

            int indexRow = tableDataGrid.SelectedIndex;
            DataManager.currentTable.Rows.RemoveAt(indexRow);
            BDChange = true;
           //DatabaseConnector.UpdateBD(nameTableTextBlock.Text);
            DataManager.currentTable.Rows.InsertAt(updateRow, indexRow);
            //DatabaseConnector.UpdateBD(nameTableTextBlock.Text);
            tableDataGrid.SelectedIndex = indexRow;
        }
        #endregion

        private void tableDataGrid_SelectionChange(object sender, EventArgs e)
        {
            if(tableDataGrid.SelectedIndex == -1)
            {
                BDChange = true;
            }

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

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (tableListBox.SelectedIndex != -1)
            {
                DatabaseConnector.UpdateBD(nameTableTextBlock.Text);
                BDChange = false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
                DataGrid tableDataGrid = new DataGrid();
                tableDataGrid.Margin = new Thickness(10);
                tableDataGrid.AutoGenerateColumns = true;

                DatabaseConnector.getInfoFromTable(tableListBox.SelectedItem.ToString());
                tableDataGrid.ItemsSource = DataManager.currentTable.DefaultView;
                if (infoStackPanel.Children.Count > 0)
                    infoStackPanel.Children.Clear();
                infoStackPanel.Children.Add(tableDataGrid);
            }
        }
    }
}

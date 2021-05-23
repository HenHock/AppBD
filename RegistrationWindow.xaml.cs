using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Windows.Shapes;

namespace AppBD
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Resource\exitImage1.png");
            bi.EndInit();
            exitImage.Source = bi;

            bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Resource\loginImage1.png");
            bi.EndInit();
            loginImage.Source = bi;

            bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Resource\passwordImage1.png");
            bi.EndInit();
            passwordImage.Source = bi;

            bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Resource\emailImage1.png");
            bi.EndInit();
            emailImage.Source = bi;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if(textBox != null)
            {
                if (textBox.Name.Equals("loginTextBox"))
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Resource\loginImage2.png");
                    bi.EndInit();
                    loginImage.Source = bi;

                    textBox.Foreground = Brushes.Black;
                    textBox.Background = Brushes.WhiteSmoke;
                    textBox.BorderBrush = Brushes.AliceBlue;
                }
                else
                {
                    //Change Image
                    textBox.Foreground = Brushes.Black;
                    textBox.Background = Brushes.WhiteSmoke;
                    textBox.BorderBrush = Brushes.AliceBlue;
                }
            }
            else
            {
                PasswordBox passwordBox = sender as PasswordBox;
                //Change Image
                passwordBox.Foreground = Brushes.Black;
                passwordBox.Background = Brushes.WhiteSmoke;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null && textBox.Name.Equals("loginTextBox"))
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Resource\loginImage1.png");
                bi.EndInit();
                loginImage.Source = bi;

                textBox.Foreground = Brushes.WhiteSmoke;
                var bc = new BrushConverter();
                textBox.Background = (Brush)bc.ConvertFrom("#FF22242C");
                textBox.BorderBrush = (Brush)bc.ConvertFrom("#FF22242C");
            }
            else if (textBox != null && textBox.Name.Equals("emailTextBox"))
            {
                //Change Image
                textBox.Foreground = Brushes.WhiteSmoke;
                var bc = new BrushConverter();
                textBox.Background = (Brush)bc.ConvertFrom("#FF22242C");
                textBox.BorderBrush = (Brush)bc.ConvertFrom("#FF22242C");
            }
            else 
            {
                PasswordBox passwordBox = sender as PasswordBox;

                //Change Image
                passwordBox.Foreground = Brushes.WhiteSmoke;
                var bc = new BrushConverter();
                passwordBox.Background = (Brush)bc.ConvertFrom("#FF22242C");
                passwordBox.BorderBrush = (Brush)bc.ConvertFrom("#FF22242C");
            }
        }

        private void ExitImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if(loginTextBox.Text.Length == 0)
            {
                loginTextBox.BorderBrush = Brushes.IndianRed;
            }
            else if(passwordTextBox.Password.Length == 0)
            {
                passwordTextBox.BorderBrush = Brushes.IndianRed;
            }
            else if(emailTextBox.Text.Length == 0)
            {
                emailTextBox.BorderBrush = Brushes.IndianRed;
            }

            DataTable dataTable = new DataTable();
            DatabaseConnector.getInfoFromTable("Users", dataTable);
            DataRow newRow = dataTable.NewRow();

            if (dataTable.Rows.Count != 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {

                    if (Convert.ToInt32(row[0]) == dataTable.Rows.Count - 1)
                    {
                        newRow[0] = Convert.ToInt32(row[0]) + 1;
                        if (row[3].Equals(emailTextBox.Text))
                        {
                            MessageBox.Show("Пользователь с такой почтой уже зарегистрирован.","Такой Email уже существует!",MessageBoxButton.OK,MessageBoxImage.Warning);
                            return;
                        }
                    }
                }
            }
            else
            {
                newRow[0] = 0;
            }

            newRow[1] = loginTextBox.Text;
            newRow[2] = passwordTextBox.Password;
            newRow[3] = emailTextBox.Text;
            newRow[4] = 0;
            dataTable.Rows.Add(newRow);

            DatabaseConnector.UpdateBD("Users", dataTable);
            DataManager.Users = dataTable;
            this.Close();
        }
    }
}

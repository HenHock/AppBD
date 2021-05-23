﻿using System;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Resource\logoImage.png");
            bi.EndInit();
            logoImage.Source = bi;

            bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Resource\loginImage1.png");
            bi.EndInit();
            loginImage.Source = bi;
            loginTextBox.SelectionBrush = Brushes.LightGoldenrodYellow;

            bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Resource\passwordImage1.png");
            bi.EndInit();
            passwordImage.Source = bi;
            passwordTextBox.CaretBrush = Brushes.LightGoldenrodYellow;

            bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Resource\exitImage1.png");
            bi.EndInit();
            exitImage.Source = bi;

            DatabaseConnector.getInfoFromTable("Users", DataManager.Users);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                textBox.Foreground = Brushes.Black;
                textBox.Background = Brushes.WhiteSmoke;
                textBox.BorderBrush = Brushes.AliceBlue;

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Resource\loginImage2.png");
                bi.EndInit();
                loginImage.Source = bi;
            }
            else
            {
                PasswordBox passwordBox = sender as PasswordBox;

                passwordBox.Foreground = Brushes.Black;
                var bc = new BrushConverter();
                passwordBox.Background = Brushes.White;
                passwordBox.BorderBrush = Brushes.AliceBlue;

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Resource\passwordImage1.png");
                bi.EndInit();
                passwordImage.Source = bi;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                textBox.Foreground = Brushes.WhiteSmoke;
                var bc = new BrushConverter();
                textBox.Background = (Brush)bc.ConvertFrom("#FF22242C");
                textBox.BorderBrush = (Brush)bc.ConvertFrom("#FF22242C");

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Resource\loginImage1.png");
                bi.EndInit();
                loginImage.Source = bi;
            }
            else
            {
                PasswordBox passwordBox = sender as PasswordBox;

                passwordBox.Foreground = Brushes.WhiteSmoke;
                var bc = new BrushConverter();
                passwordBox.Background = (Brush)bc.ConvertFrom("#FF22242C");
                passwordBox.BorderBrush = (Brush)bc.ConvertFrom("#FF22242C");

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(Directory.GetCurrentDirectory() + @"\Resource\passwordImage1.png");
                bi.EndInit();
                passwordImage.Source = bi;
            }
        }

        private void ExitImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.ShowDialog();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if(loginTextBox.Text.Length > 0 && passwordTextBox.Password.Length > 0)
            {
                int i = 0;
                foreach(DataRow row in DataManager.Users.Rows)
                { 
                    if(row[1].Equals(loginTextBox.Text) && row[2].Equals(passwordTextBox.Password))
                    {
                        MainWindow window = new MainWindow();
                        window.Show();
                        DataManager.indexUser = i;
                        this.Close();
                        return;
                    }
                    i++;
                }
                MessageBox.Show("Логин или пароль были введены не правильно или такого аккаунта не существует!", "Не удалось войти", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                loginTextBox.BorderBrush = Brushes.IndianRed;
                passwordTextBox.BorderBrush = Brushes.IndianRed;
            }
        }
    }
}

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
using System.Windows.Shapes;

namespace Salon
{
    /// <summary>
    /// Логика взаимодействия для autorization.xaml
    /// </summary>
    public partial class autorization : Window
    {
        public autorization()
        {
            InitializeComponent();
        }

        private void open(object sender, RoutedEventArgs e)
        {
            if(pass.Password=="0000")
            {
                MessageBox.Show("Панель администратора открыта");
                admin admin= new admin();
                admin.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверные данные");
            }
        }

        private void exit(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

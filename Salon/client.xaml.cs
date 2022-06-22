using Salon.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для client.xaml
    /// </summary>
    public partial class client : Window
    {
        public SalonDbEntities entities = new SalonDbEntities();
        
        public client()
        {
            InitializeComponent();
            list.ItemsSource = entities.Service.ToList();
            allcolvo.Content = entities.Service.ToList().Count;
            bindcolvo.Content = entities.Service.ToList().Count;
           
        }

        private void text_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Service> services = new List<Service>();
            services = entities.Service.ToList().Where(x => x.Title.ToLower().Contains(text.Text.ToLower())).ToList();


            list.ItemsSource = services;
        }

        private void sortirovka(object sender, SelectionChangedEventArgs e)
        {
            List<Service> services1 = new List<Service>();
            if (combo.SelectedIndex == 0)
            {
                list.ItemsSource = entities.Service.ToList();
            }
            else if (combo.SelectedIndex == 1)
            {
                services1 = entities.Service.ToList().OrderBy(x => x.CostDisc).ToList();
                list.ItemsSource = services1;
            }
            else if (combo.SelectedIndex == 2)
            {
                services1 = entities.Service.ToList().OrderByDescending(x => x.CostDisc).ToList();
                list.ItemsSource = services1;
            }
        }

        private void filtr(object sender, SelectionChangedEventArgs e)
        {
            List<Service> services2 = new List<Service>();

            if (comboproz.SelectedIndex == 0)
            {
                list.ItemsSource = entities.Service.ToList();
                bindcolvo.Content = entities.Service.ToList().Count;
            }
            else if (comboproz.SelectedIndex == 1)
            {
                services2 = entities.Service.ToList().Where(x => x.Discount >= 0 && x.Discount < 5 || x.Discount == null).ToList();
                list.ItemsSource = services2;
                bindcolvo.Content = services2.Count;
            }
            else if (comboproz.SelectedIndex == 2)
            {
                services2 = entities.Service.ToList().Where(x => x.Discount >= 5 && x.Discount < 15).ToList();
                list.ItemsSource = services2;
                bindcolvo.Content = services2.Count;
            }
            else if (comboproz.SelectedIndex == 3)
            {
                services2 = entities.Service.ToList().Where(x => x.Discount >= 15 && x.Discount < 30).ToList();
                list.ItemsSource = services2;
                bindcolvo.Content = services2.Count;
            }
            else if (comboproz.SelectedIndex == 4)
            {
                services2 = entities.Service.ToList().Where(x => x.Discount >= 30 && x.Discount < 70).ToList();
                list.ItemsSource = services2;
                bindcolvo.Content = services2.Count;
            }
            else if (comboproz.SelectedIndex == 5)
            {
                services2 = entities.Service.ToList().Where(x => x.Discount >= 70 && x.Discount < 100).ToList();
                list.ItemsSource = services2;
                bindcolvo.Content = services2.Count;
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

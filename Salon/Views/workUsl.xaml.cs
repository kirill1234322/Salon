using Salon.Models;
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

namespace Salon.Views
{
    /// <summary>
    /// Логика взаимодействия для workUsl.xaml
    /// </summary>
    public partial class workUsl : Page
    {
        public SalonDbEntities entities;

        public workUsl()
        {
            InitializeComponent();
            entities = new SalonDbEntities();
            listadmin.ItemsSource = entities.Service.ToList();
            allcolvo.Content = entities.Service.ToList().Count;
            bindcolvo.Content = entities.Service.ToList().Count;
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            entities = new SalonDbEntities();
            Button button = (Button)sender;
            Service service = (Service)button.DataContext;
            redaktusl redaktusl = new redaktusl(service);
            //Class1.id = service.ID;
            //redaktusl.namebluda.Text = service.Title;
            //redaktusl.nameingrid.Text = service.Cost.ToString();
            //redaktusl.portionname.Text= (service.DurationInSeconds * 60).ToString();
            //redaktusl.pricename.Text = service.Description;
            //redaktusl.box.Text = service.Discount.ToString() ;
            redaktusl.Show();
            redaktusl.Closing += Redaktusl_Closing;


           



        }

        private void Redaktusl_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            entities = new SalonDbEntities();
            listadmin.ItemsSource = entities.Service.ToList();
            
        }

        private void additem(object sender, RoutedEventArgs e)
        {
            addusl addusl = new addusl();
            addusl.Show();
        }






        private void text_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Service> services = new List<Service>();
            services = entities.Service.ToList().Where(x => x.Title.ToLower().Contains(text.Text.ToLower())).ToList();


            listadmin.ItemsSource = services;
        }

        private void sortirovka(object sender, SelectionChangedEventArgs e)
        {
            List<Service> services1 = new List<Service>();
            if (combo.SelectedIndex == 0)
            {
                listadmin.ItemsSource = entities.Service.ToList();
            }
            else if (combo.SelectedIndex == 1)
            {
                services1 = entities.Service.ToList().OrderBy(x => x.CostDisc).ToList();
                listadmin.ItemsSource = services1;
            }
            else if (combo.SelectedIndex == 2)
            {
                services1 = entities.Service.ToList().OrderByDescending(x => x.CostDisc).ToList();
                listadmin.ItemsSource = services1;
            }
        }

        private void filtr(object sender, SelectionChangedEventArgs e)
        {
            List<Service> services2 = new List<Service>();

            if (comboproz.SelectedIndex == 0)
            {
                listadmin.ItemsSource = entities.Service.ToList();
                bindcolvo.Content = entities.Service.ToList().Count;
            }
            else if (comboproz.SelectedIndex == 1)
            {
                services2 = entities.Service.ToList().Where(x => x.Discount >= 0 && x.Discount < 5 || x.Discount == null).ToList();
                listadmin.ItemsSource = services2;
                bindcolvo.Content = services2.Count;
            }
            else if (comboproz.SelectedIndex == 2)
            {
                services2 = entities.Service.ToList().Where(x => x.Discount >= 5 && x.Discount < 15).ToList();
                listadmin.ItemsSource = services2;
                bindcolvo.Content = services2.Count;
            }
            else if (comboproz.SelectedIndex == 3)
            {
                services2 = entities.Service.ToList().Where(x => x.Discount >= 15 && x.Discount < 30).ToList();
                listadmin.ItemsSource = services2;
                bindcolvo.Content = services2.Count;
            }
            else if (comboproz.SelectedIndex == 4)
            {
                services2 = entities.Service.ToList().Where(x => x.Discount >= 30 && x.Discount < 70).ToList();
                listadmin.ItemsSource = services2;
                bindcolvo.Content = services2.Count;
            }
            else if (comboproz.SelectedIndex == 5)
            {
                services2 = entities.Service.ToList().Where(x => x.Discount >= 70 && x.Discount < 100).ToList();
                listadmin.ItemsSource = services2;
                bindcolvo.Content = services2.Count;
            }
        }



        private void zapisclient(object sender, RoutedEventArgs e)
        {
            entities = new SalonDbEntities();
            Button button = (Button)sender;

            Service service = (Service)button.DataContext;
            zapisClient zapisClient = new zapisClient();


            Class1.id = service.ID;
            zapisClient.namebluda.Text = service.Title;

            zapisClient.nameingrid.Text = (service.DurationInSeconds * 60).ToString();

            zapisClient.Show();
           

            
            
        }

        private void removeitem(object sender, RoutedEventArgs e)
        {
            entities = new SalonDbEntities();
            Button button = (Button)sender;
            Service service = (Service)button.DataContext;
           
          
           
            MessageBoxResult result = MessageBox.Show("Удалить позицию?", "Удаление",
  MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                entities = new SalonDbEntities();

                Service service1 = entities.Service.Where(x => x.ID == service.ID).FirstOrDefault();
                if (service1.ClientService.Count != 0)
                {
                    MessageBox.Show("При использовании услуги, удаление невозможно", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                entities.Service.Attach(service1);
                entities.Service.Remove(service1);
                ServicePhoto servicePhoto=entities.ServicePhoto.Where(x => x.ServiceID == service.ID).FirstOrDefault();
               entities.ServicePhoto.Attach(servicePhoto);
                entities.ServicePhoto.Remove(servicePhoto);
                entities.SaveChanges();
                Redaktusl_Closing(null, null);
                

            }
           
        }

       
    }
}

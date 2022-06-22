using Salon.Converter;
using Salon.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Globalization;
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
using System.Windows.Shapes;

namespace Salon.Views
{
    /// <summary>
    /// Логика взаимодействия для zapisClient.xaml
    /// </summary>
    public partial class zapisClient : Window, INotifyPropertyChanged
    {
        public SalonDbEntities entities;
        private string time;
     
        public event PropertyChangedEventHandler PropertyChanged;

        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Time"));
                }
            }
        }
        public zapisClient()
        {
            entities = new SalonDbEntities();
            InitializeComponent();
            var a = from item in entities.Client
                   
                    select new {item.ID, item.FirstName, item.LastName, item.Patronymic };
           
         foreach(var item in a)
            {
                comboclient.Items.Add(item.ID+" "+ item.FirstName +" "+ item.LastName +" "+ item.Patronymic);
             
            }
         

            Time = DateTime.Now.ToString();
            txb_date.DataContext = this;

        }

        private void additem(object sender, RoutedEventArgs e)
        {
            try
            {
                Service customer = (from r in entities.Service where r.ID == Class1.id select r).SingleOrDefault();

                List<Client> clients = new List<Client>();

                DateTime date = new DateTime();
                date = calendar1.SelectedDate.Value;

                labeltime.Content = date.ToShortDateString() + " " + txb_date.Text;

                TimeSpan time = TimeSpan.Parse(txb_date.Text);
                string time1 = Convert.ToString(time);
                string date1 = Convert.ToString(date);

                Class1.time3 = Convert.ToDateTime(date1).Add(TimeSpan.Parse(time1));
                DateTime dt1 = Convert.ToDateTime(date1).Add(TimeSpan.Parse(time1));


                int idCl = comboclient.SelectedIndex + 1;

                ClientService clientService = new ClientService()
                {
                    ClientID = idCl,
                    ServiceID = Class1.id,
                    StartTime = dt1

                };
                entities.ClientService.Add(clientService);


                entities.SaveChanges();
                MessageBox.Show("Позиция добавлена");
            }
            catch
            {
                MessageBox.Show("Не все поля заполнены");
            }
            


        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            DateConvertPicker dc = new DateConvertPicker();
            DateTime dt = ((DateTime)dc.ConvertBack(txb_date.Text, null, null, null));

            if (((RepeatButton)sender).Name == "up")
                Time = dt.AddMinutes(1).ToString();
            else Time = dt.AddMinutes(-1).ToString();
        }

        private void RepeatButtonHour_Click(object sender, RoutedEventArgs e)
        {
            DateConvertPicker dc = new DateConvertPicker();
            DateTime dt = ((DateTime)dc.ConvertBack(txb_date.Text, null, null, null));

            if (((RepeatButton)sender).Name == "upH")
                Time = dt.AddHours(1).ToString();
            else Time = dt.AddHours(-1).ToString();
        }

      
    }
}

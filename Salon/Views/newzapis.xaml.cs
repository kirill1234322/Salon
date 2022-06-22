using Salon.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Salon.Views
{
    /// <summary>
    /// Логика взаимодействия для newzapis.xaml
    /// </summary>
    public partial class newzapis : Page
    {
        public SalonDbEntities entities;
       
        public newzapis()
        {
            
            InitializeComponent();
            timer_tick(null, null);
            var timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_tick);
            timer.Interval = new TimeSpan(0, 0, 30);
            timer.Start();



           

        }
        public void timer_tick(object sender, EventArgs e)
        {
            entities = new SalonDbEntities();
            DateTime date = DateTime.Today;
            DateTime date2 = DateTime.Today;
            date2 = date2.AddDays(2);
            // listnew.ItemsSource = entities.ClientService.Where(x => x.StartTime>date && x.StartTime<date2).ToList().OrderBy(x=>x.StartTime).ToList();




            //DateTime date5=Convert.ToDateTime(date4);

            // listzapis.ItemsSource = entities.ClientService.ToList().OrderBy(x => x.StartTime).ToList();



            entities = new SalonDbEntities();
            DateTime onedaylater = DateTime.Now.AddDays(1);
            var spisok = entities.ClientService.Where(x => (x.StartTime.Year == DateTime.Now.Year &&
            x.StartTime.Month == DateTime.Now.Month &&
            x.StartTime.Day == DateTime.Now.Day)
            ||
            (x.StartTime.Year == onedaylater.Year &&
            x.StartTime.Month == onedaylater.Month &&
            x.StartTime.Day == onedaylater.Day)).ToList();

            var spisokforBinding = new List<ClientService>();
            foreach (var item in spisok)
            {
                spisokforBinding.Add(item);
            }
            foreach (var item in spisok)
            {
                if (item.StartTime.Day == DateTime.Now.Day)
                    if (
                       item.StartTime.TimeOfDay > DateTime.Now.TimeOfDay
                      )
                    {
                    }
                    else
                    {
                        spisokforBinding.Remove(item);
                    }
            }

          

            listnew.ItemsSource = spisokforBinding.OrderBy(o => o.StartTime).ToList();
        }



        }
}

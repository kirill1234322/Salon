using Salon.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Salon
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
              SalonDbEntities entities = new SalonDbEntities();

        //из папки
        List<string> OnDebug = (from a in Directory.GetFiles(Environment.CurrentDirectory + "/Услуги салона красоты") select System.IO.Path.GetFileName(a)).ToList();

            //из базы
            List<Service> OnBase = new List<Service>();

            //из базы 
            List<ServicePhoto> OnBasePhotos = new List<ServicePhoto>();
            OnBase = entities.Service.ToList();
            OnBasePhotos = entities.ServicePhoto.ToList();

            foreach (var item in OnDebug)
            {
                bool b = false;
                foreach (var item2 in OnBase)
                {
                    if (item2.mainImage != null && item2.mainImage.Length > 24)
                    {
                        string loc = item2.mainImage.Substring(22, item2.mainImage.Length - 22).ToString();
                        int idd = item2.ID;
                        if (item2.mainImage.Contains("\r\n"))
                        {
                            loc = loc.Substring(0, loc.Length - 2);
                        }
                        if (item.ToString() == loc)
                        {
                            b = true;
                            break;
                        }
                    }
                }

                if (b == false)//если фотка не нашлась в базе  твблице service, то проверяем её в таблице servicephotos
                {
                    bool flag = false;
                    foreach (var items in OnDebug)
                    {
                        foreach (var item2 in OnBasePhotos)
                        {

                            string loc = item2.photoPath.Substring(22, item2.photoPath.Length - 22).ToString();
                            int idd = item2.ID;
                            if (item.Contains("\r\n"))
                            {
                                loc = loc.Substring(0, loc.Length - 2);
                            }
                            if (item.ToString() == loc)
                            {
                                flag = true;
                                break;
                            }

                        }
                    }

                    if (flag == false)
                        if (item != null && item != "" && item != " ") //если строчка в базе не пустая
                        {
                            File.Delete(Environment.CurrentDirectory + "\\Услуги салона красоты\\" + item); //удаляем фотку
                        }

                }
            }
        }

        private void client(object sender, RoutedEventArgs e)
        {
            client client = new client();
            client.Show();
            this.Close();
        }

        private void admin(object sender, RoutedEventArgs e)
        {
            autorization autorization = new autorization();
            autorization.Show();
            this.Close();
        }
    }
}

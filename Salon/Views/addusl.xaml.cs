using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace Salon.Views
{
    /// <summary>
    /// Логика взаимодействия для addusl.xaml
    /// </summary>
    public partial class addusl : Window
    {
        public SalonDbEntities entities = new SalonDbEntities();
        string pathForImg;
        public addusl()
        {
            InitializeComponent();
        }

        private void Add(object sender, RoutedEventArgs e)
        {

            if (entities.Service.FirstOrDefault(x => x.Title == namebluda.Text.Trim()) != null)
            {
                MessageBox.Show("Такая услуга уже существует!");
                return;
            }
          
           
            try
            {
                entities = new SalonDbEntities();
                Service customer = new Service();


                customer.Title = namebluda.Text;
                int value2 = 0;
                if (int.TryParse(portionname.Text, out value2))
                {
                    if (value2 > 14400)
                    {
                        throw new Exception("Длительность услуги не может быть больше 4 часов!");
                    }
                    customer.DurationInSeconds = value2 * 60;
                }
                else
                {
                    MessageBox.Show("Неверные входные данные!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                double value3 = 0;
                if (Double.TryParse(box.Text, out value3))
                {
                    if (value3 < 0 || value3 > 100)
                    {
                        MessageBox.Show("Скидка не может быть меньше 0% и больше 100% !", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    customer.Discount = value3 / 10000;
                }
                else
                {
                    MessageBox.Show("Неверные входные данные!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                decimal value = 0;
                if (decimal.TryParse(nameingrid.Text, out value))
                {
                    decimal b = value * ((decimal)customer.Discount);
                    customer.Cost = value;
                }
                else
                {
                    MessageBox.Show("Неверные входные данные! Задайте скидке значение 0", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                customer.Description = pricename.Text;
               
                customer.MainImagePath = pathForImg;
                entities.Service.Add(customer);


                entities.SaveChanges();
                MessageBox.Show("Позиция добавлена", "Уведомление");
                Close();
            }
            catch (Exception ee)
            {
                if (ee.ToString().Contains("Длительность услуги не может быть больше"))
                {
                    MessageBox.Show("Длительность услуги не может быть больше 4 часов!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Провал! Ошибка:" + ee.ToString(), "Уведомление");
            }



            
            


        }

        private void addPhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Multiselect = false;
            op.Title = "Выберите изображение";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            bool? result = op.ShowDialog();
            bool flag = false;
            if (result == true)
            {

                string imgPath = "Услуги салона красоты\\" + op.SafeFileName; //Куда сохраняется файл         
                pathForImg = imgPath;

                //проверяем, используется ли такая картинка в базе
                if (File.Exists(imgPath))
                {
                    imgName.Content = op.SafeFileName;
                    flag = true;
                }
                if (flag != true)
                {
                    MemoryStream ms = new MemoryStream();  //Поток памяти
                    FileStream fs = new FileStream(imgPath, FileMode.Create); //Поток файла

                    var bitmap = new BitmapImage(new Uri(op.FileName));

                    PngBitmapEncoder pngEnc = new PngBitmapEncoder(); //сохраняем в формате PNG
                    pngEnc.Frames.Add(BitmapFrame.Create(bitmap));
                    pngEnc.Save(fs);
                    fs.Close();
                    imgName.Content = op.SafeFileName;
                }
            }
        }
    }
}

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
    /// Логика взаимодействия для redaktusl.xaml
    /// </summary>
    public partial class redaktusl : Window
    {
        public SalonDbEntities entities;
        private Service service;
        string pathForImg;
        public redaktusl(Service _service)
        {
            InitializeComponent();
            entities = new SalonDbEntities();
            service = entities.Service.Where(x => x.ID == _service.ID).FirstOrDefault();
            namebluda.Text = service.Title;
            nameingrid.Text = Math.Round(service.Cost, 2).ToString();
            portionname.Text = (service.DurationInSeconds * 60).ToString();
            box.Text = service.Discount.ToString();
            if (service.Description != null) pricename.Text = service.Description.ToString();
            if (service.MainImagePath != null && service.MainImagePath.Length > 21)
                imgName.Content = service.mainImage.Substring(22);
            else imgName.Content = "изображение отсутствует";
            List<ServicePhoto> listdop = service.ServicePhoto.ToList();
            DopPhotoListbox.ItemsSource = listdop;
        }

       

        private void Update(object sender, RoutedEventArgs e)
        {
            try
            {
                entities = new SalonDbEntities();
                Service customer = entities.Service.Where(x => x.ID == service.ID).FirstOrDefault();


                customer.Title = namebluda.Text;
                customer.Cost = Convert.ToDecimal(nameingrid.Text);

                int value2 = 0;
                if (int.TryParse(portionname.Text, out value2))
                {
                    if (value2 > 14400)
                    {
                        throw new Exception("Длительность услуги не может быть больше 4 часов!");
                    }
                    customer.DurationInSeconds = value2 * 60;
                }

                customer.Description = pricename.Text;

                if (String.IsNullOrEmpty(box.Text) || String.IsNullOrWhiteSpace(box.Text))
                {
                    box.Text = "0";
                    customer.Discount = Convert.ToDouble(box.Text) / 10000;
                }
                else
                {
                    customer.Discount = Convert.ToDouble(box.Text) / 10000;
                }
                if (imgName.Content.ToString() == "изображение отсутствует")
                {
                    throw new Exception("Добавьте изображение!");
                }
                customer.MainImagePath = pathForImg;
                entities.SaveChanges();

                MessageBox.Show("Позиция изменена");

                Close();
            }
            catch (Exception ee)
            {
                if (ee.ToString().Contains("Длительность услуги не может быть больше"))
                {
                    MessageBox.Show("Длительность услуги не может быть больше 4 часов!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Что-то пошло не так!", "Уведомление");
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

        private void addDopPhoto(object sender, RoutedEventArgs e)
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
                entities = new SalonDbEntities();
                string imgPath = "Услуги салона красоты\\" + op.SafeFileName; //Куда сохраняется файл         
                pathForImg = imgPath;

                //проверяем, используется ли такая картинка в базе
                if (File.Exists(imgPath))
                {
                    entities.ServicePhoto.Add(new ServicePhoto() { ServiceID = service.ID, PhotoPath = imgPath });
                    entities.SaveChanges();

                    //обновляем список доп фоток                    
                    Service local = entities.Service.Where(x => x.ID == service.ID).FirstOrDefault();
                    List<ServicePhoto> listdop2 = local.ServicePhoto.ToList();
                    DopPhotoListbox.ItemsSource = listdop2;

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

                    entities.ServicePhoto.Add(new ServicePhoto() { ServiceID = service.ID, PhotoPath = imgPath });
                    entities.SaveChanges();

                    //обновляем список доп фоток                    
                    Service local = entities.Service.Where(x => x.ID == service.ID).FirstOrDefault();
                    List<ServicePhoto> listdop2 = local.ServicePhoto.ToList();
                    DopPhotoListbox.ItemsSource = listdop2;
                }
            }
        }

        private void deleteDopPhoto(object sender, RoutedEventArgs e)
        {
            MessageBoxResult pr = MessageBox.Show("Вы уверены? Данное действие необратимо", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (pr == MessageBoxResult.Yes)
            {

                entities = new SalonDbEntities();
                Button button = sender as Button;
                ServicePhoto servicephotos = button.DataContext as ServicePhoto;
                ServicePhoto inbase = entities.ServicePhoto.Where(x => x.ID == servicephotos.ID).FirstOrDefault();
                entities.ServicePhoto.Remove(inbase);
                entities.SaveChanges();

                //обновляем вывод доп картинок 
                //вывод доп картинок 
                entities = new SalonDbEntities();
                Service local = entities.Service.Where(x => x.ID == service.ID).FirstOrDefault();
                List<ServicePhoto> listdop2 = local.ServicePhoto.ToList();
                DopPhotoListbox.ItemsSource = listdop2;
               
            }
        }

        private void deletePhoto(object sender, RoutedEventArgs e)
        {
            if (imgName.Content == "изображение отсутствует")
            {
                MessageBox.Show("Изображения отсутствует", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBoxResult pr = MessageBox.Show("Вы уверены, что хотите удалить изображение?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (pr == MessageBoxResult.Yes)
            {
                imgName.Content = "изображение отсутствует";
            }
        }
    }
}

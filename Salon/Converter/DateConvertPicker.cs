using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Salon.Converter
{
    public class DateConvertPicker : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Возвращаем строку в формате 12:45
            DateTime time;
            if (DateTime.TryParse((string)value, out time))
            {
                return time.ToString("HH:mm");
            }
            else return "";
        }

       

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime result;
            if (DateTime.TryParse((string)value, out result))
                return result;
            else return null;
        }

       
    }
}

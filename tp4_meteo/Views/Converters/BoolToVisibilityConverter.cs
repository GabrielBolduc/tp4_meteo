using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace tp4_meteo.Views.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visible = value != null;
            if (value is bool b) visible = b;
            if (value is int i) visible = i > 0;

            if (parameter?.ToString() == "Inverse") visible = !visible;
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
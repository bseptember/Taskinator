using System.Globalization;

namespace Taskinator.Helper
{
    public class HourToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double offset = 0.96;
            
            #if (WINDOWS)
                offset = 0.9826;
            #endif

            if (value is int startMinute)
            {
                return new Thickness(60, (int)(startMinute * offset), 0, 0);
            }

            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

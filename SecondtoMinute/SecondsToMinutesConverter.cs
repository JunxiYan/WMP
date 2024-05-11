using System.Globalization;

namespace SecondtoMinute
{
    
        public class SecondsToMinutesConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return TimeSpan.FromSeconds((double)value).ToString(@"mm\:ss");
            }
        }
    }


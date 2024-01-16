using System.Globalization;
using System.Windows.Data;

namespace TextEditorApp.Common.Enums
{
    // in our case used to convert from CustomHorizontalTextAlignment to bool for text alignment radio buttons
    public class EnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.Equals(parameter) == true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is true)
            {
                return parameter;
            }

            return Binding.DoNothing;
        }
    }
}

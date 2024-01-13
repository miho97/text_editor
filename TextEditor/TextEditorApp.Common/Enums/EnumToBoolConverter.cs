﻿using System.Globalization;
using System.Windows.Data;

namespace TextEditorApp.Common.Enums
{
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

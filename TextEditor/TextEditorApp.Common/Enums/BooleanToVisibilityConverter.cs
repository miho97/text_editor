using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TextEditorApp.Common.Enums
{
    /// <summary>
    /// Converter class that implements the IValueConverter interface to convert between boolean and Visibility values.
    /// </summary>
    /// 

    // this converter is in practice used to bind the 'Enable Web Browser' button and the visibility of web browser without the need of handling the extra command
    public class BooleanToVisibilityConverter : IValueConverter
    {

        /// <summary>
        /// Converts a boolean value to a Visibility enumeration value.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The type of the target property (not used).</param>
        /// <param name="parameter">An optional parameter (not used).</param>
        /// <param name="culture">The culture to use for the conversion (not used).</param>
        /// <returns>Visibility.Visible if the input boolean value is true, Visibility.Collapsed otherwise.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a Visibility value back to a boolean value.
        /// </summary>
        /// <param name="value">The Visibility value to convert back.</param>
        /// <param name="targetType">The type of the target property (not used).</param>
        /// <param name="parameter">An optional parameter that specifies the value to use when the input Visibility is Visible.</param>
        /// <param name="culture">The culture to use for the conversion (not used).</param>
        /// <returns>True if the input Visibility value is Visible, or the specified parameter value; otherwise, Binding.DoNothing.</returns>
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

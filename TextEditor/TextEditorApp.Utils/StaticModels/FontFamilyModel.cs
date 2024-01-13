using System.Windows.Media;
using TextEditorApp.Common;

namespace TextEditorApp.Utils.StaticModels
{
    /// <summary>
    /// ViewModel class representing a font family selection with properties related to the font family and its string representation.
    /// </summary>
    public class FontFamilyModel : ViewModelBase
    {
        private FontFamily? fontFamily;
        private string _FontfamilyString;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontFamilyModel"/> class with the specified font family and enablement.
        /// </summary>
        /// <param name="fontFamily">The font family of type <see cref="FontFamily"/>.</param>
        /// <param name="isEnabled">Specifies whether the font family is enabled.</param>
        public FontFamilyModel(FontFamily fontFamily, bool isEnabled)
        {
            Fontfamily = fontFamily;
            _FontfamilyString = Fontfamily.ToString();
            IsEnabled = isEnabled;
        }

        /// <summary>
        /// Initializes a new default instance of the <see cref="FontFamilyModel"/> class with default values.
        /// </summary>
        public FontFamilyModel()
        {
            Fontfamily = new FontFamily();
            _FontfamilyString = Fontfamily.ToString();
            IsEnabled = isEnabled;

        }
        public FontFamily Fontfamily
        {
            get { return fontFamily ?? (fontFamily = new FontFamily()); }
            set
            {
                if (fontFamily != value)
                {
                    fontFamily = value;
                    OnPropertyChanged(nameof(fontFamily));
                    FontfamilyString = value.ToString();
                }
            }
        }

        public string FontfamilyString
        {
            get { return _FontfamilyString; }
            set
            {
                if (_FontfamilyString != value)
                {
                    _FontfamilyString = value;
                    OnPropertyChanged(nameof(_FontfamilyString));
                }
            }
        }

        private bool isEnabled;

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    OnPropertyChanged(nameof(IsEnabled));
                }
            }
        }

        public override string ToString()
        {
            return FontfamilyString;
        }
    }
}

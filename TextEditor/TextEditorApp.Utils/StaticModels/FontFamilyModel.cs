using System.Windows.Media;
using TextEditorApp.Common;

namespace TextEditorApp.Utils.StaticModels
{
    public class FontFamilyModel : ViewModelBase
    {
        private FontFamily? fontFamily;
        private string _FontfamilyString;

        public FontFamilyModel(FontFamily fontFamily, bool isEnabled)
        {
            Fontfamily = fontFamily;
            _FontfamilyString = Fontfamily.ToString();
            IsEnabled = isEnabled;
        }

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

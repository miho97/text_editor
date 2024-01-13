using TextEditorApp.Common;

namespace TextEditorApp.Utils.StaticModels
{
    /// <summary>
    /// ViewModel class representing a font size selection with properties related to the font size.
    /// </summary>

    // since all of the properties, setters and getter are verbose enough we will only explain those that are not straightforward
    public class FontSizeListModel : ViewModelBase
    {
        private double _fontSize;
        public string _FontSizeText;
        public FontSizeListModel(double fontSize, bool isEnabled)
        {
            _fontSize = fontSize;
            IsEnabled = isEnabled;
            _FontSizeText = fontSize.ToString();
        }

        public FontSizeListModel()
        {
            _fontSize = 12;
            IsEnabled = true;
            _FontSizeText = _fontSize.ToString();
        }
        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    FontSizeText = value.ToString();
                    OnPropertyChanged(nameof(FontSize));
                }
            }
        }

        /// <summary>
        /// Overrides the default ToString method to return the text representation of the font size.
        /// </summary>
        /// <returns>The string representation of the font size.</returns>

        // This was needed for implicit conversion on a couple of places later in the code
        public override string ToString()
        {
            return _FontSizeText;
        }


        /// <summary>
        /// Gets or sets the text representation of the font size.
        /// </summary>
        public string FontSizeText
        {
            get { return _FontSizeText; }
            set
            {
                if (_FontSizeText != value && value is string val2)
                {
                    _FontSizeText = val2;
                    FontSize = double.Parse(_FontSizeText);
                    OnPropertyChanged(nameof(FontSizeText));
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
    }
}

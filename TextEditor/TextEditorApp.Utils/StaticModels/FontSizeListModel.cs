using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TextEditorApp.Common;

namespace TextEditorApp.Utils.StaticModels
{
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

        public override string ToString()
        {
            return _FontSizeText;
        }

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

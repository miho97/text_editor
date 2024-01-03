using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEditorApp.Common;

namespace TextEditorApp.Utils.StaticModels
{
    public class FontSizeListModel : ViewModelBase
    {
        private int fontSize;
        public int FontSize
        {
            get { return fontSize; }
            set
            {
                if (fontSize != value)
                {
                    fontSize = value;
                    OnPropertyChanged(nameof(FontSize));
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

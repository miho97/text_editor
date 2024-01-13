using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TextEditorApp.Common;

namespace TextEditorApp.Utils.StaticModels
{
    public class LanguageViewModel : ViewModelBase
    {
        private string _SelectedLanguage;

        public LanguageViewModel(string SelectedLanguage, bool isEnabled)
        {
            _SelectedLanguage = SelectedLanguage;
            IsEnabled = isEnabled;
        }

        public LanguageViewModel()
        {
            _SelectedLanguage = "None";
            IsEnabled = true;
        }
        public string SelectedLanguage
        {
            get { return _SelectedLanguage; }
            set
            {
                if (_SelectedLanguage != value)
                {
                    _SelectedLanguage = value;
                    OnPropertyChanged(nameof(SelectedLanguage));
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
            return _SelectedLanguage;
        }
    }
}

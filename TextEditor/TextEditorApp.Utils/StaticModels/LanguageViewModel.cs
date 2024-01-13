using TextEditorApp.Common;

namespace TextEditorApp.Utils.StaticModels
{
    /// <summary>
    /// ViewModel class representing a language selection with properties related to the selected language and its enablement.
    /// In practice it is used to represent properties of all the languages used for font highlightning in the active document.
    /// </summary>

    // since all of the properties, setters and getter are verbose enough we will only explain those that are not straightforward
    public class LanguageViewModel : ViewModelBase
    {
        private string _SelectedLanguage;


        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageViewModel"/> class with the specified language and enablement.
        /// </summary>
        /// <param name="SelectedLanguage">Selected</param>
        /// <param name="isEnabled">Selected</param>
        public LanguageViewModel(string SelectedLanguage, bool isEnabled)
        {
            _SelectedLanguage = SelectedLanguage;
            IsEnabled = isEnabled;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageViewModel"/> class with the specified language and enablement.
        /// </summary>
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

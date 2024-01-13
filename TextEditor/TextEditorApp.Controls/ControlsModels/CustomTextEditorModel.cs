using System.Windows.Controls;
using TextEditorApp.Common;
using TextEditorApp.Common.Enums;
using TextEditorApp.Utils.DocumentFiles;
using System.Windows;
using Microsoft.CodeAnalysis;
using System.Windows.Documents;
using TextEditorApp.Dialogs.CodeCompetionDialog;
using ICSharpCode.AvalonEdit.Rendering;

namespace TextEditorApp.Controls.ControlsModels
{
    public class CustomTextEditorModel : CustomTextEditorBaseModel
    {
        private DocumentFiles_Model _document;
        private DockPanel? _dockParent;
        private bool uglyDirtyCompletionDisabler = false;
        private List<string> usedVariables;


        public CustomTextEditorModel() : base()
        {
            usedVariables = new List<string>();
            this.ClipToBounds = true;
            base.IsBraceCompletionEnabled = true;
            _document = new DocumentFiles_Model();
            base.ShowLineNumbers = false;
            base.IsReadOnly = false;
            base.HorizontalAlignment = HorizontalAlignment.Stretch;
            base.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

            base.TextChanged += (sender, args) =>
            {
                DocumentModel.IsSaved = false;
                UpdateCurrentWord();
                CheckForNewVariable();
                ExecuteCodeCompletion();
            };
            //base.PreviewTextInput += (sender, args) =>
            //{
            //    HandleUserInput(sender, args);
            //};
        }

        //private void HandleUserInput(object sender, TextCompositionEventArgs e)
        //{
        //    List<string> validCharacters = new List<string> { "(", "{", "[", ")", "}", "]" };
        //    if (validCharacters.Contains(e.Text))
        //    {

        //        int caretIndex = base.SelectionStart;
        //        string text = base.Text;

        //        int openBracketIndex = text.LastIndexOf('(', caretIndex - 1);
        //        int closeBracketIndex = text.IndexOf(')', caretIndex-1);

        //        if (openBracketIndex != -1 && closeBracketIndex != -1 && openBracketIndex < closeBracketIndex)
        //        {

        //            base.Select(openBracketIndex, closeBracketIndex - openBracketIndex + 1);
        //            base.TextArea.SelectionBrush = Brushes.Red;

        //            this.TextArea.Style
        //            base.TextArea.Foreground = Brushes.Yellow; 

        //            base.Select(caretIndex, 0);
        //        }
        //    }
        //}

        private CustomHorizontalTextAlignment _textAlignment = CustomHorizontalTextAlignment.Left;

        public CustomHorizontalTextAlignment TextAlignment
        {
            get => _textAlignment;
            set
            {
                
                _textAlignment = value;
                base.TextArea.TextView.HorizontalAlignment = ((HorizontalAlignment)Enum.ToObject(typeof(HorizontalAlignment), (int)value));
                OnPropertyChanged(nameof(TextAlignment));
            }
        }

        public DocumentFiles_Model DocumentModel
        {
            get => _document;
            init
            {
                _document = value;
                OnPropertyChanged(nameof(DocumentModel));
            }
        }

        private string _currentWord = string.Empty;

        public string CurrentWord
        {
            get { return _currentWord; }
            set
            {
                if (_currentWord != value)
                {
                    _currentWord = value;
                    OnPropertyChanged(nameof(CurrentWord));
                }
            }
        }

        public DockPanel DockParent
        {
            get { return _dockParent ?? (_dockParent = new DockPanel()); }
            set
            {
                if (_dockParent != value)
                {
                    _dockParent = value;
                    OnPropertyChanged(nameof(DockParent));
                }
            }
        }

        private bool _isShowNumbersEnabled;

        public bool IsShowNumbersEnabled
        {
            get => _isShowNumbersEnabled;
            set
            {
                _isShowNumbersEnabled = value;
                base.ShowLineNumbers = value;
                OnPropertyChanged(nameof(IsShowNumbersEnabled));
            }
        }
        private bool _isIntellisenseEnabled;

        public bool IsIntellisenseEnabled
        {
            get => _isIntellisenseEnabled;
            set
            {
                _isIntellisenseEnabled = value;
                OnPropertyChanged(nameof(IsIntellisenseEnabled));
            }
        }

        public bool IsPrimIntellisenseEnabled
        {
            get => !uglyDirtyCompletionDisabler;
            set
            {
                uglyDirtyCompletionDisabler = !value;
                OnPropertyChanged(nameof(IsPrimIntellisenseEnabled));
            }
        }

        private void UpdateCurrentWord()
        {
            int cursorPosition = base.SelectionStart;
            string text = base.Text;

            if (text == string.Empty) CurrentWord = string.Empty;

            int startOfWord = cursorPosition - 1;

            while (startOfWord >= 0 && !char.IsWhiteSpace(text[startOfWord]))
            {
                startOfWord--;
            }

            int endOfWord = cursorPosition;

            while (endOfWord < text.Length && !char.IsWhiteSpace(text[endOfWord]))
            {
                endOfWord++;
            }

            string currentWord = text.Substring(startOfWord + 1, endOfWord - startOfWord - 1);
            CurrentWord = currentWord;
        }

        private void InsertSelectedWord(string selectedWord)
        {
            if (string.IsNullOrEmpty(selectedWord)) return;

            int cursorIndex = base.SelectionStart;
            string text = base.Text;

            int startOfWord = cursorIndex - 1;
            while (startOfWord >= 0 && !char.IsWhiteSpace(text[startOfWord]))
            {
                startOfWord--;
            }

            int endOfWord = cursorIndex;
            while (endOfWord < text.Length && !char.IsWhiteSpace(text[endOfWord]))
            {
                endOfWord++;
            }

            var activeCompFilter = uglyDirtyCompletionDisabler;
            uglyDirtyCompletionDisabler = true;

            base.Text = text.Remove(startOfWord + 1, endOfWord - startOfWord - 1).Insert(startOfWord + 1, selectedWord + " " );

            base.SelectionStart = startOfWord + 2 + selectedWord.Length;

            uglyDirtyCompletionDisabler = activeCompFilter;
        }

        private void CheckForNewVariable()
        {
            if (CurrentWord != "=" || base.Text == string.Empty) return;

            int cursorIndex = base.SelectionStart;
            string text = base.Text;

            int endOfWord = cursorIndex - 2;
            var currentChar = text[endOfWord];
            if (!char.IsLetterOrDigit(currentChar) && !char.IsWhiteSpace(currentChar)) return;
            while (endOfWord >= 0 && !char.IsWhiteSpace(text[endOfWord]))
            {
                endOfWord--;
            }

            while (endOfWord >= 0 && char.IsWhiteSpace(text[endOfWord]))
            {
                endOfWord--;
            }

            int startOfWord = endOfWord;
            while (startOfWord >= 0 && !char.IsWhiteSpace(text[startOfWord]))
            {
                startOfWord--;
            }

            if (startOfWord >= 0)
            {
                string variableName = text.Substring(startOfWord + 1, endOfWord - startOfWord);
                if(!usedVariables.Contains(variableName))
                {
                    usedVariables.Add(variableName);
                }
            }

        }

        private List<string> GetMatchingKeywords()
        {
            if (CurrentWord == string.Empty)  {
                RemoveAllAdorners();
                return new List<string>(); ;
            }
            return usedVariables
                .Concat(CSKeywords.AllKeywords)
                .Where(w => w.StartsWith(CurrentWord, StringComparison.OrdinalIgnoreCase) || w.Contains(CurrentWord, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }


        private void ExecuteCodeCompletion()
        {
            if (uglyDirtyCompletionDisabler) return;
            if (GetMatchingKeywords() is List<string> keywords && keywords.Count > 0 && DockParent is Panel panel)
            {
                AddKeywordsAdorner(keywords);
            }
        }

        private void AddKeywordsAdorner(List<string> keywords)
        {
            var area = this;

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(area);
            RemoveAllAdorners();

            var caretPosition = base.TextArea.Caret.Position;
            var caretVisualPosition = base.TextArea.TextView.GetVisualPosition(caretPosition, VisualYPosition.LineTop);
            var comboBoxAdorner = new CustomCompletionAdorner(area, caretVisualPosition, CreateComboForAdorner(keywords));
            adornerLayer.Add(comboBoxAdorner);
        }

        private ComboBox CreateComboForAdorner(List<string> keywords)
        {
            var comboBox = new ComboBox();
            comboBox.ItemsSource = keywords;
            comboBox.IsEnabled = true;

            comboBox.SelectionChanged += (sender, args) =>
            {
                if (comboBox.SelectedValue != null)
                {
                    InsertSelectedWord(comboBox.SelectedValue.ToString() ?? string.Empty);
                    comboBox.IsDropDownOpen = false;
                    comboBox.IsEnabled = false;
                    RemoveAllAdorners();
                    this.Focus();
                    this.TextArea.Focus();
                }
            };

            return comboBox;
        }

        private void RemoveAllAdorners()
        {
            var area = this;

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(area);
            Adorner[] adorners = adornerLayer.GetAdorners(area);

            if (adorners != null)
            {
                foreach (var adorner in adorners)
                {
                    adornerLayer.Remove(adorner);
                }
            }
        }
    }
}

using ICSharpCode.AvalonEdit;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.ComponentModel;


namespace TextEditorApp
{
    /// <summary>
    /// Interaction logic for FindReplaceWindow.xaml
    /// </summary>
    public partial class FindReplaceWindow : Window
    {
        public string SearchText { get; private set; } = string.Empty;
        public string ReplaceText { get; private set; } = string.Empty;
        public bool MatchCase { get; private set; }
        public bool NextClicked { get; private set; }
        public bool PreviousClicked { get; private set; }
        public bool FindNext { get; private set; }
        private bool replacementMade = false;
        public bool CircularSearch { get; private set; }
        private int totalMatches = 0;
        private int currentMatchIndex = 0;
        private bool countingMatches = false;


        private readonly TextEditor textEditor;
        private int currentIndex = -1;

        public FindReplaceWindow(TextEditor textEditor)
        {
            InitializeComponent();
            this.textEditor = textEditor;
            UpdateHighlighting();
        }

        public int TotalMatches
        {
            get { return totalMatches; }
            set
            {
                if (totalMatches != value)
                {
                    totalMatches = value;
                    OnPropertyChanged(nameof(TotalMatches));
                }
            }
        }

        public int CurrentMatchIndex
        {
            get { return currentMatchIndex; }
            set
            {
                if (currentMatchIndex != value)
                {
                    currentMatchIndex = value;
                    OnPropertyChanged(nameof(CurrentMatchIndex));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            SearchText = txtFind.Text;
            ReplaceText = txtReplace.Text;
            MatchCase = chkMatchCase.IsChecked ?? false;
            FindNext = chkFindNext.IsChecked ?? false;

            if (FindNext)
            {
                --currentMatchIndex;
                replacementMade = FindAndReplaceNext();
                UpdateMatchLabels();
            }
            else
            {
                ReplaceSelectedText();
            }
        }

        private void ReplaceSelectedText()
        {
            if (textEditor != null && textEditor.SelectionLength > 0)
            {
                textEditor.Document.Replace(textEditor.SelectionStart, textEditor.SelectionLength, ReplaceText);
            }
        }

        private void ReplaceAll_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtFind.Text;
            string replaceText = txtReplace.Text;

            StringComparison comparison = MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            string editorText = textEditor.Text;

            string newText = MatchCase
                ? editorText.Replace(searchText, replaceText)
                : Regex.Replace(editorText, searchText, replaceText, RegexOptions.IgnoreCase);

            textEditor.Document.Text = newText;

            totalMatches = 0;
            currentMatchIndex = 0;
            UpdateMatchLabels();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            SearchText = txtFind.Text;
            ReplaceText = txtReplace.Text;
            MatchCase = chkMatchCase.IsChecked ?? false;
            FindNext = chkFindNext.IsChecked ?? false;

            NextClicked = true;

            UpdateHighlighting();

            if (FindNext && chkFindNext.IsChecked == true)
            {
                ReplaceTextInTextEditor();
            }
        }

        private void ReplaceTextInTextEditor()
        {
            if (textEditor != null)
            {
                int selectionStart = textEditor.SelectionStart;
                int selectionLength = textEditor.SelectionLength;

                if (selectionLength > 0)
                {
                    textEditor.Document.Replace(selectionStart, selectionLength, ReplaceText);

                    // Move the cursor to the end of the replaced text
                    textEditor.Select(selectionStart + ReplaceText.Length, 0);
                }
            }
        }

        private bool FindAndReplaceNext()
        {
            if (textEditor != null)
            {
                string searchText = SearchText;
                StringComparison comparison = MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                currentIndex = textEditor.Text.IndexOf(searchText, currentIndex + 1, comparison);

                if (currentIndex != -1)
                {
                    textEditor.Select(currentIndex, searchText.Length);
                    textEditor.ScrollToLine(textEditor.Document.GetLineByOffset(currentIndex).LineNumber);

                    ReplaceTextInTextEditor();

                    return true; //Found and replaced, continue searching
                }
                else
                {
                    MessageBox.Show("No more matches found", "Replace");
                    currentIndex = -1; // Reset for the next search
                    return false; //No more matches found, stop searching
                }
            }

            return false;
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            SearchText = txtFind.Text;
            ReplaceText = txtReplace.Text;
            MatchCase = chkMatchCase.IsChecked ?? false;

            PreviousClicked = true;

            UpdateHighlighting();
        }

        /*
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Delay setting DialogResult until the window is loaded
            DialogResult = NextClicked; // Set DialogResult based on whether "Next" button was clicked
        }
        */

        public bool FindAndHighlight()
        {
            ShowDialog();

            return NextClicked;
        }

        private void UpdateHighlighting()
        {
            if (textEditor != null)
            {
                HighlightMatches();
            }
        }

        private void HighlightMatches()
        {
            if (textEditor != null)
            {
                textEditor.TextArea.TextView.LineTransformers.Clear();

                if (NextClicked)
                {
                    string searchText = SearchText;
                    StringComparison comparison = MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                    currentIndex = textEditor.Text.IndexOf(searchText, currentIndex + 1, comparison);

                    // Ako nije nađen par vrati se na početak
                    if (currentIndex == -1)
                    {
                        currentIndex = textEditor.Text.IndexOf(searchText, 0, comparison);
                        currentMatchIndex = 0;
                        UpdateMatchLabels();
                    }

                    if (currentIndex != -1)
                    {
                        textEditor.Select(currentIndex, searchText.Length);
                        textEditor.ScrollToLine(textEditor.Document.GetLineByOffset(currentIndex).LineNumber);

                        // Update countera
                        totalMatches = CountMatches(searchText, CircularSearch);
                        currentMatchIndex++;

                        UpdateMatchLabels();
                    }
                    else
                    {
                        MessageBox.Show("No more matches found", "Find");
                        currentIndex = -1; // Reset za sljedeće traženje
                    }

                    NextClicked = false; // Reset za sljedeću iteraciju
                }
                else if (PreviousClicked)
                {
                    string searchText = SearchText;
                    StringComparison comparison = MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                    currentIndex = textEditor.Text.LastIndexOf(searchText, Math.Max(0, currentIndex - 1), comparison);

                    // Ako nije nađen par vrati se na početak
                    if (currentIndex == -1)
                    {
                        // Update countera
                        currentIndex = textEditor.Text.LastIndexOf(searchText, textEditor.Text.Length - 1, comparison);
                        currentMatchIndex = CountMatches(searchText, CircularSearch) + 1;
                        UpdateMatchLabels();
                    }

                    if (currentIndex != -1)
                    {
                        textEditor.Select(currentIndex, searchText.Length);
                        textEditor.ScrollToLine(textEditor.Document.GetLineByOffset(currentIndex).LineNumber);

                        // Update countera
                        totalMatches = CountMatches(searchText, CircularSearch);
                        currentMatchIndex--;

                        UpdateMatchLabels();
                    }
                    else
                    {
                        MessageBox.Show("No more matches found", "Find");
                        currentIndex = textEditor.Text.Length; // Reset za sljedeće traženje
                    }

                    PreviousClicked = false; // Reset za sljedeću iteraciju
                }
            }
        }

        private void UpdateMatchLabels()
        {
            // Update totalMatches i currentMatchIndex
            lblTotalMatches.Content = totalMatches.ToString();
            lblCurrentMatch.Content = currentMatchIndex.ToString();
        }

        private int CountMatches(string searchText, bool circularSearch)
        {
            int count = 0;
            int startPosition = 0;

            StringComparison comparison = MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            while (startPosition < textEditor.Text.Length)
            {
                int index = textEditor.Text.IndexOf(searchText, startPosition, comparison);

                if (index == -1)
                {
                    if (circularSearch)
                    {
                        // Ukoliko je cirkularno traženje omogućemo kreni ispočetka
                        index = textEditor.Text.IndexOf(searchText, 0, comparison);
                        count = 0;
                    }
                    else
                    {
                        // Nema više parova
                        break;
                    }
                }

                startPosition = index + searchText.Length;
                count++;

                if (circularSearch && startPosition >= textEditor.Text.Length)
                {
                    // Restart početne pozicije ako je cirkularno traženje omogućemo
                    startPosition = 0;
                }
            }

            return count;
        }

    }
}
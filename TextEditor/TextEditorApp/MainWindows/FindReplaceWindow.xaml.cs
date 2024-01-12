using ICSharpCode.AvalonEdit;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.ComponentModel;
using System.Diagnostics;

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
        //public bool CircularSearch { get; private set; }
        private int totalMatches = 0;
        private int currentMatchIndex = 0;
        private bool countingMatches = false;
        private bool replaced;

        public bool CircularSearch
        {
            get { return chkCircularSearch.IsChecked ?? false; }
            private set
            {
                chkCircularSearch.IsChecked = value;
                OnPropertyChanged(nameof(CircularSearch));
            }
        }

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
            replaced = true;

            if (FindNext)
            {
                //--currentMatchIndex;
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

                    // Pomak kursora na kraj zamijenjenog texta
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

                    return true; // Pronađen match, zamijenjen i nastavlja se traženje
                }
                else
                {
                    MessageBox.Show("No more matches found", "Replace");
                    currentIndex = -1; // Reset za sljedeće traženje
                    return false; // Match nije nađen, prekid traženja
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Delay setting DialogResult until the window is loaded
            DialogResult = NextClicked; // Set DialogResult based on whether "Next" button was clicked
        }

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
                    if(replaced == true)
                    {
                        currentMatchIndex--;
                        replaced = false;
                    }

                    // Ako nije nađen par vrati se na početak
                    if (currentIndex == -1 && CircularSearch)
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
                        totalMatches = CountTotalMatches(searchText);
                        currentMatchIndex++;

                        UpdateMatchLabels();
                    }
                    else
                    {
                        MessageBox.Show("No more matches found", "Find");
                        currentMatchIndex = 0;
                        currentIndex = 0; // Reset za sljedeće traženje
                    }

                    NextClicked = false; // Reset za sljedeću iteraciju
                }
                else if (PreviousClicked)
                {
                    string searchText = SearchText;
                    StringComparison comparison = MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                    currentIndex = textEditor.Text.LastIndexOf(searchText, Math.Max(0, currentIndex - 1), comparison);

                    // Ako nije nađen par vrati se na početak
                    if (currentIndex == -1 && CircularSearch)
                    {
                        // Update countera
                        currentIndex = textEditor.Text.LastIndexOf(searchText, textEditor.Text.Length, comparison);
                        currentMatchIndex = CountTotalMatches(searchText) + 1;
                        UpdateMatchLabels();
                    }

                    if (currentIndex != -1)
                    {
                        textEditor.Select(currentIndex, searchText.Length);
                        textEditor.ScrollToLine(textEditor.Document.GetLineByOffset(currentIndex).LineNumber);

                        // Update countera
                        totalMatches = CountTotalMatches(searchText);
                        currentMatchIndex --;

                        UpdateMatchLabels();
                    }
                    else
                    {
                        MessageBox.Show("No more matches found", "Find");
                        CurrentMatchIndex = CountTotalMatches(searchText) + 1;
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
                Debug.WriteLine("Circular Search while");

                if (index == -1)
                {
                    if (circularSearch)
                    {
                        Debug.WriteLine("Circular Search if");

                        // If circular search is enabled, restart from the beginning
                        index = textEditor.Text.IndexOf(searchText, 0, comparison);

                        if (index >= 0 && index < startPosition)
                        {
                            // If the circular search restarts, update the counter
                            count++;
                            startPosition = index + searchText.Length; // Move the start position
                            continue; // Continue to the next iteration without incrementing count
                        }
                        else
                        {
                            // No more matches found
                            break;
                        }
                    }
                    else
                    {
                        // No more matches found
                        break;
                    }
                }
                else
                {
                    startPosition = index + searchText.Length;
                    count++;

                    if (circularSearch && startPosition >= textEditor.Text.Length)
                    {
                        // Restart from the beginning if circular search is enabled
                        startPosition = 0;
                    }
                    break;
                }
            }

            return count;
        }

        private int CountTotalMatches(string searchText)
        {
            int count = 0;
            int startPosition = 0;

            StringComparison comparison = MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            while (startPosition < textEditor.Text.Length)
            {
                int index = textEditor.Text.IndexOf(searchText, startPosition, comparison);

                if (index == -1)
                {
                    // Nema više parova
                    break;
                }

                startPosition = index + searchText.Length;
                count++;
            }

            return count;
        }

        // Circular Search uključeno/isključeno
        private void chkCircularSearch_Checked(object sender, RoutedEventArgs e)
        {
            CircularSearch = true;
        }

        private void chkCircularSearch_Unchecked(object sender, RoutedEventArgs e)
        {
            CircularSearch = false;
        }
    }
}
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
        public bool ReplaceNext { get; private set; }
        public bool CircularSearch { get; private set; }
        private int totalMatches = 0;
        private int currentMatchIndex = 0;
        private bool replaced;

        private readonly TextEditor textEditor;
        private int currentIndex = -1;

        public FindReplaceWindow(TextEditor textEditor)
        {
            InitializeComponent();
            this.textEditor = textEditor;
            UpdateHighlighting();
        }

        // Circular Search označeno
        private void ChkCircularSearch_Checked(object sender, RoutedEventArgs e)
        {
            CircularSearch = true;
        }

        // Circular Search ne označeno
        private void ChkCircularSearch_Unchecked(object sender, RoutedEventArgs e)
        {
            CircularSearch = false;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            SearchText = txtFind.Text;
            ReplaceText = txtReplace.Text;
            MatchCase = chkMatchCase.IsChecked ?? false;
            ReplaceNext = chkReplaceNext.IsChecked ?? false;

            NextClicked = true;

            HighlightMatches();
        }

        private void ExecuteNext()
        {
            string searchText = SearchText;
            StringComparison comparison = MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            currentIndex = textEditor.Text.IndexOf(searchText, currentIndex + 1, comparison);

            // Ako nije nađen match vrati se na početak
            if (currentIndex == -1 && CircularSearch)
            {
                if (replaced == true)
                {
                    currentMatchIndex--;
                    replaced = false;
                }

                currentIndex = textEditor.Text.IndexOf(searchText, 0, comparison);
                currentMatchIndex = 0;
                UpdateMatchLabels();
            }

            // Nismo došli do kraja, nastavi tražiti
            if (currentIndex != -1)
            {
                if (replaced == true)
                {
                    currentMatchIndex--;
                    replaced = false;
                }

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
                currentIndex = -1; // Reset za sljedeće traženje
            }

            NextClicked = false; // Reset za sljedeću iteraciju
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            SearchText = txtFind.Text;
            ReplaceText = txtReplace.Text;
            MatchCase = chkMatchCase.IsChecked ?? false;

            PreviousClicked = true;

            HighlightMatches();
        }

        private void ExecutePrevious()
        {
            string searchText = SearchText;
            StringComparison comparison = MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            replaced = false;
            currentIndex = textEditor.Text.LastIndexOf(searchText, Math.Max(0, currentIndex - 1), comparison);

            // Ako nije nađen par vrati se na početak
            if (currentIndex == -1 && CircularSearch)
            {
                // Update countera
                currentIndex = textEditor.Text.LastIndexOf(searchText, textEditor.Text.Length, comparison);
                currentMatchIndex = CountTotalMatches(searchText) + 1;
                UpdateMatchLabels();
            }

            // Nismo došli do kraja, nastavi tražiti
            if (currentIndex != -1)
            {
                textEditor.Select(currentIndex, searchText.Length);
                textEditor.ScrollToLine(textEditor.Document.GetLineByOffset(currentIndex).LineNumber);

                // Update countera
                totalMatches = CountTotalMatches(searchText);
                currentMatchIndex--;

                UpdateMatchLabels();
            }
            else
            {
                MessageBox.Show("No more matches found", "Find");
                currentMatchIndex = CountTotalMatches(searchText) + 1;
                currentIndex = textEditor.Text.Length; // Reset za sljedeće traženje
            }

            PreviousClicked = false; // Reset za sljedeću iteraciju
        }

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            SearchText = txtFind.Text;
            ReplaceText = txtReplace.Text;
            MatchCase = chkMatchCase.IsChecked ?? false;
            ReplaceNext = chkReplaceNext.IsChecked ?? false;
            replaced = true;
    
            // Ako je Replace Next checkbox označen zamijeni sljedeći match
            // Ako ne zamijeni označeni text
            if (ReplaceNext)
            {
                replaced = FindAndReplaceNext();
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
            string editorText = textEditor.Text;

            // Zamijeni sve matcheve
            string newText = MatchCase
                ? editorText.Replace(searchText, replaceText)
                : Regex.Replace(editorText, searchText, replaceText, RegexOptions.IgnoreCase);

            textEditor.Document.Text = newText;

            // Reset countera za ukupan broj matcheva
            totalMatches = 0;
            currentMatchIndex = 0;
            UpdateMatchLabels();
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

                // Ako smo došli do kraja kreni ispočetka tražiti
                if(currentIndex == -1 && CircularSearch)
                {
                    currentIndex = textEditor.Text.IndexOf(searchText, 0, comparison);
                    currentMatchIndex = 0;
                    if(currentIndex == -1)
                    {
                        MessageBox.Show("No more matches found", "Replace");
                        return false;
                    }
                    textEditor.Select(currentIndex, searchText.Length);
                    textEditor.ScrollToLine(textEditor.Document.GetLineByOffset(currentIndex).LineNumber);

                    ReplaceTextInTextEditor();

                    totalMatches--;

                    return true; // Pronađen match, zamijenjen i nastavlja se traženje
                }
                
                // Zamjena ako nismo došli do kraja dokumenta
                if (currentIndex != -1)
                {
                    textEditor.Select(currentIndex, searchText.Length);
                    textEditor.ScrollToLine(textEditor.Document.GetLineByOffset(currentIndex).LineNumber);

                    ReplaceTextInTextEditor();

                    totalMatches--;

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

        private void UpdateHighlighting()
        {
            if (textEditor != null)
            {
                HighlightMatches();
            }
        }

        public bool FindAndHighlight()
        {
            ShowDialog();

            return NextClicked;
        }

        private void HighlightMatches()
        {
            if (textEditor != null)
            {
                textEditor.TextArea.TextView.LineTransformers.Clear();

                if (NextClicked)
                {
                    ExecuteNext();
                }
                else if (PreviousClicked)
                {
                    ExecutePrevious();
                }
            }
        }

        private void UpdateMatchLabels()
        {
            // Update totalMatches i currentMatchIndex
            lblTotalMatches.Content = totalMatches.ToString();
            lblCurrentMatch.Content = currentMatchIndex.ToString();
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
    }
}
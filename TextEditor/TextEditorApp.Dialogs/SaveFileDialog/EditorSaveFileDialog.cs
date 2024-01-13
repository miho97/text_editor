using Microsoft.Win32;



namespace TextEditorApp.Dialogs.EditorSaveFileDialog
{
    public class EditorSaveFileDialog
    {
        public void ShowDialog()
        {

            var saveFileDialog = new SaveFileDialog();
            //dlg.FileName = "Document"; // Default file name
            //dlg.DefaultExt = ".txt"; // Default file extension
            //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            //return saveFileDialog.ShowDialog() switch
            //{
            //    true => OnTrueResult(saveFileDialog),
            //    false => OnFalseResult(),
            //    _ => throw new SaveFileDialogUnknownResultTypeException("An unknown error occurred while reading the result of the dialog box!")
            //};
        }

    }
}

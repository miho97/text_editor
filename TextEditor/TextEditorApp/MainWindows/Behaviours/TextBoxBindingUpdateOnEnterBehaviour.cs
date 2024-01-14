using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows.Input;

namespace TextEditorApp.MainWindows.Behaviours
{
    /// <summary>
    /// Behavior class that updates the source of a TextBox binding when the Enter key is pressed.
    /// </summary> 

    // In a nutshell we are using this class to handle the newly entered web adress for a web browser
    public class TextBoxBindingUpdateOnEnterBehaviour : Behavior<TextBox>
    {

        /// <summary>
        /// Attaches the behavior to the TextBox by subscribing to the KeyDown event.
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.KeyDown += OnTextBoxKeyDown;
        }


        /// <summary>
        /// Detaches the behavior by unsubscribing from the KeyDown event.
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.KeyDown -= OnTextBoxKeyDown;
        }

        /// <summary>
        /// Handles the KeyDown event and updates the source of the TextBox binding when the Enter key is pressed.
        /// </summary>
        /// <param name="sender">The TextBox triggering the event.</param>
        /// <param name="e">The KeyEventArgs containing information about the key press.</param>

        // we only want to update the web adress if enter button is pressed
        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var txtBox = sender as TextBox;
                txtBox?.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }
    }
}

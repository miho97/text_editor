using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Documents;

namespace TextEditorApp.Controls.CodeCompletion
{ 

    /// <summary>
    /// Represents a custom adorner that displays a ComboBox for completion suggestions.
    /// </summary>


// since combobox used for code suggestions must render over the currently active text box we decided to use Adorner
// this is a custom implementation od an Adorner that will display completion suggestions
    public class CustomCompletionAdorner : Adorner
    {
        public ComboBox comboBox;
        public Point _pos;


        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCompletionAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The UI element to be adorned.</param>
        /// <param name="pos">The position where the adorner should be displayed. The top left corner.</param>
        /// <param name="combo">The ComboBox used for displaying completion suggestions.</param>
        public CustomCompletionAdorner(UIElement adornedElement, Point pos, ComboBox combo) : base(adornedElement)
        {
            comboBox = combo;
            comboBox.Background = Brushes.Black;
            comboBox.BorderBrush = Brushes.Black;

            _pos = pos;

            AddVisualChild(comboBox);
            AddLogicalChild(comboBox);
        }


        /// <summary>
        /// Gets or sets the ComboBox used for displaying completion suggestions.
        /// </summary>
        public ComboBox Combo
        {
            get { return comboBox; }
            set
            {
                if(comboBox != value)
                {
                    comboBox = value;
                }
            }
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            comboBox.IsDropDownOpen = true;
        }
        protected override Visual GetVisualChild(int index)
        {
            return comboBox;
        }

        protected override int VisualChildrenCount => 1;

        protected override Size MeasureOverride(Size constraint)
        {
            comboBox.Measure(constraint);
            return comboBox.DesiredSize;
        }

        // actually we want the combobox to be the adorner so we are setting it at the top left corner of the adorner
        protected override Size ArrangeOverride(Size finalSize)
        {
            comboBox.Arrange(new Rect(_pos.X, _pos.Y, 100, 0));
            return base.ArrangeOverride(finalSize);
        }
    }
}

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Documents;

namespace TextEditorApp.Dialogs.CodeCompetionDialog
{
    public class CustomCompletionAdorner : Adorner
    {
        public ComboBox comboBox;
        public Point _pos;

        public CustomCompletionAdorner(UIElement adornedElement, Point pos, ComboBox combo) : base(adornedElement)
        {
            comboBox = combo;
            comboBox.Background = Brushes.Black;
            comboBox.BorderBrush = Brushes.Black;

            _pos = pos;

            AddVisualChild(comboBox);
            AddLogicalChild(comboBox);
        }

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

            // Prikazi samo ComboBox na mjestu _pos
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

        protected override Size ArrangeOverride(Size finalSize)
        {
            comboBox.Arrange(new Rect(_pos.X, _pos.Y, 100, 0));
            return base.ArrangeOverride(finalSize);
        }
    }
}

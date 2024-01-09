using ICSharpCode.AvalonEdit;
using RoslynPad.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TextEditorApp.Common;
using TextEditorApp.Common.Enums;
using TextEditorApp.Utils.DocumentFiles;
using TextEditorApp.Utils.StaticModels;
using System.Windows;
using Microsoft.CodeAnalysis;
using RoslynPad.Roslyn;
using System.IO;
using System.Reflection;

namespace TextEditorApp.Controls.ControlsModels
{
    public class CustomTextEditorModel : CustomTextEditorBaseModel
    {
        private DocumentFiles_Model _document;

        public CustomTextEditorModel() : base()
        {
            _document = new DocumentFiles_Model();
            base.ShowLineNumbers = false;
            base.IsReadOnly = false;
            base.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            base.TextChanged += (sender, args) =>
            {
                DocumentModel.IsSaved = false;
            };
        }

        private CustomHorizontalTextAlignment _textAlignment = CustomHorizontalTextAlignment.Left;

        public CustomHorizontalTextAlignment TextAlignment
        {
            get => _textAlignment;
            set
            {
                _textAlignment = value;
                base.TextArea.HorizontalAlignment =  ((HorizontalAlignment)Enum.ToObject(typeof(HorizontalAlignment), (int)value));
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
    }
}

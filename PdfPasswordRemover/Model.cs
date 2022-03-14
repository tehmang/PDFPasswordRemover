using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Drawing;
using System.Windows.Media;

namespace PdfPasswordRemover
{
    public class Model: INotifyPropertyChanged
    {
        public Model(string[] arguments)
        {
            if(arguments.Length == 1)
            {
                PdfPath = arguments[0];
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            RaisePropertyChanged(propertyName);
        }
        protected void RaisePropertyChanged(string? propertyName)
        { 
            if(!string.IsNullOrEmpty(propertyName))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged

        private string pdfPath = string.Empty;
        public string PdfPath { 
            get { return pdfPath; } 
            set 
            { 
                pdfPath = value; 
                OnPropertyChanged();
                UnprotectedPdfPath = value;
                RaisePropertyChanged("ShowControls"); 
            } 
        }
        private string unprotectedPdfPath = string.Empty;
        public string UnprotectedPdfPath
        {
            get { return unprotectedPdfPath; }
            set 
            {
                string ext = Path.GetExtension(PdfPath);
                unprotectedPdfPath = value.Replace(ext, ReplacePdfFile ? ext : $".u{ext}"); 
                OnPropertyChanged(); 
            }
        }
        bool replacePdfFile = false;
        public bool ReplacePdfFile
        {
            get { return replacePdfFile; }
            set 
            { 
                replacePdfFile = value; 
                OnPropertyChanged();
                UnprotectedPdfPath =  PdfPath;
            }
        }
        bool showControls = false;
        public bool ShowControls
        {
            get => showControls;
            set
            {
                showControls = value;
                OnPropertyChanged();
            }
        }

        System.Windows.Media.SolidColorBrush backgroundColor = Brushes.White;
        public System.Windows.Media.SolidColorBrush BackgroundColor 
        { 
            get => backgroundColor; 
            set
            {
                backgroundColor = value;
                OnPropertyChanged();
            }
        }

    }
}

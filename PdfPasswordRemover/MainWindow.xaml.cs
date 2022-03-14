using Microsoft.Win32;
using PdfSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PdfPasswordRemover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model model;
        public MainWindow(Model model)
        {
            InitializeComponent();
            this.DataContext = model;
            this.model = model;
            if(!string.IsNullOrEmpty(model.PdfPath))
            {
                InitPdfPath(model.PdfPath);
            }
        }
        private void InitPdfPath(string pdfPath)
        {
            model.PdfPath = pdfPath;
            model.BackgroundColor = Brushes.White;
            if (!string.IsNullOrEmpty(PsswdBox.Password))
            {
                PsswdBox.Clear();
            }
            model.ReplacePdfFile = false;
            model.ShowControls = true;
        }
        private void DropZone_DragEnter(object sender, DragEventArgs e)
        {
            model.BackgroundColor = Brushes.Bisque;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                e.Effects = files?.Length == 1 ? DragDropEffects.Copy : DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
        private void DropZone_DragLeave(object sender, DragEventArgs e)
        {
            model.BackgroundColor = Brushes.White;
        }
        private void DropZone_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            InitPdfPath(files[0]);
        }
        private void buttonRemovePassword_Click(object sender, RoutedEventArgs e)
        {
            string password = PsswdBox.Password;
            if(PasswordExtensions.RemovePassword(model.PdfPath, password, model.UnprotectedPdfPath))
            {
                Process.Start("explorer", $"/Select,\"{model.UnprotectedPdfPath}\"");
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string? path = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty)?.ToString();
            openFileDialog.InitialDirectory = path ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); 
            openFileDialog.Filter = "Fichiers PDF|*.pdf";
            if (openFileDialog.ShowDialog() == true)
            {
                InitPdfPath(openFileDialog.FileName);
            }
        }
    }
}

using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Security;
using System;
using System.IO;
using System.Windows;

namespace PdfSharp.Extensions
{
    public static class PasswordExtensions
    {
        static public bool RemovePassword(string path, string password, string unprotectedPath)
        {
            try
            {
                int nTries = 0;
                PdfDocument document = PdfReader.Open(path, PdfDocumentOpenMode.ReadOnly, (PdfPasswordProviderArgs pppa) =>
                {
                    if (nTries++ == 0)
                    {
                        pppa.Password = password;
                    }
                    else throw new InvalidPasswordException();
                });
                if(document != null)
                {
                    document.SecuritySettings.DocumentSecurityLevel = PdfDocumentSecurityLevel.None;
                    // force loading of pages (sometimes Pages collection is empty)
                    foreach (PdfPage page in document.Pages) { }
                }
                document.Save(unprotectedPath);
                File.Move(unprotectedPath, Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(unprotectedPath)));
                return true;
            }
            catch(InvalidPasswordException)
            {
                MessageBox.Show("Mot de passe invalide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}

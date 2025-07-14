using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Final_Interview
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Helpers.Controltime(ClockTxt);
        }

        private bool _nameAccepted = false;
        private const string InvalidNameMsg = "Please enter two up to ten letters (A–Z) only.";
        private const string ImageFilter = "Images only (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
        private const string NamePrompt = "Enter your name please";
        private const string NameRequiredMsg = "Please enter your name first in order to upload a file.";
        private const string NoFilePickedMsg = "Error: No file was picked.";
        private const string InvalidImageMsg = "Error: That file must be a .jpg/.jpeg/.png/.bmp and its contents must match the format.";
        private const string unexpected_error = "An error occurred while processing the image:\n";
        private const string is_file_null= "Path cannot be null or empty.";


        private void NameBox_GotFocus(object _, RoutedEventArgs __)// theres "focus" on object. - user wants to write in text block
        {
            if (NameBox.Text == "Enter your name please")
            {
                NameBox.Text = "";
                NameBox.Foreground = Brushes.Black;
            }
        }

        private void NameBox_LostFocus(object _, RoutedEventArgs __)// if object got foucs- user wanta to write- but then for instance clicked off
        {
            NameBox.Text = NamePrompt;
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                NameBox.Text = "Enter your name please";
                NameBox.Foreground = (Brush)new BrushConverter().ConvertFrom("#808080"); // convert to brush
            }
        }
        private void NameBox_KeyDown(object _, KeyEventArgs e)// user wrote in the textblock- add name to row 0 text
        {
            if (e.Key != Key.Enter) return;

            if (Helpers.IsValidName(NameBox.Text))
            {
                // valid: accept name
                welcome_txt.Text = $"Welcome {NameBox.Text}, to NSA's BGR Converter";
                NameBox.Visibility = Visibility.Collapsed;
                _nameAccepted = true;
            }
            else
            {
                // invalid: show feedback
                MessageBox.Show(InvalidNameMsg, "Invalid Name", MessageBoxButton.OK, MessageBoxImage.Warning);
                NameBox.SelectAll();            // highlight for easy re-entry
            }
            e.Handled = true;
        }



        private void btUPload_Click(object _, RoutedEventArgs e)// exacute when upload file is pressed
        {

            if (!_nameAccepted)// write name first
            {
                MessageBox.Show(NameRequiredMsg, "Name required", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dlg = new OpenFileDialog { Filter = ImageFilter };// filter to only image so the user knows
            if (dlg.ShowDialog() != true)// was file actually picked?
            {
                MessageBox.Show(NoFilePickedMsg, "File Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string path = dlg.FileName;
            string ext = Path.GetExtension(path).ToLowerInvariant();
            if (!Helpers.IsValidImageExtension(ext) || !Helpers.IsImageFileMagic(path))// requiremnts
            {
                MessageBox.Show(InvalidImageMsg, "Invalid Image File", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ProcessImage(path);// files ok, time to convert to bitmap and display
        }

        private void ProcessImage(string path)
        {
            // Check if path is null or empty BEFORE using it or creating backup
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show(
                    is_file_null, "Invalid Path",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Stop further processing if the input is invalid
            }

            try
            {
                // Create a backup so we don't ruin the original file
                string backupPath = CreateBackup(path);

                // Display the backup image, not the original
                DisplayOriginalImage(backupPath);

                Helpers.ShowBgrChannels(backupPath, RGBblue, RGBgreen, RGBred);
                Helpers.TurnVS(Ocaption, Bcaption, Gcaption, Rcaption);
            }
            catch (Exception ex) // something unpredictable happened
            {
                MessageBox.Show(
                    unexpected_error + ex.Message, "Processing Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // This function creates a backup and returns the backup path
        private string CreateBackup(string path)
        {
            // Build the backup filename: original name + "_backup" + original extension
            string backup = Path.ChangeExtension(path, null) + "_backup" + Path.GetExtension(path);
            if (!File.Exists(backup))
                File.Copy(path, backup); // creates the backup file if it doesn't exist yet
            return backup; // return the backup path to use instead of the original
        }

        private void DisplayOriginalImage(string path)
        {
            originalPic.Source = new BitmapImage(new Uri(path, UriKind.Absolute));//  make source of pic bit map image type
        }
    }
}

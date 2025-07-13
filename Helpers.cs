using Emgu.CV;
using Emgu.CV.Util; // oc bgr
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls; // for buttons image ect
using System.Windows.Media.Imaging;
using System.Windows.Threading; // for clock

namespace Final_Interview
{
    public static class Helpers
    {
        private const int byte_Size = 12; // number of bytes we read for magic-number ( 12 and not 8 to match all the types of images)
        private const string BmpEncodeFormat = ".bmp";// format string passed to Imencode
        private const string TimeFormat = "yyyy-MM-dd  HH:mm:ss";
        public static bool IsValidImageExtension(string fileExtension)
        {
            return fileExtension is ".jpg" or ".jpeg" or ".png" or ".bmp";
        }
        public static bool IsNameLongEnough(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return (name.Length >= 2 && name.Length <= 10);
        }
        public static bool IsLettersOnly(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            foreach (char c in name)
                if (!Char.IsLetter(c)) return false;
            return true;
        }
        public static bool IsValidName(string name)
        {
            if (!IsNameLongEnough(name))
                return false;

            if (!IsLettersOnly(name))
                return false;

            return true;
        }
        public static BitmapImage MatToBitmap(Mat mat) // recieves mat obj, returns converted image
        {
            using var temp = new VectorOfByte();// array of byte type
            CvInvoke.Imencode(BmpEncodeFormat, mat, temp); // bmp cause its faster, invoke bridge between c# and low level
            using var memory = new MemoryStream(temp.ToArray());// temporary memory stream
            BitmapImage bm = new BitmapImage();// create bit image object
            bm.BeginInit(); // touch stream
            bm.CacheOption = BitmapCacheOption.OnLoad;
            bm.StreamSource = memory;
            bm.EndInit();
            bm.Freeze(); // done, no changes to bmp
            return bm;
        }
        public static void ShowBgrChannels(string path, Image blueImg, Image greenImg, Image redImg)// split the original pic to the bgr types
        {
            using Mat img = CvInvoke.Imread(path, (Emgu.CV.CvEnum.ImreadModes)1); // 1 is like colors. 
            using var split = new VectorOfMat(); // array of mats type
            CvInvoke.Split(img, split); // [0] B, [1] G, [2] R
            blueImg.Source = MatToBitmap(split[0]); // telling wpf source what to paint on screen  
            greenImg.Source = MatToBitmap(split[1]);
            redImg.Source = MatToBitmap(split[2]);
        }
        public static bool IsImageFileMagic(string path)// there are files that are images but dont have an ending of one- so saf check with magic number
        {
            byte[] header = new byte[byte_Size]; // read the first 12 bytes—enough for all our headers
            using var fs = File.OpenRead(path);
            fs.Read(header, 0, 12);
            //4 nect lines check bytes 
            if (header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF) return true; // JPG / JPEG: FF D8 FF
            if (header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47) return true; // PNG: 89 50 4E 47
            if (header[0] == 0x42 && header[1] == 0x4D) return true; // BMP: 42 4D ('B' 'M')
            if (header[0] == 0x00 && header[1] == 0x00 && header[2] == 0x01 && header[3] == 0x00) return true; // OPTIONAL: ICO: 00 00 01 00
            return false;
        }
        public static void TurnVS(TextBlock Ot, TextBlock Bt, TextBlock Gt, TextBlock Rt) // recieves text buttons- makes them visible
        {
            Ot.Visibility = Visibility.Visible;
            Bt.Visibility = Visibility.Visible;
            Gt.Visibility = Visibility.Visible;
            Rt.Visibility = Visibility.Visible;
        }
        public static void Controltime(TextBlock clock) // useable in mainwindow.
        {
            clock.Text = DateTime.Now.ToString(TimeFormat); // show the current time before the updating starts
            var t = new DispatcherTimer();// threads (not raw threads)
            t.Tag = clock;// make the text block which displays the clock- A tag.
            t.Interval = TimeSpan.FromSeconds(1);// every second fire tick and the methoed thats subscribed to it
            t.Tick += new EventHandler(updateClock); // "subscribes" t to the sender object in updateclock so we could mention it in the second line in method
            t.Start();
        }
        private static void updateClock(object? sender, EventArgs e) // use for helpers only- for controltime.
        {
            var t = (DispatcherTimer)sender;// thread. (not raw thread)
            TextBlock clock = (TextBlock)t.Tag; // make dispatcher a tag to show time- reuseable
            clock.Text = DateTime.Now.ToString(TimeFormat);// convert at the end for a readable string for user w- date and hour.
        }
    }
}

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Globalization;
using System.Windows.Data;
using TagLib;
using TagLib.Ape;
using System.IO;
using System.Diagnostics;

namespace WMP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {


            InitializeComponent();
            
           
        }
        
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (buttonPlay.Background.ToString() != "#FF000000")
            {
                buttonPlay.Background = new SolidColorBrush(Colors.Black);
                mediaElement.Pause();
            }
            else 
            {
                buttonPlay.Background = new SolidColorBrush(Colors.White);
                mediaElement.Play();
            }

        }

        DispatcherTimer timer = null;
        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            sliderPosition.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            //媒体文件打开成功
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();
        }
        private void timer_tick(object sender, EventArgs e)
        {
            sliderPosition.Value = mediaElement.Position.TotalSeconds;
        }
        //控制视频的位置
        private void sliderPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //mediaElement.Stop();
            mediaElement.Position = TimeSpan.FromSeconds(sliderPosition.Value);
            //mediaElement.Play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = ""; // Default file name
            dialog.DefaultExt = ""; // Default file extension
            dialog.Filter = "audio forms|*.mp3;*.flac;*.wav;*.opus;*.m4a"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                MessageBox.Show("打开文件: " + filename);
                mediaElement.Source = new Uri(filename);
                
                var tfile = TagLib.File.Create(filename);
                string title = tfile.Tag.Title;
                //将封面图片显示在界面上
                var pic = tfile.Tag.Pictures[0];
                MemoryStream ms = new MemoryStream(pic.Data.Data);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                MusicImage.Source = bitmap;
                //显示歌曲信息
                MusicTitle.Text = title;
            }
        }
    }
}
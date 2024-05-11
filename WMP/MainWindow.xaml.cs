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
            if (button.Content.ToString() == "播放")
            {
                button.Content = "暂停";
                mediaElement.Play();
            }
            else
            {
                button.Content = "播放";
                mediaElement.Pause();
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
        

    }
}
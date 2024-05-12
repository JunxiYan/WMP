using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WMP
{
    class ViewModel : INotifyPropertyChanged
    {
        MediaElement MusicMedia;
        DispatcherTimer timer = null;
        bool betrue() { return true; }
        string StopColor = "#FF000000";
        string PlayColor = "#FFFFFFFF";
        public string CurrentColor;

        private string _MusicVolume = "0.5";
        public string MusicVolume
        {
            get { return _MusicVolume; }
            set
            {
                _MusicVolume = value;
                RaisePropertyChanged("MusicVolume");
            }
        }

        private string _MusicTitle = "";
        public string MusicTitle
        {
            get { return _MusicTitle; }
            set
            {
                _MusicTitle = value;
                RaisePropertyChanged("MusicTitle");
            }
        }

        private double _MusicDuration;
        public double MusicDuration
        {
            get { return _MusicDuration; }
            set
            {
                _MusicDuration = value;
                RaisePropertyChanged("MusicDuration");
            }
        }

        private double _MusicPosition = 0;
        public double MusicPosition
        {
            get { return _MusicPosition; }
            set
            {
                _MusicPosition = value;
                RaisePropertyChanged("MusicPosition");
            }
        }

        private BitmapImage _MusicImage;
        public BitmapImage MusicImage
        {
            get { return _MusicImage; }
            set
            {
                _MusicImage = value;
                RaisePropertyChanged("MusicImage");
            }
        }


        public ViewModel()
        {
            MusicMedia = new MediaElement();
            MusicMedia.LoadedBehavior = MediaState.Manual;
            MusicMedia.UnloadedBehavior = MediaState.Manual;
            //MusicMedia.MediaOpened += MusicMedia_MediaOpened;
            //MusicMedia.MediaEnded += MusicMedia_MediaEnded;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        async Task openMediaAsync()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "";
            dialog.DefaultExt = "";
            dialog.Filter = "audio forms|*.mp3;*.flac;*.wav;*.opus;*.m4a";
            // Show open file dialog box
            bool? result = dialog.ShowDialog();
            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;

                MusicMedia.Source = new Uri(filename);


                var tfile = await Task.Run(() => TagLib.File.Create(filename));
                string title = tfile.Tag.Title;
                _MusicTitle = title;
                MusicTitle = _MusicTitle;
                //将封面图片显示在界面上
                var pic = tfile.Tag.Pictures[0];
                MemoryStream ms = new MemoryStream(pic.Data.Data);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                MusicImage = bitmap;



                MusicMedia.MediaOpened += (s, e) =>
                {
                    _MusicDuration = MusicMedia.NaturalDuration.TimeSpan.TotalSeconds;
                    MusicDuration = _MusicDuration;
                    _MusicPosition = MusicMedia.Position.TotalSeconds;
                    MusicPosition = _MusicPosition;

                };
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(0.5);
                timer.Tick += new EventHandler(timer_tick);
                timer.Start();

            }
        }
        private void timer_tick(object sender, EventArgs e)
        {

            
            _MusicPosition = MusicMedia.Position.TotalSeconds;
            MusicPosition = _MusicPosition;
            MusicMedia.Volume = double.Parse(MusicVolume);
        }

        //控制音量


        void PlayandStop()
        {
            //if (CurrentColor != "#FF000000")
            //{
            //    CurrentColor = StopColor;
            //    MusicMedia.Pause();
            //}
            //else
            //{
            //    CurrentColor = PlayColor;
            //    MusicMedia.Play();
            //}
            MusicMedia.Play();
        }
        void DragStarted()
        {
            timer.Stop();
        }
        void DragCompleted()
        {
            MusicMedia.Position = TimeSpan.FromSeconds(MusicPosition);
            timer.Start();
        }
        public ICommand PlayandStopCommand => new RelayCommand(o => PlayandStop(), o => betrue());
        public ICommand OpenMediaCommand => new RelayCommand(async o => await openMediaAsync(), o => betrue());
        public ICommand DragStartedCommand => new RelayCommand(o => DragStarted(), o => betrue());
        public ICommand DragCompletedCommand => new RelayCommand(o => DragCompleted(), o => betrue());

    }
}

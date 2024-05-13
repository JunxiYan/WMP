using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WMP
{
    class Model : INotifyPropertyChanged
    {
        public Model()
        {
            _dialog = new Microsoft.Win32.OpenFileDialog();
            _MusicMedia = new MediaElement();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _CurrentColor = "#FF000000";
            _MusicVolume = "0.5";
            _MusicTitle = "";
            _MusicDuration = 0;
            _MusicPosition = 0;
            _MusicImage = new BitmapImage();
        }

        private Microsoft.Win32.OpenFileDialog _dialog;

        public Microsoft.Win32.OpenFileDialog dialog
        {
            get { return _dialog; }
            set { _dialog = value; }
        }

        private MediaElement _MusicMedia;

        public MediaElement MusicMedia
        {
            get { return _MusicMedia; }
            set { _MusicMedia = value; }
        }

        private DispatcherTimer _timer;

        public DispatcherTimer timer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public Boolean isPlaying = false;

        public bool betrue() { return true; }

        public string StopColor = "#FF000000";
        public string PlayColor = "#FFFFFFFF";

        private string _MusicTitle;

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

        private string _CurrentColor;

        public string CurrentColor
        {
            get { return _CurrentColor; }
            set
            {
                _CurrentColor = value;
                RaisePropertyChanged("CurrentColor");
            }
        }

        private string _CurrentVolume;

        public string CurrentVolume
        {
            get { return _CurrentVolume; }
            set
            {
                _CurrentVolume = value;
                RaisePropertyChanged("CurrentVolume");
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

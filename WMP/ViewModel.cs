using Microsoft.Data.Sqlite;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Data.Sqlite;

namespace WMP
{
    class ViewModel : INotifyPropertyChanged
    {
        Microsoft.Win32.OpenFileDialog dialog;
        MediaElement MusicMedia;
        DispatcherTimer timer = null;
        Boolean isPlaying = false;
        bool betrue() { return true; }
        string StopColor = "#FF000000";
        string PlayColor = "#FFFFFFFF";
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
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        void openMedia()
        {
            dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "";
            dialog.DefaultExt = "";
            dialog.Filter = "audio forms|*.mp3;*.flac;*.wav;*.opus;*.m4a";
            // Show open file dialog box
            bool? result = dialog.ShowDialog();
            // Process open file dialog box results
            if (result == true)
            {
                LoadMedia(dialog.FileName);
            }
        }

        void openFolder()
        {
            Microsoft.Win32.OpenFolderDialog Folderdialog = new Microsoft.Win32.OpenFolderDialog();
            Folderdialog.ShowDialog();
            MessageBox.Show(Folderdialog.FolderName);
            //查找所有音频文件，将文件名存入一个列表中，并在messagebox输出
            string[] files = Directory.GetFiles(Folderdialog.FolderName, "*.mp3", SearchOption.AllDirectories);
            string filelist = "";
            //创建一个数据表，将files 中的文件名存入数据库中
            using (var connection = new SqliteConnection("Data Source=Music.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE IF NOT EXISTS Music (id INTEGER PRIMARY KEY, filename TEXT)";
                command.ExecuteNonQuery();
                foreach (string file in files)
                {
                    command.CommandText = "INSERT INTO Music (filename) VALUES ('" + file + "')";
                    command.ExecuteNonQuery();
                }
            }
            //从数据库中读取文件名，并在messagebox输出
            using (var connection = new SqliteConnection("Data Source=Music.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT filename FROM Music";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MessageBox.Show(reader.GetString(0));
                    }
                }
            }

        }

        private void timer_tick(object sender, EventArgs e)
        {

            
            _MusicPosition = MusicMedia.Position.TotalSeconds;
            MusicPosition = _MusicPosition;
            MusicMedia.Volume = double.Parse(MusicVolume);
        }


        private async void ReadTitle()
        {
            var tfile = await Task.Run(() => TagLib.File.Create(dialog.FileName));
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
        }


        private async void LoadMedia(string filename)
        {
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
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();

        }

        //控制音量

        void PlayandStop()
        {
            //判断当前音乐是否在播放，如果在播放则暂停，否则播放
            if (isPlaying)
            {
                MusicMedia.Pause();
                CurrentColor = PlayColor;
                isPlaying = false;
            }
            else
            {
                MusicMedia.Play();
                CurrentColor = StopColor;
                isPlaying = true;
            }
        }
 
        void forward()
        {
            MusicMedia.Position = TimeSpan.FromSeconds(MusicPosition + 15);
        }
        void backward()
        {
            MusicMedia.Position = TimeSpan.FromSeconds(MusicPosition - 15);
        }

        void PositionChanged()
        {
            //timer.Stop();
            MusicMedia.Position = TimeSpan.FromSeconds(MusicPosition);
        }
        public ICommand PlayandStopCommand => new RelayCommand(o => PlayandStop(), o => betrue());
        public ICommand OpenMediaCommand => new RelayCommand(async o => openMedia(), o => betrue());
        public ICommand PositionChangedCommand => new RelayCommand(o => PositionChanged(), o => betrue());
        public ICommand forwardCommand => new RelayCommand(o => forward(), o => betrue());
        public ICommand backwardCommand => new RelayCommand(o => backward(), o => betrue());
        public ICommand OpenFolderCommand => new RelayCommand(o => openFolder(), o => betrue());

    }
}

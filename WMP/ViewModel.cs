using Microsoft.Data.Sqlite;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WMP
{
    class ViewModel
    {
        private Model _Model;

        public Model Model
        {
            get
            {
                if (_Model == null)
                    _Model = new Model();
                return _Model;
            }
            set { _Model = value; }
        }

        public ViewModel()
        {
            Model = new Model();

            Model.MusicMedia = new MediaElement();
            Model.MusicMedia.LoadedBehavior = MediaState.Manual;
            Model.MusicMedia.UnloadedBehavior = MediaState.Manual;
        }

        void openMedia()
        {
            Model.dialog = new Microsoft.Win32.OpenFileDialog();
            Model.dialog.FileName = "";
            Model.dialog.DefaultExt = "";
            Model.dialog.Filter = "audio forms|*.mp3;*.flac;*.wav;*.opus;*.m4a";
            // Show open file dialog box
            bool? result = Model.dialog.ShowDialog();
            // Process open file dialog box results
            if (result == true)
            {
                LoadMedia(Model.dialog.FileName);
            }
        }

        void openFolder()
        {
            Microsoft.Win32.OpenFolderDialog Folderdialog = new Microsoft.Win32.OpenFolderDialog();
            Folderdialog.ShowDialog();
            MessageBox.Show(Folderdialog.FolderName);
            //查找所有音频文件，将文件名存入一个列表中，并在messagebox输出
            string[] filesPath = Directory.GetFiles(Folderdialog.FolderName, "*.flac", SearchOption.AllDirectories);
            string[] files = new string[filesPath.Length];
            //遍历filesPath，使用ReadTitle转化为名称存入files中
            for (int i = 0; i < filesPath.Length; i++)
            {
                files[i] = ReadTitle(filesPath[i]);
            }
            //输出文件名
            string allfiles = "";
            foreach (string file in files)
            {
                allfiles += file + "\n";
            }
            MessageBox.Show(allfiles);

            //创建一个新页面，将文件名传入
            FolderWindow folderWindow = new FolderWindow();
            folderWindow.Show();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            Model.MusicPosition = Model.MusicMedia.Position.TotalSeconds;
            Model.MusicMedia.Volume = double.Parse(Model.MusicVolume);
        }

        private string ReadTitle(string filesUri)
        {
            var tfile = TagLib.File.Create(filesUri);
            return tfile.Tag.Title;
        }

        private async void LoadMedia(string filename)
        {
            Model.MusicMedia.Source = new Uri(filename);
            var tfile = await Task.Run(() => TagLib.File.Create(filename));
            string title = tfile.Tag.Title;
            Model.MusicTitle = title;
            //将封面图片显示在界面上
            var pic = tfile.Tag.Pictures[0];
            MemoryStream ms = new MemoryStream(pic.Data.Data);
            ms.Seek(0, SeekOrigin.Begin);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            Model.MusicImage = bitmap;
            Model.MusicMedia.MediaOpened += (s, e) =>
            {
                Model.MusicDuration = Model.MusicMedia.NaturalDuration.TimeSpan.TotalSeconds;
                Model.MusicPosition = Model.MusicMedia.Position.TotalSeconds;
            };
            Model.timer = new DispatcherTimer();
            Model.timer.Interval = TimeSpan.FromSeconds(0.1);
            Model.timer.Tick += new EventHandler(timer_tick);
            Model.timer.Start();
        }

        //控制音量
        void PlayandStop()
        {
            //判断当前音乐是否在播放，如果在播放则暂停，否则播放
            if (Model.isPlaying)
            {
                Model.MusicMedia.Pause();
                Model.CurrentColor = Model.PlayColor;
                Model.isPlaying = false;
            }
            else
            {
                Model.MusicMedia.Play();
                Model.CurrentColor = Model.StopColor;
                Model.isPlaying = true;
            }
        }

        void forward()
        {
            Model.MusicMedia.Position = TimeSpan.FromSeconds(Model.MusicPosition + 15);
        }

        void backward()
        {
            Model.MusicMedia.Position = TimeSpan.FromSeconds(Model.MusicPosition - 15);
        }

        void PositionChanged()
        {
            //timer.Stop();
            Model.MusicMedia.Position = TimeSpan.FromSeconds(Model.MusicPosition);
        }

        public ICommand PlayandStopCommand => new RelayCommand(o => PlayandStop(), o => Model.betrue());
        public ICommand OpenMediaCommand => new RelayCommand(async o => openMedia(), o => Model.betrue());
        public ICommand PositionChangedCommand => new RelayCommand(o => PositionChanged(), o => Model.betrue());
        public ICommand forwardCommand => new RelayCommand(o => forward(), o => Model.betrue());
        public ICommand backwardCommand => new RelayCommand(o => backward(), o => Model.betrue());
        public ICommand OpenFolderCommand => new RelayCommand(o => openFolder(), o => Model.betrue());
    }
}

﻿using Microsoft.Data.Sqlite;
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
        private PlayModel _Model;

        public PlayModel playModel
        {
            get
            {
                if (_Model == null)
                    _Model = new PlayModel();
                return _Model;
            }
            set { _Model = value; }
        }

        public ViewModel()
        {
            playModel = new PlayModel();

            playModel.MusicMedia = new MediaElement();
            playModel.MusicMedia.LoadedBehavior = MediaState.Manual;
            playModel.MusicMedia.UnloadedBehavior = MediaState.Manual;
        }

        void openMedia()
        {
            playModel.dialog = new Microsoft.Win32.OpenFileDialog();
            playModel.dialog.FileName = "";
            playModel.dialog.DefaultExt = "";
            playModel.dialog.Filter = "audio forms|*.mp3;*.flac;*.wav;*.opus;*.m4a";
            // Show open file dialog box
            bool? result = playModel.dialog.ShowDialog();
            // Process open file dialog box results
            if (result == true)
            {
                LoadMedia(playModel.dialog.FileName);
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
            playModel.MusicPosition = playModel.MusicMedia.Position.TotalSeconds;
            playModel.MusicMedia.Volume = double.Parse(playModel.MusicVolume);
        }

        private string ReadTitle(string filesUri)
        {
            var tfile = TagLib.File.Create(filesUri);
            return tfile.Tag.Title;
        }

        private async void LoadMedia(string filename)
        {
            playModel.MusicMedia.Source = new Uri(filename);
            var tfile = await Task.Run(() => TagLib.File.Create(filename));
            string title = tfile.Tag.Title;
            playModel.MusicTitle = title;
            //将封面图片显示在界面上
            var pic = tfile.Tag.Pictures[0];
            MemoryStream ms = new MemoryStream(pic.Data.Data);
            ms.Seek(0, SeekOrigin.Begin);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            playModel.MusicImage = bitmap;
            playModel.MusicMedia.MediaOpened += (s, e) =>
            {
                playModel.MusicDuration = playModel.MusicMedia.NaturalDuration.TimeSpan.TotalSeconds;
                playModel.MusicPosition = playModel.MusicMedia.Position.TotalSeconds;
            };
            playModel.timer = new DispatcherTimer();
            playModel.timer.Interval = TimeSpan.FromSeconds(0.1);
            playModel.timer.Tick += new EventHandler(timer_tick);
            playModel.timer.Start();
        }

        //控制音量
        void PlayandStop()
        {
            //判断当前音乐是否在播放，如果在播放则暂停，否则播放
            if (playModel.isPlaying)
            {
                playModel.MusicMedia.Pause();
                playModel.CurrentColor = playModel.PlayColor;
                playModel.isPlaying = false;
            }
            else
            {
                playModel.MusicMedia.Play();
                playModel.CurrentColor = playModel.StopColor;
                playModel.isPlaying = true;
            }
        }

        void forward()
        {
            playModel.MusicMedia.Position = TimeSpan.FromSeconds(playModel.MusicPosition + 15);
        }

        void backward()
        {
            playModel.MusicMedia.Position = TimeSpan.FromSeconds(playModel.MusicPosition - 15);
        }

        void PositionChanged()
        {
            //timer.Stop();
            playModel.MusicMedia.Position = TimeSpan.FromSeconds(playModel.MusicPosition);
        }

        public ICommand PlayandStopCommand => new RelayCommand(o => PlayandStop(), o => playModel.betrue());
        public ICommand OpenMediaCommand => new RelayCommand(async o => openMedia(), o => playModel.betrue());
        public ICommand PositionChangedCommand => new RelayCommand(o => PositionChanged(), o => playModel.betrue());
        public ICommand forwardCommand => new RelayCommand(o => forward(), o => playModel.betrue());
        public ICommand backwardCommand => new RelayCommand(o => backward(), o => playModel.betrue());
        public ICommand OpenFolderCommand => new RelayCommand(o => openFolder(), o => playModel.betrue());
    }
}

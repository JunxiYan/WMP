using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Data.Sqlite;

namespace WMP
{
    class PlayModel : INotifyPropertyChanged
    {
        public PlayModel()
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

        public bool isPlaying { get; set; } = false;

        public bool beTrue() { return true; }

        public string StopColor { get; set; } = "#FF000000";
        public string PlayColor { get; set; } = "#FFFFFFFF";

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

    class FolderModel
    {
        public FolderModel()
        { }
        //在folderModel开启时利用数据库创建表来存储文件夹里待播放的音乐，只作为Model层使用 不书写业务逻辑
        //创建数据库
        public void CreateDatabase()
        {
            using (SqliteConnection db = new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();
                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY, " +
                    "Name NVARCHAR(2048) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }
        //创建表
        public void CreateTable()
        {
            using (SqliteConnection db = new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();
                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY, " +
                    "Name NVARCHAR(2048) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }
        //插入数据
        public void InsertData(string inputName)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @EntryName);";
                insertCommand.Parameters.AddWithValue("@EntryName", inputName);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        //创建一个属性用来存储歌曲列表和其路径
        private List<string> _MusicList = new List<string>();

        public List<string> MusicList
        {
            get { return _MusicList; }
            set { _MusicList = value; }
        }

        private List<string> _MusicPath = new List<string>();

        public List<string> MusicPath
        {
            get { return _MusicPath; }
            set { _MusicPath = value; }
        }

        private List<string> _MusicTitle = new List<string>();

        public List<string> MusicTitle
        {
            get { return _MusicTitle; }
            set { _MusicTitle = value; }
        }
    }
}

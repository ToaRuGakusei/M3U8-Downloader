using System.Collections.ObjectModel;

namespace M3U8_Downloader
{
    public class ViewModel : Prism.Mvvm.BindableBase
    {
        public ViewModel()
        {

        }
        public class Plist()
        {
            public string m3u8Url { get; set; }
            public string fileName { get; set; }
        }
        private string _filePath = "";
        public string filePath
        {
            get => _filePath;
            set
            {
                SetProperty(ref _filePath, value, nameof(filePath));
            }
        }
        private ObservableCollection<Plist> _playlist = new ObservableCollection<Plist>();
        public ObservableCollection<Plist> Playlist
        {
            get => _playlist;
            set
            {
                SetProperty(ref _playlist, value, nameof(Playlist));
            }
        }

        private int _processId = 0;
        public int processId
        {
            get => _processId;
            set
            {
                SetProperty(ref _processId, value, nameof(processId));
            }
        }

        private bool _isRunning = false;
        public bool isRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value, nameof(isRunning));
        }
        private string _status = "Status: Waiting";
        public string Status
        {
            get
            {
                return _status;
            }
            set => SetProperty(ref _status, value, nameof(Status));
        }
        private string _Size = "";
        public string Size
        {
            get => _Size;
            set => SetProperty(ref _Size, value, nameof(Size));
        }
        private string _Time = "";
        public string Time
        {
            get => _Time;
            set => SetProperty(ref _Time, value, nameof(Time));
        }
        private string _Bitrate = "";
        public string Bitrate
        {
            get => _Bitrate;
            set => SetProperty(ref _Bitrate, value, nameof(Bitrate));
        }
        private string _Speed = "";
        public string Speed
        {
            get => _Speed;
            set => SetProperty(ref _Speed, value, nameof(Speed));
        }
        //Status: Waiting
    }
}

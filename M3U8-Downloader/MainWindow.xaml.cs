using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
using M3U8_Downloader.Tool;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace M3U8_Downloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _vm = new ViewModel();
            this.DataContext = _vm;
            (App.Current as App).ViewModel = _vm;
            CheckIsFoundFFMPEG();
        }
        ViewModel _vm;
        private string ffmpeg_Download_URL = "https://github.com/yt-dlp/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl-shared.zip";

       
        private async void CheckIsFoundFFMPEG()
        {
            if(!Directory.Exists(@".\ffmpeg-master-latest-win64-gpl-shared"))
            {               
                FileDownloader fld = new FileDownloader();
                DownloadNow dln = new DownloadNow();
                await ffmpeg_Download(fld, dln);
            }
        }
        private async Task ffmpeg_Download(FileDownloader fld, DownloadNow dln)
        {
            Toast.ShowToast("Download Now!", "FFMPEGのダウンロードが始まります");
            var ffmpeg = await fld.GetContent(ffmpeg_Download_URL, "");
            try
            {
                ZipFile.ExtractToDirectory(ffmpeg, @".\", true);
            }
            catch (Exception)
            {

            }
            ffmpeg.Close();
            dln.Close();
            Toast.ShowToast("Download Done!", "FFMPEGのダウンロードが終わりました");
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            m3u8tovideofile down = new m3u8tovideofile(2);
            if(_vm.Playlist.Count > 0)
            {
                down.StartDownload();
            }else
            {
                MessageBox.Show("Urlを追加してください","エラー",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FetchButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.Playlist.Add(new ViewModel.Plist { m3u8Url = UrlTextBox.Text,fileName=EntryFileName.Text});
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.IsFolderPicker = true;
            var result = dlg.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                _vm.filePath = dlg.FileName;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Process.GetProcessById(_vm.processId).Kill();
            Environment.Exit(0);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Process.GetProcessById(_vm.processId).Kill();
            MessageBox.Show("キャンセルしました","情報",MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeletePlaylist_Click(object sender, RoutedEventArgs e)
        {
            if (PlaylistListBox.SelectedItems.Count != 0) 
            {
                var t = new ObservableCollection<ViewModel.Plist>();
                foreach(var a in PlaylistListBox.SelectedItems)
                {
                    t.Add(a as ViewModel.Plist);
                    //
                }
                foreach(var b in t)
                {
                    _vm.Playlist.Remove(b);
                }
            }
            else _vm.Playlist.Clear();
        }

        private void GetM3U8_Click(object sender, RoutedEventArgs e)
        {
            M3U8Extractor m3U8Extractor = new M3U8Extractor();
            m3U8Extractor.GetM3U8();
        }
    }
}
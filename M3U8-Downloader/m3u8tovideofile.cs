using M3U8_Downloader.Tool;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace M3U8_Downloader
{
    public class m3u8tovideofile
    {
        public m3u8tovideofile(int splitDownload)
        {
            semaphoreSlim = new SemaphoreSlim(splitDownload);
            _viewModel = (App.Current as App).ViewModel;
            (App.Current as App).ViewModel = _viewModel;
        }
        private ViewModel _viewModel;
        private SemaphoreSlim semaphoreSlim;

        public async void StartDownload()
        {
            string ffmpegPath = @".\ffmpeg-master-latest-win64-gpl-shared\bin\ffmpeg.exe"; // FFmpegのパス
            string m3u8Url = _viewModel.Playlist[0].m3u8Url;
            string outputFilePath = $@"{_viewModel.filePath}\{_viewModel.Playlist[0].fileName}.mp4";

            //size= 1985471KiB time=01:10:54.40 bitrate=3823.1kbits/s speed=40.1x    
            await Task.Run(() =>
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = $"-i \"{m3u8Url}\" -c copy \"{outputFilePath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process process = new Process
                {
                    StartInfo = processStartInfo
                };
                process.ErrorDataReceived += (sender, args) =>
                {
                    if (args.Data == null)
                    {
                        End();
                        return;
                    }
                    Debug.WriteLine(args.Data);
                    Match match = Regex.Match(args.Data, @"size=\s*(\d+\s*KiB)\s*time=(\d{2}:\d{2}:\d{2}\.\d{2})\s*bitrate=\s*([\d\.]+kbits/s)\s*speed=\s*(\d+\.?\d*x)");
                    if (match.Success)
                    {
                        string size = match.Groups[1].Value;
                        string time = match.Groups[2].Value;
                        string bitrate = match.Groups[3].Value;
                        string speed = match.Groups[4].Value;
                        _viewModel.Time = time;
                        _viewModel.Bitrate = bitrate;   
                        _viewModel.Speed = speed;   
                        _viewModel.Size = (Convert.ToInt32(size.Replace("KiB","").Replace("size= ","")) /1024).ToString() + "MB";
                    }
                };

                process.Start();
                if (File.Exists(outputFilePath))
                {
                    process.Close();

                    Toast.ShowToast("Information", "ファイルが存在しています。");
                    End();
                }
                else
                {
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    _viewModel.processId = process.Id;
                    _viewModel.isRunning = true;
                    _viewModel.Status = "Status: Running";
                    process.WaitForExit();
                }


            });

        }

        private void End()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _viewModel.Playlist.RemoveAt(0);
            });
            if (_viewModel.Playlist.Count > 0)
            {
                StartDownload();
            }
            _viewModel.isRunning = false;
            _viewModel.Status = "Status: Waiting";
            Toast.ShowToast("Information", "ダウンロードが終わりました！");
        }
    }
}

using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace MLauncher.src
{
    internal class Downloader
    {
        public readonly static string _documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        
        private readonly string _relativeUri = "/wow.zip";
        private readonly string _fullUri;


        public bool _IsDownloaded = false;
        public bool _IsDownloading = false;
        
        private bool Downloading { get; set; }

        private Button _button1;
        private WebClient _webClient = new();
        private ProgressBar _progressBar;

        private Label _label;
        
        public Downloader(Button button1, ProgressBar progressBar, Label label)
        {
            button1.Text = _IsDownloaded ? "Play" : "Download";
            _button1 = button1;
            _progressBar = progressBar;
            _label = label;
            _webClient.BaseAddress = "https://download.mythicforge.online";
            _fullUri = _webClient.BaseAddress + _relativeUri;
            _webClient.DownloadProgressChanged += OnDownloadProgressChange;
            _webClient.DownloadFileCompleted += OnDownloadComplete;
        }

        private void OnDownloadProgressChange(object sender, DownloadProgressChangedEventArgs e)
        {
            if (Downloading)
            {
                _progressBar.Value = e.ProgressPercentage;
                Console.WriteLine(e.BytesReceived);
            }
        }

        private void OnDownloadComplete(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (Downloading)
            {
                Downloading = false;
                _progressBar.Hide();
                _progressBar.Dispose();
                _progressBar = null;
                _IsDownloaded = true;
                _button1.Text = "Play";
                MessageBox.Show("Download complete!");
            }
            MessageBox.Show(e.Error.Message);
        }
        public void Download()
        {
            try
            {
                Downloading = true;
                _webClient.DownloadFileAsync(new Uri(_fullUri), Path.Combine(_documentsPath, "wow.zip"));
            } catch (InvalidOperationException e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}

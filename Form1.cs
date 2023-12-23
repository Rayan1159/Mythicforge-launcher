using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Numerics;
using MLauncher.src;

namespace MLauncher
{
    public partial class Form1 : Form
    {
        Downloader _downloader;
        private ProgressBar _progressBar = new();

        private Label _label = new();

        public Form1()
        {
            InitializeComponent();
            _downloader = new Downloader(button1, _progressBar, _label);
            Controls.Add(_progressBar);
            Controls.Add(_label);
        }
    
        private void button1_Click(object sender, EventArgs e)
        {
            if (!_downloader._IsDownloaded)
            {
                _progressBar.Location = new Point(button1.Location.X + button1.Width + 10, button1.Location.Y);
                _label.Location = new Point(_progressBar.Location.X + _progressBar.Width + 5, _progressBar.Location.Y);
                _progressBar.Height = button1.Height;
                _progressBar.Width = button1.Width * 2;
                _progressBar.Show();

                if (!_downloader._IsDownloading)
                {
                    _downloader.Download();
                }
            }
        }
    }
}

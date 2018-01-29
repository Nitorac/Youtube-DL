using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Youtube_DL_GFX
{
    public partial class UpdatingDLAlert : Form
    {
        public UpdatingDLAlert(string version)
        {
            InitializeComponent();
            downloader_DoWork(version);
        }

        private void downloader_DoWork(string version)
        {
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += downloader_ProgressChanged;
                wc.DownloadFileCompleted += downloader_RunWorkerCompleted;
                wc.DownloadFileAsync(new Uri("https://github.com/rg3/youtube-dl/releases/download/" + version + "/youtube-dl.exe"), MainForm.BaseFile);
            }
        }

        private void downloader_RunWorkerCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine("youtube-dl téléchargé ! (" + new FileInfo(MainForm.BaseFile).Length / 1000 + " Ko)");
            Hide();
            Dispose();
            Close();
        }

        private void downloader_ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            barText.Text = String.Format("Mise à jour en cours ({0} %, {1} Ko / {2} Ko)", e.ProgressPercentage, e.BytesReceived / 1000, e.TotalBytesToReceive / 1000);
            progressBar.Value = e.ProgressPercentage;
        }
    }
}

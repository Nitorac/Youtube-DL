using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Youtube_DL_GFX
{
    public partial class MainForm : Form
    {
        public static string BaseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Youtube-DL");
        public static string BaseFile = Path.Combine(BaseFolder, "youtube-dl.exe");
        public static string LatestUrl = "https://github.com/rg3/youtube-dl/releases/latest";

        public MainForm()
        {
            InitializeComponent();
            Hide();
            if (!(File.Exists(BaseFile) && TestCurrentExe()))
            {
                Show();
                MakeUpdate();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine(Has_Update(true));
        }

        public static void MakeUpdate(string version = "")
        {
            if (version.Length == 0)
            {
                version = GetRemoteVersion();
            }
            Console.WriteLine("Téléchargement de youtube-dl (version " + version + ") ....");
            new UpdatingDLAlert(version).ShowDialog();
        }

        public static bool Has_Update(bool DialogOverride)
        {
            String remote_ver = GetRemoteVersion().Trim();
            String local_ver = GetLocalVersion().Trim();

            if (remote_ver.Equals(local_ver))
            {
                return false;
            }

            if (DialogOverride)
            {
                return true;
            }
            else if (local_ver.Equals("000666000"))
            {
                return true;
            }
            else
            {
                return true;
            }
        }

        public static string GetLocalVersion()
        {
            if (!File.Exists(BaseFile))
            {
                return "000666000";
            }
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = BaseFile;
            p.StartInfo.Arguments = "--version";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            Console.WriteLine("Local version : " + output);
            return output;
        }

        public static string GetRemoteVersion()
        {
            Console.WriteLine("Tentative de connexion au serveur ...");
            String final_url = GetFinalRedirect(LatestUrl) + "/";
            if (final_url.EndsWith("/"))
            {
                final_url = final_url.Substring(0, final_url.Length - 1);
            }
            String[] url = final_url.Split('/');
            Console.WriteLine("Remote version : " + url[url.Length - 1]);
            return url[url.Length - 1];
        }

        public static string GetFinalRedirect(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            int maxRedirCount = 10;  // prevent infinite loops
            string newUrl = url;
            do
            {
                HttpWebRequest req = null;
                HttpWebResponse resp = null;
                try
                {
                    req = (HttpWebRequest)HttpWebRequest.Create(url);
                    req.Method = "HEAD";
                    req.AllowAutoRedirect = false;
                    resp = (HttpWebResponse)req.GetResponse();
                    switch (resp.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            return newUrl;
                        case HttpStatusCode.Redirect:
                        case HttpStatusCode.MovedPermanently:
                        case HttpStatusCode.RedirectKeepVerb:
                        case HttpStatusCode.RedirectMethod:
                            newUrl = resp.Headers["Location"];
                            if (newUrl == null)
                                return url;

                            if (newUrl.IndexOf("://", System.StringComparison.Ordinal) == -1)
                            {
                                // Doesn't have a URL Schema, meaning it's a relative or absolute URL
                                Uri u = new Uri(new Uri(url), newUrl);
                                newUrl = u.ToString();
                            }
                            break;
                        default:
                            return newUrl;
                    }
                    url = newUrl;
                }
                catch (WebException)
                {
                    // Return the last known good URL
                    return newUrl;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (resp != null)
                        resp.Close();
                }
            } while (maxRedirCount-- > 0);

            return newUrl;
        }

        public static bool TestCurrentExe()
        {
            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = BaseFile;
                p.StartInfo.Arguments = "--version";
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                if (output.Length == 0)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            if(sidebar.Width == 240)
            {
                sidebar.Visible = false;
                sidebar.Width = 50;
                logo.Hide();
                sidebarAnimator.ShowSync(sidebar);
                
            }
            else
            {
                sidebar.Visible = false;
                sidebar.Width = 240;
                sidebarAnimator.ShowSync(sidebar);
                logoAnimator.ShowSync(logo);
            }
        }
    }
}


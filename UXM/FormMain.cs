using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UXM
{
    public partial class FormMain : Form
    {
        private static Properties.Settings settings = Properties.Settings.Default;

        private bool closing;
        private CancellationTokenSource cts;
        private IProgress<(double value, string status)> progress;

        public FormMain()
        {
            InitializeComponent();

            closing = false;
            cts = null;
            progress = new Progress<(double value, string status)>(ReportProgress);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Text = "UXM " + Application.ProductVersion;
            EnableControls(true);

            Location = settings.WindowLocation;
            if (settings.WindowSize.Width >= MinimumSize.Width && settings.WindowSize.Height >= MinimumSize.Height)
                Size = settings.WindowSize;
            if (settings.WindowMaximized)
                WindowState = FormWindowState.Maximized;

            txtExePath.Text = settings.ExePath;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cts != null)
            {
                txtStatus.Text = "Aborting...";
                closing = true;
                btnAbort.Enabled = false;
                cts.Cancel();
                e.Cancel = true;
            }
            else
            {
                settings.WindowMaximized = WindowState == FormWindowState.Maximized;
                if (WindowState == FormWindowState.Normal)
                {
                    settings.WindowLocation = Location;
                    settings.WindowSize = Size;
                }
                else
                {
                    settings.WindowLocation = RestoreBounds.Location;
                    settings.WindowSize = RestoreBounds.Size;
                }

                settings.ExePath = txtExePath.Text;
            }
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            txtStatus.Text = "Aborting...";
            btnAbort.Enabled = false;
            cts.Cancel();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            ofdExe.InitialDirectory = Path.GetDirectoryName(txtExePath.Text);
            if (ofdExe.ShowDialog() == DialogResult.OK)
                txtExePath.Text = ofdExe.FileName;
        }

        private void btnExplore_Click(object sender, EventArgs e)
        {
            string dir = Path.GetDirectoryName(txtExePath.Text);
            if (Directory.Exists(dir))
                Process.Start(new ProcessStartInfo(dir) { UseShellExecute = true });
            else
                SystemSounds.Hand.Play();
        }

        private async void btnPatch_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            cts = new CancellationTokenSource();
            string error = await Task.Run(() => ExePatcher.Patch(txtExePath.Text, progress, cts.Token));

            if (cts.Token.IsCancellationRequested)
            {
                progress.Report((0, "Patching aborted."));
            }
            else if (error != null)
            {
                progress.Report((0, "Patching failed."));
                ShowError(error);
            }
            else
            {
                SystemSounds.Asterisk.Play();
            }

            cts.Dispose();
            cts = null;
            EnableControls(true);

            if (closing)
                Close();
        }

        private async void btnRestore_Click(object sender, EventArgs e)
        {
            DialogResult choice = MessageBox.Show("Restoring the game will delete any modified files you have installed.\n" +
                "Do you want to proceed?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (choice == DialogResult.No)
                return;

            EnableControls(false);
            cts = new CancellationTokenSource();
            string error = await Task.Run(() => GameRestorer.Restore(txtExePath.Text, progress, cts.Token));

            if (cts.Token.IsCancellationRequested)
            {
                progress.Report((0, "Restoration aborted."));
            }
            else if (error != null)
            {
                progress.Report((0, "Restoration failed."));
                ShowError(error);
            }
            else
            {
                SystemSounds.Asterisk.Play();
            }

            cts.Dispose();
            cts = null;
            EnableControls(true);

            if (closing)
                Close();
        }

        private async void btnUnpack_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            cts = new CancellationTokenSource();
            string error = await Task.Run(() => ArchiveUnpacker.Unpack(txtExePath.Text, progress, cts.Token));

            if (cts.Token.IsCancellationRequested)
            {
                progress.Report((0, "Unpacking aborted."));
            }
            else if (error != null)
            {
                progress.Report((0, "Unpacking failed."));
                ShowError(error);
            }
            else
            {
                SystemSounds.Asterisk.Play();
            }

            cts.Dispose();
            cts = null;
            EnableControls(true);

            if (closing)
                Close();
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void EnableControls(bool enable)
        {
            txtExePath.Enabled = enable;
            btnBrowse.Enabled = enable;
            btnAbort.Enabled = !enable;
            btnRestore.Enabled = enable;
            btnPatch.Enabled = enable;
            btnUnpack.Enabled = enable;
        }

        private void ReportProgress((double value, string status) report)
        {
            if (report.value < 0 || report.value > 1)
                throw new ArgumentOutOfRangeException("Progress value must be between 0 and 1, inclusive.");

            int percent = (int)Math.Floor(report.value * pbrProgress.Maximum);
            pbrProgress.Value = percent;
            txtStatus.Text = report.status;
            if (TaskbarManager.IsPlatformSupported)
            {
                TaskbarManager.Instance.SetProgressValue(percent, pbrProgress.Maximum);
                if (percent == pbrProgress.Maximum && ActiveForm == this)
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
            }
        }

        private void FormMain_Activated(object sender, EventArgs e)
        {
            if (TaskbarManager.IsPlatformSupported && pbrProgress.Value == pbrProgress.Maximum)
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
            }
        }
    }
}

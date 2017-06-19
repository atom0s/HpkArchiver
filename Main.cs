using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HpkArchiver
{
    public partial class Main : Form
    {
        private HpkFile _File = new HpkFile();

        public Main()
        {
            InitializeComponent();
            Main_SizeChanged(this, new EventArgs());

            prgStatus.Maximum = Int32.MaxValue;

            _File.FileSystemEntryProcessed += new FileSystemEntryProcessedHandler(FileSystemEntryProcessed);
            _File.FileSystemCount += new FileSystemCountHandler(FileSystemCount);
            _File.ProcessingFileDates += new ProcessingFileDatesHandler(ProcessingFileDates);
            _File.Completed += new CompletedHandler(Completed);

            if (File.Exists(Static.ConfigurationFile))
            {
                try
                {
                    StreamReader sr = new StreamReader(Static.ConfigurationFile);
                    txtArchiveOpen.Text = sr.ReadLine();
                    txtArchiveSave.Text = sr.ReadLine();
                    txtExtractOpen.Text = sr.ReadLine();
                    txtExtractSave.Text = sr.ReadLine();
                    if (sr.Peek() != -1)
                    {
                        _File.Game = (Game)Enum.Parse(typeof(Game), sr.ReadLine());
                        switch (_File.Game)
                        {
                            case Game.Tropico4:
                                radTropico4.Checked = true;
                                break;
                            case Game.Tropico5:
                                radTropico5.Checked = true;
                                chkZip.Enabled = false;
                                break;
                            case Game.VictorVran:
                                radVictorVran.Checked = true;
                                chkZip.Enabled = false;
                            break;
                            default:
                                radTropico3.Checked = true;
                                break;
                        }
                    }
                    if (sr.Peek() != -1)
                    {
                        if (sr.ReadLine() == "True")
                            chkZip.Checked = true;
                        else
                            chkZip.Checked = false;
                    }
                    sr.Close();
                }
                catch {}
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            _File.Abort();

            StreamWriter sw = new StreamWriter(Static.ConfigurationFile);
            sw.WriteLine(txtArchiveOpen.Text);
            sw.WriteLine(txtArchiveSave.Text);
            sw.WriteLine(txtExtractOpen.Text);
            sw.WriteLine(txtExtractSave.Text);
            sw.WriteLine(_File.Game.ToString());
            sw.WriteLine(chkZip.Checked.ToString());
            sw.Flush();
            sw.Close();
        }

        #region " Interface "
        private void Main_SizeChanged(object sender, EventArgs e)
        {
            lblMain.Size = new Size(this.Size.Width - 170, this.Size.Height);
        }
        private void FreezeInterface()
        {
            txtExtractOpen.Enabled = false;
            btnExtractOpen.Enabled = false;
            txtExtractSave.Enabled = false;
            btnExtractSave.Enabled = false;
            btnExtract.Enabled = false;

            txtArchiveOpen.Enabled = false;
            btnArchiveOpen.Enabled = false;
            txtArchiveSave.Enabled = false;
            btnArchiveSave.Enabled = false;
            btnArchive.Enabled = false;
        }
        private void ThawInterface()
        {
            txtExtractOpen.Enabled = true;
            btnExtractOpen.Enabled = true;
            txtExtractSave.Enabled = true;
            btnExtractSave.Enabled = true;
            btnExtract.Enabled = true;

            txtArchiveOpen.Enabled = true;
            btnArchiveOpen.Enabled = true;
            txtArchiveSave.Enabled = true;
            btnArchiveSave.Enabled = true;
            btnArchive.Enabled = true;

            prgStatus.Maximum = Int32.MaxValue;
            prgStatus.Value = 0;
            lblStatus.Text = "0%";
        }
        #endregion

        #region " Events "
        private void FileSystemCount(int count)
        {
            if (this.InvokeRequired)
            {
                try { this.Invoke(new FileSystemCountHandler(FileSystemCount), count); }
                catch { }
            }
            else
            {
                prgStatus.Maximum = count;

                lblMain.Text = "Discovered " + count + " directories and files with " + _File.ResidualBytes.ToString() + " residual bytes...";
            }
        }
        private void FileSystemEntryProcessed(FragmentType type)
        {
            if (this.InvokeRequired)
            {
                try { this.Invoke(new FileSystemEntryProcessedHandler(FileSystemEntryProcessed), type); }
                catch { }
            }
            else
            {
                prgStatus.Value++;
                lblStatus.Text = Math.Round((double)prgStatus.Value / (double)prgStatus.Maximum * (double)100, 0).ToString() + "%";
            }
        }
        private void ProcessingFileDates()
        {
            if (this.InvokeRequired)
            {
                try { this.Invoke(new ProcessingFileDatesHandler(ProcessingFileDates)); }
                catch { }
            }
            else
                lblMain.Text = "Applying last modified dates to files...";
        }
        private void Completed(TimeSpan duration, string description)
        {
            if (this.InvokeRequired)
            {
                try { this.Invoke(new CompletedHandler(Completed), duration, description); }
                catch { }
            }
            else
            {
                if (_File.ErrorsCaught > 0)
                    lblMain.Text = "Error: " + _File.ErrorsCaught + " errors caught.  See error.log for details.";
                else
                    lblMain.Text = _File.DirectoriesHandled + " directories and " + _File.FilesHandled + " files " + description + " in " + Static.GetSeconds(duration).ToString() + " seconds!";

                ThawInterface();
            }
        }
        #endregion

        #region " Extract "
        private void btnExtractOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "HPK File (*.hpk)|*.hpk|All Files (*.*)|*.*";
            dlg.FileName = txtExtractOpen.Text;
            dlg.Multiselect = false;
            dlg.CheckFileExists = true;
            dlg.ShowDialog();
            if (dlg.FileName.Length > 0)
                txtExtractOpen.Text = Static.PlatformPath(dlg.FileName);
        }
        private void btnExtractSave_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = txtExtractSave.Text;
            dlg.ShowNewFolderButton = true;
            dlg.ShowDialog();
            if (dlg.SelectedPath.Length > 0)
                txtExtractSave.Text = Static.PlatformPath(dlg.SelectedPath);
        }
        private void btnExtract_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtExtractOpen.Text))
                lblMain.Text = "Error: Extract file could not be found.";
            else
            {
                FreezeInterface();

                lblMain.Text = "Extraction started...";

                _File.StartExtract(txtExtractOpen.Text, txtExtractSave.Text);
            }
        }
        #endregion

        #region " Archive "
        private void btnArchiveOpen_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = txtArchiveOpen.Text;
            dlg.ShowNewFolderButton = false;
            dlg.ShowDialog();
            if (dlg.SelectedPath.Length > 0)
                txtArchiveOpen.Text = Static.PlatformPath(dlg.SelectedPath);
        }
        private void btnArchiveSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "HPK File (*.hpk)|*.hpk|All Files (*.*)|*.*";
            dlg.FileName = txtArchiveSave.Text;
            dlg.CheckFileExists = false;
            dlg.CheckPathExists = true;
            dlg.DefaultExt = ".hpk";
            dlg.AddExtension = true;
            dlg.ShowDialog();
            if (dlg.FileName.Length > 0)
                txtArchiveSave.Text = Static.PlatformPath(dlg.FileName);
        }
        private void btnArchive_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtArchiveOpen.Text))
                lblMain.Text = "Error: Directory to archive cannot be found.";
            else
            {
                FreezeInterface();

                lblMain.Text = "Archiving started...";

                if (chkZip.Enabled)
                    _File.StartArchive(txtArchiveOpen.Text, txtArchiveSave.Text, chkZip.Checked);
                else
                    _File.StartArchive(txtArchiveOpen.Text, txtArchiveSave.Text, false); // Zipping not allowed.
            }
        }
        #endregion

        private void GameChanged(object sender, EventArgs e)
        {
            RadioButton radio = (RadioButton)sender;
            switch (radio.Name)
            {
                case "radTropico4":
                    _File.Game = Game.Tropico4;
                    chkZip.Enabled = true;
                    break;
                case "radTropico5":
                    _File.Game = Game.Tropico5;
                    chkZip.Enabled = false;
                    break;
                case "radVictorVran":
                    _File.Game = Game.VictorVran;
                    break;
                default:
                    _File.Game = Game.Tropico3;
                    chkZip.Enabled = true;
                    break;
            }
        }
    }
}

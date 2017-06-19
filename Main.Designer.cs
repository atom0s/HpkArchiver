namespace HpkArchiver
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.lblMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.prgStatus = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.grpArchive = new System.Windows.Forms.GroupBox();
            this.tlpArchive = new System.Windows.Forms.TableLayoutPanel();
            this.lblArchiveOpen = new System.Windows.Forms.Label();
            this.txtArchiveOpen = new System.Windows.Forms.TextBox();
            this.btnArchiveOpen = new System.Windows.Forms.Button();
            this.lblArchiveSave = new System.Windows.Forms.Label();
            this.txtArchiveSave = new System.Windows.Forms.TextBox();
            this.btnArchiveSave = new System.Windows.Forms.Button();
            this.btnArchive = new System.Windows.Forms.Button();
            this.chkZip = new System.Windows.Forms.CheckBox();
            this.grpExtract = new System.Windows.Forms.GroupBox();
            this.tlpExtract = new System.Windows.Forms.TableLayoutPanel();
            this.lblExtractOpen = new System.Windows.Forms.Label();
            this.txtExtractOpen = new System.Windows.Forms.TextBox();
            this.btnExtractOpen = new System.Windows.Forms.Button();
            this.lblExtractSave = new System.Windows.Forms.Label();
            this.txtExtractSave = new System.Windows.Forms.TextBox();
            this.btnExtractSave = new System.Windows.Forms.Button();
            this.btnExtract = new System.Windows.Forms.Button();
            this.grpGame = new System.Windows.Forms.GroupBox();
            this.tlpGame = new System.Windows.Forms.TableLayoutPanel();
            this.radTropico3 = new System.Windows.Forms.RadioButton();
            this.radTropico4 = new System.Windows.Forms.RadioButton();
            this.radTropico5 = new System.Windows.Forms.RadioButton();
            this.radVictorVran = new System.Windows.Forms.RadioButton();
            this.ssMain.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.grpArchive.SuspendLayout();
            this.tlpArchive.SuspendLayout();
            this.grpExtract.SuspendLayout();
            this.tlpExtract.SuspendLayout();
            this.grpGame.SuspendLayout();
            this.tlpGame.SuspendLayout();
            this.SuspendLayout();
            // 
            // ssMain
            // 
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMain,
            this.prgStatus,
            this.lblStatus});
            this.ssMain.Location = new System.Drawing.Point(0, 270);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(624, 22);
            this.ssMain.TabIndex = 0;
            // 
            // lblMain
            // 
            this.lblMain.AutoSize = false;
            this.lblMain.Name = "lblMain";
            this.lblMain.Size = new System.Drawing.Size(470, 17);
            this.lblMain.Text = "Welcome";
            this.lblMain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // prgStatus
            // 
            this.prgStatus.Name = "prgStatus";
            this.prgStatus.Size = new System.Drawing.Size(100, 16);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = false;
            this.lblStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(35, 17);
            this.lblStatus.Text = "0%";
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.grpArchive, 0, 2);
            this.tlpMain.Controls.Add(this.grpExtract, 0, 1);
            this.tlpMain.Controls.Add(this.grpGame, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Size = new System.Drawing.Size(624, 270);
            this.tlpMain.TabIndex = 1;
            // 
            // grpArchive
            // 
            this.grpArchive.Controls.Add(this.tlpArchive);
            this.grpArchive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpArchive.Location = new System.Drawing.Point(3, 163);
            this.grpArchive.Name = "grpArchive";
            this.grpArchive.Size = new System.Drawing.Size(618, 104);
            this.grpArchive.TabIndex = 2;
            this.grpArchive.TabStop = false;
            this.grpArchive.Text = "Archive";
            // 
            // tlpArchive
            // 
            this.tlpArchive.ColumnCount = 3;
            this.tlpArchive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpArchive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpArchive.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpArchive.Controls.Add(this.lblArchiveOpen, 0, 0);
            this.tlpArchive.Controls.Add(this.txtArchiveOpen, 1, 0);
            this.tlpArchive.Controls.Add(this.btnArchiveOpen, 2, 0);
            this.tlpArchive.Controls.Add(this.lblArchiveSave, 0, 1);
            this.tlpArchive.Controls.Add(this.txtArchiveSave, 1, 1);
            this.tlpArchive.Controls.Add(this.btnArchiveSave, 2, 1);
            this.tlpArchive.Controls.Add(this.btnArchive, 1, 2);
            this.tlpArchive.Controls.Add(this.chkZip, 0, 2);
            this.tlpArchive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpArchive.Location = new System.Drawing.Point(3, 16);
            this.tlpArchive.Name = "tlpArchive";
            this.tlpArchive.RowCount = 3;
            this.tlpArchive.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpArchive.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpArchive.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpArchive.Size = new System.Drawing.Size(612, 85);
            this.tlpArchive.TabIndex = 0;
            // 
            // lblArchiveOpen
            // 
            this.lblArchiveOpen.AutoSize = true;
            this.lblArchiveOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblArchiveOpen.Location = new System.Drawing.Point(3, 0);
            this.lblArchiveOpen.Name = "lblArchiveOpen";
            this.lblArchiveOpen.Size = new System.Drawing.Size(94, 25);
            this.lblArchiveOpen.TabIndex = 0;
            this.lblArchiveOpen.Text = "Open Directory:";
            this.lblArchiveOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtArchiveOpen
            // 
            this.txtArchiveOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtArchiveOpen.Location = new System.Drawing.Point(103, 3);
            this.txtArchiveOpen.Name = "txtArchiveOpen";
            this.txtArchiveOpen.Size = new System.Drawing.Size(476, 20);
            this.txtArchiveOpen.TabIndex = 1;
            // 
            // btnArchiveOpen
            // 
            this.btnArchiveOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnArchiveOpen.Location = new System.Drawing.Point(585, 3);
            this.btnArchiveOpen.Name = "btnArchiveOpen";
            this.btnArchiveOpen.Size = new System.Drawing.Size(24, 19);
            this.btnArchiveOpen.TabIndex = 2;
            this.btnArchiveOpen.Text = "...";
            this.btnArchiveOpen.UseVisualStyleBackColor = true;
            this.btnArchiveOpen.Click += new System.EventHandler(this.btnArchiveOpen_Click);
            // 
            // lblArchiveSave
            // 
            this.lblArchiveSave.AutoSize = true;
            this.lblArchiveSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblArchiveSave.Location = new System.Drawing.Point(3, 25);
            this.lblArchiveSave.Name = "lblArchiveSave";
            this.lblArchiveSave.Size = new System.Drawing.Size(94, 25);
            this.lblArchiveSave.TabIndex = 3;
            this.lblArchiveSave.Text = "Save File:";
            this.lblArchiveSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtArchiveSave
            // 
            this.txtArchiveSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtArchiveSave.Location = new System.Drawing.Point(103, 28);
            this.txtArchiveSave.Name = "txtArchiveSave";
            this.txtArchiveSave.Size = new System.Drawing.Size(476, 20);
            this.txtArchiveSave.TabIndex = 4;
            // 
            // btnArchiveSave
            // 
            this.btnArchiveSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnArchiveSave.Location = new System.Drawing.Point(585, 28);
            this.btnArchiveSave.Name = "btnArchiveSave";
            this.btnArchiveSave.Size = new System.Drawing.Size(24, 19);
            this.btnArchiveSave.TabIndex = 5;
            this.btnArchiveSave.Text = "...";
            this.btnArchiveSave.UseVisualStyleBackColor = true;
            this.btnArchiveSave.Click += new System.EventHandler(this.btnArchiveSave_Click);
            // 
            // btnArchive
            // 
            this.tlpArchive.SetColumnSpan(this.btnArchive, 2);
            this.btnArchive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnArchive.Location = new System.Drawing.Point(103, 53);
            this.btnArchive.Name = "btnArchive";
            this.btnArchive.Size = new System.Drawing.Size(506, 29);
            this.btnArchive.TabIndex = 6;
            this.btnArchive.Text = "Archive";
            this.btnArchive.UseVisualStyleBackColor = true;
            this.btnArchive.Click += new System.EventHandler(this.btnArchive_Click);
            // 
            // chkZip
            // 
            this.chkZip.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkZip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkZip.Location = new System.Drawing.Point(3, 53);
            this.chkZip.Name = "chkZip";
            this.chkZip.Size = new System.Drawing.Size(94, 29);
            this.chkZip.TabIndex = 7;
            this.chkZip.Text = "Compress It:";
            this.chkZip.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkZip.UseVisualStyleBackColor = true;
            // 
            // grpExtract
            // 
            this.grpExtract.Controls.Add(this.tlpExtract);
            this.grpExtract.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpExtract.Location = new System.Drawing.Point(3, 53);
            this.grpExtract.Name = "grpExtract";
            this.grpExtract.Size = new System.Drawing.Size(618, 104);
            this.grpExtract.TabIndex = 1;
            this.grpExtract.TabStop = false;
            this.grpExtract.Text = "Extract";
            // 
            // tlpExtract
            // 
            this.tlpExtract.ColumnCount = 3;
            this.tlpExtract.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpExtract.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpExtract.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpExtract.Controls.Add(this.lblExtractOpen, 0, 0);
            this.tlpExtract.Controls.Add(this.txtExtractOpen, 1, 0);
            this.tlpExtract.Controls.Add(this.btnExtractOpen, 2, 0);
            this.tlpExtract.Controls.Add(this.lblExtractSave, 0, 1);
            this.tlpExtract.Controls.Add(this.txtExtractSave, 1, 1);
            this.tlpExtract.Controls.Add(this.btnExtractSave, 2, 1);
            this.tlpExtract.Controls.Add(this.btnExtract, 1, 2);
            this.tlpExtract.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpExtract.Location = new System.Drawing.Point(3, 16);
            this.tlpExtract.Name = "tlpExtract";
            this.tlpExtract.RowCount = 3;
            this.tlpExtract.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpExtract.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpExtract.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpExtract.Size = new System.Drawing.Size(612, 85);
            this.tlpExtract.TabIndex = 0;
            // 
            // lblExtractOpen
            // 
            this.lblExtractOpen.AutoSize = true;
            this.lblExtractOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExtractOpen.Location = new System.Drawing.Point(3, 0);
            this.lblExtractOpen.Name = "lblExtractOpen";
            this.lblExtractOpen.Size = new System.Drawing.Size(94, 25);
            this.lblExtractOpen.TabIndex = 0;
            this.lblExtractOpen.Text = "Open File:";
            this.lblExtractOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtExtractOpen
            // 
            this.txtExtractOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExtractOpen.Location = new System.Drawing.Point(103, 3);
            this.txtExtractOpen.Name = "txtExtractOpen";
            this.txtExtractOpen.Size = new System.Drawing.Size(476, 20);
            this.txtExtractOpen.TabIndex = 1;
            this.txtExtractOpen.Text = "D:\\Games\\Steam\\steamapps\\common\\Victor Vran\\Packs\\Lua.hpk";
            // 
            // btnExtractOpen
            // 
            this.btnExtractOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExtractOpen.Location = new System.Drawing.Point(585, 3);
            this.btnExtractOpen.Name = "btnExtractOpen";
            this.btnExtractOpen.Size = new System.Drawing.Size(24, 19);
            this.btnExtractOpen.TabIndex = 2;
            this.btnExtractOpen.Text = "...";
            this.btnExtractOpen.UseVisualStyleBackColor = true;
            this.btnExtractOpen.Click += new System.EventHandler(this.btnExtractOpen_Click);
            // 
            // lblExtractSave
            // 
            this.lblExtractSave.AutoSize = true;
            this.lblExtractSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExtractSave.Location = new System.Drawing.Point(3, 25);
            this.lblExtractSave.Name = "lblExtractSave";
            this.lblExtractSave.Size = new System.Drawing.Size(94, 25);
            this.lblExtractSave.TabIndex = 3;
            this.lblExtractSave.Text = "Save Directory:";
            this.lblExtractSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtExtractSave
            // 
            this.txtExtractSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExtractSave.Location = new System.Drawing.Point(103, 28);
            this.txtExtractSave.Name = "txtExtractSave";
            this.txtExtractSave.Size = new System.Drawing.Size(476, 20);
            this.txtExtractSave.TabIndex = 4;
            this.txtExtractSave.Text = "C:\\Users\\atom0s\\Desktop\\lua_decomp";
            // 
            // btnExtractSave
            // 
            this.btnExtractSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExtractSave.Location = new System.Drawing.Point(585, 28);
            this.btnExtractSave.Name = "btnExtractSave";
            this.btnExtractSave.Size = new System.Drawing.Size(24, 19);
            this.btnExtractSave.TabIndex = 5;
            this.btnExtractSave.Text = "...";
            this.btnExtractSave.UseVisualStyleBackColor = true;
            this.btnExtractSave.Click += new System.EventHandler(this.btnExtractSave_Click);
            // 
            // btnExtract
            // 
            this.tlpExtract.SetColumnSpan(this.btnExtract, 2);
            this.btnExtract.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExtract.Location = new System.Drawing.Point(103, 53);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(506, 29);
            this.btnExtract.TabIndex = 6;
            this.btnExtract.Text = "Extract";
            this.btnExtract.UseVisualStyleBackColor = true;
            this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
            // 
            // grpGame
            // 
            this.grpGame.Controls.Add(this.tlpGame);
            this.grpGame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpGame.Location = new System.Drawing.Point(3, 3);
            this.grpGame.Name = "grpGame";
            this.grpGame.Size = new System.Drawing.Size(618, 44);
            this.grpGame.TabIndex = 0;
            this.grpGame.TabStop = false;
            this.grpGame.Text = "Game";
            // 
            // tlpGame
            // 
            this.tlpGame.ColumnCount = 4;
            this.tlpGame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpGame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpGame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpGame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpGame.Controls.Add(this.radTropico3, 0, 0);
            this.tlpGame.Controls.Add(this.radTropico4, 1, 0);
            this.tlpGame.Controls.Add(this.radTropico5, 2, 0);
            this.tlpGame.Controls.Add(this.radVictorVran, 3, 0);
            this.tlpGame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpGame.Location = new System.Drawing.Point(3, 16);
            this.tlpGame.Name = "tlpGame";
            this.tlpGame.RowCount = 1;
            this.tlpGame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGame.Size = new System.Drawing.Size(612, 25);
            this.tlpGame.TabIndex = 0;
            // 
            // radTropico3
            // 
            this.radTropico3.AutoSize = true;
            this.radTropico3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radTropico3.Location = new System.Drawing.Point(3, 3);
            this.radTropico3.Name = "radTropico3";
            this.radTropico3.Size = new System.Drawing.Size(147, 19);
            this.radTropico3.TabIndex = 0;
            this.radTropico3.Text = "Tropico 3 / Grand Ages: Rome";
            this.radTropico3.UseVisualStyleBackColor = true;
            this.radTropico3.CheckedChanged += new System.EventHandler(this.GameChanged);
            // 
            // radTropico4
            // 
            this.radTropico4.AutoSize = true;
            this.radTropico4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radTropico4.Location = new System.Drawing.Point(156, 3);
            this.radTropico4.Name = "radTropico4";
            this.radTropico4.Size = new System.Drawing.Size(147, 19);
            this.radTropico4.TabIndex = 1;
            this.radTropico4.Text = "Tropico 4";
            this.radTropico4.UseVisualStyleBackColor = true;
            this.radTropico4.CheckedChanged += new System.EventHandler(this.GameChanged);
            // 
            // radTropico5
            // 
            this.radTropico5.AutoSize = true;
            this.radTropico5.Checked = true;
            this.radTropico5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radTropico5.Location = new System.Drawing.Point(309, 3);
            this.radTropico5.Name = "radTropico5";
            this.radTropico5.Size = new System.Drawing.Size(147, 19);
            this.radTropico5.TabIndex = 2;
            this.radTropico5.TabStop = true;
            this.radTropico5.Text = "Tropico 5";
            this.radTropico5.UseVisualStyleBackColor = true;
            this.radTropico5.CheckedChanged += new System.EventHandler(this.GameChanged);
            // 
            // radVictorVran
            // 
            this.radVictorVran.AutoSize = true;
            this.radVictorVran.Checked = true;
            this.radVictorVran.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radVictorVran.Location = new System.Drawing.Point(462, 3);
            this.radVictorVran.Name = "radVictorVran";
            this.radVictorVran.Size = new System.Drawing.Size(147, 19);
            this.radVictorVran.TabIndex = 3;
            this.radVictorVran.TabStop = true;
            this.radVictorVran.Text = "Victor Vran";
            this.radVictorVran.UseVisualStyleBackColor = true;
            this.radVictorVran.CheckedChanged += new System.EventHandler(this.GameChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 292);
            this.Controls.Add(this.tlpMain);
            this.Controls.Add(this.ssMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(640, 330);
            this.Name = "Main";
            this.Text = "HPK Archiver";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.SizeChanged += new System.EventHandler(this.Main_SizeChanged);
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            this.tlpMain.ResumeLayout(false);
            this.grpArchive.ResumeLayout(false);
            this.tlpArchive.ResumeLayout(false);
            this.tlpArchive.PerformLayout();
            this.grpExtract.ResumeLayout(false);
            this.tlpExtract.ResumeLayout(false);
            this.tlpExtract.PerformLayout();
            this.grpGame.ResumeLayout(false);
            this.tlpGame.ResumeLayout(false);
            this.tlpGame.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripStatusLabel lblMain;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.GroupBox grpExtract;
        private System.Windows.Forms.TableLayoutPanel tlpExtract;
        private System.Windows.Forms.Label lblExtractOpen;
        private System.Windows.Forms.TextBox txtExtractOpen;
        private System.Windows.Forms.Button btnExtractOpen;
        private System.Windows.Forms.Label lblExtractSave;
        private System.Windows.Forms.TextBox txtExtractSave;
        private System.Windows.Forms.Button btnExtractSave;
        private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.ToolStripProgressBar prgStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.GroupBox grpArchive;
        private System.Windows.Forms.TableLayoutPanel tlpArchive;
        private System.Windows.Forms.Label lblArchiveOpen;
        private System.Windows.Forms.TextBox txtArchiveOpen;
        private System.Windows.Forms.Button btnArchiveOpen;
        private System.Windows.Forms.Label lblArchiveSave;
        private System.Windows.Forms.TextBox txtArchiveSave;
        private System.Windows.Forms.Button btnArchiveSave;
        private System.Windows.Forms.Button btnArchive;
        private System.Windows.Forms.GroupBox grpGame;
        private System.Windows.Forms.TableLayoutPanel tlpGame;
        private System.Windows.Forms.RadioButton radTropico3;
        private System.Windows.Forms.RadioButton radTropico4;
        private System.Windows.Forms.CheckBox chkZip;
        private System.Windows.Forms.RadioButton radTropico5;
        private System.Windows.Forms.RadioButton radVictorVran;
    }
}
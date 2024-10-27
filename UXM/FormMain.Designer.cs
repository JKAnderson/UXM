namespace UXM
{
    partial class FormMain
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
            components = new System.ComponentModel.Container();
            System.Windows.Forms.Label lblBreak;
            System.Windows.Forms.Label lblExePath;
            System.Windows.Forms.Label lblStatus;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            btnPatch = new System.Windows.Forms.Button();
            btnUnpack = new System.Windows.Forms.Button();
            btnRestore = new System.Windows.Forms.Button();
            btnAbort = new System.Windows.Forms.Button();
            btnExplore = new System.Windows.Forms.Button();
            btnBrowse = new System.Windows.Forms.Button();
            txtExePath = new System.Windows.Forms.TextBox();
            txtStatus = new System.Windows.Forms.TextBox();
            pbrProgress = new System.Windows.Forms.ProgressBar();
            ofdExe = new System.Windows.Forms.OpenFileDialog();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            lblBreak = new System.Windows.Forms.Label();
            lblExePath = new System.Windows.Forms.Label();
            lblStatus = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // lblBreak
            // 
            lblBreak.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblBreak.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            lblBreak.Location = new System.Drawing.Point(-17, 131);
            lblBreak.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblBreak.Name = "lblBreak";
            lblBreak.Size = new System.Drawing.Size(887, 3);
            lblBreak.TabIndex = 31;
            // 
            // lblExePath
            // 
            lblExePath.AutoSize = true;
            lblExePath.Location = new System.Drawing.Point(16, 14);
            lblExePath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblExePath.Name = "lblExePath";
            lblExePath.Size = new System.Drawing.Size(113, 20);
            lblExePath.TabIndex = 30;
            lblExePath.Text = "Executable Path";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new System.Drawing.Point(16, 143);
            lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(49, 20);
            lblStatus.TabIndex = 32;
            lblStatus.Text = "Status";
            // 
            // btnPatch
            // 
            btnPatch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnPatch.Location = new System.Drawing.Point(523, 80);
            btnPatch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnPatch.Name = "btnPatch";
            btnPatch.Size = new System.Drawing.Size(100, 35);
            btnPatch.TabIndex = 27;
            btnPatch.Text = "Patch";
            btnPatch.UseVisualStyleBackColor = true;
            btnPatch.Click += btnPatch_Click;
            // 
            // btnUnpack
            // 
            btnUnpack.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnUnpack.Location = new System.Drawing.Point(415, 80);
            btnUnpack.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnUnpack.Name = "btnUnpack";
            btnUnpack.Size = new System.Drawing.Size(100, 35);
            btnUnpack.TabIndex = 26;
            btnUnpack.Text = "Unpack";
            btnUnpack.UseVisualStyleBackColor = true;
            btnUnpack.Click += btnUnpack_Click;
            // 
            // btnRestore
            // 
            btnRestore.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnRestore.Location = new System.Drawing.Point(631, 80);
            btnRestore.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new System.Drawing.Size(100, 35);
            btnRestore.TabIndex = 28;
            btnRestore.Text = "Restore";
            btnRestore.UseVisualStyleBackColor = true;
            btnRestore.Click += btnRestore_Click;
            // 
            // btnAbort
            // 
            btnAbort.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAbort.Enabled = false;
            btnAbort.Location = new System.Drawing.Point(739, 80);
            btnAbort.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnAbort.Name = "btnAbort";
            btnAbort.Size = new System.Drawing.Size(100, 35);
            btnAbort.TabIndex = 29;
            btnAbort.Text = "Abort";
            btnAbort.UseVisualStyleBackColor = true;
            btnAbort.Click += btnAbort_Click;
            // 
            // btnExplore
            // 
            btnExplore.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnExplore.Location = new System.Drawing.Point(739, 35);
            btnExplore.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnExplore.Name = "btnExplore";
            btnExplore.Size = new System.Drawing.Size(100, 35);
            btnExplore.TabIndex = 25;
            btnExplore.Text = "Explore";
            btnExplore.UseVisualStyleBackColor = true;
            btnExplore.Click += btnExplore_Click;
            // 
            // btnBrowse
            // 
            btnBrowse.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnBrowse.Location = new System.Drawing.Point(631, 35);
            btnBrowse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new System.Drawing.Size(100, 35);
            btnBrowse.TabIndex = 24;
            btnBrowse.Text = "Browse";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // txtExePath
            // 
            txtExePath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtExePath.Location = new System.Drawing.Point(16, 38);
            txtExePath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtExePath.Name = "txtExePath";
            txtExePath.Size = new System.Drawing.Size(605, 27);
            txtExePath.TabIndex = 23;
            txtExePath.Text = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\DARK SOULS III\\Game\\DarkSoulsIII.exe";
            // 
            // txtStatus
            // 
            txtStatus.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtStatus.Location = new System.Drawing.Point(16, 168);
            txtStatus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtStatus.Name = "txtStatus";
            txtStatus.ReadOnly = true;
            txtStatus.Size = new System.Drawing.Size(821, 27);
            txtStatus.TabIndex = 33;
            // 
            // pbrProgress
            // 
            pbrProgress.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pbrProgress.Location = new System.Drawing.Point(16, 205);
            pbrProgress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            pbrProgress.Maximum = 1000;
            pbrProgress.Name = "pbrProgress";
            pbrProgress.Size = new System.Drawing.Size(823, 35);
            pbrProgress.TabIndex = 34;
            // 
            // ofdExe
            // 
            ofdExe.FileName = "DarkSoulsIII.exe";
            ofdExe.Filter = "Dark Souls Executable|*.exe";
            ofdExe.Title = "Select Dark Souls executable...";
            // 
            // FormMain
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(855, 254);
            Controls.Add(txtStatus);
            Controls.Add(lblStatus);
            Controls.Add(pbrProgress);
            Controls.Add(lblBreak);
            Controls.Add(btnPatch);
            Controls.Add(btnUnpack);
            Controls.Add(btnRestore);
            Controls.Add(btnAbort);
            Controls.Add(btnExplore);
            Controls.Add(btnBrowse);
            Controls.Add(txtExePath);
            Controls.Add(lblExePath);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MaximumSize = new System.Drawing.Size(2661, 301);
            MinimumSize = new System.Drawing.Size(470, 301);
            Name = "FormMain";
            Text = "UXM <version>";
            Activated += FormMain_Activated;
            FormClosing += FormMain_FormClosing;
            Load += FormMain_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnPatch;
        private System.Windows.Forms.Button btnUnpack;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.Button btnExplore;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtExePath;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.ProgressBar pbrProgress;
        private System.Windows.Forms.OpenFileDialog ofdExe;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}


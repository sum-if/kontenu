namespace OmahSoftware.Sistem {
    partial class SisDatabaseBackup {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btnBackup = new DevExpress.XtraEditors.SimpleButton();
            this.beFile = new DevExpress.XtraEditors.ButtonEdit();
            this.lblNama = new DevExpress.XtraEditors.LabelControl();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.beFile.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBackup
            // 
            this.btnBackup.Location = new System.Drawing.Point(84, 45);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(75, 23);
            this.btnBackup.TabIndex = 20;
            this.btnBackup.Text = "Backup";
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // beFile
            // 
            this.beFile.Location = new System.Drawing.Point(84, 18);
            this.beFile.Name = "beFile";
            this.beFile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beFile.Size = new System.Drawing.Size(347, 20);
            this.beFile.TabIndex = 10;
            this.beFile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beFile_ButtonClick);
            // 
            // lblNama
            // 
            this.lblNama.Location = new System.Drawing.Point(22, 21);
            this.lblNama.Name = "lblNama";
            this.lblNama.Size = new System.Drawing.Size(46, 13);
            this.lblNama.TabIndex = 22;
            this.lblNama.Text = "Nama File";
            // 
            // SisDatabaseBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 92);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.beFile);
            this.Controls.Add(this.lblNama);
            this.Name = "SisDatabaseBackup";
            this.Text = "Database Backup";
            this.Load += new System.EventHandler(this.SisDatabaseBackup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.beFile.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnBackup;
        private DevExpress.XtraEditors.ButtonEdit beFile;
        private DevExpress.XtraEditors.LabelControl lblNama;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}
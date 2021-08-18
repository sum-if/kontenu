namespace Kontenu.Sistem {
    partial class SisDatabaseRestore {
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
            this.btnRestore = new DevExpress.XtraEditors.SimpleButton();
            this.beFile = new DevExpress.XtraEditors.ButtonEdit();
            this.lblNama = new DevExpress.XtraEditors.LabelControl();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.beFile.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(84, 45);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(75, 23);
            this.btnRestore.TabIndex = 20;
            this.btnRestore.Text = "Restore";
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
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
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // SisDatabaseRestore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 92);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.beFile);
            this.Controls.Add(this.lblNama);
            this.Name = "SisDatabaseRestore";
            this.Text = "Database Restore";
            this.Load += new System.EventHandler(this.SisDatabaseRestore_Load);
            ((System.ComponentModel.ISupportInitialize)(this.beFile.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnRestore;
        private DevExpress.XtraEditors.ButtonEdit beFile;
        private DevExpress.XtraEditors.LabelControl lblNama;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}
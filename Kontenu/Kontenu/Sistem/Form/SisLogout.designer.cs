namespace Kontenu.Sistem {
    partial class SisLogout {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
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
            this.picBtnYa = new System.Windows.Forms.PictureBox();
            this.picBtnTidak = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBtnYa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBtnTidak)).BeginInit();
            this.SuspendLayout();
            // 
            // picBtnYa
            // 
            this.picBtnYa.BackColor = System.Drawing.Color.Transparent;
            this.picBtnYa.Image = global::Kontenu.Properties.Resources.button_ya;
            this.picBtnYa.Location = new System.Drawing.Point(76, 144);
            this.picBtnYa.Name = "picBtnYa";
            this.picBtnYa.Size = new System.Drawing.Size(102, 29);
            this.picBtnYa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBtnYa.TabIndex = 32;
            this.picBtnYa.TabStop = false;
            this.picBtnYa.Click += new System.EventHandler(this.picBtnYa_Click);
            // 
            // picBtnTidak
            // 
            this.picBtnTidak.BackColor = System.Drawing.Color.Transparent;
            this.picBtnTidak.Image = global::Kontenu.Properties.Resources.button_tidak;
            this.picBtnTidak.Location = new System.Drawing.Point(184, 144);
            this.picBtnTidak.Name = "picBtnTidak";
            this.picBtnTidak.Size = new System.Drawing.Size(99, 29);
            this.picBtnTidak.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBtnTidak.TabIndex = 33;
            this.picBtnTidak.TabStop = false;
            this.picBtnTidak.Click += new System.EventHandler(this.picBtnTidak_Click);
            // 
            // SisLogout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Zoom;
            this.BackgroundImageStore = global::Kontenu.Properties.Resources.logout_page;
            this.ClientSize = new System.Drawing.Size(587, 244);
            this.Controls.Add(this.picBtnTidak);
            this.Controls.Add(this.picBtnYa);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.None;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SisLogout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LOGOUT";
            ((System.ComponentModel.ISupportInitialize)(this.picBtnYa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBtnTidak)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picBtnYa;
        private System.Windows.Forms.PictureBox picBtnTidak;
    }
}
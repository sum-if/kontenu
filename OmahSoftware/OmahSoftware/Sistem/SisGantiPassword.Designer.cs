namespace OmahSoftware.Sistem {
    partial class SisGantiPassword {
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
            this.components = new System.ComponentModel.Container();
            this.txtPasswordBaruKonfirmasi = new DevExpress.XtraEditors.TextEdit();
            this.lblKonfirmasiPasswordBaru = new DevExpress.XtraEditors.LabelControl();
            this.txtPasswordBaru = new DevExpress.XtraEditors.TextEdit();
            this.lblPasswordBaru = new DevExpress.XtraEditors.LabelControl();
            this.txtPasswordLama = new DevExpress.XtraEditors.TextEdit();
            this.lblPasswordLama = new DevExpress.XtraEditors.LabelControl();
            this.btnSimpan = new DevExpress.XtraEditors.SimpleButton();
            this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtPasswordBaruKonfirmasi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPasswordBaru.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPasswordLama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPasswordBaruKonfirmasi
            // 
            this.txtPasswordBaruKonfirmasi.Location = new System.Drawing.Point(154, 64);
            this.txtPasswordBaruKonfirmasi.Name = "txtPasswordBaruKonfirmasi";
            this.txtPasswordBaruKonfirmasi.Properties.PasswordChar = '*';
            this.txtPasswordBaruKonfirmasi.Size = new System.Drawing.Size(266, 20);
            this.txtPasswordBaruKonfirmasi.TabIndex = 30;
            // 
            // lblKonfirmasiPasswordBaru
            // 
            this.lblKonfirmasiPasswordBaru.Location = new System.Drawing.Point(18, 67);
            this.lblKonfirmasiPasswordBaru.Name = "lblKonfirmasiPasswordBaru";
            this.lblKonfirmasiPasswordBaru.Size = new System.Drawing.Size(123, 13);
            this.lblKonfirmasiPasswordBaru.TabIndex = 55;
            this.lblKonfirmasiPasswordBaru.Text = "Konfirmasi Password Baru";
            // 
            // txtPasswordBaru
            // 
            this.txtPasswordBaru.Location = new System.Drawing.Point(154, 38);
            this.txtPasswordBaru.Name = "txtPasswordBaru";
            this.txtPasswordBaru.Properties.PasswordChar = '*';
            this.txtPasswordBaru.Size = new System.Drawing.Size(266, 20);
            this.txtPasswordBaru.TabIndex = 20;
            // 
            // lblPasswordBaru
            // 
            this.lblPasswordBaru.Location = new System.Drawing.Point(18, 41);
            this.lblPasswordBaru.Name = "lblPasswordBaru";
            this.lblPasswordBaru.Size = new System.Drawing.Size(71, 13);
            this.lblPasswordBaru.TabIndex = 54;
            this.lblPasswordBaru.Text = "Password Baru";
            // 
            // txtPasswordLama
            // 
            this.txtPasswordLama.Location = new System.Drawing.Point(154, 12);
            this.txtPasswordLama.Name = "txtPasswordLama";
            this.txtPasswordLama.Properties.PasswordChar = '*';
            this.txtPasswordLama.Size = new System.Drawing.Size(266, 20);
            this.txtPasswordLama.TabIndex = 10;
            // 
            // lblPasswordLama
            // 
            this.lblPasswordLama.Location = new System.Drawing.Point(18, 15);
            this.lblPasswordLama.Name = "lblPasswordLama";
            this.lblPasswordLama.Size = new System.Drawing.Size(74, 13);
            this.lblPasswordLama.TabIndex = 52;
            this.lblPasswordLama.Text = "Password Lama";
            // 
            // btnSimpan
            // 
            this.btnSimpan.Image = global::OmahSoftware.Properties.Resources.simpan_16;
            this.btnSimpan.Location = new System.Drawing.Point(154, 90);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(75, 23);
            this.btnSimpan.TabIndex = 40;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // SisGantiPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 127);
            this.Controls.Add(this.btnSimpan);
            this.Controls.Add(this.txtPasswordBaruKonfirmasi);
            this.Controls.Add(this.lblKonfirmasiPasswordBaru);
            this.Controls.Add(this.txtPasswordBaru);
            this.Controls.Add(this.lblPasswordBaru);
            this.Controls.Add(this.txtPasswordLama);
            this.Controls.Add(this.lblPasswordLama);
            this.Name = "SisGantiPassword";
            this.Text = "Ganti Password";
            this.Load += new System.EventHandler(this.SisGantiPassword_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtPasswordBaruKonfirmasi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPasswordBaru.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPasswordLama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraEditors.TextEdit txtPasswordBaruKonfirmasi;
        private DevExpress.XtraEditors.LabelControl lblKonfirmasiPasswordBaru;
        public DevExpress.XtraEditors.TextEdit txtPasswordBaru;
        private DevExpress.XtraEditors.LabelControl lblPasswordBaru;
        public DevExpress.XtraEditors.TextEdit txtPasswordLama;
        private DevExpress.XtraEditors.LabelControl lblPasswordLama;
        private DevExpress.XtraEditors.SimpleButton btnSimpan;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;

    }
}
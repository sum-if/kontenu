namespace Kontenu.Sistem {
    partial class SisPerusahaan {
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
            this.btnSimpan = new DevExpress.XtraEditors.SimpleButton();
            this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
            this.labelControl18 = new DevExpress.XtraEditors.LabelControl();
            this.txtEmail = new DevExpress.XtraEditors.TextEdit();
            this.txtNama = new DevExpress.XtraEditors.TextEdit();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            this.txtTelepon = new DevExpress.XtraEditors.TextEdit();
            this.labelControl22 = new DevExpress.XtraEditors.LabelControl();
            this.txtAlamat = new DevExpress.XtraEditors.TextEdit();
            this.labelControl26 = new DevExpress.XtraEditors.LabelControl();
            this.txtNPWP = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtAlamatPajak = new DevExpress.XtraEditors.TextEdit();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.txtTipePajak = new DevExpress.XtraEditors.TextEdit();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.btnHapusGambar = new DevExpress.XtraEditors.SimpleButton();
            this.btnAmbilGambar = new DevExpress.XtraEditors.SimpleButton();
            this.picGambar = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtFooterRekening = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTelepon.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlamat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNPWP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlamatPajak.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTipePajak.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGambar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFooterRekening.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSimpan
            // 
            this.btnSimpan.Image = global::Kontenu.Properties.Resources.simpan_16;
            this.btnSimpan.Location = new System.Drawing.Point(138, 269);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(75, 23);
            this.btnSimpan.TabIndex = 150;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // labelControl18
            // 
            this.labelControl18.Location = new System.Drawing.Point(12, 90);
            this.labelControl18.Name = "labelControl18";
            this.labelControl18.Size = new System.Drawing.Size(24, 13);
            this.labelControl18.TabIndex = 215;
            this.labelControl18.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.EditValue = "";
            this.txtEmail.Location = new System.Drawing.Point(138, 87);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(381, 20);
            this.txtEmail.TabIndex = 90;
            // 
            // txtNama
            // 
            this.txtNama.Location = new System.Drawing.Point(138, 9);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(381, 20);
            this.txtNama.TabIndex = 10;
            // 
            // labelControl21
            // 
            this.labelControl21.Location = new System.Drawing.Point(12, 64);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(38, 13);
            this.labelControl21.TabIndex = 206;
            this.labelControl21.Text = "Telepon";
            // 
            // txtTelepon
            // 
            this.txtTelepon.EditValue = "";
            this.txtTelepon.Location = new System.Drawing.Point(138, 61);
            this.txtTelepon.Name = "txtTelepon";
            this.txtTelepon.Size = new System.Drawing.Size(381, 20);
            this.txtTelepon.TabIndex = 60;
            // 
            // labelControl22
            // 
            this.labelControl22.Location = new System.Drawing.Point(12, 12);
            this.labelControl22.Name = "labelControl22";
            this.labelControl22.Size = new System.Drawing.Size(27, 13);
            this.labelControl22.TabIndex = 203;
            this.labelControl22.Text = "Nama";
            // 
            // txtAlamat
            // 
            this.txtAlamat.Location = new System.Drawing.Point(138, 35);
            this.txtAlamat.Name = "txtAlamat";
            this.txtAlamat.Size = new System.Drawing.Size(381, 20);
            this.txtAlamat.TabIndex = 20;
            // 
            // labelControl26
            // 
            this.labelControl26.Location = new System.Drawing.Point(12, 38);
            this.labelControl26.Name = "labelControl26";
            this.labelControl26.Size = new System.Drawing.Size(33, 13);
            this.labelControl26.TabIndex = 204;
            this.labelControl26.Text = "Alamat";
            // 
            // txtNPWP
            // 
            this.txtNPWP.Location = new System.Drawing.Point(138, 113);
            this.txtNPWP.Name = "txtNPWP";
            this.txtNPWP.Size = new System.Drawing.Size(381, 20);
            this.txtNPWP.TabIndex = 100;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 116);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(29, 13);
            this.labelControl1.TabIndex = 221;
            this.labelControl1.Text = "NPWP";
            // 
            // txtAlamatPajak
            // 
            this.txtAlamatPajak.Location = new System.Drawing.Point(138, 139);
            this.txtAlamatPajak.Name = "txtAlamatPajak";
            this.txtAlamatPajak.Size = new System.Drawing.Size(381, 20);
            this.txtAlamatPajak.TabIndex = 110;
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(12, 142);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(62, 13);
            this.labelControl12.TabIndex = 217;
            this.labelControl12.Text = "Alamat Pajak";
            // 
            // txtTipePajak
            // 
            this.txtTipePajak.Location = new System.Drawing.Point(138, 165);
            this.txtTipePajak.Name = "txtTipePajak";
            this.txtTipePajak.Size = new System.Drawing.Size(381, 20);
            this.txtTipePajak.TabIndex = 120;
            // 
            // labelControl13
            // 
            this.labelControl13.Location = new System.Drawing.Point(12, 168);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(49, 13);
            this.labelControl13.TabIndex = 218;
            this.labelControl13.Text = "Tipe Pajak";
            // 
            // btnHapusGambar
            // 
            this.btnHapusGambar.Location = new System.Drawing.Point(628, 131);
            this.btnHapusGambar.Name = "btnHapusGambar";
            this.btnHapusGambar.Size = new System.Drawing.Size(85, 23);
            this.btnHapusGambar.TabIndex = 140;
            this.btnHapusGambar.Text = "Hapus Logo";
            this.btnHapusGambar.Click += new System.EventHandler(this.btnHapusGambar_Click);
            // 
            // btnAmbilGambar
            // 
            this.btnAmbilGambar.Location = new System.Drawing.Point(537, 131);
            this.btnAmbilGambar.Name = "btnAmbilGambar";
            this.btnAmbilGambar.Size = new System.Drawing.Size(85, 23);
            this.btnAmbilGambar.TabIndex = 130;
            this.btnAmbilGambar.Text = "Ambil Logo";
            this.btnAmbilGambar.Click += new System.EventHandler(this.btnAmbilGambar_Click);
            // 
            // picGambar
            // 
            this.picGambar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGambar.InitialImage = null;
            this.picGambar.Location = new System.Drawing.Point(537, 9);
            this.picGambar.Name = "picGambar";
            this.picGambar.Size = new System.Drawing.Size(176, 116);
            this.picGambar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picGambar.TabIndex = 223;
            this.picGambar.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtFooterRekening
            // 
            this.txtFooterRekening.Location = new System.Drawing.Point(138, 191);
            this.txtFooterRekening.Name = "txtFooterRekening";
            this.txtFooterRekening.Size = new System.Drawing.Size(381, 72);
            this.txtFooterRekening.TabIndex = 224;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 192);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(79, 13);
            this.labelControl2.TabIndex = 225;
            this.labelControl2.Text = "Footer Rekening";
            // 
            // SisPerusahaan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 305);
            this.Controls.Add(this.txtFooterRekening);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.btnHapusGambar);
            this.Controls.Add(this.btnAmbilGambar);
            this.Controls.Add(this.picGambar);
            this.Controls.Add(this.txtNPWP);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtAlamatPajak);
            this.Controls.Add(this.labelControl12);
            this.Controls.Add(this.txtTipePajak);
            this.Controls.Add(this.labelControl13);
            this.Controls.Add(this.labelControl18);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtNama);
            this.Controls.Add(this.labelControl21);
            this.Controls.Add(this.txtTelepon);
            this.Controls.Add(this.labelControl22);
            this.Controls.Add(this.txtAlamat);
            this.Controls.Add(this.labelControl26);
            this.Controls.Add(this.btnSimpan);
            this.Name = "SisPerusahaan";
            this.Text = "Perusahaan";
            this.Load += new System.EventHandler(this.SisPerusahaan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTelepon.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlamat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNPWP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlamatPajak.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTipePajak.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGambar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFooterRekening.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnSimpan;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
        private DevExpress.XtraEditors.LabelControl labelControl18;
        private DevExpress.XtraEditors.TextEdit txtEmail;
        public DevExpress.XtraEditors.TextEdit txtNama;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        private DevExpress.XtraEditors.TextEdit txtTelepon;
        private DevExpress.XtraEditors.LabelControl labelControl22;
        private DevExpress.XtraEditors.TextEdit txtAlamat;
        private DevExpress.XtraEditors.LabelControl labelControl26;
        public DevExpress.XtraEditors.TextEdit txtNPWP;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        public DevExpress.XtraEditors.TextEdit txtAlamatPajak;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.TextEdit txtTipePajak;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.SimpleButton btnHapusGambar;
        private DevExpress.XtraEditors.SimpleButton btnAmbilGambar;
        private System.Windows.Forms.PictureBox picGambar;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private DevExpress.XtraEditors.MemoEdit txtFooterRekening;
        private DevExpress.XtraEditors.LabelControl labelControl2;

    }
}
namespace Kontenu.Master {
    partial class FrmPerusahaan {
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
            this.txtKota = new DevExpress.XtraEditors.TextEdit();
            this.labelControl22 = new DevExpress.XtraEditors.LabelControl();
            this.txtAlamat = new DevExpress.XtraEditors.TextEdit();
            this.labelControl26 = new DevExpress.XtraEditors.LabelControl();
            this.txtWebsite = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnHapusGambar = new DevExpress.XtraEditors.SimpleButton();
            this.btnAmbilGambar = new DevExpress.XtraEditors.SimpleButton();
            this.picGambar = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtTelf = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKota.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlamat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWebsite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGambar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTelf.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSimpan
            // 
            this.btnSimpan.Image = global::Kontenu.Properties.Resources.simpan_16;
            this.btnSimpan.Location = new System.Drawing.Point(207, 430);
            this.btnSimpan.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(112, 34);
            this.btnSimpan.TabIndex = 150;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // labelControl18
            // 
            this.labelControl18.Location = new System.Drawing.Point(18, 168);
            this.labelControl18.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl18.Name = "labelControl18";
            this.labelControl18.Size = new System.Drawing.Size(39, 19);
            this.labelControl18.TabIndex = 215;
            this.labelControl18.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.EditValue = "";
            this.txtEmail.Location = new System.Drawing.Point(207, 163);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(572, 26);
            this.txtEmail.TabIndex = 90;
            // 
            // txtNama
            // 
            this.txtNama.Location = new System.Drawing.Point(207, 13);
            this.txtNama.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(572, 26);
            this.txtNama.TabIndex = 10;
            // 
            // labelControl21
            // 
            this.labelControl21.Location = new System.Drawing.Point(18, 94);
            this.labelControl21.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(31, 19);
            this.labelControl21.TabIndex = 206;
            this.labelControl21.Text = "Kota";
            // 
            // txtKota
            // 
            this.txtKota.EditValue = "";
            this.txtKota.Location = new System.Drawing.Point(207, 89);
            this.txtKota.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtKota.Name = "txtKota";
            this.txtKota.Size = new System.Drawing.Size(572, 26);
            this.txtKota.TabIndex = 60;
            // 
            // labelControl22
            // 
            this.labelControl22.Location = new System.Drawing.Point(18, 18);
            this.labelControl22.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl22.Name = "labelControl22";
            this.labelControl22.Size = new System.Drawing.Size(41, 19);
            this.labelControl22.TabIndex = 203;
            this.labelControl22.Text = "Nama";
            // 
            // txtAlamat
            // 
            this.txtAlamat.Location = new System.Drawing.Point(207, 51);
            this.txtAlamat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtAlamat.Name = "txtAlamat";
            this.txtAlamat.Size = new System.Drawing.Size(572, 26);
            this.txtAlamat.TabIndex = 20;
            // 
            // labelControl26
            // 
            this.labelControl26.Location = new System.Drawing.Point(18, 56);
            this.labelControl26.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl26.Name = "labelControl26";
            this.labelControl26.Size = new System.Drawing.Size(50, 19);
            this.labelControl26.TabIndex = 204;
            this.labelControl26.Text = "Alamat";
            // 
            // txtWebsite
            // 
            this.txtWebsite.Location = new System.Drawing.Point(207, 201);
            this.txtWebsite.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtWebsite.Name = "txtWebsite";
            this.txtWebsite.Size = new System.Drawing.Size(572, 26);
            this.txtWebsite.TabIndex = 100;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(18, 206);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(55, 19);
            this.labelControl1.TabIndex = 221;
            this.labelControl1.Text = "Website";
            // 
            // btnHapusGambar
            // 
            this.btnHapusGambar.Location = new System.Drawing.Point(480, 295);
            this.btnHapusGambar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHapusGambar.Name = "btnHapusGambar";
            this.btnHapusGambar.Size = new System.Drawing.Size(128, 34);
            this.btnHapusGambar.TabIndex = 140;
            this.btnHapusGambar.Text = "Hapus Logo";
            this.btnHapusGambar.Click += new System.EventHandler(this.btnHapusGambar_Click);
            // 
            // btnAmbilGambar
            // 
            this.btnAmbilGambar.Location = new System.Drawing.Point(480, 252);
            this.btnAmbilGambar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAmbilGambar.Name = "btnAmbilGambar";
            this.btnAmbilGambar.Size = new System.Drawing.Size(128, 34);
            this.btnAmbilGambar.TabIndex = 130;
            this.btnAmbilGambar.Text = "Ambil Logo";
            this.btnAmbilGambar.Click += new System.EventHandler(this.btnAmbilGambar_Click);
            // 
            // picGambar
            // 
            this.picGambar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGambar.InitialImage = null;
            this.picGambar.Location = new System.Drawing.Point(207, 252);
            this.picGambar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picGambar.Name = "picGambar";
            this.picGambar.Size = new System.Drawing.Size(263, 169);
            this.picGambar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picGambar.TabIndex = 223;
            this.picGambar.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(18, 131);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(28, 19);
            this.labelControl2.TabIndex = 225;
            this.labelControl2.Text = "Telf";
            // 
            // txtTelf
            // 
            this.txtTelf.EditValue = "";
            this.txtTelf.Location = new System.Drawing.Point(207, 126);
            this.txtTelf.Margin = new System.Windows.Forms.Padding(4);
            this.txtTelf.Name = "txtTelf";
            this.txtTelf.Size = new System.Drawing.Size(572, 26);
            this.txtTelf.TabIndex = 224;
            // 
            // FrmPerusahaan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 476);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtTelf);
            this.Controls.Add(this.btnHapusGambar);
            this.Controls.Add(this.btnAmbilGambar);
            this.Controls.Add(this.picGambar);
            this.Controls.Add(this.txtWebsite);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl18);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtNama);
            this.Controls.Add(this.labelControl21);
            this.Controls.Add(this.txtKota);
            this.Controls.Add(this.labelControl22);
            this.Controls.Add(this.txtAlamat);
            this.Controls.Add(this.labelControl26);
            this.Controls.Add(this.btnSimpan);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmPerusahaan";
            this.Text = "Perusahaan";
            this.Load += new System.EventHandler(this.FrmPerusahaan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKota.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlamat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWebsite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGambar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTelf.Properties)).EndInit();
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
        private DevExpress.XtraEditors.TextEdit txtKota;
        private DevExpress.XtraEditors.LabelControl labelControl22;
        private DevExpress.XtraEditors.TextEdit txtAlamat;
        private DevExpress.XtraEditors.LabelControl labelControl26;
        public DevExpress.XtraEditors.TextEdit txtWebsite;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnHapusGambar;
        private DevExpress.XtraEditors.SimpleButton btnAmbilGambar;
        private System.Windows.Forms.PictureBox picGambar;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtTelf;

    }
}
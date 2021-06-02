namespace OmahSoftware.Umum {
    partial class FrmKlienAdd {
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
            this.txtKode = new DevExpress.XtraEditors.TextEdit();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.txtNama = new DevExpress.XtraEditors.TextEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.txtAlamat = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtProvinsi = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtTelp = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtHandphone = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtEmail = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtKota = new DevExpress.XtraEditors.TextEdit();
            this.txtKodePos = new DevExpress.XtraEditors.TextEdit();
            this.txtKTP = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlamat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProvinsi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTelp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandphone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKota.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKodePos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKTP.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSimpan
            // 
            this.btnSimpan.Image = global::OmahSoftware.Properties.Resources.simpan_16;
            this.btnSimpan.Location = new System.Drawing.Point(133, 220);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(75, 23);
            this.btnSimpan.TabIndex = 110;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // txtKode
            // 
            this.txtKode.Location = new System.Drawing.Point(133, 12);
            this.txtKode.Name = "txtKode";
            this.txtKode.Size = new System.Drawing.Size(321, 20);
            this.txtKode.TabIndex = 10;
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(14, 15);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(24, 13);
            this.labelControl11.TabIndex = 189;
            this.labelControl11.Text = "Kode";
            // 
            // txtNama
            // 
            this.txtNama.Location = new System.Drawing.Point(133, 64);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(321, 20);
            this.txtNama.TabIndex = 30;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(14, 68);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(27, 13);
            this.labelControl10.TabIndex = 252;
            this.labelControl10.Text = "Nama";
            // 
            // txtAlamat
            // 
            this.txtAlamat.Location = new System.Drawing.Point(133, 90);
            this.txtAlamat.Name = "txtAlamat";
            this.txtAlamat.Size = new System.Drawing.Size(321, 20);
            this.txtAlamat.TabIndex = 40;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 94);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(33, 13);
            this.labelControl1.TabIndex = 254;
            this.labelControl1.Text = "Alamat";
            // 
            // txtProvinsi
            // 
            this.txtProvinsi.Location = new System.Drawing.Point(133, 116);
            this.txtProvinsi.Name = "txtProvinsi";
            this.txtProvinsi.Size = new System.Drawing.Size(116, 20);
            this.txtProvinsi.TabIndex = 50;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(14, 120);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(96, 13);
            this.labelControl2.TabIndex = 256;
            this.labelControl2.Text = "Prov/Kota/Kode Pos";
            // 
            // txtTelp
            // 
            this.txtTelp.Location = new System.Drawing.Point(133, 142);
            this.txtTelp.Name = "txtTelp";
            this.txtTelp.Size = new System.Drawing.Size(321, 20);
            this.txtTelp.TabIndex = 80;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(14, 146);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(20, 13);
            this.labelControl3.TabIndex = 258;
            this.labelControl3.Text = "Telp";
            // 
            // txtHandphone
            // 
            this.txtHandphone.Location = new System.Drawing.Point(133, 168);
            this.txtHandphone.Name = "txtHandphone";
            this.txtHandphone.Size = new System.Drawing.Size(321, 20);
            this.txtHandphone.TabIndex = 90;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(14, 172);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(55, 13);
            this.labelControl4.TabIndex = 260;
            this.labelControl4.Text = "Handphone";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(133, 194);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(321, 20);
            this.txtEmail.TabIndex = 100;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(14, 198);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(24, 13);
            this.labelControl5.TabIndex = 262;
            this.labelControl5.Text = "Email";
            // 
            // txtKota
            // 
            this.txtKota.Location = new System.Drawing.Point(255, 117);
            this.txtKota.Name = "txtKota";
            this.txtKota.Size = new System.Drawing.Size(116, 20);
            this.txtKota.TabIndex = 60;
            // 
            // txtKodePos
            // 
            this.txtKodePos.Location = new System.Drawing.Point(377, 117);
            this.txtKodePos.Name = "txtKodePos";
            this.txtKodePos.Size = new System.Drawing.Size(77, 20);
            this.txtKodePos.TabIndex = 70;
            // 
            // txtKTP
            // 
            this.txtKTP.Location = new System.Drawing.Point(133, 38);
            this.txtKTP.Name = "txtKTP";
            this.txtKTP.Size = new System.Drawing.Size(321, 20);
            this.txtKTP.TabIndex = 20;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(14, 42);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(18, 13);
            this.labelControl6.TabIndex = 266;
            this.labelControl6.Text = "KTP";
            // 
            // FrmKlienAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 253);
            this.Controls.Add(this.txtKTP);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.txtKodePos);
            this.Controls.Add(this.txtKota);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.txtHandphone);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtTelp);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.txtProvinsi);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtAlamat);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtKode);
            this.Controls.Add(this.labelControl11);
            this.Controls.Add(this.txtNama);
            this.Controls.Add(this.btnSimpan);
            this.Controls.Add(this.labelControl10);
            this.Name = "FrmKlienAdd";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmKlienAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlamat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProvinsi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTelp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandphone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKota.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKodePos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKTP.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnSimpan;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
        public DevExpress.XtraEditors.TextEdit txtKode;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.TextEdit txtNama;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.TextEdit txtAlamat;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtProvinsi;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtTelp;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtHandphone;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtEmail;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtKota;
        private DevExpress.XtraEditors.TextEdit txtKodePos;
        private DevExpress.XtraEditors.TextEdit txtKTP;
        private DevExpress.XtraEditors.LabelControl labelControl6;
    }
}
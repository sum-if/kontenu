namespace Kontenu.Master {
    partial class FrmPIC {
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
            this.btnCetak = new DevExpress.XtraEditors.SimpleButton();
            this.btnHapus = new DevExpress.XtraEditors.SimpleButton();
            this.btnUbah = new DevExpress.XtraEditors.SimpleButton();
            this.btnTambah = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl = new DevExpress.XtraEditors.GroupControl();
            this.cmbJabatan = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtEmail = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtHandphone = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtKota = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtAlamat = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtKTP = new DevExpress.XtraEditors.TextEdit();
            this.lblAlamat = new DevExpress.XtraEditors.LabelControl();
            this.txtNama = new DevExpress.XtraEditors.TextEdit();
            this.btnCari = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
            this.groupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbJabatan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandphone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKota.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlamat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKTP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCetak
            // 
            this.btnCetak.Image = global::Kontenu.Properties.Resources.cetak_16;
            this.btnCetak.Location = new System.Drawing.Point(301, 16);
            this.btnCetak.Name = "btnCetak";
            this.btnCetak.Size = new System.Drawing.Size(90, 23);
            this.btnCetak.TabIndex = 40;
            this.btnCetak.Text = "Cetak [F4]";
            this.btnCetak.Click += new System.EventHandler(this.btnCetak_Click);
            // 
            // btnHapus
            // 
            this.btnHapus.Image = global::Kontenu.Properties.Resources.hapus_16;
            this.btnHapus.Location = new System.Drawing.Point(205, 16);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(90, 23);
            this.btnHapus.TabIndex = 30;
            this.btnHapus.Text = "Hapus [F3]";
            this.btnHapus.Click += new System.EventHandler(this.btnHapus_Click);
            // 
            // btnUbah
            // 
            this.btnUbah.Image = global::Kontenu.Properties.Resources.ubah_16;
            this.btnUbah.Location = new System.Drawing.Point(109, 16);
            this.btnUbah.Name = "btnUbah";
            this.btnUbah.Size = new System.Drawing.Size(90, 23);
            this.btnUbah.TabIndex = 20;
            this.btnUbah.Text = "Ubah [F2]";
            this.btnUbah.Click += new System.EventHandler(this.btnUbah_Click);
            // 
            // btnTambah
            // 
            this.btnTambah.Image = global::Kontenu.Properties.Resources.tambah_16;
            this.btnTambah.Location = new System.Drawing.Point(13, 16);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(90, 23);
            this.btnTambah.TabIndex = 10;
            this.btnTambah.Text = "Tambah [F1]";
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // groupControl
            // 
            this.groupControl.Controls.Add(this.cmbJabatan);
            this.groupControl.Controls.Add(this.labelControl4);
            this.groupControl.Controls.Add(this.txtEmail);
            this.groupControl.Controls.Add(this.labelControl5);
            this.groupControl.Controls.Add(this.txtHandphone);
            this.groupControl.Controls.Add(this.labelControl6);
            this.groupControl.Controls.Add(this.labelControl3);
            this.groupControl.Controls.Add(this.txtKota);
            this.groupControl.Controls.Add(this.labelControl2);
            this.groupControl.Controls.Add(this.txtAlamat);
            this.groupControl.Controls.Add(this.labelControl1);
            this.groupControl.Controls.Add(this.txtKTP);
            this.groupControl.Controls.Add(this.lblAlamat);
            this.groupControl.Controls.Add(this.txtNama);
            this.groupControl.Controls.Add(this.btnCari);
            this.groupControl.Location = new System.Drawing.Point(12, 54);
            this.groupControl.Name = "groupControl";
            this.groupControl.Size = new System.Drawing.Size(775, 156);
            this.groupControl.TabIndex = 31;
            this.groupControl.Text = "Pencarian";
            // 
            // cmbJabatan
            // 
            this.cmbJabatan.Location = new System.Drawing.Point(488, 23);
            this.cmbJabatan.Name = "cmbJabatan";
            this.cmbJabatan.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbJabatan.Size = new System.Drawing.Size(282, 20);
            this.cmbJabatan.TabIndex = 158;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(419, 78);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(24, 13);
            this.labelControl4.TabIndex = 157;
            this.labelControl4.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(488, 75);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(282, 20);
            this.txtEmail.TabIndex = 156;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(419, 52);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(20, 13);
            this.labelControl5.TabIndex = 155;
            this.labelControl5.Text = "Telp";
            // 
            // txtHandphone
            // 
            this.txtHandphone.Location = new System.Drawing.Point(488, 49);
            this.txtHandphone.Name = "txtHandphone";
            this.txtHandphone.Size = new System.Drawing.Size(282, 20);
            this.txtHandphone.TabIndex = 154;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(419, 26);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(39, 13);
            this.labelControl6.TabIndex = 153;
            this.labelControl6.Text = "Jabatan";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(28, 102);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(22, 13);
            this.labelControl3.TabIndex = 151;
            this.labelControl3.Text = "Kota";
            // 
            // txtKota
            // 
            this.txtKota.Location = new System.Drawing.Point(97, 99);
            this.txtKota.Name = "txtKota";
            this.txtKota.Size = new System.Drawing.Size(282, 20);
            this.txtKota.TabIndex = 150;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(28, 76);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(33, 13);
            this.labelControl2.TabIndex = 149;
            this.labelControl2.Text = "Alamat";
            // 
            // txtAlamat
            // 
            this.txtAlamat.Location = new System.Drawing.Point(97, 73);
            this.txtAlamat.Name = "txtAlamat";
            this.txtAlamat.Size = new System.Drawing.Size(282, 20);
            this.txtAlamat.TabIndex = 148;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(28, 26);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(18, 13);
            this.labelControl1.TabIndex = 133;
            this.labelControl1.Text = "KTP";
            // 
            // txtKTP
            // 
            this.txtKTP.Location = new System.Drawing.Point(97, 23);
            this.txtKTP.Name = "txtKTP";
            this.txtKTP.Size = new System.Drawing.Size(282, 20);
            this.txtKTP.TabIndex = 50;
            // 
            // lblAlamat
            // 
            this.lblAlamat.Location = new System.Drawing.Point(28, 50);
            this.lblAlamat.Name = "lblAlamat";
            this.lblAlamat.Size = new System.Drawing.Size(27, 13);
            this.lblAlamat.TabIndex = 129;
            this.lblAlamat.Text = "Nama";
            // 
            // txtNama
            // 
            this.txtNama.Location = new System.Drawing.Point(97, 47);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(282, 20);
            this.txtNama.TabIndex = 60;
            // 
            // btnCari
            // 
            this.btnCari.Image = global::Kontenu.Properties.Resources.cari_16;
            this.btnCari.Location = new System.Drawing.Point(97, 125);
            this.btnCari.Name = "btnCari";
            this.btnCari.Size = new System.Drawing.Size(59, 23);
            this.btnCari.TabIndex = 100;
            this.btnCari.Text = "Cari";
            this.btnCari.Click += new System.EventHandler(this.btnCari_Click);
            // 
            // gridControl
            // 
            this.gridControl.Location = new System.Drawing.Point(11, 216);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(775, 313);
            this.gridControl.TabIndex = 110;
            this.gridControl.TabStop = false;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            // 
            // FrmPIC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 570);
            this.Controls.Add(this.gridControl);
            this.Controls.Add(this.groupControl);
            this.Controls.Add(this.btnCetak);
            this.Controls.Add(this.btnHapus);
            this.Controls.Add(this.btnUbah);
            this.Controls.Add(this.btnTambah);
            this.Name = "FrmPIC";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmPIC_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
            this.groupControl.ResumeLayout(false);
            this.groupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbJabatan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandphone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKota.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlamat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKTP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnTambah;
        private DevExpress.XtraEditors.SimpleButton btnUbah;
        private DevExpress.XtraEditors.SimpleButton btnHapus;
        private DevExpress.XtraEditors.SimpleButton btnCetak;
        private DevExpress.XtraEditors.GroupControl groupControl;
        private DevExpress.XtraEditors.SimpleButton btnCari;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtKTP;
        private DevExpress.XtraEditors.LabelControl lblAlamat;
        private DevExpress.XtraEditors.TextEdit txtNama;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtAlamat;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtKota;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtEmail;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtHandphone;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LookUpEdit cmbJabatan;
    }
}
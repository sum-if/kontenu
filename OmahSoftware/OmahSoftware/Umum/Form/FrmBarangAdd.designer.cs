namespace OmahSoftware.Umum {
    partial class FrmBarangAdd {
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
            this.btnSimpan = new DevExpress.XtraEditors.SimpleButton();
            this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.txtNoPart = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl20 = new DevExpress.XtraEditors.LabelControl();
            this.txtStokMin = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.cmbKategori = new DevExpress.XtraEditors.LookUpEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.cmbSatuan = new DevExpress.XtraEditors.LookUpEdit();
            this.txtCatatan = new DevExpress.XtraEditors.MemoEdit();
            this.txtKode = new DevExpress.XtraEditors.TextEdit();
            this.chkStatus = new System.Windows.Forms.CheckBox();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.txtNama = new DevExpress.XtraEditors.TextEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtHargaJual2 = new DevExpress.XtraEditors.TextEdit();
            this.txtHargaBeli = new DevExpress.XtraEditors.TextEdit();
            this.txtHargaJual1 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.cmbRak = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoPart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStokMin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbKategori.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSatuan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCatatan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHargaJual2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHargaBeli.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHargaJual1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRak.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSimpan
            // 
            this.btnSimpan.Image = global::OmahSoftware.Properties.Resources.simpan_16;
            this.btnSimpan.Location = new System.Drawing.Point(12, 385);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(75, 23);
            this.btnSimpan.TabIndex = 240;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.cmbRak);
            this.groupControl2.Controls.Add(this.txtNoPart);
            this.groupControl2.Controls.Add(this.labelControl6);
            this.groupControl2.Controls.Add(this.labelControl5);
            this.groupControl2.Controls.Add(this.labelControl20);
            this.groupControl2.Controls.Add(this.txtStokMin);
            this.groupControl2.Controls.Add(this.labelControl9);
            this.groupControl2.Controls.Add(this.cmbKategori);
            this.groupControl2.Location = new System.Drawing.Point(12, 241);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(639, 139);
            this.groupControl2.TabIndex = 277;
            this.groupControl2.Text = "Info Tambahan";
            // 
            // txtNoPart
            // 
            this.txtNoPart.Location = new System.Drawing.Point(166, 105);
            this.txtNoPart.Name = "txtNoPart";
            this.txtNoPart.Size = new System.Drawing.Size(461, 20);
            this.txtNoPart.TabIndex = 277;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(14, 108);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(36, 13);
            this.labelControl6.TabIndex = 278;
            this.labelControl6.Text = "No Part";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(14, 82);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(18, 13);
            this.labelControl5.TabIndex = 276;
            this.labelControl5.Text = "Rak";
            // 
            // labelControl20
            // 
            this.labelControl20.Location = new System.Drawing.Point(13, 56);
            this.labelControl20.Name = "labelControl20";
            this.labelControl20.Size = new System.Drawing.Size(64, 13);
            this.labelControl20.TabIndex = 274;
            this.labelControl20.Text = "Stok Minimum";
            // 
            // txtStokMin
            // 
            this.txtStokMin.Location = new System.Drawing.Point(166, 53);
            this.txtStokMin.Name = "txtStokMin";
            this.txtStokMin.Size = new System.Drawing.Size(461, 20);
            this.txtStokMin.TabIndex = 160;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(14, 30);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(40, 13);
            this.labelControl9.TabIndex = 270;
            this.labelControl9.Text = "Kategori";
            // 
            // cmbKategori
            // 
            this.cmbKategori.Location = new System.Drawing.Point(166, 28);
            this.cmbKategori.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbKategori.Name = "cmbKategori";
            this.cmbKategori.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbKategori.Size = new System.Drawing.Size(461, 20);
            this.cmbKategori.TabIndex = 140;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.cmbSatuan);
            this.groupControl1.Controls.Add(this.txtCatatan);
            this.groupControl1.Controls.Add(this.txtKode);
            this.groupControl1.Controls.Add(this.chkStatus);
            this.groupControl1.Controls.Add(this.labelControl11);
            this.groupControl1.Controls.Add(this.labelControl8);
            this.groupControl1.Controls.Add(this.txtNama);
            this.groupControl1.Controls.Add(this.labelControl10);
            this.groupControl1.Controls.Add(this.labelControl7);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.txtHargaJual2);
            this.groupControl1.Controls.Add(this.txtHargaBeli);
            this.groupControl1.Controls.Add(this.txtHargaJual1);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Location = new System.Drawing.Point(12, 11);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(639, 226);
            this.groupControl1.TabIndex = 276;
            this.groupControl1.Text = "Barang";
            // 
            // cmbSatuan
            // 
            this.cmbSatuan.Location = new System.Drawing.Point(166, 82);
            this.cmbSatuan.Name = "cmbSatuan";
            this.cmbSatuan.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbSatuan.Size = new System.Drawing.Size(175, 20);
            this.cmbSatuan.TabIndex = 270;
            // 
            // txtCatatan
            // 
            this.txtCatatan.Location = new System.Drawing.Point(166, 137);
            this.txtCatatan.Name = "txtCatatan";
            this.txtCatatan.Size = new System.Drawing.Size(461, 57);
            this.txtCatatan.TabIndex = 120;
            // 
            // txtKode
            // 
            this.txtKode.Location = new System.Drawing.Point(166, 32);
            this.txtKode.Name = "txtKode";
            this.txtKode.Size = new System.Drawing.Size(461, 20);
            this.txtKode.TabIndex = 20;
            // 
            // chkStatus
            // 
            this.chkStatus.AutoSize = true;
            this.chkStatus.Location = new System.Drawing.Point(166, 200);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(48, 17);
            this.chkStatus.TabIndex = 130;
            this.chkStatus.Text = "Aktif";
            this.chkStatus.UseVisualStyleBackColor = true;
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(14, 34);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(24, 13);
            this.labelControl11.TabIndex = 189;
            this.labelControl11.Text = "Kode";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(13, 139);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(56, 13);
            this.labelControl8.TabIndex = 269;
            this.labelControl8.Text = "Keterangan";
            // 
            // txtNama
            // 
            this.txtNama.Location = new System.Drawing.Point(166, 56);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(461, 20);
            this.txtNama.TabIndex = 30;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(14, 59);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(27, 13);
            this.labelControl10.TabIndex = 252;
            this.labelControl10.Text = "Nama";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(306, 200);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(0, 13);
            this.labelControl7.TabIndex = 267;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 85);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(34, 13);
            this.labelControl1.TabIndex = 253;
            this.labelControl1.Text = "Satuan";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(14, 110);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 13);
            this.labelControl2.TabIndex = 254;
            this.labelControl2.Text = "Harga Jual 1";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(352, 113);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 13);
            this.labelControl3.TabIndex = 255;
            this.labelControl3.Text = "Harga Jual 2";
            // 
            // txtHargaJual2
            // 
            this.txtHargaJual2.Location = new System.Drawing.Point(452, 110);
            this.txtHargaJual2.Name = "txtHargaJual2";
            this.txtHargaJual2.Size = new System.Drawing.Size(175, 20);
            this.txtHargaJual2.TabIndex = 80;
            // 
            // txtHargaBeli
            // 
            this.txtHargaBeli.Enabled = false;
            this.txtHargaBeli.Location = new System.Drawing.Point(452, 82);
            this.txtHargaBeli.Name = "txtHargaBeli";
            this.txtHargaBeli.Size = new System.Drawing.Size(175, 20);
            this.txtHargaBeli.TabIndex = 70;
            // 
            // txtHargaJual1
            // 
            this.txtHargaJual1.Location = new System.Drawing.Point(166, 110);
            this.txtHargaJual1.Name = "txtHargaJual1";
            this.txtHargaJual1.Size = new System.Drawing.Size(175, 20);
            this.txtHargaJual1.TabIndex = 60;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(352, 84);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(90, 13);
            this.labelControl4.TabIndex = 260;
            this.labelControl4.Text = "Harga Beli Terakhir";
            // 
            // cmbRak
            // 
            this.cmbRak.Location = new System.Drawing.Point(166, 79);
            this.cmbRak.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbRak.Name = "cmbRak";
            this.cmbRak.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbRak.Size = new System.Drawing.Size(461, 20);
            this.cmbRak.TabIndex = 279;
            // 
            // FrmBarangAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 416);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.btnSimpan);
            this.Name = "FrmBarangAdd";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmBarangAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoPart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStokMin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbKategori.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSatuan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCatatan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHargaJual2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHargaBeli.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHargaJual1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRak.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnSimpan;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.TextEdit txtNoPart;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl20;
        private DevExpress.XtraEditors.TextEdit txtStokMin;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LookUpEdit cmbKategori;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LookUpEdit cmbSatuan;
        private DevExpress.XtraEditors.MemoEdit txtCatatan;
        public DevExpress.XtraEditors.TextEdit txtKode;
        private System.Windows.Forms.CheckBox chkStatus;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.TextEdit txtNama;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtHargaJual2;
        private DevExpress.XtraEditors.TextEdit txtHargaBeli;
        private DevExpress.XtraEditors.TextEdit txtHargaJual1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LookUpEdit cmbRak;
    }
}
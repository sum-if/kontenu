﻿namespace Kontenu.Akuntansi {
    partial class FrmAkunSubKategori {
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
            this.cmbAkunKategori = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblAlamat = new DevExpress.XtraEditors.LabelControl();
            this.txtNama = new DevExpress.XtraEditors.TextEdit();
            this.txtKode = new DevExpress.XtraEditors.TextEdit();
            this.lblNama = new DevExpress.XtraEditors.LabelControl();
            this.btnCari = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
            this.groupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunKategori.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).BeginInit();
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
            this.groupControl.Controls.Add(this.cmbAkunKategori);
            this.groupControl.Controls.Add(this.labelControl1);
            this.groupControl.Controls.Add(this.lblAlamat);
            this.groupControl.Controls.Add(this.txtNama);
            this.groupControl.Controls.Add(this.txtKode);
            this.groupControl.Controls.Add(this.lblNama);
            this.groupControl.Controls.Add(this.btnCari);
            this.groupControl.Location = new System.Drawing.Point(12, 54);
            this.groupControl.Name = "groupControl";
            this.groupControl.Size = new System.Drawing.Size(775, 140);
            this.groupControl.TabIndex = 31;
            this.groupControl.Text = "Pencarian";
            // 
            // cmbAkunKategori
            // 
            this.cmbAkunKategori.Location = new System.Drawing.Point(97, 81);
            this.cmbAkunKategori.Name = "cmbAkunKategori";
            this.cmbAkunKategori.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAkunKategori.Properties.View = this.searchLookUpEdit1View;
            this.cmbAkunKategori.Size = new System.Drawing.Size(282, 20);
            this.cmbAkunKategori.TabIndex = 70;
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(22, 84);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(40, 13);
            this.labelControl1.TabIndex = 61;
            this.labelControl1.Text = "Kategori";
            // 
            // lblAlamat
            // 
            this.lblAlamat.Location = new System.Drawing.Point(22, 57);
            this.lblAlamat.Name = "lblAlamat";
            this.lblAlamat.Size = new System.Drawing.Size(27, 13);
            this.lblAlamat.TabIndex = 13;
            this.lblAlamat.Text = "Nama";
            // 
            // txtNama
            // 
            this.txtNama.Location = new System.Drawing.Point(97, 54);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(282, 20);
            this.txtNama.TabIndex = 60;
            // 
            // txtKode
            // 
            this.txtKode.Location = new System.Drawing.Point(97, 28);
            this.txtKode.Name = "txtKode";
            this.txtKode.Size = new System.Drawing.Size(282, 20);
            this.txtKode.TabIndex = 50;
            // 
            // lblNama
            // 
            this.lblNama.Location = new System.Drawing.Point(22, 31);
            this.lblNama.Name = "lblNama";
            this.lblNama.Size = new System.Drawing.Size(24, 13);
            this.lblNama.TabIndex = 3;
            this.lblNama.Text = "Kode";
            // 
            // btnCari
            // 
            this.btnCari.Image = global::Kontenu.Properties.Resources.cari_16;
            this.btnCari.Location = new System.Drawing.Point(97, 107);
            this.btnCari.Name = "btnCari";
            this.btnCari.Size = new System.Drawing.Size(59, 23);
            this.btnCari.TabIndex = 80;
            this.btnCari.Text = "Cari";
            this.btnCari.Click += new System.EventHandler(this.btnCari_Click);
            // 
            // gridControl
            // 
            this.gridControl.Location = new System.Drawing.Point(12, 200);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(775, 315);
            this.gridControl.TabIndex = 90;
            this.gridControl.TabStop = false;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            // 
            // FrmAkunSubKategori
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 524);
            this.Controls.Add(this.gridControl);
            this.Controls.Add(this.groupControl);
            this.Controls.Add(this.btnCetak);
            this.Controls.Add(this.btnHapus);
            this.Controls.Add(this.btnUbah);
            this.Controls.Add(this.btnTambah);
            this.Name = "FrmAkunSubKategori";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmAkunSubKategori_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
            this.groupControl.ResumeLayout(false);
            this.groupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunKategori.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).EndInit();
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
        private DevExpress.XtraEditors.LabelControl lblNama;
        private DevExpress.XtraEditors.TextEdit txtKode;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraEditors.TextEdit txtNama;
        private DevExpress.XtraEditors.LabelControl lblAlamat;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbAkunKategori;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
    }
}
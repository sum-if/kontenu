﻿namespace Kontenu.Sistem {
    partial class SisUserGroup {
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
            this.cmbStatus = new DevExpress.XtraEditors.LookUpEdit();
            this.lblStatus = new DevExpress.XtraEditors.LabelControl();
            this.txtNama = new DevExpress.XtraEditors.TextEdit();
            this.txtKode = new DevExpress.XtraEditors.TextEdit();
            this.lblNama = new DevExpress.XtraEditors.LabelControl();
            this.lblKode = new DevExpress.XtraEditors.LabelControl();
            this.btnCari = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
            this.groupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStatus.Properties)).BeginInit();
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
            this.groupControl.Controls.Add(this.cmbStatus);
            this.groupControl.Controls.Add(this.lblStatus);
            this.groupControl.Controls.Add(this.txtNama);
            this.groupControl.Controls.Add(this.txtKode);
            this.groupControl.Controls.Add(this.lblNama);
            this.groupControl.Controls.Add(this.lblKode);
            this.groupControl.Controls.Add(this.btnCari);
            this.groupControl.Location = new System.Drawing.Point(12, 54);
            this.groupControl.Name = "groupControl";
            this.groupControl.Size = new System.Drawing.Size(663, 141);
            this.groupControl.TabIndex = 31;
            this.groupControl.Text = "Pencarian";
            // 
            // cmbStatus
            // 
            this.cmbStatus.Location = new System.Drawing.Point(77, 80);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbStatus.Size = new System.Drawing.Size(345, 20);
            this.cmbStatus.TabIndex = 70;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(22, 83);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(31, 13);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Status";
            // 
            // txtNama
            // 
            this.txtNama.Location = new System.Drawing.Point(77, 54);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(345, 20);
            this.txtNama.TabIndex = 60;
            // 
            // txtKode
            // 
            this.txtKode.Location = new System.Drawing.Point(77, 28);
            this.txtKode.Name = "txtKode";
            this.txtKode.Size = new System.Drawing.Size(345, 20);
            this.txtKode.TabIndex = 50;
            // 
            // lblNama
            // 
            this.lblNama.Location = new System.Drawing.Point(22, 57);
            this.lblNama.Name = "lblNama";
            this.lblNama.Size = new System.Drawing.Size(27, 13);
            this.lblNama.TabIndex = 3;
            this.lblNama.Text = "Nama";
            // 
            // lblKode
            // 
            this.lblKode.Location = new System.Drawing.Point(22, 31);
            this.lblKode.Name = "lblKode";
            this.lblKode.Size = new System.Drawing.Size(24, 13);
            this.lblKode.TabIndex = 2;
            this.lblKode.Text = "Kode";
            // 
            // btnCari
            // 
            this.btnCari.Image = global::Kontenu.Properties.Resources.cari_16;
            this.btnCari.Location = new System.Drawing.Point(77, 106);
            this.btnCari.Name = "btnCari";
            this.btnCari.Size = new System.Drawing.Size(59, 23);
            this.btnCari.TabIndex = 80;
            this.btnCari.Text = "Cari";
            this.btnCari.Click += new System.EventHandler(this.btnCari_Click);
            // 
            // gridControl
            // 
            this.gridControl.Location = new System.Drawing.Point(12, 206);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(663, 309);
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
            // SisUserGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 524);
            this.Controls.Add(this.gridControl);
            this.Controls.Add(this.groupControl);
            this.Controls.Add(this.btnCetak);
            this.Controls.Add(this.btnHapus);
            this.Controls.Add(this.btnUbah);
            this.Controls.Add(this.btnTambah);
            this.Name = "SisUserGroup";
            this.Text = "Master User Group";
            this.Load += new System.EventHandler(this.MstUserGroup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
            this.groupControl.ResumeLayout(false);
            this.groupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStatus.Properties)).EndInit();
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
        private DevExpress.XtraEditors.LabelControl lblKode;
        private DevExpress.XtraEditors.LabelControl lblNama;
        private DevExpress.XtraEditors.TextEdit txtKode;
        private DevExpress.XtraEditors.TextEdit txtNama;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraEditors.LabelControl lblStatus;
        private DevExpress.XtraEditors.LookUpEdit cmbStatus;
    }
}
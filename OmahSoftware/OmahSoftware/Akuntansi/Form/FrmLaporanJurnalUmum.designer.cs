﻿namespace OmahSoftware.Akuntansi {
    partial class FrmLaporanJurnalUmum {
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
            this.groupControl = new DevExpress.XtraEditors.GroupControl();
            this.btnCetak = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.deTanggalAkhir = new DevExpress.XtraEditors.DateEdit();
            this.deTanggalAwal = new DevExpress.XtraEditors.DateEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtReferensi = new DevExpress.XtraEditors.TextEdit();
            this.cmbAkun = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
            this.groupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAkhir.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAkhir.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAwal.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAwal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReferensi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkun.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl
            // 
            this.groupControl.Controls.Add(this.cmbAkun);
            this.groupControl.Controls.Add(this.labelControl3);
            this.groupControl.Controls.Add(this.btnCetak);
            this.groupControl.Controls.Add(this.labelControl11);
            this.groupControl.Controls.Add(this.deTanggalAkhir);
            this.groupControl.Controls.Add(this.deTanggalAwal);
            this.groupControl.Controls.Add(this.labelControl10);
            this.groupControl.Controls.Add(this.labelControl1);
            this.groupControl.Controls.Add(this.txtReferensi);
            this.groupControl.Location = new System.Drawing.Point(12, 12);
            this.groupControl.Name = "groupControl";
            this.groupControl.Size = new System.Drawing.Size(406, 133);
            this.groupControl.TabIndex = 31;
            this.groupControl.Text = "Pencarian";
            // 
            // btnCetak
            // 
            this.btnCetak.Image = global::OmahSoftware.Properties.Resources.cetak_16;
            this.btnCetak.Location = new System.Drawing.Point(97, 101);
            this.btnCetak.Name = "btnCetak";
            this.btnCetak.Size = new System.Drawing.Size(75, 23);
            this.btnCetak.TabIndex = 251;
            this.btnCetak.Text = "Cetak";
            this.btnCetak.Click += new System.EventHandler(this.btnCetak_Click);
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(236, 26);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(4, 13);
            this.labelControl11.TabIndex = 152;
            this.labelControl11.Text = "-";
            // 
            // deTanggalAkhir
            // 
            this.deTanggalAkhir.EditValue = null;
            this.deTanggalAkhir.Location = new System.Drawing.Point(246, 23);
            this.deTanggalAkhir.Name = "deTanggalAkhir";
            this.deTanggalAkhir.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggalAkhir.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggalAkhir.Size = new System.Drawing.Size(133, 20);
            this.deTanggalAkhir.TabIndex = 151;
            // 
            // deTanggalAwal
            // 
            this.deTanggalAwal.EditValue = null;
            this.deTanggalAwal.Location = new System.Drawing.Point(97, 23);
            this.deTanggalAwal.Name = "deTanggalAwal";
            this.deTanggalAwal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggalAwal.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggalAwal.Size = new System.Drawing.Size(133, 20);
            this.deTanggalAwal.TabIndex = 150;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(28, 26);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(38, 13);
            this.labelControl10.TabIndex = 149;
            this.labelControl10.Text = "Tanggal";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(28, 78);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(46, 13);
            this.labelControl1.TabIndex = 133;
            this.labelControl1.Text = "Referensi";
            // 
            // txtReferensi
            // 
            this.txtReferensi.Location = new System.Drawing.Point(97, 75);
            this.txtReferensi.Name = "txtReferensi";
            this.txtReferensi.Size = new System.Drawing.Size(282, 20);
            this.txtReferensi.TabIndex = 50;
            // 
            // cmbAkun
            // 
            this.cmbAkun.Location = new System.Drawing.Point(97, 49);
            this.cmbAkun.Name = "cmbAkun";
            this.cmbAkun.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAkun.Properties.View = this.searchLookUpEdit1View;
            this.cmbAkun.Size = new System.Drawing.Size(282, 20);
            this.cmbAkun.TabIndex = 253;
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(28, 52);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(24, 13);
            this.labelControl3.TabIndex = 252;
            this.labelControl3.Text = "Akun";
            // 
            // FrmLaporanJurnalUmum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 163);
            this.Controls.Add(this.groupControl);
            this.Name = "FrmLaporanJurnalUmum";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmLaporanJurnalUmum_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
            this.groupControl.ResumeLayout(false);
            this.groupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAkhir.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAkhir.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAwal.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAwal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReferensi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkun.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtReferensi;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.DateEdit deTanggalAkhir;
        private DevExpress.XtraEditors.DateEdit deTanggalAwal;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.SimpleButton btnCetak;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbAkun;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}
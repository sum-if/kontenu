namespace OmahSoftware.Akuntansi {
    partial class FrmLaporanPriceListUpdate {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLaporanPriceListUpdate));
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnFilter = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnCetak = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl = new DevExpress.XtraEditors.GroupControl();
            this.btnCetakPriceListUpdate = new DevExpress.XtraEditors.SimpleButton();
            this.txtNamaBarang = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtKodeBarang = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbKategori = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.deTanggalAkhir = new DevExpress.XtraEditors.DateEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.deTanggalAwal = new DevExpress.XtraEditors.DateEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
            this.groupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNamaBarang.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKodeBarang.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbKategori.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAkhir.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAkhir.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAwal.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAwal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl
            // 
            this.gridControl.Location = new System.Drawing.Point(12, 186);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(802, 329);
            this.gridControl.TabIndex = 130;
            this.gridControl.TabStop = false;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            // 
            // btnFilter
            // 
            this.btnFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnFilter.Image")));
            this.btnFilter.Location = new System.Drawing.Point(120, 135);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(59, 23);
            this.btnFilter.TabIndex = 120;
            this.btnFilter.Text = "Filter";
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(14, 60);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(40, 13);
            this.labelControl3.TabIndex = 66;
            this.labelControl3.Text = "Kategori";
            // 
            // btnCetak
            // 
            this.btnCetak.Image = global::OmahSoftware.Properties.Resources.cetak_16;
            this.btnCetak.Location = new System.Drawing.Point(185, 135);
            this.btnCetak.Name = "btnCetak";
            this.btnCetak.Size = new System.Drawing.Size(101, 23);
            this.btnCetak.TabIndex = 232;
            this.btnCetak.Text = "Cetak Tabel";
            this.btnCetak.Click += new System.EventHandler(this.btnCetak_Click);
            // 
            // groupControl
            // 
            this.groupControl.Controls.Add(this.deTanggalAkhir);
            this.groupControl.Controls.Add(this.labelControl4);
            this.groupControl.Controls.Add(this.deTanggalAwal);
            this.groupControl.Controls.Add(this.labelControl5);
            this.groupControl.Controls.Add(this.btnCetakPriceListUpdate);
            this.groupControl.Controls.Add(this.txtNamaBarang);
            this.groupControl.Controls.Add(this.labelControl1);
            this.groupControl.Controls.Add(this.txtKodeBarang);
            this.groupControl.Controls.Add(this.labelControl2);
            this.groupControl.Controls.Add(this.cmbKategori);
            this.groupControl.Controls.Add(this.btnCetak);
            this.groupControl.Controls.Add(this.labelControl3);
            this.groupControl.Controls.Add(this.btnFilter);
            this.groupControl.Location = new System.Drawing.Point(12, 12);
            this.groupControl.Name = "groupControl";
            this.groupControl.Size = new System.Drawing.Size(802, 168);
            this.groupControl.TabIndex = 31;
            this.groupControl.Text = "Pencarian";
            // 
            // btnCetakPriceListUpdate
            // 
            this.btnCetakPriceListUpdate.Image = global::OmahSoftware.Properties.Resources.cetak_16;
            this.btnCetakPriceListUpdate.Location = new System.Drawing.Point(292, 135);
            this.btnCetakPriceListUpdate.Name = "btnCetakPriceListUpdate";
            this.btnCetakPriceListUpdate.Size = new System.Drawing.Size(113, 23);
            this.btnCetakPriceListUpdate.TabIndex = 241;
            this.btnCetakPriceListUpdate.Text = "Cetak Price List";
            this.btnCetakPriceListUpdate.Click += new System.EventHandler(this.btnCetakPriceListUpdate_Click);
            // 
            // txtNamaBarang
            // 
            this.txtNamaBarang.Location = new System.Drawing.Point(120, 109);
            this.txtNamaBarang.Name = "txtNamaBarang";
            this.txtNamaBarang.Size = new System.Drawing.Size(297, 20);
            this.txtNamaBarang.TabIndex = 240;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 112);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(64, 13);
            this.labelControl1.TabIndex = 239;
            this.labelControl1.Text = "Nama Barang";
            // 
            // txtKodeBarang
            // 
            this.txtKodeBarang.Location = new System.Drawing.Point(120, 83);
            this.txtKodeBarang.Name = "txtKodeBarang";
            this.txtKodeBarang.Size = new System.Drawing.Size(297, 20);
            this.txtKodeBarang.TabIndex = 238;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(14, 86);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(61, 13);
            this.labelControl2.TabIndex = 237;
            this.labelControl2.Text = "Kode Barang";
            // 
            // cmbKategori
            // 
            this.cmbKategori.Location = new System.Drawing.Point(120, 57);
            this.cmbKategori.Name = "cmbKategori";
            this.cmbKategori.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbKategori.Properties.View = this.searchLookUpEdit1View;
            this.cmbKategori.Size = new System.Drawing.Size(297, 20);
            this.cmbKategori.TabIndex = 233;
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // deTanggalAkhir
            // 
            this.deTanggalAkhir.EditValue = null;
            this.deTanggalAkhir.Location = new System.Drawing.Point(280, 31);
            this.deTanggalAkhir.Name = "deTanggalAkhir";
            this.deTanggalAkhir.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggalAkhir.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggalAkhir.Size = new System.Drawing.Size(137, 20);
            this.deTanggalAkhir.TabIndex = 243;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(265, 34);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(4, 13);
            this.labelControl4.TabIndex = 245;
            this.labelControl4.Text = "-";
            // 
            // deTanggalAwal
            // 
            this.deTanggalAwal.EditValue = null;
            this.deTanggalAwal.Location = new System.Drawing.Point(120, 31);
            this.deTanggalAwal.Name = "deTanggalAwal";
            this.deTanggalAwal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggalAwal.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggalAwal.Size = new System.Drawing.Size(137, 20);
            this.deTanggalAwal.TabIndex = 242;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(14, 34);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(93, 13);
            this.labelControl5.TabIndex = 244;
            this.labelControl5.Text = "Tanggal Perubahan";
            // 
            // FrmLaporanPriceListUpdate
            // 
            this.AcceptButton = this.btnFilter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 524);
            this.Controls.Add(this.gridControl);
            this.Controls.Add(this.groupControl);
            this.Name = "FrmLaporanPriceListUpdate";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmLaporanPriceListUpdate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
            this.groupControl.ResumeLayout(false);
            this.groupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNamaBarang.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKodeBarang.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbKategori.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAkhir.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAkhir.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAwal.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAwal.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraEditors.SimpleButton btnFilter;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btnCetak;
        private DevExpress.XtraEditors.GroupControl groupControl;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbKategori;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        public DevExpress.XtraEditors.TextEdit txtNamaBarang;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        public DevExpress.XtraEditors.TextEdit txtKodeBarang;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnCetakPriceListUpdate;
        private DevExpress.XtraEditors.DateEdit deTanggalAkhir;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.DateEdit deTanggalAwal;
        private DevExpress.XtraEditors.LabelControl labelControl5;
    }
}
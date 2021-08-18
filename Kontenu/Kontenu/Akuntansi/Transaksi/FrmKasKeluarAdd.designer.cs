namespace Kontenu.Akuntansi {
    partial class FrmKasKeluarAdd {
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.txtKode = new DevExpress.XtraEditors.TextEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtNoUrut = new DevExpress.XtraEditors.TextEdit();
            this.cmbAkunKasKeluar = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtCatatan = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.deTanggal = new DevExpress.XtraEditors.DateEdit();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.lblTotal = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnCetak = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoUrut.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunKasKeluar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCatatan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggal.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSimpan
            // 
            this.btnSimpan.Image = global::Kontenu.Properties.Resources.simpan_16;
            this.btnSimpan.Location = new System.Drawing.Point(12, 459);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(75, 23);
            this.btnSimpan.TabIndex = 60;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(17, 63);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(38, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Tanggal";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(17, 37);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(31, 13);
            this.labelControl8.TabIndex = 177;
            this.labelControl8.Text = "Nomor";
            // 
            // txtKode
            // 
            this.txtKode.Location = new System.Drawing.Point(115, 34);
            this.txtKode.Name = "txtKode";
            this.txtKode.Size = new System.Drawing.Size(496, 20);
            this.txtKode.TabIndex = 10;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.Controls.Add(this.txtNoUrut);
            this.groupControl1.Controls.Add(this.cmbAkunKasKeluar);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.txtCatatan);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.deTanggal);
            this.groupControl1.Controls.Add(this.labelControl8);
            this.groupControl1.Controls.Add(this.txtKode);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Location = new System.Drawing.Point(12, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(629, 206);
            this.groupControl1.TabIndex = 189;
            this.groupControl1.Text = "Data Kas Keluar";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(17, 115);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(37, 13);
            this.labelControl6.TabIndex = 186;
            this.labelControl6.Text = "No Urut";
            // 
            // txtNoUrut
            // 
            this.txtNoUrut.Location = new System.Drawing.Point(115, 112);
            this.txtNoUrut.Name = "txtNoUrut";
            this.txtNoUrut.Size = new System.Drawing.Size(496, 20);
            this.txtNoUrut.TabIndex = 185;
            // 
            // cmbAkunKasKeluar
            // 
            this.cmbAkunKasKeluar.Location = new System.Drawing.Point(115, 86);
            this.cmbAkunKasKeluar.Name = "cmbAkunKasKeluar";
            this.cmbAkunKasKeluar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAkunKasKeluar.Properties.View = this.searchLookUpEdit1View;
            this.cmbAkunKasKeluar.Size = new System.Drawing.Size(496, 20);
            this.cmbAkunKasKeluar.TabIndex = 30;
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(17, 89);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(77, 13);
            this.labelControl4.TabIndex = 182;
            this.labelControl4.Text = "Akun Kas Keluar";
            // 
            // txtCatatan
            // 
            this.txtCatatan.Location = new System.Drawing.Point(115, 138);
            this.txtCatatan.Name = "txtCatatan";
            this.txtCatatan.Size = new System.Drawing.Size(496, 57);
            this.txtCatatan.TabIndex = 40;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(17, 140);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(39, 13);
            this.labelControl2.TabIndex = 180;
            this.labelControl2.Text = "Catatan";
            // 
            // deTanggal
            // 
            this.deTanggal.EditValue = null;
            this.deTanggal.Location = new System.Drawing.Point(115, 60);
            this.deTanggal.Name = "deTanggal";
            this.deTanggal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggal.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggal.Size = new System.Drawing.Size(496, 20);
            this.deTanggal.TabIndex = 20;
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(13, 224);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(628, 187);
            this.gridControl1.TabIndex = 50;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gridControl1_ProcessGridKey);
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.gridView1_FocusedColumnChanged);
            this.gridView1.RowDeleted += new DevExpress.Data.RowDeletedEventHandler(this.gridView1_RowDeleted);
            this.gridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView1_ValidateRow);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.lblTotal);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Location = new System.Drawing.Point(13, 417);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(628, 36);
            this.panelControl1.TabIndex = 191;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.labelControl5.Location = new System.Drawing.Point(398, 5);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(6, 19);
            this.labelControl5.TabIndex = 183;
            this.labelControl5.Text = ":";
            // 
            // lblTotal
            // 
            this.lblTotal.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblTotal.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblTotal.Location = new System.Drawing.Point(454, 5);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(164, 19);
            this.lblTotal.TabIndex = 182;
            this.lblTotal.Text = "Total";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Location = new System.Drawing.Point(349, 5);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(43, 19);
            this.labelControl3.TabIndex = 181;
            this.labelControl3.Text = "Total";
            // 
            // btnCetak
            // 
            this.btnCetak.Image = global::Kontenu.Properties.Resources.cetak_16;
            this.btnCetak.Location = new System.Drawing.Point(93, 459);
            this.btnCetak.Name = "btnCetak";
            this.btnCetak.Size = new System.Drawing.Size(75, 23);
            this.btnCetak.TabIndex = 70;
            this.btnCetak.Text = "Cetak";
            this.btnCetak.Click += new System.EventHandler(this.btnCetak_Click);
            // 
            // FrmKasKeluarAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 493);
            this.Controls.Add(this.btnCetak);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.btnSimpan);
            this.Name = "FrmKasKeluarAdd";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmKasKeluarAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoUrut.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunKasKeluar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCatatan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggal.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnSimpan;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        public DevExpress.XtraEditors.TextEdit txtKode;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.DateEdit deTanggal;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.MemoEdit txtCatatan;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl lblTotal;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbAkunKasKeluar;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraEditors.SimpleButton btnCetak;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        public DevExpress.XtraEditors.TextEdit txtNoUrut;
    }
}
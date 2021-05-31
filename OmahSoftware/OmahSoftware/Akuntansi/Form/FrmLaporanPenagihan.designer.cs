namespace OmahSoftware.Akuntansi {
    partial class FrmLaporanPenagihan {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLaporanPenagihan));
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnFilter = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl = new DevExpress.XtraEditors.GroupControl();
            this.btnCetakPenagihan = new DevExpress.XtraEditors.SimpleButton();
            this.txtNamaCustomer = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtKodeCustomer = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbKotaGroup = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
            this.groupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNamaCustomer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKodeCustomer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbKotaGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl
            // 
            this.gridControl.Location = new System.Drawing.Point(12, 160);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(802, 355);
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
            this.btnFilter.Location = new System.Drawing.Point(93, 106);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(59, 23);
            this.btnFilter.TabIndex = 120;
            this.btnFilter.Text = "Filter";
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(11, 31);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(54, 13);
            this.labelControl3.TabIndex = 66;
            this.labelControl3.Text = "Kota Group";
            // 
            // groupControl
            // 
            this.groupControl.Controls.Add(this.btnCetakPenagihan);
            this.groupControl.Controls.Add(this.txtNamaCustomer);
            this.groupControl.Controls.Add(this.labelControl1);
            this.groupControl.Controls.Add(this.txtKodeCustomer);
            this.groupControl.Controls.Add(this.labelControl2);
            this.groupControl.Controls.Add(this.cmbKotaGroup);
            this.groupControl.Controls.Add(this.labelControl3);
            this.groupControl.Controls.Add(this.btnFilter);
            this.groupControl.Location = new System.Drawing.Point(12, 12);
            this.groupControl.Name = "groupControl";
            this.groupControl.Size = new System.Drawing.Size(802, 142);
            this.groupControl.TabIndex = 31;
            this.groupControl.Text = "Pencarian";
            // 
            // btnCetakPenagihan
            // 
            this.btnCetakPenagihan.Image = global::OmahSoftware.Properties.Resources.cetak_16;
            this.btnCetakPenagihan.Location = new System.Drawing.Point(158, 106);
            this.btnCetakPenagihan.Name = "btnCetakPenagihan";
            this.btnCetakPenagihan.Size = new System.Drawing.Size(113, 23);
            this.btnCetakPenagihan.TabIndex = 241;
            this.btnCetakPenagihan.Text = "Cetak Penagihan";
            this.btnCetakPenagihan.Click += new System.EventHandler(this.btnCetakPenagihan_Click);
            // 
            // txtNamaCustomer
            // 
            this.txtNamaCustomer.Location = new System.Drawing.Point(93, 80);
            this.txtNamaCustomer.Name = "txtNamaCustomer";
            this.txtNamaCustomer.Size = new System.Drawing.Size(297, 20);
            this.txtNamaCustomer.TabIndex = 240;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(11, 83);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(76, 13);
            this.labelControl1.TabIndex = 239;
            this.labelControl1.Text = "Nama Customer";
            // 
            // txtKodeCustomer
            // 
            this.txtKodeCustomer.Location = new System.Drawing.Point(93, 54);
            this.txtKodeCustomer.Name = "txtKodeCustomer";
            this.txtKodeCustomer.Size = new System.Drawing.Size(297, 20);
            this.txtKodeCustomer.TabIndex = 238;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(11, 57);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(73, 13);
            this.labelControl2.TabIndex = 237;
            this.labelControl2.Text = "Kode Customer";
            // 
            // cmbKotaGroup
            // 
            this.cmbKotaGroup.Location = new System.Drawing.Point(93, 28);
            this.cmbKotaGroup.Name = "cmbKotaGroup";
            this.cmbKotaGroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbKotaGroup.Properties.View = this.searchLookUpEdit1View;
            this.cmbKotaGroup.Size = new System.Drawing.Size(297, 20);
            this.cmbKotaGroup.TabIndex = 233;
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // FrmLaporanPenagihan
            // 
            this.AcceptButton = this.btnFilter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 524);
            this.Controls.Add(this.gridControl);
            this.Controls.Add(this.groupControl);
            this.Name = "FrmLaporanPenagihan";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmLaporanPenagihan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
            this.groupControl.ResumeLayout(false);
            this.groupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNamaCustomer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKodeCustomer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbKotaGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraEditors.SimpleButton btnFilter;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.GroupControl groupControl;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbKotaGroup;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        public DevExpress.XtraEditors.TextEdit txtNamaCustomer;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        public DevExpress.XtraEditors.TextEdit txtKodeCustomer;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnCetakPenagihan;
    }
}
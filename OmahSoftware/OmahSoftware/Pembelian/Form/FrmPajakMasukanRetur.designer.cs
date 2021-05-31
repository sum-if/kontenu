namespace OmahSoftware.Pembelian {
    partial class FrmPajakMasukanRetur {
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
            this.cmbBulan = new DevExpress.XtraEditors.LookUpEdit();
            this.lblPasswordLama = new DevExpress.XtraEditors.LabelControl();
            this.cmbTahun = new DevExpress.XtraEditors.LookUpEdit();
            this.cmbSupplier = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.chkUpload = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBulan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTahun.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSupplier.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUpload.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCetak
            // 
            this.btnCetak.Image = global::OmahSoftware.Properties.Resources.cetak_16;
            this.btnCetak.Location = new System.Drawing.Point(89, 86);
            this.btnCetak.Name = "btnCetak";
            this.btnCetak.Size = new System.Drawing.Size(75, 23);
            this.btnCetak.TabIndex = 252;
            this.btnCetak.Text = "Cetak";
            this.btnCetak.Click += new System.EventHandler(this.btnCetak_Click);
            // 
            // cmbBulan
            // 
            this.cmbBulan.Location = new System.Drawing.Point(89, 9);
            this.cmbBulan.Name = "cmbBulan";
            this.cmbBulan.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBulan.Size = new System.Drawing.Size(174, 20);
            this.cmbBulan.TabIndex = 253;
            // 
            // lblPasswordLama
            // 
            this.lblPasswordLama.Location = new System.Drawing.Point(21, 12);
            this.lblPasswordLama.Name = "lblPasswordLama";
            this.lblPasswordLama.Size = new System.Drawing.Size(54, 13);
            this.lblPasswordLama.TabIndex = 255;
            this.lblPasswordLama.Text = "Masa Pajak";
            // 
            // cmbTahun
            // 
            this.cmbTahun.Location = new System.Drawing.Point(269, 9);
            this.cmbTahun.Name = "cmbTahun";
            this.cmbTahun.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTahun.Size = new System.Drawing.Size(174, 20);
            this.cmbTahun.TabIndex = 257;
            // 
            // cmbSupplier
            // 
            this.cmbSupplier.Location = new System.Drawing.Point(89, 35);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbSupplier.Properties.View = this.searchLookUpEdit1View;
            this.cmbSupplier.Size = new System.Drawing.Size(354, 20);
            this.cmbSupplier.TabIndex = 259;
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
            this.labelControl3.Location = new System.Drawing.Point(21, 38);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(38, 13);
            this.labelControl3.TabIndex = 258;
            this.labelControl3.Text = "Supplier";
            // 
            // chkUpload
            // 
            this.chkUpload.Location = new System.Drawing.Point(89, 61);
            this.chkUpload.Name = "chkUpload";
            this.chkUpload.Properties.Caption = "Belum Upload";
            this.chkUpload.Size = new System.Drawing.Size(113, 19);
            this.chkUpload.TabIndex = 260;
            // 
            // FrmPajakMasukanRetur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 119);
            this.Controls.Add(this.chkUpload);
            this.Controls.Add(this.cmbSupplier);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.cmbTahun);
            this.Controls.Add(this.cmbBulan);
            this.Controls.Add(this.lblPasswordLama);
            this.Controls.Add(this.btnCetak);
            this.Name = "FrmPajakMasukanRetur";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmPajakMasukanRetur_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbBulan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTahun.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSupplier.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUpload.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnCetak;
        private DevExpress.XtraEditors.LookUpEdit cmbBulan;
        private DevExpress.XtraEditors.LabelControl lblPasswordLama;
        private DevExpress.XtraEditors.LookUpEdit cmbTahun;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbSupplier;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.CheckEdit chkUpload;

    }
}
namespace OmahSoftware.Akuntansi {
    partial class FrmLaporanLabaRugi {
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbBulan = new DevExpress.XtraEditors.LookUpEdit();
            this.lblPasswordLama = new DevExpress.XtraEditors.LabelControl();
            this.cmbTahun = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBulan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTahun.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCetak
            // 
            this.btnCetak.Image = global::OmahSoftware.Properties.Resources.cetak_16;
            this.btnCetak.Location = new System.Drawing.Point(89, 61);
            this.btnCetak.Name = "btnCetak";
            this.btnCetak.Size = new System.Drawing.Size(75, 23);
            this.btnCetak.TabIndex = 252;
            this.btnCetak.Text = "Cetak";
            this.btnCetak.Click += new System.EventHandler(this.btnCetak_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(21, 38);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(30, 13);
            this.labelControl1.TabIndex = 256;
            this.labelControl1.Text = "Tahun";
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
            this.lblPasswordLama.Size = new System.Drawing.Size(26, 13);
            this.lblPasswordLama.TabIndex = 255;
            this.lblPasswordLama.Text = "Bulan";
            // 
            // cmbTahun
            // 
            this.cmbTahun.Location = new System.Drawing.Point(89, 35);
            this.cmbTahun.Name = "cmbTahun";
            this.cmbTahun.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTahun.Size = new System.Drawing.Size(174, 20);
            this.cmbTahun.TabIndex = 257;
            // 
            // FrmLaporanLabaRugi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 94);
            this.Controls.Add(this.cmbTahun);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.cmbBulan);
            this.Controls.Add(this.lblPasswordLama);
            this.Controls.Add(this.btnCetak);
            this.Name = "FrmLaporanLabaRugi";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmLaporanLabaRugi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbBulan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTahun.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnCetak;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LookUpEdit cmbBulan;
        private DevExpress.XtraEditors.LabelControl lblPasswordLama;
        private DevExpress.XtraEditors.LookUpEdit cmbTahun;

    }
}
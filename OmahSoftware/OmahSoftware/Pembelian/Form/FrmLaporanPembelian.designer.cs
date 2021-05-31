namespace OmahSoftware.Pembelian {
    partial class FrmLaporanPembelian {
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
            this.lblPasswordLama = new DevExpress.XtraEditors.LabelControl();
            this.txtSupplier = new DevExpress.XtraEditors.TextEdit();
            this.btnCariCustomer = new DevExpress.XtraEditors.SimpleButton();
            this.btnCetak = new DevExpress.XtraEditors.SimpleButton();
            this.deTanggalAkhir = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.deTanggalAwal = new DevExpress.XtraEditors.DateEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplier.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAkhir.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAkhir.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAwal.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAwal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPasswordLama
            // 
            this.lblPasswordLama.Location = new System.Drawing.Point(17, 38);
            this.lblPasswordLama.Name = "lblPasswordLama";
            this.lblPasswordLama.Size = new System.Drawing.Size(38, 13);
            this.lblPasswordLama.TabIndex = 255;
            this.lblPasswordLama.Text = "Supplier";
            // 
            // txtSupplier
            // 
            this.txtSupplier.EditValue = "";
            this.txtSupplier.Enabled = false;
            this.txtSupplier.Location = new System.Drawing.Point(92, 35);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Size = new System.Drawing.Size(215, 20);
            this.txtSupplier.TabIndex = 257;
            // 
            // btnCariCustomer
            // 
            this.btnCariCustomer.Image = global::OmahSoftware.Properties.Resources.cari_16;
            this.btnCariCustomer.Location = new System.Drawing.Point(313, 33);
            this.btnCariCustomer.Name = "btnCariCustomer";
            this.btnCariCustomer.Size = new System.Drawing.Size(55, 23);
            this.btnCariCustomer.TabIndex = 258;
            this.btnCariCustomer.Text = "Cari";
            this.btnCariCustomer.Click += new System.EventHandler(this.btnCariSupplier_Click);
            // 
            // btnCetak
            // 
            this.btnCetak.Image = global::OmahSoftware.Properties.Resources.cetak_16;
            this.btnCetak.Location = new System.Drawing.Point(92, 59);
            this.btnCetak.Name = "btnCetak";
            this.btnCetak.Size = new System.Drawing.Size(75, 23);
            this.btnCetak.TabIndex = 252;
            this.btnCetak.Text = "Cetak";
            this.btnCetak.Click += new System.EventHandler(this.btnCetak_Click);
            // 
            // deTanggalAkhir
            // 
            this.deTanggalAkhir.EditValue = null;
            this.deTanggalAkhir.Location = new System.Drawing.Point(238, 9);
            this.deTanggalAkhir.Name = "deTanggalAkhir";
            this.deTanggalAkhir.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggalAkhir.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggalAkhir.Size = new System.Drawing.Size(130, 20);
            this.deTanggalAkhir.TabIndex = 262;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(228, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(4, 13);
            this.labelControl2.TabIndex = 264;
            this.labelControl2.Text = "-";
            // 
            // deTanggalAwal
            // 
            this.deTanggalAwal.EditValue = null;
            this.deTanggalAwal.Location = new System.Drawing.Point(92, 9);
            this.deTanggalAwal.Name = "deTanggalAwal";
            this.deTanggalAwal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggalAwal.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggalAwal.Size = new System.Drawing.Size(130, 20);
            this.deTanggalAwal.TabIndex = 261;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(17, 12);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(38, 13);
            this.labelControl3.TabIndex = 263;
            this.labelControl3.Text = "Tanggal";
            // 
            // FrmLaporanPembelian
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 92);
            this.Controls.Add(this.deTanggalAkhir);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.deTanggalAwal);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.btnCariCustomer);
            this.Controls.Add(this.txtSupplier);
            this.Controls.Add(this.lblPasswordLama);
            this.Controls.Add(this.btnCetak);
            this.Name = "FrmLaporanPembelian";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmLaporanPembelian_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplier.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAkhir.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAkhir.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAwal.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggalAwal.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnCetak;
        private DevExpress.XtraEditors.LabelControl lblPasswordLama;
        private DevExpress.XtraEditors.SimpleButton btnCariCustomer;
        public DevExpress.XtraEditors.TextEdit txtSupplier;
        private DevExpress.XtraEditors.DateEdit deTanggalAkhir;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit deTanggalAwal;
        private DevExpress.XtraEditors.LabelControl labelControl3;

    }
}
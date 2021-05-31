namespace OmahSoftware.Persediaan {
    partial class FrmPersediaan {
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
            this.chkStok = new System.Windows.Forms.CheckBox();
            this.chkDiBawahStokMinimum = new System.Windows.Forms.CheckBox();
            this.btnCetak = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtKode = new DevExpress.XtraEditors.TextEdit();
            this.lblAlamat = new DevExpress.XtraEditors.LabelControl();
            this.txtNama = new DevExpress.XtraEditors.TextEdit();
            this.btnCari = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtNoPart = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
            this.groupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoPart.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl
            // 
            this.groupControl.Controls.Add(this.labelControl2);
            this.groupControl.Controls.Add(this.txtNoPart);
            this.groupControl.Controls.Add(this.chkStok);
            this.groupControl.Controls.Add(this.chkDiBawahStokMinimum);
            this.groupControl.Controls.Add(this.btnCetak);
            this.groupControl.Controls.Add(this.labelControl1);
            this.groupControl.Controls.Add(this.txtKode);
            this.groupControl.Controls.Add(this.lblAlamat);
            this.groupControl.Controls.Add(this.txtNama);
            this.groupControl.Controls.Add(this.btnCari);
            this.groupControl.Location = new System.Drawing.Point(12, 12);
            this.groupControl.Name = "groupControl";
            this.groupControl.Size = new System.Drawing.Size(806, 157);
            this.groupControl.TabIndex = 31;
            this.groupControl.Text = "Pencarian";
            // 
            // chkStok
            // 
            this.chkStok.AutoSize = true;
            this.chkStok.Location = new System.Drawing.Point(243, 101);
            this.chkStok.Name = "chkStok";
            this.chkStok.Size = new System.Drawing.Size(101, 17);
            this.chkStok.TabIndex = 154;
            this.chkStok.Text = "Hanya ada stok";
            this.chkStok.UseVisualStyleBackColor = true;
            // 
            // chkDiBawahStokMinimum
            // 
            this.chkDiBawahStokMinimum.AutoSize = true;
            this.chkDiBawahStokMinimum.Location = new System.Drawing.Point(86, 101);
            this.chkDiBawahStokMinimum.Name = "chkDiBawahStokMinimum";
            this.chkDiBawahStokMinimum.Size = new System.Drawing.Size(137, 17);
            this.chkDiBawahStokMinimum.TabIndex = 153;
            this.chkDiBawahStokMinimum.Text = "Di Bawah Stok Minimum";
            this.chkDiBawahStokMinimum.UseVisualStyleBackColor = true;
            // 
            // btnCetak
            // 
            this.btnCetak.Image = global::OmahSoftware.Properties.Resources.cetak_16;
            this.btnCetak.Location = new System.Drawing.Point(150, 124);
            this.btnCetak.Name = "btnCetak";
            this.btnCetak.Size = new System.Drawing.Size(90, 23);
            this.btnCetak.TabIndex = 150;
            this.btnCetak.Text = "Cetak [F4]";
            this.btnCetak.Click += new System.EventHandler(this.btnCetak_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(17, 26);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 13);
            this.labelControl1.TabIndex = 133;
            this.labelControl1.Text = "Kode";
            // 
            // txtKode
            // 
            this.txtKode.Location = new System.Drawing.Point(86, 23);
            this.txtKode.Name = "txtKode";
            this.txtKode.Size = new System.Drawing.Size(282, 20);
            this.txtKode.TabIndex = 50;
            // 
            // lblAlamat
            // 
            this.lblAlamat.Location = new System.Drawing.Point(17, 52);
            this.lblAlamat.Name = "lblAlamat";
            this.lblAlamat.Size = new System.Drawing.Size(27, 13);
            this.lblAlamat.TabIndex = 129;
            this.lblAlamat.Text = "Nama";
            // 
            // txtNama
            // 
            this.txtNama.Location = new System.Drawing.Point(86, 49);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(282, 20);
            this.txtNama.TabIndex = 60;
            // 
            // btnCari
            // 
            this.btnCari.Image = global::OmahSoftware.Properties.Resources.cari_16;
            this.btnCari.Location = new System.Drawing.Point(85, 124);
            this.btnCari.Name = "btnCari";
            this.btnCari.Size = new System.Drawing.Size(59, 23);
            this.btnCari.TabIndex = 140;
            this.btnCari.Text = "Cari";
            this.btnCari.Click += new System.EventHandler(this.btnCari_Click);
            // 
            // gridControl
            // 
            this.gridControl.Location = new System.Drawing.Point(12, 175);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(806, 257);
            this.gridControl.TabIndex = 150;
            this.gridControl.TabStop = false;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(17, 78);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(36, 13);
            this.labelControl2.TabIndex = 156;
            this.labelControl2.Text = "No Part";
            // 
            // txtNoPart
            // 
            this.txtNoPart.Location = new System.Drawing.Point(86, 75);
            this.txtNoPart.Name = "txtNoPart";
            this.txtNoPart.Size = new System.Drawing.Size(282, 20);
            this.txtNoPart.TabIndex = 155;
            // 
            // FrmPersediaan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 443);
            this.Controls.Add(this.gridControl);
            this.Controls.Add(this.groupControl);
            this.Name = "FrmPersediaan";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmPersediaan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
            this.groupControl.ResumeLayout(false);
            this.groupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoPart.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl;
        private DevExpress.XtraEditors.SimpleButton btnCari;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtKode;
        private DevExpress.XtraEditors.LabelControl lblAlamat;
        private DevExpress.XtraEditors.TextEdit txtNama;
        private DevExpress.XtraEditors.SimpleButton btnCetak;
        private System.Windows.Forms.CheckBox chkDiBawahStokMinimum;
        private System.Windows.Forms.CheckBox chkStok;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtNoPart;
    }
}
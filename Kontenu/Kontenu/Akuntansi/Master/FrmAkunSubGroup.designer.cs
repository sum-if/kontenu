namespace Kontenu.Akuntansi {
    partial class FrmAkunSubGroup {
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
            this.cmbAkunGroup = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lblAlamat = new DevExpress.XtraEditors.LabelControl();
            this.txtNama = new DevExpress.XtraEditors.TextEdit();
            this.txtKode = new DevExpress.XtraEditors.TextEdit();
            this.lblNama = new DevExpress.XtraEditors.LabelControl();
            this.btnCari = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
            this.groupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCetak
            // 
            this.btnCetak.Image = global::Kontenu.Properties.Resources.cetak_16;
            this.btnCetak.Location = new System.Drawing.Point(351, 20);
            this.btnCetak.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCetak.Name = "btnCetak";
            this.btnCetak.Size = new System.Drawing.Size(105, 28);
            this.btnCetak.TabIndex = 40;
            this.btnCetak.Text = "Cetak [F4]";
            this.btnCetak.Click += new System.EventHandler(this.btnCetak_Click);
            // 
            // btnHapus
            // 
            this.btnHapus.Image = global::Kontenu.Properties.Resources.hapus_16;
            this.btnHapus.Location = new System.Drawing.Point(239, 20);
            this.btnHapus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(105, 28);
            this.btnHapus.TabIndex = 30;
            this.btnHapus.Text = "Hapus [F3]";
            this.btnHapus.Click += new System.EventHandler(this.btnHapus_Click);
            // 
            // btnUbah
            // 
            this.btnUbah.Image = global::Kontenu.Properties.Resources.ubah_16;
            this.btnUbah.Location = new System.Drawing.Point(127, 20);
            this.btnUbah.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUbah.Name = "btnUbah";
            this.btnUbah.Size = new System.Drawing.Size(105, 28);
            this.btnUbah.TabIndex = 20;
            this.btnUbah.Text = "Ubah [F2]";
            this.btnUbah.Click += new System.EventHandler(this.btnUbah_Click);
            // 
            // btnTambah
            // 
            this.btnTambah.Image = global::Kontenu.Properties.Resources.tambah_16;
            this.btnTambah.Location = new System.Drawing.Point(15, 20);
            this.btnTambah.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(105, 28);
            this.btnTambah.TabIndex = 10;
            this.btnTambah.Text = "Tambah [F1]";
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // groupControl
            // 
            this.groupControl.Controls.Add(this.cmbAkunGroup);
            this.groupControl.Controls.Add(this.labelControl3);
            this.groupControl.Controls.Add(this.lblAlamat);
            this.groupControl.Controls.Add(this.txtNama);
            this.groupControl.Controls.Add(this.txtKode);
            this.groupControl.Controls.Add(this.lblNama);
            this.groupControl.Controls.Add(this.btnCari);
            this.groupControl.Location = new System.Drawing.Point(14, 66);
            this.groupControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl.Name = "groupControl";
            this.groupControl.Size = new System.Drawing.Size(904, 170);
            this.groupControl.TabIndex = 31;
            this.groupControl.Text = "Pencarian";
            // 
            // cmbAkunGroup
            // 
            this.cmbAkunGroup.Location = new System.Drawing.Point(113, 96);
            this.cmbAkunGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbAkunGroup.Name = "cmbAkunGroup";
            this.cmbAkunGroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAkunGroup.Properties.View = this.gridView2;
            this.cmbAkunGroup.Size = new System.Drawing.Size(329, 22);
            this.cmbAkunGroup.TabIndex = 70;
            // 
            // gridView2
            // 
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(26, 99);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(34, 16);
            this.labelControl3.TabIndex = 65;
            this.labelControl3.Text = "Group";
            // 
            // lblAlamat
            // 
            this.lblAlamat.Location = new System.Drawing.Point(26, 70);
            this.lblAlamat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblAlamat.Name = "lblAlamat";
            this.lblAlamat.Size = new System.Drawing.Size(33, 16);
            this.lblAlamat.TabIndex = 13;
            this.lblAlamat.Text = "Nama";
            // 
            // txtNama
            // 
            this.txtNama.Location = new System.Drawing.Point(113, 66);
            this.txtNama.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(329, 22);
            this.txtNama.TabIndex = 60;
            // 
            // txtKode
            // 
            this.txtKode.Location = new System.Drawing.Point(113, 34);
            this.txtKode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtKode.Name = "txtKode";
            this.txtKode.Size = new System.Drawing.Size(329, 22);
            this.txtKode.TabIndex = 50;
            // 
            // lblNama
            // 
            this.lblNama.Location = new System.Drawing.Point(26, 38);
            this.lblNama.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblNama.Name = "lblNama";
            this.lblNama.Size = new System.Drawing.Size(28, 16);
            this.lblNama.TabIndex = 3;
            this.lblNama.Text = "Kode";
            // 
            // btnCari
            // 
            this.btnCari.Image = global::Kontenu.Properties.Resources.cari_16;
            this.btnCari.Location = new System.Drawing.Point(113, 126);
            this.btnCari.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCari.Name = "btnCari";
            this.btnCari.Size = new System.Drawing.Size(69, 28);
            this.btnCari.TabIndex = 80;
            this.btnCari.Text = "Cari";
            this.btnCari.Click += new System.EventHandler(this.btnCari_Click);
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl.Location = new System.Drawing.Point(14, 244);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(904, 390);
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
            // FrmAkunSubGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 645);
            this.Controls.Add(this.gridControl);
            this.Controls.Add(this.groupControl);
            this.Controls.Add(this.btnCetak);
            this.Controls.Add(this.btnHapus);
            this.Controls.Add(this.btnUbah);
            this.Controls.Add(this.btnTambah);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmAkunSubGroup";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmAkunSubGroup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
            this.groupControl.ResumeLayout(false);
            this.groupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
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
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbAkunGroup;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
    }
}
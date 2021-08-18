namespace Kontenu.Akuntansi {
    partial class FrmAkun {
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
            this.cmbAkunSubGroup = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cmbAkunGroup = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cmbAkunSubKategori = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit2View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cmbAkunKategori = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblAlamat = new DevExpress.XtraEditors.LabelControl();
            this.txtNama = new DevExpress.XtraEditors.TextEdit();
            this.txtKode = new DevExpress.XtraEditors.TextEdit();
            this.lblNama = new DevExpress.XtraEditors.LabelControl();
            this.btnCari = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
            this.groupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunSubGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunSubKategori.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit2View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunKategori.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
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
            this.groupControl.Controls.Add(this.cmbAkunSubGroup);
            this.groupControl.Controls.Add(this.cmbAkunGroup);
            this.groupControl.Controls.Add(this.cmbAkunSubKategori);
            this.groupControl.Controls.Add(this.cmbAkunKategori);
            this.groupControl.Controls.Add(this.labelControl4);
            this.groupControl.Controls.Add(this.labelControl3);
            this.groupControl.Controls.Add(this.labelControl2);
            this.groupControl.Controls.Add(this.labelControl1);
            this.groupControl.Controls.Add(this.lblAlamat);
            this.groupControl.Controls.Add(this.txtNama);
            this.groupControl.Controls.Add(this.txtKode);
            this.groupControl.Controls.Add(this.lblNama);
            this.groupControl.Controls.Add(this.btnCari);
            this.groupControl.Location = new System.Drawing.Point(14, 66);
            this.groupControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl.Name = "groupControl";
            this.groupControl.Size = new System.Drawing.Size(904, 169);
            this.groupControl.TabIndex = 31;
            this.groupControl.Text = "Pencarian";
            // 
            // cmbAkunSubGroup
            // 
            this.cmbAkunSubGroup.Location = new System.Drawing.Point(554, 100);
            this.cmbAkunSubGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbAkunSubGroup.Name = "cmbAkunSubGroup";
            this.cmbAkunSubGroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAkunSubGroup.Properties.View = this.gridView1;
            this.cmbAkunSubGroup.Size = new System.Drawing.Size(329, 22);
            this.cmbAkunSubGroup.TabIndex = 100;
            // 
            // gridView1
            // 
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // cmbAkunGroup
            // 
            this.cmbAkunGroup.Location = new System.Drawing.Point(554, 67);
            this.cmbAkunGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbAkunGroup.Name = "cmbAkunGroup";
            this.cmbAkunGroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAkunGroup.Properties.View = this.gridView2;
            this.cmbAkunGroup.Size = new System.Drawing.Size(329, 22);
            this.cmbAkunGroup.TabIndex = 90;
            this.cmbAkunGroup.EditValueChanged += new System.EventHandler(this.cmbAkunGroup_EditValueChanged);
            // 
            // gridView2
            // 
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // cmbAkunSubKategori
            // 
            this.cmbAkunSubKategori.Location = new System.Drawing.Point(554, 35);
            this.cmbAkunSubKategori.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbAkunSubKategori.Name = "cmbAkunSubKategori";
            this.cmbAkunSubKategori.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAkunSubKategori.Properties.View = this.searchLookUpEdit2View;
            this.cmbAkunSubKategori.Size = new System.Drawing.Size(329, 22);
            this.cmbAkunSubKategori.TabIndex = 80;
            this.cmbAkunSubKategori.EditValueChanged += new System.EventHandler(this.cmbAkunSubKategori_EditValueChanged);
            // 
            // searchLookUpEdit2View
            // 
            this.searchLookUpEdit2View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit2View.Name = "searchLookUpEdit2View";
            this.searchLookUpEdit2View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit2View.OptionsView.ShowGroupPanel = false;
            // 
            // cmbAkunKategori
            // 
            this.cmbAkunKategori.Location = new System.Drawing.Point(113, 100);
            this.cmbAkunKategori.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbAkunKategori.Name = "cmbAkunKategori";
            this.cmbAkunKategori.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAkunKategori.Properties.View = this.searchLookUpEdit1View;
            this.cmbAkunKategori.Size = new System.Drawing.Size(329, 22);
            this.cmbAkunKategori.TabIndex = 70;
            this.cmbAkunKategori.EditValueChanged += new System.EventHandler(this.cmbAkunKategori_EditValueChanged);
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
            this.labelControl4.Location = new System.Drawing.Point(467, 103);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 16);
            this.labelControl4.TabIndex = 67;
            this.labelControl4.Text = "Sub Group";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(467, 70);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(34, 16);
            this.labelControl3.TabIndex = 65;
            this.labelControl3.Text = "Group";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(467, 38);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(73, 16);
            this.labelControl2.TabIndex = 63;
            this.labelControl2.Text = "Sub Kategori";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(26, 103);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(47, 16);
            this.labelControl1.TabIndex = 61;
            this.labelControl1.Text = "Kategori";
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
            this.btnCari.Location = new System.Drawing.Point(113, 130);
            this.btnCari.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCari.Name = "btnCari";
            this.btnCari.Size = new System.Drawing.Size(69, 28);
            this.btnCari.TabIndex = 110;
            this.btnCari.Text = "Cari";
            this.btnCari.Click += new System.EventHandler(this.btnCari_Click);
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl.Location = new System.Drawing.Point(14, 243);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(904, 390);
            this.gridControl.TabIndex = 120;
            this.gridControl.TabStop = false;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            // 
            // FrmAkun
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
            this.Name = "FrmAkun";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmAkun_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
            this.groupControl.ResumeLayout(false);
            this.groupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunSubGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunSubKategori.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit2View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunKategori.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
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
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbAkunSubKategori;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit2View;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbAkunKategori;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbAkunSubGroup;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbAkunGroup;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
    }
}
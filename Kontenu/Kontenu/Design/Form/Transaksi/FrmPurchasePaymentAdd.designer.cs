namespace Kontenu.Design {
    partial class FrmPurchasePaymentAdd {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPurchasePaymentAdd));
            this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
            this.xtraScrollableControl1 = new DevExpress.XtraEditors.XtraScrollableControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnSimpan = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.lblGrandTotal = new DevExpress.XtraEditors.LabelControl();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.cmbAkunKasBayar = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.btnCariOutsource = new DevExpress.XtraEditors.SimpleButton();
            this.deTanggal = new DevExpress.XtraEditors.DateEdit();
            this.txtKodeOutsource = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.txtKode = new DevExpress.XtraEditors.TextEdit();
            this.txtHandphone = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl18 = new DevExpress.XtraEditors.LabelControl();
            this.txtKota = new DevExpress.XtraEditors.TextEdit();
            this.txtEmail = new DevExpress.XtraEditors.TextEdit();
            this.labelControl26 = new DevExpress.XtraEditors.LabelControl();
            this.txtNama = new DevExpress.XtraEditors.TextEdit();
            this.txtAlamat = new DevExpress.XtraEditors.TextEdit();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            this.txtKodePos = new DevExpress.XtraEditors.TextEdit();
            this.txtTelepon = new DevExpress.XtraEditors.TextEdit();
            this.labelControl25 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl22 = new DevExpress.XtraEditors.LabelControl();
            this.txtProvinsi = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            this.xtraScrollableControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunKasBayar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggal.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKodeOutsource.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandphone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKota.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlamat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKodePos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTelepon.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProvinsi.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraScrollableControl1
            // 
            this.xtraScrollableControl1.Controls.Add(this.gridControl1);
            this.xtraScrollableControl1.Controls.Add(this.btnSimpan);
            this.xtraScrollableControl1.Controls.Add(this.panelControl1);
            this.xtraScrollableControl1.Controls.Add(this.groupControl1);
            this.xtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraScrollableControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraScrollableControl1.Name = "xtraScrollableControl1";
            this.xtraScrollableControl1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.xtraScrollableControl1.Size = new System.Drawing.Size(983, 581);
            this.xtraScrollableControl1.TabIndex = 0;
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(12, 309);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(960, 223);
            this.gridControl1.TabIndex = 261;
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
            // btnSimpan
            // 
            this.btnSimpan.Image = ((System.Drawing.Image)(resources.GetObject("btnSimpan.Image")));
            this.btnSimpan.Location = new System.Drawing.Point(12, 546);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(86, 23);
            this.btnSimpan.TabIndex = 250;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl13);
            this.panelControl1.Controls.Add(this.lblGrandTotal);
            this.panelControl1.Controls.Add(this.labelControl15);
            this.panelControl1.Location = new System.Drawing.Point(674, 538);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(298, 30);
            this.panelControl1.TabIndex = 203;
            // 
            // labelControl13
            // 
            this.labelControl13.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.labelControl13.Location = new System.Drawing.Point(108, 5);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(6, 19);
            this.labelControl13.TabIndex = 192;
            this.labelControl13.Text = ":";
            // 
            // lblGrandTotal
            // 
            this.lblGrandTotal.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblGrandTotal.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblGrandTotal.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblGrandTotal.Location = new System.Drawing.Point(120, 5);
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Size = new System.Drawing.Size(164, 19);
            this.lblGrandTotal.TabIndex = 191;
            this.lblGrandTotal.Text = "Grand Total";
            // 
            // labelControl15
            // 
            this.labelControl15.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.labelControl15.Location = new System.Drawing.Point(5, 5);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(97, 19);
            this.labelControl15.TabIndex = 190;
            this.labelControl15.Text = "Grand Total";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.cmbAkunKasBayar);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.btnCariOutsource);
            this.groupControl1.Controls.Add(this.deTanggal);
            this.groupControl1.Controls.Add(this.txtKodeOutsource);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl8);
            this.groupControl1.Controls.Add(this.labelControl17);
            this.groupControl1.Controls.Add(this.txtKode);
            this.groupControl1.Controls.Add(this.txtHandphone);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.labelControl18);
            this.groupControl1.Controls.Add(this.txtKota);
            this.groupControl1.Controls.Add(this.txtEmail);
            this.groupControl1.Controls.Add(this.labelControl26);
            this.groupControl1.Controls.Add(this.txtNama);
            this.groupControl1.Controls.Add(this.txtAlamat);
            this.groupControl1.Controls.Add(this.labelControl21);
            this.groupControl1.Controls.Add(this.txtKodePos);
            this.groupControl1.Controls.Add(this.txtTelepon);
            this.groupControl1.Controls.Add(this.labelControl25);
            this.groupControl1.Controls.Add(this.labelControl22);
            this.groupControl1.Controls.Add(this.txtProvinsi);
            this.groupControl1.Location = new System.Drawing.Point(12, 7);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(960, 296);
            this.groupControl1.TabIndex = 201;
            this.groupControl1.Text = "Data PurchasePayment";
            // 
            // cmbAkunKasBayar
            // 
            this.cmbAkunKasBayar.Location = new System.Drawing.Point(124, 81);
            this.cmbAkunKasBayar.Name = "cmbAkunKasBayar";
            this.cmbAkunKasBayar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAkunKasBayar.Properties.View = this.searchLookUpEdit1View;
            this.cmbAkunKasBayar.Size = new System.Drawing.Size(353, 20);
            this.cmbAkunKasBayar.TabIndex = 249;
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
            this.labelControl4.Location = new System.Drawing.Point(12, 84);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 13);
            this.labelControl4.TabIndex = 250;
            this.labelControl4.Text = "Kas Bayar";
            // 
            // btnCariOutsource
            // 
            this.btnCariOutsource.Image = global::Kontenu.Properties.Resources.cari_16;
            this.btnCariOutsource.Location = new System.Drawing.Point(421, 104);
            this.btnCariOutsource.Name = "btnCariOutsource";
            this.btnCariOutsource.Size = new System.Drawing.Size(55, 23);
            this.btnCariOutsource.TabIndex = 248;
            this.btnCariOutsource.Text = "Cari";
            this.btnCariOutsource.Click += new System.EventHandler(this.btnCariOutsource_Click);
            // 
            // deTanggal
            // 
            this.deTanggal.EditValue = null;
            this.deTanggal.Location = new System.Drawing.Point(123, 55);
            this.deTanggal.Name = "deTanggal";
            this.deTanggal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggal.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggal.Size = new System.Drawing.Size(353, 20);
            this.deTanggal.TabIndex = 20;
            // 
            // txtKodeOutsource
            // 
            this.txtKodeOutsource.Enabled = false;
            this.txtKodeOutsource.Location = new System.Drawing.Point(123, 107);
            this.txtKodeOutsource.Name = "txtKodeOutsource";
            this.txtKodeOutsource.Size = new System.Drawing.Size(292, 20);
            this.txtKodeOutsource.TabIndex = 246;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 110);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(77, 13);
            this.labelControl2.TabIndex = 247;
            this.labelControl2.Text = "Kode Outsource";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(12, 31);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(102, 13);
            this.labelControl8.TabIndex = 177;
            this.labelControl8.Text = "No PurchasePayment";
            // 
            // labelControl17
            // 
            this.labelControl17.Location = new System.Drawing.Point(13, 241);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(55, 13);
            this.labelControl17.TabIndex = 244;
            this.labelControl17.Text = "Handphone";
            // 
            // txtKode
            // 
            this.txtKode.Enabled = false;
            this.txtKode.Location = new System.Drawing.Point(123, 29);
            this.txtKode.Name = "txtKode";
            this.txtKode.Size = new System.Drawing.Size(353, 20);
            this.txtKode.TabIndex = 10;
            // 
            // txtHandphone
            // 
            this.txtHandphone.EditValue = "";
            this.txtHandphone.Enabled = false;
            this.txtHandphone.Location = new System.Drawing.Point(124, 238);
            this.txtHandphone.Name = "txtHandphone";
            this.txtHandphone.Size = new System.Drawing.Size(353, 20);
            this.txtHandphone.TabIndex = 243;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 57);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(38, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Tanggal";
            // 
            // labelControl18
            // 
            this.labelControl18.Location = new System.Drawing.Point(13, 267);
            this.labelControl18.Name = "labelControl18";
            this.labelControl18.Size = new System.Drawing.Size(24, 13);
            this.labelControl18.TabIndex = 215;
            this.labelControl18.Text = "Email";
            // 
            // txtKota
            // 
            this.txtKota.EditValue = "";
            this.txtKota.Enabled = false;
            this.txtKota.Location = new System.Drawing.Point(123, 185);
            this.txtKota.Name = "txtKota";
            this.txtKota.Size = new System.Drawing.Size(157, 20);
            this.txtKota.TabIndex = 150;
            // 
            // txtEmail
            // 
            this.txtEmail.EditValue = "";
            this.txtEmail.Enabled = false;
            this.txtEmail.Location = new System.Drawing.Point(124, 264);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(353, 20);
            this.txtEmail.TabIndex = 200;
            // 
            // labelControl26
            // 
            this.labelControl26.Location = new System.Drawing.Point(12, 162);
            this.labelControl26.Name = "labelControl26";
            this.labelControl26.Size = new System.Drawing.Size(33, 13);
            this.labelControl26.TabIndex = 204;
            this.labelControl26.Text = "Alamat";
            // 
            // txtNama
            // 
            this.txtNama.Enabled = false;
            this.txtNama.Location = new System.Drawing.Point(123, 133);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(353, 20);
            this.txtNama.TabIndex = 110;
            // 
            // txtAlamat
            // 
            this.txtAlamat.Enabled = false;
            this.txtAlamat.Location = new System.Drawing.Point(123, 159);
            this.txtAlamat.Name = "txtAlamat";
            this.txtAlamat.Size = new System.Drawing.Size(354, 20);
            this.txtAlamat.TabIndex = 120;
            // 
            // labelControl21
            // 
            this.labelControl21.Location = new System.Drawing.Point(12, 214);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(38, 13);
            this.labelControl21.TabIndex = 206;
            this.labelControl21.Text = "Telepon";
            // 
            // txtKodePos
            // 
            this.txtKodePos.EditValue = "";
            this.txtKodePos.Enabled = false;
            this.txtKodePos.Location = new System.Drawing.Point(404, 185);
            this.txtKodePos.Name = "txtKodePos";
            this.txtKodePos.Size = new System.Drawing.Size(72, 20);
            this.txtKodePos.TabIndex = 160;
            // 
            // txtTelepon
            // 
            this.txtTelepon.EditValue = "";
            this.txtTelepon.Enabled = false;
            this.txtTelepon.Location = new System.Drawing.Point(123, 211);
            this.txtTelepon.Name = "txtTelepon";
            this.txtTelepon.Size = new System.Drawing.Size(353, 20);
            this.txtTelepon.TabIndex = 170;
            // 
            // labelControl25
            // 
            this.labelControl25.Location = new System.Drawing.Point(12, 188);
            this.labelControl25.Name = "labelControl25";
            this.labelControl25.Size = new System.Drawing.Size(96, 13);
            this.labelControl25.TabIndex = 205;
            this.labelControl25.Text = "Kota/Prov/Kode Pos";
            // 
            // labelControl22
            // 
            this.labelControl22.Location = new System.Drawing.Point(12, 136);
            this.labelControl22.Name = "labelControl22";
            this.labelControl22.Size = new System.Drawing.Size(27, 13);
            this.labelControl22.TabIndex = 203;
            this.labelControl22.Text = "Nama";
            // 
            // txtProvinsi
            // 
            this.txtProvinsi.EditValue = "";
            this.txtProvinsi.Enabled = false;
            this.txtProvinsi.Location = new System.Drawing.Point(285, 185);
            this.txtProvinsi.Name = "txtProvinsi";
            this.txtProvinsi.Size = new System.Drawing.Size(113, 20);
            this.txtProvinsi.TabIndex = 140;
            // 
            // FrmPurchasePaymentAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 581);
            this.Controls.Add(this.xtraScrollableControl1);
            this.Name = "FrmPurchasePaymentAdd";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmPurchasePaymentAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            this.xtraScrollableControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunKasBayar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggal.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKodeOutsource.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandphone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKota.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlamat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKodePos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTelepon.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProvinsi.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
        private DevExpress.XtraEditors.XtraScrollableControl xtraScrollableControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.LabelControl lblGrandTotal;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraEditors.LabelControl labelControl18;
        private DevExpress.XtraEditors.TextEdit txtEmail;
        public DevExpress.XtraEditors.TextEdit txtNama;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        private DevExpress.XtraEditors.TextEdit txtTelepon;
        private DevExpress.XtraEditors.LabelControl labelControl22;
        private DevExpress.XtraEditors.TextEdit txtProvinsi;
        private DevExpress.XtraEditors.LabelControl labelControl25;
        private DevExpress.XtraEditors.TextEdit txtKodePos;
        private DevExpress.XtraEditors.TextEdit txtAlamat;
        private DevExpress.XtraEditors.LabelControl labelControl26;
        private DevExpress.XtraEditors.TextEdit txtKota;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.DateEdit deTanggal;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        public DevExpress.XtraEditors.TextEdit txtKode;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private DevExpress.XtraEditors.TextEdit txtHandphone;
        public DevExpress.XtraEditors.SimpleButton btnSimpan;
        public DevExpress.XtraEditors.TextEdit txtKodeOutsource;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        public DevExpress.XtraEditors.SimpleButton btnCariOutsource;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbAkunKasBayar;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}
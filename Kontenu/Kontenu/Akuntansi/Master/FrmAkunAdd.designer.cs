namespace Kontenu.Akuntansi {
    partial class FrmAkunAdd {
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
            this.txtNama = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.txtKode = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.chkStatus = new System.Windows.Forms.CheckBox();
            this.chkJurnalManual = new System.Windows.Forms.CheckBox();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.cmbAkunKategori = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cmbAkunSubKategori = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit2View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cmbAkunGroup = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit3View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cmbAkunSubGroup = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit4View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cmbSaldoNormal = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunKategori.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunSubKategori.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit2View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit3View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunSubGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit4View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSaldoNormal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSimpan
            // 
            this.btnSimpan.Image = global::Kontenu.Properties.Resources.simpan_16;
            this.btnSimpan.Location = new System.Drawing.Point(100, 217);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(75, 23);
            this.btnSimpan.TabIndex = 100;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // txtNama
            // 
            this.txtNama.Location = new System.Drawing.Point(100, 142);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(475, 20);
            this.txtNama.TabIndex = 60;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 145);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(27, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Nama";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(12, 15);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(40, 13);
            this.labelControl6.TabIndex = 172;
            this.labelControl6.Text = "Kategori";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(12, 119);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(24, 13);
            this.labelControl8.TabIndex = 177;
            this.labelControl8.Text = "Kode";
            // 
            // txtKode
            // 
            this.txtKode.Location = new System.Drawing.Point(100, 116);
            this.txtKode.Name = "txtKode";
            this.txtKode.Size = new System.Drawing.Size(475, 20);
            this.txtKode.TabIndex = 50;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 41);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(61, 13);
            this.labelControl2.TabIndex = 179;
            this.labelControl2.Text = "Sub Kategori";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(12, 67);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(29, 13);
            this.labelControl3.TabIndex = 181;
            this.labelControl3.Text = "Group";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(12, 93);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(50, 13);
            this.labelControl4.TabIndex = 183;
            this.labelControl4.Text = "Sub Group";
            // 
            // chkStatus
            // 
            this.chkStatus.AutoSize = true;
            this.chkStatus.Location = new System.Drawing.Point(198, 194);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(48, 17);
            this.chkStatus.TabIndex = 90;
            this.chkStatus.Text = "Aktif";
            this.chkStatus.UseVisualStyleBackColor = true;
            // 
            // chkJurnalManual
            // 
            this.chkJurnalManual.AutoSize = true;
            this.chkJurnalManual.Location = new System.Drawing.Point(100, 194);
            this.chkJurnalManual.Name = "chkJurnalManual";
            this.chkJurnalManual.Size = new System.Drawing.Size(92, 17);
            this.chkJurnalManual.TabIndex = 80;
            this.chkJurnalManual.Text = "Jurnal Manual";
            this.chkJurnalManual.UseVisualStyleBackColor = true;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(12, 171);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(62, 13);
            this.labelControl5.TabIndex = 187;
            this.labelControl5.Text = "Saldo Normal";
            // 
            // cmbAkunKategori
            // 
            this.cmbAkunKategori.Location = new System.Drawing.Point(100, 12);
            this.cmbAkunKategori.Name = "cmbAkunKategori";
            this.cmbAkunKategori.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAkunKategori.Properties.View = this.searchLookUpEdit1View;
            this.cmbAkunKategori.Size = new System.Drawing.Size(475, 20);
            this.cmbAkunKategori.TabIndex = 10;
            this.cmbAkunKategori.EditValueChanged += new System.EventHandler(this.cmbAkunKategori_EditValueChanged);
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // cmbAkunSubKategori
            // 
            this.cmbAkunSubKategori.Location = new System.Drawing.Point(100, 38);
            this.cmbAkunSubKategori.Name = "cmbAkunSubKategori";
            this.cmbAkunSubKategori.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAkunSubKategori.Properties.View = this.searchLookUpEdit2View;
            this.cmbAkunSubKategori.Size = new System.Drawing.Size(475, 20);
            this.cmbAkunSubKategori.TabIndex = 20;
            this.cmbAkunSubKategori.EditValueChanged += new System.EventHandler(this.cmbAkunSubKategori_EditValueChanged);
            // 
            // searchLookUpEdit2View
            // 
            this.searchLookUpEdit2View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit2View.Name = "searchLookUpEdit2View";
            this.searchLookUpEdit2View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit2View.OptionsView.ShowGroupPanel = false;
            // 
            // cmbAkunGroup
            // 
            this.cmbAkunGroup.Location = new System.Drawing.Point(100, 64);
            this.cmbAkunGroup.Name = "cmbAkunGroup";
            this.cmbAkunGroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAkunGroup.Properties.View = this.searchLookUpEdit3View;
            this.cmbAkunGroup.Size = new System.Drawing.Size(475, 20);
            this.cmbAkunGroup.TabIndex = 30;
            this.cmbAkunGroup.EditValueChanged += new System.EventHandler(this.cmbAkunGroup_EditValueChanged);
            // 
            // searchLookUpEdit3View
            // 
            this.searchLookUpEdit3View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit3View.Name = "searchLookUpEdit3View";
            this.searchLookUpEdit3View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit3View.OptionsView.ShowGroupPanel = false;
            // 
            // cmbAkunSubGroup
            // 
            this.cmbAkunSubGroup.Location = new System.Drawing.Point(100, 90);
            this.cmbAkunSubGroup.Name = "cmbAkunSubGroup";
            this.cmbAkunSubGroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAkunSubGroup.Properties.View = this.searchLookUpEdit4View;
            this.cmbAkunSubGroup.Size = new System.Drawing.Size(475, 20);
            this.cmbAkunSubGroup.TabIndex = 40;
            // 
            // searchLookUpEdit4View
            // 
            this.searchLookUpEdit4View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit4View.Name = "searchLookUpEdit4View";
            this.searchLookUpEdit4View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit4View.OptionsView.ShowGroupPanel = false;
            // 
            // cmbSaldoNormal
            // 
            this.cmbSaldoNormal.Location = new System.Drawing.Point(100, 168);
            this.cmbSaldoNormal.Name = "cmbSaldoNormal";
            this.cmbSaldoNormal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbSaldoNormal.Size = new System.Drawing.Size(475, 20);
            this.cmbSaldoNormal.TabIndex = 70;
            // 
            // FrmAkunAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 254);
            this.Controls.Add(this.cmbSaldoNormal);
            this.Controls.Add(this.cmbAkunSubGroup);
            this.Controls.Add(this.cmbAkunGroup);
            this.Controls.Add(this.cmbAkunSubKategori);
            this.Controls.Add(this.cmbAkunKategori);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.chkJurnalManual);
            this.Controls.Add(this.chkStatus);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtKode);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.btnSimpan);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtNama);
            this.Name = "FrmAkunAdd";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmAkunAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunKategori.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunSubKategori.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit2View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit3View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAkunSubGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit4View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSaldoNormal.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnSimpan;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        public DevExpress.XtraEditors.TextEdit txtNama;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        public DevExpress.XtraEditors.TextEdit txtKode;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.CheckBox chkStatus;
        private System.Windows.Forms.CheckBox chkJurnalManual;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbAkunKategori;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbAkunSubKategori;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit2View;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbAkunGroup;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit3View;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbAkunSubGroup;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit4View;
        private DevExpress.XtraEditors.LookUpEdit cmbSaldoNormal;
    }
}
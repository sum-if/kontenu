namespace Kontenu.Master {
    partial class FrmMaterialAdd {
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
            this.txtKode = new DevExpress.XtraEditors.TextEdit();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.txtNama = new DevExpress.XtraEditors.TextEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbKategoriMaterial = new DevExpress.XtraEditors.LookUpEdit();
            this.cmbUnit = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbKategoriMaterial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSimpan
            // 
            this.btnSimpan.Image = global::Kontenu.Properties.Resources.simpan_16;
            this.btnSimpan.Location = new System.Drawing.Point(94, 118);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(75, 23);
            this.btnSimpan.TabIndex = 110;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // txtKode
            // 
            this.txtKode.Location = new System.Drawing.Point(94, 12);
            this.txtKode.Name = "txtKode";
            this.txtKode.Size = new System.Drawing.Size(195, 20);
            this.txtKode.TabIndex = 10;
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(14, 15);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(24, 13);
            this.labelControl11.TabIndex = 189;
            this.labelControl11.Text = "Kode";
            // 
            // txtNama
            // 
            this.txtNama.Location = new System.Drawing.Point(94, 38);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(195, 20);
            this.txtNama.TabIndex = 30;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(14, 42);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(27, 13);
            this.labelControl10.TabIndex = 252;
            this.labelControl10.Text = "Nama";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 68);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(40, 13);
            this.labelControl1.TabIndex = 254;
            this.labelControl1.Text = "Kategori";
            // 
            // cmbKategoriMaterial
            // 
            this.cmbKategoriMaterial.Location = new System.Drawing.Point(94, 65);
            this.cmbKategoriMaterial.Name = "cmbKategoriMaterial";
            this.cmbKategoriMaterial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbKategoriMaterial.Size = new System.Drawing.Size(195, 20);
            this.cmbKategoriMaterial.TabIndex = 40;
            // 
            // cmbUnit
            // 
            this.cmbUnit.Location = new System.Drawing.Point(94, 91);
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbUnit.Size = new System.Drawing.Size(195, 20);
            this.cmbUnit.TabIndex = 255;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(14, 94);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(19, 13);
            this.labelControl2.TabIndex = 256;
            this.labelControl2.Text = "Unit";
            // 
            // FrmMaterialAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 153);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cmbKategoriMaterial);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtKode);
            this.Controls.Add(this.labelControl11);
            this.Controls.Add(this.txtNama);
            this.Controls.Add(this.btnSimpan);
            this.Controls.Add(this.labelControl10);
            this.Name = "FrmMaterialAdd";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmMaterialAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbKategoriMaterial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnit.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnSimpan;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
        public DevExpress.XtraEditors.TextEdit txtKode;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.TextEdit txtNama;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LookUpEdit cmbKategoriMaterial;
        private DevExpress.XtraEditors.LookUpEdit cmbUnit;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}
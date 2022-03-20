namespace Kontenu.Master {
    partial class FrmKategoriMaterialAdd {
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
            this.cmbCosting = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCosting.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSimpan
            // 
            this.btnSimpan.Image = global::Kontenu.Properties.Resources.simpan_16;
            this.btnSimpan.Location = new System.Drawing.Point(68, 90);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(75, 23);
            this.btnSimpan.TabIndex = 110;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // txtKode
            // 
            this.txtKode.Location = new System.Drawing.Point(68, 12);
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
            this.txtNama.Location = new System.Drawing.Point(68, 38);
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
            // cmbCosting
            // 
            this.cmbCosting.Location = new System.Drawing.Point(68, 64);
            this.cmbCosting.Name = "cmbCosting";
            this.cmbCosting.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbCosting.Size = new System.Drawing.Size(195, 20);
            this.cmbCosting.TabIndex = 253;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 67);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 13);
            this.labelControl1.TabIndex = 252;
            this.labelControl1.Text = "Costing";
            // 
            // FrmKategoriMaterialAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 122);
            this.Controls.Add(this.cmbCosting);
            this.Controls.Add(this.txtKode);
            this.Controls.Add(this.labelControl11);
            this.Controls.Add(this.txtNama);
            this.Controls.Add(this.btnSimpan);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl10);
            this.Name = "FrmKategoriMaterialAdd";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmKategoriMaterialAdd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCosting.Properties)).EndInit();
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
        private DevExpress.XtraEditors.LookUpEdit cmbCosting;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}
namespace OmahSoftware.Akuntansi {
    partial class FrmLaporanPostDatedCheque
    {
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
            this.rdoTanggal = new System.Windows.Forms.RadioButton();
            this.rdoAktual = new System.Windows.Forms.RadioButton();
            this.btnCetak = new DevExpress.XtraEditors.SimpleButton();
            this.deTanggal = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
            this.groupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggal.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl
            // 
            this.groupControl.Controls.Add(this.rdoTanggal);
            this.groupControl.Controls.Add(this.rdoAktual);
            this.groupControl.Controls.Add(this.btnCetak);
            this.groupControl.Controls.Add(this.deTanggal);
            this.groupControl.Location = new System.Drawing.Point(12, 12);
            this.groupControl.Name = "groupControl";
            this.groupControl.Size = new System.Drawing.Size(406, 118);
            this.groupControl.TabIndex = 31;
            this.groupControl.Text = "Pencarian";
            // 
            // rdoTanggal
            // 
            this.rdoTanggal.AutoSize = true;
            this.rdoTanggal.Location = new System.Drawing.Point(28, 57);
            this.rdoTanggal.Name = "rdoTanggal";
            this.rdoTanggal.Size = new System.Drawing.Size(82, 17);
            this.rdoTanggal.TabIndex = 253;
            this.rdoTanggal.TabStop = true;
            this.rdoTanggal.Text = "Per Tanggal";
            this.rdoTanggal.UseVisualStyleBackColor = true;
            this.rdoTanggal.CheckedChanged += new System.EventHandler(this.rdoTanggal_CheckedChanged);
            // 
            // rdoAktual
            // 
            this.rdoAktual.AutoSize = true;
            this.rdoAktual.Checked = true;
            this.rdoAktual.Location = new System.Drawing.Point(28, 34);
            this.rdoAktual.Name = "rdoAktual";
            this.rdoAktual.Size = new System.Drawing.Size(55, 17);
            this.rdoAktual.TabIndex = 252;
            this.rdoAktual.TabStop = true;
            this.rdoAktual.Text = "Aktual";
            this.rdoAktual.UseVisualStyleBackColor = true;
            this.rdoAktual.CheckedChanged += new System.EventHandler(this.rdoAktual_CheckedChanged);
            // 
            // btnCetak
            // 
            this.btnCetak.Image = global::OmahSoftware.Properties.Resources.cetak_16;
            this.btnCetak.Location = new System.Drawing.Point(123, 80);
            this.btnCetak.Name = "btnCetak";
            this.btnCetak.Size = new System.Drawing.Size(75, 23);
            this.btnCetak.TabIndex = 251;
            this.btnCetak.Text = "Cetak";
            this.btnCetak.Click += new System.EventHandler(this.btnCetak_Click);
            // 
            // deTanggal
            // 
            this.deTanggal.EditValue = null;
            this.deTanggal.Enabled = false;
            this.deTanggal.Location = new System.Drawing.Point(123, 54);
            this.deTanggal.Name = "deTanggal";
            this.deTanggal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggal.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTanggal.Size = new System.Drawing.Size(133, 20);
            this.deTanggal.TabIndex = 150;
            // 
            // FrmLaporanPostDatedCheque
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 139);
            this.Controls.Add(this.groupControl);
            this.Name = "FrmLaporanPostDatedCheque";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.FrmLaporanPostDatedCheque_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
            this.groupControl.ResumeLayout(false);
            this.groupControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggal.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTanggal.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl;
        private DevExpress.XtraEditors.DateEdit deTanggal;
        private DevExpress.XtraEditors.SimpleButton btnCetak;
        private System.Windows.Forms.RadioButton rdoTanggal;
        private System.Windows.Forms.RadioButton rdoAktual;
    }
}
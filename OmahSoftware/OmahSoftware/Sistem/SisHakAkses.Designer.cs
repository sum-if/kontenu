namespace OmahSoftware.Sistem {
    partial class SisHakAkses {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SisHakAkses));
            this.lblUserGroup = new DevExpress.XtraEditors.LabelControl();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnSimpan = new DevExpress.XtraEditors.SimpleButton();
            this.cmbUsergroup = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnPilihSemua = new DevExpress.XtraEditors.SimpleButton();
            this.btnHapusSemua = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUsergroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            this.SuspendLayout();
            // 
            // lblUserGroup
            // 
            this.lblUserGroup.Location = new System.Drawing.Point(12, 19);
            this.lblUserGroup.Name = "lblUserGroup";
            this.lblUserGroup.Size = new System.Drawing.Size(54, 13);
            this.lblUserGroup.TabIndex = 0;
            this.lblUserGroup.Text = "User Group";
            // 
            // gridControl
            // 
            this.gridControl.Location = new System.Drawing.Point(12, 71);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(645, 401);
            this.gridControl.TabIndex = 30;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            // 
            // btnSimpan
            // 
            this.btnSimpan.Image = global::OmahSoftware.Properties.Resources.simpan_16;
            this.btnSimpan.Location = new System.Drawing.Point(88, 42);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(75, 23);
            this.btnSimpan.TabIndex = 20;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // cmbUsergroup
            // 
            this.cmbUsergroup.Location = new System.Drawing.Point(88, 16);
            this.cmbUsergroup.Name = "cmbUsergroup";
            this.cmbUsergroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbUsergroup.Properties.View = this.searchLookUpEdit1View;
            this.cmbUsergroup.Size = new System.Drawing.Size(324, 20);
            this.cmbUsergroup.TabIndex = 10;
            this.cmbUsergroup.EditValueChanged += new System.EventHandler(this.cmbUsergroup_EditValueChanged);
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // btnPilihSemua
            // 
            this.btnPilihSemua.Image = ((System.Drawing.Image)(resources.GetObject("btnPilihSemua.Image")));
            this.btnPilihSemua.Location = new System.Drawing.Point(215, 42);
            this.btnPilihSemua.Name = "btnPilihSemua";
            this.btnPilihSemua.Size = new System.Drawing.Size(95, 23);
            this.btnPilihSemua.TabIndex = 31;
            this.btnPilihSemua.Text = "Pilih Semua";
            this.btnPilihSemua.Click += new System.EventHandler(this.btnPilihSemua_Click);
            // 
            // btnHapusSemua
            // 
            this.btnHapusSemua.Image = ((System.Drawing.Image)(resources.GetObject("btnHapusSemua.Image")));
            this.btnHapusSemua.Location = new System.Drawing.Point(317, 42);
            this.btnHapusSemua.Name = "btnHapusSemua";
            this.btnHapusSemua.Size = new System.Drawing.Size(95, 23);
            this.btnHapusSemua.TabIndex = 32;
            this.btnHapusSemua.Text = "Hapus Semua";
            this.btnHapusSemua.Click += new System.EventHandler(this.btnHapusSemua_Click);
            // 
            // SisHakAkses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 501);
            this.Controls.Add(this.btnHapusSemua);
            this.Controls.Add(this.btnPilihSemua);
            this.Controls.Add(this.cmbUsergroup);
            this.Controls.Add(this.btnSimpan);
            this.Controls.Add(this.gridControl);
            this.Controls.Add(this.lblUserGroup);
            this.Name = "SisHakAkses";
            this.Text = "Hak Akses";
            this.Load += new System.EventHandler(this.SisHakAkses_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUsergroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblUserGroup;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraEditors.SimpleButton btnSimpan;
        private DevExpress.XtraEditors.SearchLookUpEdit cmbUsergroup;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraEditors.SimpleButton btnPilihSemua;
        private DevExpress.XtraEditors.SimpleButton btnHapusSemua;
    }
}
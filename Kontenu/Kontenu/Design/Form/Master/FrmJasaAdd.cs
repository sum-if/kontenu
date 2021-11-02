using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTabbedMdi;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using OswLib;
using Kontenu.Sistem;
using DevExpress.XtraEditors.DXErrorProvider;
using System.Net;
using Kontenu.OswLib;
using DevExpress.XtraEditors.Controls;
using Kontenu.Umum;

namespace Kontenu.Master {
    public partial class FrmJasaAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "JASA";
        private String dokumen = "JASA";
        private Boolean isAdd;

        public FrmJasaAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmJasaAdd_Load(object sender, EventArgs e) {
            SplashScreenManager.ShowForm(typeof(SplashUtama));
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // Function Code
                DataOswJenisDokumen dOswJenisDokumen = new DataOswJenisDokumen(command, id);
                if(!this.isAdd) {
                    this.dokumen = "Ubah " + dOswJenisDokumen.nama;
                } else {
                    this.dokumen = "Tambah " + dOswJenisDokumen.nama;
                }

                this.Text = this.dokumen;

                OswControlDefaultProperties.setInput(this, id, command);

                cmbUnit = ComboQueryUmum.getUnit(cmbUnit, command);

                this.setDefaultInput(command);

                // Commit Transaction
                command.Transaction.Commit();
            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } finally {
                con.Close();
                try {
                    SplashScreenManager.CloseForm();
                } catch(Exception ex) {
                }
            }
        }

        private void setDefaultInput(MySqlCommand command) {
            if(!this.isAdd) {
                txtKode.Enabled = false;

                // data
                String strngKode = txtKode.Text;
                DataJasa dJasa = new DataJasa(command, strngKode);
                txtNama.Text = dJasa.nama;
                cmbUnit.EditValue = dJasa.unit;
            } else {
                OswControlDefaultProperties.resetAllInput(this);

                cmbUnit.ItemIndex = 0;
            }

            txtKode.Focus();
        }

        private void btnSimpan_Click(object sender, EventArgs e) {
            SplashScreenManager.ShowForm(typeof(SplashUtama));
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // Function Code
                // validation
                dxValidationProvider1.SetValidationRule(txtNama, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbUnit, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                // simpan header
                String strngKode = txtKode.Text;
                String strngNama = txtNama.Text;
                String strngUnit = cmbUnit.EditValue.ToString();

                DataJasa dJasa = new DataJasa(command, strngKode);
                dJasa.nama = strngNama;
                dJasa.unit = strngUnit;

                if(this.isAdd) {
                    dJasa.tambah();
                    // update kode header --> setelah generate
                    strngKode = dJasa.kode;
                } else {
                    dJasa.ubah();
                }

                // tulis log
                // OswLog.setTransaksi(command, dokumen, dJasa.ToString());

                // reload grid di form header
                FrmJasa frmJasa = (FrmJasa)this.Owner;
                frmJasa.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses " + dokumen + " berhasil.");

                if(this.isAdd) {
                    setDefaultInput(command);
                } else {
                    this.Close();
                }
            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } finally {
                con.Close();
                try {
                    SplashScreenManager.CloseForm();
                } catch(Exception ex) {
                }
            }
        }

    }
}
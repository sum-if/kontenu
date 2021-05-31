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
using OmahSoftware.Sistem;
using DevExpress.XtraEditors.DXErrorProvider;
using System.Net;
using OmahSoftware.OswLib;
using OmahSoftware.Umum;
using DevExpress.XtraEditors.Controls;

namespace OmahSoftware.Akuntansi {
    public partial class FrmTutupPeriodeAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "TUTUPPERIODE";
        private String dokumen = "TUTUPPERIODE";
        private String dokumenDetail = "TUTUPPERIODE";
        private Boolean isAdd;

        public FrmTutupPeriodeAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmTutupPeriodeAdd_Load(object sender, EventArgs e) {
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

                this.dokumenDetail = "Tambah " + dOswJenisDokumen.nama;

                this.Text = this.dokumen;

                OswControlDefaultProperties.setInput(this, id, command);
                cmbBulan = ComboConstantUmum.getBulan(cmbBulan);
                cmbBulan.EditValue = OswDate.getBulan(OswDate.getStringTanggalHariIni());
                txtTahun.Text = OswDate.getTahun(OswDate.getStringTanggalHariIni());

                // tutup periode terakhir
                String query = @"SELECT periode
                                FROM admin 
                                WHERE proses = @prosesTutupPeriode
                                ORDER BY periode DESC
                                LIMIT 1";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("prosesTutupPeriode", Constants.PROSES_TUTUP_PERIODE);

                String periode = OswDataAccess.executeScalarQuery(query, parameters, command);

                if(periode != "") {
                    txtPeriodeTerakhir.Text = OswConvert.toNamaPeriode(periode);
                } else {
                    txtPeriodeTerakhir.Text = "Belum Pernah";
                }


                setGrid(command);

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
                dxValidationProvider1.SetValidationRule(cmbBulan, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtTahun, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                dxValidationProvider1.SetValidationRule(txtTahun, OswValidation.isTahun());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }
                String strngPeriode = txtTahun.Text + cmbBulan.EditValue.ToString();
                String strngNamaBualan = cmbBulan.Text;
                String strngTahun = txtTahun.Text;

                Dictionary<String, String> parameters = new Dictionary<string, string>();
                parameters.Add("varGuid", OswConstants.GUID);
                parameters.Add("varPeriode", strngPeriode);
                parameters.Add("varNamaBulan", strngNamaBualan);
                parameters.Add("varTahun", strngTahun);
                parameters.Add("varFormatDate", OswConfig.FORMAT_TANGGAL_MYSQL);
                parameters.Add("varUserLogin", OswConstants.KODEUSER);

                OswDataAccess.executeNonQueryProcedure(command, "spTutupPeriode", parameters);

                setGrid(command);

                if(gridView1.DataRowCount > 0) {
                    throw new Exception("Terdapat Saldo Bulanan Minus");
                } else {
                    if(OswPesan.pesanKonfirmasi("Tutup Periode", "Checking tutup periode berhasil, Apakah anda yakin untuk memproses tutup periode?") == DialogResult.Yes) {
                        // tulis log
                        OswLog.setTransaksi(command, dokumen, parameters.ToString());

                        // reload grid di form header
                        FrmTutupPeriode frmTutupPeriode = (FrmTutupPeriode)this.Owner;
                        frmTutupPeriode.setGrid(command);

                        // Commit Transaction
                        command.Transaction.Commit();

                        OswPesan.pesanInfo("Proses " + dokumen + " berhasil.");

                        this.Close();
                    } else {
                        // Rollback Transaction
                        command.Transaction.Rollback();
                    }
                }
            } catch(MySqlException ex) {
                command.CommandType = CommandType.Text;
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } catch(Exception ex) {
                command.CommandType = CommandType.Text;
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } finally {
                con.Close();
                try {
                    SplashScreenManager.CloseForm();
                } catch(Exception ex) {
                }
            }
        }

        public void setGrid(MySqlCommand command) {
            String strngPeriode = txtTahun.Text + cmbBulan.EditValue.ToString();

            String query = @"SELECT A.jenis AS Jenis, A.kode AS Kode, A.nama AS Nama, 
                                    A.awal AS Awal, A.mutasitambah AS Tambah, A.mutasikurang AS Kurang, A.akhir AS Akhir
                             FROM tempsaldobulananerror A
                             WHERE A.periode = @peridoe";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("peridoe", strngPeriode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 845 - 21 (kiri) - 17 (vertikal lines)
            widths.Add("Jenis", 110);
            widths.Add("Kode", 160);
            widths.Add("Nama", 280);
            widths.Add("Awal", 100);
            widths.Add("Tambah", 100);
            widths.Add("Kurang", 100);
            widths.Add("Akhir", 100);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Awal", OswInputType.NUMBER);
            inputType.Add("Tambah", OswInputType.NUMBER);
            inputType.Add("Kurang", OswInputType.NUMBER);
            inputType.Add("Akhir", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                new String[] { },
                new String[] { },
                false);

        }

        private void btnCetak_Click(object sender, EventArgs e) {
            String strngNamaBualan = cmbBulan.Text;
            String strngTahun = txtTahun.Text;

            Tools.cetakBrowse(gridControl1, "Data Saldo Bulanan Minus Periode " + strngNamaBualan + " " + strngTahun);
        }

        private void cmbBulan_EditValueChanged(object sender, EventArgs e) {
            updateGrid();
        }

        private void txtTahun_EditValueChanged(object sender, EventArgs e) {
            updateGrid();
        }

        private void updateGrid() {
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
                setGrid(command);

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
    }
}
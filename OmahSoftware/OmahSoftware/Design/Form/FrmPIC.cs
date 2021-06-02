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

namespace OmahSoftware.Master {
    public partial class FrmPIC : DevExpress.XtraEditors.XtraForm {
        private String id = "PIC";
        private String dokumen = "PIC";

        public FrmPIC() {
            InitializeComponent();
        }

        private void FrmPIC_Load(object sender, EventArgs e) {
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
                this.dokumen = dOswJenisDokumen.nama;
                this.Text = this.dokumen;

                OswControlDefaultProperties.setBrowse(this, id, command);

                cmbJabatan = ComboQueryUmum.getJabatan(cmbJabatan, command, true);
                cmbJabatan.ItemIndex = 0;

                this.setGrid(command);

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

        public void setGrid(MySqlCommand command) {
            String strngKTP = txtKTP.Text;
            String strngNama = txtNama.Text;
            String strngAlamat = txtAlamat.Text;
            String strngKota = txtKota.Text;
            String strngJabatan = cmbJabatan.EditValue == null ? "" : cmbJabatan.EditValue.ToString();
            String strngHandphone = txtHandphone.Text;
            String strngEmail = txtEmail.Text;


            String query = @"SELECT A.kode AS Kode, A.ktp AS KTP, A.nama AS Nama, A.alamat AS Alamat, A.provinsi AS Provinsi, A.kota AS Kota, A.kodepos AS 'Kode Pos', B.nama AS Jabatan, A.handphone AS Handphone, A.email AS Email
                            FROM pic A
                            INNER JOIN jabatan B ON A.jabatan = B.kode
                            WHERE A.ktp LIKE @ktp AND A.nama LIKE @nama AND A.alamat LIKE @alamat AND A.kota LIKE @kota AND A.jabatan LIKE @jabatan AND A.handphone LIKE @handphone AND A.email LIKE @email
                            ORDER BY A.kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("ktp", "%" + strngKTP + "%");
            parameters.Add("nama", "%" + strngNama + "%");
            parameters.Add("alamat", "%" + strngAlamat + "%");
            parameters.Add("kota", "%" + strngKota + "%");
            parameters.Add("jabatan", strngJabatan);
            parameters.Add("handphone", "%" + strngHandphone + "%");
            parameters.Add("email", "%" + strngEmail + "%");

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters,
                                                new String[] { "Kode" },
                                                new String[] { });
        }

        private void btnCari_Click(object sender, EventArgs e) {
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
                this.setGrid(command);

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

        private void btnCetak_Click(object sender, EventArgs e) {
            Tools.cetakBrowse(gridControl, dokumen);
        }

        private void btnUbah_Click(object sender, EventArgs e) {
            FrmPICAdd form = new FrmPICAdd(false);
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
                if(gridView.GetSelectedRows().Length == 0) {
                    OswPesan.pesanError("Silahkan pilih " + dokumen + " yang akan diubah.");
                    return;
                }

                String strngKode = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode").ToString();

                DataPIC dPIC = new DataPIC(command, strngKode);
                if(!dPIC.isExist) {
                    throw new Exception(dokumen + " tidak ditemukan.");
                }

                this.AddOwnedForm(form);
                form.txtKode.Text = strngKode;

                // Commit Transaction
                command.Transaction.Commit();
            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } finally {
                con.Close();
            }

            form.ShowDialog();
        }

        private void btnTambah_Click(object sender, EventArgs e) {
            FrmPICAdd form = new FrmPICAdd(true);
            this.AddOwnedForm(form);
            form.ShowDialog();
        }

        private void btnHapus_Click(object sender, EventArgs e) {
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
                if(gridView.GetSelectedRows().Length == 0) {
                    OswPesan.pesanError("Silahkan pilih " + dokumen + " yang akan dihapus.");
                    return;
                }

                if(OswPesan.pesanKonfirmasiHapus() == DialogResult.Yes) {
                    String strngKode = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode").ToString();

                    DataPIC dPIC = new DataPIC(command, strngKode);
                    if(!dPIC.isExist) {
                        throw new Exception(dokumen + " tidak ditemukan.");
                    }

                    dPIC.hapus();

                    // tulis log
                    OswLog.setTransaksi(command, "Hapus " + dokumen, dPIC.ToString());

                    // reload grid
                    this.setGrid(command);

                    // Commit Transaction
                    command.Transaction.Commit();

                    OswPesan.pesanInfo(dokumen + " berhasil dihapus.");
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
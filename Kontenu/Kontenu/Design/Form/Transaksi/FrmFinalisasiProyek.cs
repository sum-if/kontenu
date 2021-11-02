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
using Kontenu.Umum;

namespace Kontenu.Design
{
    public partial class FrmFinalisasiProyek : DevExpress.XtraEditors.XtraForm
    {
        private String id = "FINALISASIPROYEK";
        private String dokumen = "FINALISASIPROYEK";

        public FrmFinalisasiProyek()
        {
            InitializeComponent();
        }

        private void FrmFinalisasiProyek_Load(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try
            {
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
                OswControlDefaultProperties.setTanggal(deTanggalAwal);
                OswControlDefaultProperties.setTanggal(deTanggalAkhir);
                deTanggalAwal.DateTime = OswDate.getDateTimeTanggalAwalBulan();
                deTanggalAkhir.DateTime = OswDate.getDateTimeTanggalHariIni();

                this.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();
            }
            catch (MySqlException ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            catch (Exception ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            finally
            {
                con.Close();
                try
                {
                    SplashScreenManager.CloseForm();
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void setGrid(MySqlCommand command)
        {
            String strngTanggalAwal = deTanggalAwal.Text;
            String strngTanggalAkhir = deTanggalAkhir.Text;
            String strngKode = txtKode.Text;
            String strngKodeProyek = txtKodeProyek.Text;
            String strngNamaProyek = txtNamaProyek.Text;
            String strngNamaKlien = txtNamaKlien.Text;

            String query = @"SELECT A.kode AS Nomor, A.tanggal AS Tanggal, C.kode AS 'Proyek ID', C.nama AS 'Nama Proyek', C.alamat AS Alamat, B.nama AS Klien
                            FROM finalisasiproyek A
                            INNER JOIN proyek C ON A.proyek = C.kode
                            INNER JOIN klien B ON C.klien = B.kode
                            WHERE toDate(A.tanggal) BETWEEN toDate(@tanggalawal) AND toDate(@tanggalakhir) AND 
                                  A.kode LIKE @kode AND C.kode LIKE @kodeproyek AND C.nama LIKE @namaproyek AND B.nama LIKE @namaklien
                            ORDER BY A.kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("tanggalawal", strngTanggalAwal);
            parameters.Add("tanggalakhir", strngTanggalAkhir);
            parameters.Add("kode", "%" + strngKode + "%");
            parameters.Add("kodeproyek", "%" + strngKodeProyek + "%");
            parameters.Add("namaproyek", "%" + strngNamaProyek + "%");
            parameters.Add("namaklien", "%" + strngNamaKlien + "%");

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters,
                                                new String[] { },
                                                new String[] { });
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(typeof(SplashUtama));
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try
            {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // Function Code
                this.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();
            }
            catch (MySqlException ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            catch (Exception ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            finally
            {
                con.Close();
                try
                {
                    SplashScreenManager.CloseForm();
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void btnCetak_Click(object sender, EventArgs e)
        {
            Tools.cetakBrowse(gridControl, dokumen);
        }

        private void btnUbah_Click(object sender, EventArgs e)
        {
            FrmFinalisasiProyekAdd form = new FrmFinalisasiProyekAdd(false);
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try
            {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // Function Code
                if (gridView.GetSelectedRows().Length == 0)
                {
                    OswPesan.pesanError("Silahkan pilih " + dokumen + " yang akan diubah.");
                    return;
                }

                String strngKode = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nomor").ToString();

                DataFinalisasiProyek dFinalisasiProyek = new DataFinalisasiProyek(command, strngKode);
                if (!dFinalisasiProyek.isExist)
                {
                    throw new Exception(dokumen + " tidak ditemukan.");
                }

                this.AddOwnedForm(form);
                form.txtKode.Text = strngKode;

                // Commit Transaction
                command.Transaction.Commit();
            }
            catch (MySqlException ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            catch (Exception ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            finally
            {
                con.Close();
            }

            form.ShowDialog();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            FrmFinalisasiProyekAdd form = new FrmFinalisasiProyekAdd(true);
            this.AddOwnedForm(form);
            form.ShowDialog();
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(typeof(SplashUtama));
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try
            {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // Function Code
                if (gridView.GetSelectedRows().Length == 0)
                {
                    OswPesan.pesanError("Silahkan pilih " + dokumen + " yang akan dihapus.");
                    return;
                }

                if (OswPesan.pesanKonfirmasiHapus() == DialogResult.Yes)
                {
                    String strngKode = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nomor").ToString();

                    DataFinalisasiProyek dFinalisasiProyek = new DataFinalisasiProyek(command, strngKode);
                    if (!dFinalisasiProyek.isExist)
                    {
                        throw new Exception(dokumen + " tidak ditemukan.");
                    }

                    dFinalisasiProyek.hapus();

                    // tulis log
                    // OswLog.setTransaksi(command, "Hapus " + dokumen, dFinalisasiProyek.ToString());

                    // reload grid
                    this.setGrid(command);

                    // Commit Transaction
                    command.Transaction.Commit();

                    OswPesan.pesanInfo(dokumen + " berhasil dihapus.");
                }
            }
            catch (MySqlException ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            catch (Exception ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            finally
            {
                con.Close();
                try
                {
                    SplashScreenManager.CloseForm();
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
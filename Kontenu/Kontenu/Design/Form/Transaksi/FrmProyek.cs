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
    public partial class FrmProyek : DevExpress.XtraEditors.XtraForm
    {
        private String id = "PROYEK";
        private String dokumen = "PROYEK";

        public FrmProyek()
        {
            InitializeComponent();
        }

        private void FrmProyek_Load(object sender, EventArgs e)
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

                cmbProyekJenis = ComboQueryUmum.getJenisProyek(cmbProyekJenis, command, "%", true);
                cmbProyekJenis.ItemIndex = 0;

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
            String strngNamaKlien = txtNamaKlien.Text;
            String strngNamaProyek = txtNamaProyek.Text;
            String strngJenisProyek = cmbProyekJenis.EditValue.ToString();

            String query = @"SELECT A.kode AS 'Proyek ID', A.tanggaldeal AS Tanggal, A.nama AS Nama, A.alamat AS Alamat, A.kota AS Kota, 
                                    B.nama AS Klien, B.telp AS Telp, B.email AS Email, C.nama AS Jenis, 
		                            A.status AS Status
                            FROM proyek A
                            INNER JOIN klien B ON A.klien = B.kode
                            INNER JOIN jenisproyek C ON A.jenisproyek = C.kode
                            WHERE toDate(A.tanggaldeal) BETWEEN toDate(@tanggalawal) AND toDate(@tanggalakhir) AND 
                                  A.kode LIKE @kode AND B.nama LIKE @namaklien AND A.nama LIKE @namaproyek AND A.jenisproyek LIKE @jenisproyek
                            ORDER BY A.kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("tanggalawal", strngTanggalAwal);
            parameters.Add("tanggalakhir", strngTanggalAkhir);
            parameters.Add("kode", "%" + strngKode + "%");
            parameters.Add("namaklien", "%" + strngNamaKlien + "%");
            parameters.Add("namaproyek", "%" + strngNamaProyek + "%");
            parameters.Add("jenisproyek", strngJenisProyek);

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
            FrmProyekAdd form = new FrmProyekAdd(false);
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

                String strngKode = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Proyek ID").ToString();

                DataProyek dProyek = new DataProyek(command, strngKode);
                if (!dProyek.isExist)
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
            FrmProyekAdd form = new FrmProyekAdd(true);
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
                    String strngKode = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Proyek ID").ToString();

                    DataProyek dProyek = new DataProyek(command, strngKode);
                    if (!dProyek.isExist)
                    {
                        throw new Exception(dokumen + " tidak ditemukan.");
                    }

                    dProyek.hapus();

                    // tulis log
                    // OswLog.setTransaksi(command, "Hapus " + dokumen, dProyek.ToString());

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
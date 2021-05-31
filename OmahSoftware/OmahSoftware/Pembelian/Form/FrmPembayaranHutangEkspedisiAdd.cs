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
using OmahSoftware.Umum.Laporan;
using OmahSoftware.Akuntansi;
using OmahSoftware.Pembelian.Laporan;
using DevExpress.XtraEditors.Controls;
using OmahSoftware.Penjualan;

namespace OmahSoftware.Pembelian {
    public partial class FrmPembayaranHutangEkspedisiAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "PEMBAYARANHUTANGEKSPEDISI";
        private String dokumen = "PEMBAYARANHUTANGEKSPEDISI";
        private String dokumenDetail = "PEMBAYARANHUTANGEKSPEDISI";
        private Boolean isAdd;

        public FrmPembayaranHutangEkspedisiAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmPembayaranHutangEkspedisiAdd_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setTanggal(deTanggal);

                cmbAkunPembayaran = ComboQueryUmum.getAkun(cmbAkunPembayaran, command, "akun_pembayaran_hutang_ekspedisi");
                cmbEkspedisi = ComboQueryUmum.getEkspedisi(cmbEkspedisi, command);

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
                String strngKode = txtKode.Text;
                txtKode.Enabled = false;
                cmbAkunPembayaran.Enabled = false;

                // data
                DataPembayaranHutangEkspedisi dPembayaranHutangEkspedisi = new DataPembayaranHutangEkspedisi(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dPembayaranHutangEkspedisi.tanggal);
                cmbAkunPembayaran.EditValue = dPembayaranHutangEkspedisi.akunpembayaran;
                txtCatatan.Text = dPembayaranHutangEkspedisi.catatan;
                cmbEkspedisi.EditValue = dPembayaranHutangEkspedisi.ekspedisi;
                txtNoUrut.EditValue = dPembayaranHutangEkspedisi.nourut;

                setGrid(command);
            } else {
                OswControlDefaultProperties.resetAllInput(this);
                cmbAkunPembayaran.EditValue = OswCombo.getFirstEditValue(cmbAkunPembayaran);
                cmbEkspedisi.EditValue = OswCombo.getFirstEditValue(cmbEkspedisi);
                setGrid(command);
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
                dxValidationProvider1.SetValidationRule(deTanggal, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbAkunPembayaran, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbEkspedisi, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtNoUrut, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                String strngKode = txtKode.Text;
                String strngTanggal = deTanggal.Text;
                String strngEkspedisi = cmbEkspedisi.EditValue.ToString();
                String strngAkunPembayaran = cmbAkunPembayaran.EditValue.ToString();
                String strngCatatan = txtCatatan.Text;
                String strngNoUrut = txtNoUrut.EditValue.ToString();

                DataPembayaranHutangEkspedisi dPembayaranHutangEkspedisi = new DataPembayaranHutangEkspedisi(command, strngKode);
                dPembayaranHutangEkspedisi.tanggal = strngTanggal;
                dPembayaranHutangEkspedisi.ekspedisi = strngEkspedisi;
                dPembayaranHutangEkspedisi.akunpembayaran = strngAkunPembayaran;
                dPembayaranHutangEkspedisi.catatan = strngCatatan;
                dPembayaranHutangEkspedisi.nourut = strngNoUrut;

                if(this.isAdd) {
                    dPembayaranHutangEkspedisi.tambah();
                    // update kode header --> setelah generate
                    strngKode = dPembayaranHutangEkspedisi.kode;
                } else {
                    dPembayaranHutangEkspedisi.hapusDetail();
                    dPembayaranHutangEkspedisi.ubah();
                }

                // simpan detail
                GridView gridView = gridView1;

                double dblTotal = 0;

                for(int i = 0; i < gridView.DataRowCount; i++) {
                    if(gridView.GetRowCellValue(i, "Faktur Pembelian").ToString() == "") {
                        continue;
                    }

                    String strngNo = gridView.GetRowCellValue(i, "No").ToString();
                    String strngFakturPembelian = gridView.GetRowCellValue(i, "Faktur Pembelian").ToString();
                    double dblJumlah = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Jumlah").ToString()));

                    dblTotal = Tools.getRoundMoney(dblTotal + dblJumlah);

                    // simpan detail
                    DataPembayaranHutangEkspedisiDetail dPembayaranHutangEkspedisiDetail = new DataPembayaranHutangEkspedisiDetail(command, strngKode, strngNo);
                    dPembayaranHutangEkspedisiDetail.fakturpembelian = strngFakturPembelian;
                    dPembayaranHutangEkspedisiDetail.jumlah = dblJumlah.ToString();
                    dPembayaranHutangEkspedisiDetail.tambah();

                    // tulis log detail
                    OswLog.setTransaksi(command, dokumenDetail, dPembayaranHutangEkspedisiDetail.ToString());
                }

                dPembayaranHutangEkspedisi = new DataPembayaranHutangEkspedisi(command, strngKode);
                dPembayaranHutangEkspedisi.total = dblTotal.ToString();
                dPembayaranHutangEkspedisi.ubah();

                // validasi setelah simpan
                dPembayaranHutangEkspedisi.valJumlahDetail();
                dPembayaranHutangEkspedisi.prosesJurnal();

                // tulis log
                OswLog.setTransaksi(command, dokumen, dPembayaranHutangEkspedisi.ToString());

                // reload grid di form header
                FrmPembayaranHutangEkspedisi frmPembayaranHutangEkspedisi = (FrmPembayaranHutangEkspedisi)this.Owner;
                frmPembayaranHutangEkspedisi.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses " + dokumen + " berhasil.");

                if(this.isAdd) {
                    if(OswPesan.pesanKonfirmasi("Konfirmasi", "Cetak Bukti Pembayaran Pembelian?") == DialogResult.Yes) {
                        cetak(strngKode);
                    }

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

        public void setGrid(MySqlCommand command) {
            String strngKode = txtKode.Text;

            String query = @"SELECT B.no AS 'No', B.fakturpembelian AS 'Faktur Pembelian', B.jumlah AS Jumlah
                                FROM pembayaranhutangekspedisi A
                                INNER JOIN pembayaranhutangekspedisidetail B ON A.kode = B.pembayaranhutangekspedisi
                                WHERE A.kode = @kode
                                ORDER BY B.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", strngKode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 917 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 927
            widths.Add("No", 35);
            widths.Add("Faktur Pembelian", 584);
            widths.Add("Jumlah", 260);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Jumlah", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { },
                                 new String[] { "No"});

            // search
            RepositoryItemButtonEdit search = new RepositoryItemButtonEdit();
            search.Buttons[0].Kind = ButtonPredefines.Glyph;
            search.Buttons[0].Image = OswResources.getImage("cari-16.png", typeof(Program).Assembly);
            search.Buttons[0].Visible = true;
            search.ButtonClick += search_ButtonClick;

            GridView gridView = gridView1;
            gridView.OptionsView.ShowFooter = true;

            gridView.Columns["Faktur Pembelian"].ColumnEdit = search;
            gridView.Columns["Faktur Pembelian"].ColumnEdit.ReadOnly = true;

            gridView.Columns["Jumlah"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Jumlah"].SummaryItem.DisplayFormat = "{0:N2}";
        }

        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e) {
            GridView gridView = gridView1;
            if(e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "Faktur Pembelian") {
                infoFakturPembelian();
            }
        }

        void search_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            infoFakturPembelian();
        }

        private void infoFakturPembelian() {
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
                GridView gridView = gridView1;

                String strngKodeEkspedisi = cmbEkspedisi.EditValue.ToString();

                String query = @"SELECT A.tanggal AS Tanggal, A.kode AS 'Faktur Pembelian', 
		                                A.biayaekspedisi AS 'Biaya Ekspedisi', A.totalbayarekspedisi AS 'Total Bayar', A.biayaekspedisi-A.totalbayarekspedisi AS 'Sisa Bayar'
                                FROM fakturpembelian A
                                WHERE A.ekspedisi = @ekspedisi AND A.biayaekspedisi > A.totalbayarekspedisi
                                ORDER BY toDate(A.jatuhtempo), toDate(A.tanggal), A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("ekspedisi", strngKodeEkspedisi);

                InfUtamaDataTable form = new InfUtamaDataTable("Info Faktur Pembelian", query, parameters,
                                                                new String[] { },
                                                                new String[] { "Biaya Ekspedisi", "Total Bayar", "Sisa Bayar" },
                                                                new DataTable());
                this.AddOwnedForm(form);
                form.ShowDialog();

                if(form.hasil.Rows.Count == 0) {
                    return;
                }

                foreach(DataRow row in form.hasil.Rows) {
                    String strngFakturPembelian = row["Faktur Pembelian"].ToString();
                    String strngSisaBayar = row["Sisa Bayar"].ToString();

                    gridView.AddNewRow();
                    gridView.MoveLast();
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Faktur Pembelian"], strngFakturPembelian);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Jumlah"], strngSisaBayar);
                    gridView.UpdateCurrentRow();
                }

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

        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Pembelian") == null) {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
                return;
            }

            if(gridView.FocusedColumn == null) {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Faktur Pembelian" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Pembelian").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
                return;
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Pembelian") == null) {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
                return;
            }

            if(gridView.FocusedColumn == null) {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Faktur Pembelian" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Pembelian").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
                return;
            }
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Faktur Pembelian"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Total Faktur"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Total Bayar"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Total Retur"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Jumlah"], "0");
        }

        private void gridView1_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e) {
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
                GridView gridView = sender as GridView;
                for(int no = 1; no <= gridView.DataRowCount; no++) {
                    gridView.SetRowCellValue(no - 1, "No", no);
                }

                // Commit Transaction
                command.Transaction.Commit();
            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } finally {
                con.Close();
            }
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e) {
            GridView gridView = gridView1;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Pembelian") == null) {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
                return;
            }

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Pembelian").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
                return;
            }
        }

        private void btnCetak_Click(object sender, EventArgs e) {
            String strngKode = txtKode.Text;
            cetak(strngKode);
        }

        private void cetak(string kode) {
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

                RptCetakPembayaranHutangEkspedisi report = new RptCetakPembayaranHutangEkspedisi();

                DataOswPerusahaan dPerusahaan = new DataOswPerusahaan(command);
                report.Parameters["perusahaanNama"].Value = dPerusahaan.nama;
                report.Parameters["perusahaanAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["perusahaanTelepon"].Value = dPerusahaan.telp;
                report.Parameters["perusahaanEmail"].Value = dPerusahaan.email;
                report.Parameters["perusahaanNPWP"].Value = dPerusahaan.npwp;

                DataPembayaranHutangEkspedisi dPembayaranHutangEkspedisi = new DataPembayaranHutangEkspedisi(command, kode);
                DataAkun dAkun = new DataAkun(command, dPembayaranHutangEkspedisi.akunpembayaran);
                DataEkspedisi dEkspedisi = new DataEkspedisi(command, dPembayaranHutangEkspedisi.ekspedisi);

                report.Parameters["nomor"].Value = kode;
                report.Parameters["tanggal"].Value = dPembayaranHutangEkspedisi.tanggal;
                report.Parameters["akun"].Value = dPembayaranHutangEkspedisi.akunpembayaran + " " + dAkun.nama;
                report.Parameters["total"].Value = dPembayaranHutangEkspedisi.total;
                report.Parameters["terbilang"].Value = OswMath.Terbilang(Convert.ToDouble(dPembayaranHutangEkspedisi.total));
                report.Parameters["catatan"].Value = dPembayaranHutangEkspedisi.catatan;
                report.Parameters["nourut"].Value = dPembayaranHutangEkspedisi.nourut;
                report.Parameters["ekspedisi"].Value = dEkspedisi.nama;

                // Assign the printing system to the document viewer.
                LaporanPrintPreview laporan = new LaporanPrintPreview();
                laporan.documentViewer1.DocumentSource = report;

                //ReportPrintTool printTool = new ReportPrintTool(report);
                //printTool.Print();

                OswLog.setLaporan(command, dokumen);

                laporan.Show();

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
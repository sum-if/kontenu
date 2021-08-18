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
using DevExpress.XtraEditors.Controls;
using Kontenu.Pembelian;
using Kontenu.Penjualan;
using Kontenu.Akuntansi.Laporan;
using Kontenu.Umum.Laporan;

namespace Kontenu.Akuntansi {
    public partial class FrmJurnalVoucherAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "JURNALVOUCHER";
        private String dokumen = "JURNALVOUCHER";
        private String dokumenDetail = "JURNALVOUCHER";
        private Boolean isAdd;

        public FrmJurnalVoucherAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmJurnalVoucherAdd_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setTanggal(deTanggal);

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

                // data
                DataJurnalVoucher dJurnalVoucher = new DataJurnalVoucher(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dJurnalVoucher.tanggal);
                txtCatatan.EditValue = dJurnalVoucher.catatan;

                this.setGrid(command);
            } else {
                OswControlDefaultProperties.resetAllInput(this);

                btnCetak.Enabled = false;

                this.setGrid(command);
            }

            txtKode.Focus();
        }

        public void setGrid(MySqlCommand command) {
            String strngKode = txtKode.Text;

            String query = @"SELECT B.no AS No, B.akun AS 'Kode Akun', C.nama AS 'Nama Akun', B.keterangan AS Keterangan, B.debit AS Debit, B.kredit AS Kredit
                             FROM jurnalvoucher A
                             INNER JOIN jurnalvoucherdetail B ON A.kode = B.jurnalvoucher
                             INNER JOIN akun C ON B.akun = C.kode
                             WHERE A.kode = @kode
                             ORDER BY B.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("Kode", strngKode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 949 - 21 (kiri) - 17 (vertikal lines) - 35 (No) 
            widths.Add("No", 35);
            widths.Add("Kode Akun", 100);
            widths.Add("Nama Akun", 166);
            widths.Add("Keterangan", 370);
            widths.Add("Debit", 120);
            widths.Add("Kredit", 120);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Debit", OswInputType.NUMBER);
            inputType.Add("Kredit", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType, new String[] { }, new String[] { "No", "Nama Akun" });

            // search produk di kolom kode
            RepositoryItemButtonEdit search = new RepositoryItemButtonEdit();
            search.Buttons[0].Kind = ButtonPredefines.Glyph;
            search.Buttons[0].Image = OswResources.getImage("cari-16.png", typeof(Program).Assembly);
            search.Buttons[0].Visible = true;
            search.ButtonClick += search_ButtonClick;

            GridView gridView = gridView1;
            gridView.Columns["Kode Akun"].ColumnEdit = search;
            gridView.Columns["Kode Akun"].ColumnEdit.ReadOnly = true;

            // hitung total
            setFooter();
        }

        void search_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            infoAkun();
        }

        private void infoAkun() {
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
                String query = @"SELECT A.kode AS Kode, A.nama AS Nama
                                FROM akun A
                                WHERE A.jurnalmanual = @statusjurnalmanual AND A.status = @status
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("statusjurnalmanual", Constants.STATUS_YA);
                parameters.Add("status", Constants.STATUS_AKTIF);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Akun", query, parameters,
                                                                new String[] { "Kode", "Nama" },
                                                                new String[] { },
                                                                new String[] { });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("Kode")) {
                    return;
                }

                String strngKode = form.hasil["Kode"];
                String strngNama = form.hasil["Nama"];

                GridView gridView = gridView1;
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Akun"], strngKode);
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Nama Akun"], strngNama);

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
                dxValidationProvider1.SetValidationRule(deTanggal, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtCatatan, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                String strngKode = txtKode.Text;
                String strngTanggal = deTanggal.Text;
                String strngCatatan = txtCatatan.Text;

                DataJurnalVoucher dJurnalVoucher = new DataJurnalVoucher(command, strngKode);
                dJurnalVoucher.tanggal = strngTanggal;
                dJurnalVoucher.catatan = strngCatatan;

                if(this.isAdd) {
                    dJurnalVoucher.tambah();
                    // update kode header --> setelah generate
                    strngKode = dJurnalVoucher.kode;
                } else {
                    dJurnalVoucher.hapusDetail();
                    dJurnalVoucher.ubah();
                }

                // simpan detail
                GridView gridView = gridView1;

                double dblTotalDebit = 0;
                double dblTotalKredit = 0;
                for(int i = 0; i < gridView.DataRowCount; i++) {
                    if(gridView.GetRowCellValue(i, "Kode Akun").ToString() == "") {
                        continue;
                    }

                    String strngNo = gridView.GetRowCellValue(i, "No").ToString();
                    String strngKodeAkun = gridView.GetRowCellValue(i, "Kode Akun").ToString();
                    String strngNamaAkun = gridView.GetRowCellValue(i, "Nama Akun").ToString();
                    String strngKeterangan = gridView.GetRowCellValue(i, "Keterangan").ToString();
                    String strngDebit = Tools.getRoundMoney(gridView.GetRowCellValue(i, "Debit").ToString());
                    String strngKredit = Tools.getRoundMoney(gridView.GetRowCellValue(i, "Kredit").ToString());

                    DataJurnalVoucherDetail dJurnalVoucherDetail = new DataJurnalVoucherDetail(command, strngKode, strngNo);
                    dJurnalVoucherDetail.akun = strngKodeAkun;
                    dJurnalVoucherDetail.keterangan = strngKeterangan;
                    dJurnalVoucherDetail.debit = strngDebit;
                    dJurnalVoucherDetail.kredit = strngKredit;
                    dJurnalVoucherDetail.tambah();

                    dblTotalDebit += double.Parse(strngDebit);
                    dblTotalKredit += double.Parse(strngKredit);

                    // tulis log detail
                    OswLog.setTransaksi(command, dokumenDetail, dJurnalVoucherDetail.ToString());
                }

                // update total header, di buat object baru karena kalo pake yang lama isExist = False
                dJurnalVoucher = new DataJurnalVoucher(command, strngKode);
                dJurnalVoucher.totaldebit = dblTotalDebit.ToString();
                dJurnalVoucher.totalkredit = dblTotalKredit.ToString();
                dJurnalVoucher.ubah();

                // validasi setelah simpan
                dJurnalVoucher.valJumlahDetail();
                dJurnalVoucher.valBalance();
                dJurnalVoucher.prosesJurnal();

                // tulis log
                OswLog.setTransaksi(command, dokumen, dJurnalVoucher.ToString());

                // reload grid di form header
                FrmJurnalVoucher frmJurnalVoucher = (FrmJurnalVoucher)this.Owner;
                frmJurnalVoucher.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses " + dokumen + " berhasil.");

                if(this.isAdd) {
                    if(OswPesan.pesanKonfirmasi("Konfirmasi", "Cetak Jurnal Voucher?") == DialogResult.Yes) {
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

        private void setFooter() {
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

                double dblTotalDebit = 0;
                double dblTotalKredit = 0;

                for(int i = 0; i < gridView.DataRowCount; i++) {
                    if(gridView.GetRowCellValue(i, "Kode Akun").ToString() == "") {
                        continue;
                    }

                    String strngDebit = gridView.GetRowCellValue(i, "Debit").ToString();
                    String strngKredit = gridView.GetRowCellValue(i, "Kredit").ToString();

                    dblTotalDebit += double.Parse(strngDebit);
                    dblTotalKredit += double.Parse(strngKredit);
                }

                lblTotalDebit.Text = OswConvert.convertToRupiah(dblTotalDebit);
                lblTotalKredit.Text = OswConvert.convertToRupiah(dblTotalKredit);
                lblBalance.Text = OswConvert.convertToRupiah(dblTotalDebit - dblTotalKredit);

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

        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e) {
            GridView gridView = gridView1;
            if(e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "Kode Akun") {
                infoAkun();
            }
        }

        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e) {
            GridView gridView = sender as GridView;

            // untuk pertama kali (pertama, langsung fokus soalnya, sebelum init) --> sehingga nilainya null
            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Akun") == null) {
                gridView.FocusedColumn = gridView.Columns["Kode Akun"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Kode Akun" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Akun").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Kode Akun"];
                return;
            }

            setFooter();
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Akun"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Nama Akun"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Debit"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kredit"], "0");

            setFooter();
        }

        private void gridView1_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e) {
            GridView gridView = sender as GridView;
            for(int no = 1; no <= gridView.DataRowCount; no++) {
                gridView.SetRowCellValue(no - 1, "No", no);
            }

            setFooter();
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e) {
            GridView gridView = sender as GridView;
            String strngKodeAkun = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Akun").ToString();
            if(strngKodeAkun == "") {
                return;
            }

            setFooter();
        }

        private void btnCetak_Click(object sender, EventArgs e) {
            string strngKode = txtKode.Text;
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
                RptCetakJurnalVoucher report = new RptCetakJurnalVoucher();

                DataOswPerusahaan dPerusahaan = new DataOswPerusahaan(command);
                report.Parameters["perusahaanNama"].Value = dPerusahaan.nama;
                report.Parameters["perusahaanAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["perusahaanTelepon"].Value = dPerusahaan.telp;
                report.Parameters["perusahaanEmail"].Value = dPerusahaan.email;
                report.Parameters["perusahaanNPWP"].Value = dPerusahaan.npwp;

                DataJurnalVoucher dJurnalVoucher = new DataJurnalVoucher(command, kode);
                report.Parameters["nomor"].Value = kode;
                report.Parameters["tanggal"].Value = dJurnalVoucher.tanggal;
                report.Parameters["catatan"].Value = dJurnalVoucher.catatan;

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
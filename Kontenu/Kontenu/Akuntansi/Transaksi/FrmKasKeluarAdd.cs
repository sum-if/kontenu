﻿using DevExpress.XtraBars.Ribbon;
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
using Kontenu.Umum.Laporan;
using Kontenu.Master;
using DevExpress.XtraEditors.Controls;

namespace Kontenu.Akuntansi {
    public partial class FrmKasKeluarAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "KASKELUAR";
        private String dokumen = "KASKELUAR";
        private String dokumenDetail = "KASKELUAR";
        private Boolean isAdd;

        public FrmKasKeluarAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmKasKeluarAdd_Load(object sender, EventArgs e) {
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

                cmbAkunKasKeluar = ComboQueryUmum.getAkun(cmbAkunKasKeluar, command, "akun_input_kas_keluar");
                cmbAkunKasKeluar.EditValue = OswCombo.getFirstEditValue(cmbAkunKasKeluar);

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
                DataKasKeluar dKasKeluar = new DataKasKeluar(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dKasKeluar.tanggal);
                txtCatatan.EditValue = dKasKeluar.catatan;
                cmbAkunKasKeluar.EditValue = dKasKeluar.akun;
                txtNoUrut.EditValue = dKasKeluar.nourut;

                this.setGrid(command);
            } else {
                OswControlDefaultProperties.resetAllInput(this);
                cmbAkunKasKeluar.EditValue = OswCombo.getFirstEditValue(cmbAkunKasKeluar);

                this.setGrid(command);
            }

            txtKode.Focus();
        }

        public void setGrid(MySqlCommand command) {
            String strngKode = txtKode.Text;

            String query = @"SELECT B.no AS No, B.akun AS 'Kode Akun', C.nama AS 'Nama Akun', B.keterangan AS Keterangan, B.jumlah AS Nominal
                             FROM kaskeluar A
                             INNER JOIN kaskeluardetail B ON A.kode = B.kaskeluar
                             INNER JOIN akun C ON B.akun = C.kode
                             WHERE A.kode = @kode
                             ORDER BY B.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("Kode", strngKode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 628 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 555
            widths.Add("No", 30);
            widths.Add("Kode Akun", 130);
            widths.Add("Nama Akun", 200);
            widths.Add("Keterangan", 140);
            widths.Add("Nominal", 90);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Nominal", OswInputType.NUMBER);

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
                                INNER JOIN kelompokakunsetting B ON A.kode LIKE B.akun AND A.akunkategori LIKE B.kategori AND A.akunsubkategori LIKE B.subkategori AND A.akungroup LIKE B.group AND A.akunsubgroup LIKE B.subgroup
                                WHERE B.kelompokakun = @kelompokakun AND A.jurnalmanual = @statusjurnalmanual AND A.status = @status
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("statusjurnalmanual", Constants.STATUS_YA);
                parameters.Add("status", Constants.STATUS_AKTIF);
                parameters.Add("kelompokakun", "akun_lawan_input_kas_keluar");

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
                dxValidationProvider1.SetValidationRule(cmbAkunKasKeluar, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtCatatan, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtNoUrut, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                String strngKode = txtKode.Text;
                String strngTanggal = deTanggal.Text;
                String strngCatatan = txtCatatan.Text;
                String strngAkun = cmbAkunKasKeluar.EditValue.ToString();
                String strngNoUrut = txtNoUrut.EditValue.ToString();

                DataKasKeluar dKasKeluar = new DataKasKeluar(command, strngKode);
                dKasKeluar.tanggal = strngTanggal;
                dKasKeluar.catatan = strngCatatan;
                dKasKeluar.akun = strngAkun;
                dKasKeluar.nourut = strngNoUrut;

                if(this.isAdd) {
                    dKasKeluar.tambah();
                    // update kode header --> setelah generate
                    strngKode = dKasKeluar.kode;
                } else {
                    dKasKeluar.hapusDetail();
                    dKasKeluar.ubah();
                }

                // simpan detail
                GridView gridView = gridView1;

                decimal dblTotal = 0;
                for(int i = 0; i < gridView.DataRowCount; i++) {
                    if(gridView.GetRowCellValue(i, "Kode Akun").ToString() == "") {
                        continue;
                    }

                    String strngNo = gridView.GetRowCellValue(i, "No").ToString();
                    String strngKodeAkun = gridView.GetRowCellValue(i, "Kode Akun").ToString();
                    String strngKeterangan = gridView.GetRowCellValue(i, "Keterangan").ToString();
                    String strngNominal = Tools.getRoundMoney(gridView.GetRowCellValue(i, "Nominal").ToString());

                    DataKasKeluarDetail dKasKeluarDetail = new DataKasKeluarDetail(command, strngKode, strngNo);
                    dKasKeluarDetail.akun = strngKodeAkun;
                    dKasKeluarDetail.keterangan = strngKeterangan;
                    dKasKeluarDetail.jumlah = strngNominal;
                    dKasKeluarDetail.tambah();

                    dblTotal += decimal.Parse(strngNominal);
                    dblTotal = Tools.getRoundMoney(dblTotal);

                    // tulis log detail
                    OswLog.setTransaksi(command, dokumenDetail, dKasKeluarDetail.ToString());
                }

                // update total header, di buat object baru karena kalo pake yang lama isExist = False
                dKasKeluar = new DataKasKeluar(command, strngKode);
                dKasKeluar.total = dblTotal.ToString();
                dKasKeluar.ubah();

                // validasi setelah simpan
                dKasKeluar.valJumlahDetail();
                dKasKeluar.prosesJurnal();

                // tulis log
                OswLog.setTransaksi(command, dokumen, dKasKeluar.ToString());

                // reload grid di form header
                FrmKasKeluar frmKasKeluar = (FrmKasKeluar)this.Owner;
                frmKasKeluar.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses " + dokumen + " berhasil.");

                if(this.isAdd) {
                    if(OswPesan.pesanKonfirmasi("Konfirmasi", "Cetak Kas Keluar?") == DialogResult.Yes) {
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

                decimal dblTotal = 0;
                for(int i = 0; i < gridView.DataRowCount; i++) {
                    if(gridView.GetRowCellValue(i, "Kode Akun").ToString() == "") {
                        continue;
                    }
                    String strngNominal = Tools.getRoundMoney(gridView.GetRowCellValue(i, "Nominal").ToString());

                    dblTotal += decimal.Parse(strngNominal);
                    dblTotal = Tools.getRoundMoney(dblTotal);
                }

                lblTotal.Text = OswConvert.convertToRupiah(dblTotal);

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

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            GridView gridView = sender as GridView;

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
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Keterangan"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Nominal"], "0");

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
            String strngNominal = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nominal").ToString();

            if(strngKodeAkun == "") {
                return;
            }

            setFooter();
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

                RptCetakKasKeluar report = new RptCetakKasKeluar();

                DataPerusahaan dPerusahaan = new DataPerusahaan(command, Constants.PERUSAHAAN_KONTENU);
                report.Parameters["perusahaanNama"].Value = dPerusahaan.nama;
                report.Parameters["perusahaanAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["perusahaanTelepon"].Value = "";
                report.Parameters["perusahaanEmail"].Value = dPerusahaan.email;
                report.Parameters["perusahaanNPWP"].Value = "";

                DataKasKeluar dKasKeluar = new DataKasKeluar(command, kode);
                report.Parameters["nomor"].Value = kode;
                report.Parameters["tanggal"].Value = dKasKeluar.tanggal;
                DataAkun dAkun = new DataAkun(command, dKasKeluar.akun);
                report.Parameters["akun"].Value = dKasKeluar.akun + " " + dAkun.nama;
                report.Parameters["total"].Value = dKasKeluar.total;
                report.Parameters["terbilang"].Value = OswMath.Terbilang(Convert.ToDouble(dKasKeluar.total));
                report.Parameters["catatan"].Value = dKasKeluar.catatan;
                report.Parameters["nourut"].Value = dKasKeluar.nourut;



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
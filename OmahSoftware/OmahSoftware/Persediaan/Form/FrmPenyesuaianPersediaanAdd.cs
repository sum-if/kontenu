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
using OmahSoftware.Persediaan.Laporan;
using DevExpress.XtraEditors.Controls;

namespace OmahSoftware.Persediaan {
    public partial class FrmPenyesuaianPersediaanAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "PENYESUAIANBARANG";
        private String dokumen = "PENYESUAIANBARANG";
        private String dokumenDetail = "PENYESUAIANBARANG";
        private Boolean isAdd;

        public FrmPenyesuaianPersediaanAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmPenyesuaianPersediaanAdd_Load(object sender, EventArgs e) {
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
                btnSimpan.Enabled = false;

                // data
                DataPenyesuaianPersediaan dPenyesuaianPersediaan = new DataPenyesuaianPersediaan(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dPenyesuaianPersediaan.tanggal);
                txtCatatan.EditValue = dPenyesuaianPersediaan.catatan;
            } else {
                OswControlDefaultProperties.resetAllInput(this);
            }

            this.setGrid(command);
            txtKode.Focus();
        }

        public void setGrid(MySqlCommand command) {
            String strngKode = txtKode.Text;

            String query = @"SELECT B.no AS No, B.barang AS Kode, C.nama AS Nama, C.nopart AS 'No Part', F.nama AS Satuan, D.nama AS Kategori, 
                                    B.jumlahsekarang AS 'Qty Awal', B.jumlahpenyesuaian AS 'Qty Pny', B.jumlahbaru AS 'Qty Baru'
                             FROM penyesuaianpersediaan A
                             INNER JOIN penyesuaianpersediaandetail B ON A.kode = B.penyesuaianpersediaan
                             INNER JOIN barang C ON B.barang = C.kode
                             INNER JOIN barangkategori D ON C.barangkategori = D.kode
                             INNER JOIN barangsatuan F ON C.standarsatuan = F.kode
                             WHERE A.kode = @kode
                             ORDER BY B.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("Kode", strngKode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 950 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 555
            widths.Add("No", 35);
            widths.Add("Kode", 140);
            widths.Add("Nama", 217);
            widths.Add("No Part", 90);
            widths.Add("Satuan", 100);
            widths.Add("Kategori", 120);
            widths.Add("Qty Awal", 70);
            widths.Add("Qty Pny", 70);
            widths.Add("Qty Baru", 70);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Qty Awal", OswInputType.NUMBER);
            inputType.Add("Qty Pny", OswInputType.NUMBER);
            inputType.Add("Qty Baru", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                new String[] { },
                new String[] { "No", "Nama", "No Part", "Satuan", "Kategori", "Qty Awal", "Qty Baru" });

            // search produk di kolom kode
            RepositoryItemButtonEdit searchBarang = new RepositoryItemButtonEdit();
            searchBarang.Buttons[0].Kind = ButtonPredefines.Glyph;
            searchBarang.Buttons[0].Image = OswResources.getImage("cari-16.png", typeof(Program).Assembly);
            searchBarang.Buttons[0].Visible = true;
            searchBarang.ButtonClick += searchBarang_ButtonClick;

            GridView gridView = gridView1;
            gridView.Columns["Kode"].ColumnEdit = searchBarang;

            gridView.Columns["Kode"].ColumnEdit.ReadOnly = true;
        }

        void searchBarang_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            infoBarang();
        }

        private void infoBarang() {
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
                String query = @"SELECT A.kode AS Kode, A.nama AS Nama, C.nama AS Kategori, A.nopart AS 'No Part', F.nama AS Rak, B.nama AS Satuan, COALESCE(E.jumlah,0) AS Stok
                                FROM barang A
                                INNER JOIN barangsatuan B ON A.standarsatuan = B.kode
                                INNER JOIN barangkategori C ON A.barangkategori = C.kode
                                INNER JOIN barangrak F ON A.rak = F.kode
                                LEFT JOIN saldopersediaanaktual E ON E.barang = A.kode
                                ORDER BY C.nama,A.nama,B.nama";

                Dictionary<String, String> parameters = new Dictionary<String, String>();

                InfUtamaDataTable form = new InfUtamaDataTable("Info Barang", query, parameters,
                                                                new String[] { },
                                                                new String[] { "Stok" },
                                                                new DataTable());
                this.AddOwnedForm(form);
                form.ShowDialog();

                if(form.hasil.Rows.Count == 0) {
                    return;
                }

                GridView gridView = gridView1;

                foreach(DataRow row in form.hasil.Rows) {
                    String strngKode = row["Kode"].ToString();
                    String strngNama = row["Nama"].ToString();
                    String strngNoPart = row["No Part"].ToString();
                    String strngSatuan = row["Satuan"].ToString();
                    String strngKategori = row["Kategori"].ToString();
                    String strngStok = row["Stok"].ToString();

                    gridView.AddNewRow();
                    gridView.MoveLast();
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode"], strngKode);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Nama"], strngNama);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["No Part"], strngNoPart);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Satuan"], strngSatuan);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kategori"], strngKategori);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Qty Awal"], strngStok);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Qty Pny"], "0");
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Qty Baru"], strngStok);
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

                DataPenyesuaianPersediaan dPenyesuaianPersediaan = new DataPenyesuaianPersediaan(command, strngKode);
                dPenyesuaianPersediaan.tanggal = strngTanggal;
                dPenyesuaianPersediaan.catatan = strngCatatan;

                if(this.isAdd) {
                    dPenyesuaianPersediaan.tambah();
                    // update kode header --> setelah generate
                    strngKode = dPenyesuaianPersediaan.kode;
                } else {
                    return;
                }

                // simpan detail
                GridView gridView = gridView1;

                for(int i = 0; i < gridView.DataRowCount; i++) {
                    if(gridView.GetRowCellValue(i, "Kode").ToString() == "") {
                        continue;
                    }

                    String strngNo = gridView.GetRowCellValue(i, "No").ToString();
                    String strngKodeBarang = gridView.GetRowCellValue(i, "Kode").ToString();
                    String strngQtySkrg = gridView.GetRowCellValue(i, "Qty Awal").ToString();
                    String strngQtyPny = gridView.GetRowCellValue(i, "Qty Pny").ToString();
                    String strngQtyBaru = gridView.GetRowCellValue(i, "Qty Baru").ToString();

                    DataPenyesuaianPersediaanDetail dPenyesuaianPersediaanDetail = new DataPenyesuaianPersediaanDetail(command, strngKode, strngNo);
                    dPenyesuaianPersediaanDetail.barang = strngKodeBarang;
                    dPenyesuaianPersediaanDetail.jumlahsekarang = strngQtySkrg;
                    dPenyesuaianPersediaanDetail.jumlahpenyesuaian = strngQtyPny;
                    dPenyesuaianPersediaanDetail.jumlahbaru = strngQtyBaru;
                    dPenyesuaianPersediaanDetail.tambah();

                    // tulis log detail
                    OswLog.setTransaksi(command, dokumenDetail, dPenyesuaianPersediaanDetail.ToString());
                }

                // validasi setelah simpan
                dPenyesuaianPersediaan.valJumlahDetail();

                //dPenyesuaianPersediaan.prosesJurnal();

                // tulis log
                OswLog.setTransaksi(command, dokumen, dPenyesuaianPersediaan.ToString());

                // reload grid di form header
                FrmPenyesuaianPersediaan frmPenyesuaianPersediaan = (FrmPenyesuaianPersediaan)this.Owner;
                frmPenyesuaianPersediaan.setGrid(command);

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

        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e) {
            GridView gridView = gridView1;
            if(e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "Kode") {
                infoBarang();
            }
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Nama"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No Part"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Satuan"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kategori"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Qty Awal"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Qty Pny"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Qty Baru"], "0");

        }

        private void gridView1_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e) {
            GridView gridView = sender as GridView;
            for(int no = 1; no <= gridView.DataRowCount; no++) {
                gridView.SetRowCellValue(no - 1, "No", no);
            }
        }

        private void btnCetak_Click(object sender, EventArgs e) {
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
                string kode = txtKode.Text;

                RptCetakPenyesuaianPersediaan report = new RptCetakPenyesuaianPersediaan();

                DataOswPerusahaan dPerusahaan = new DataOswPerusahaan(command);
                report.Parameters["perusahaanNama"].Value = dPerusahaan.nama;
                report.Parameters["perusahaanAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["perusahaanTelepon"].Value = dPerusahaan.telp;
                report.Parameters["perusahaanEmail"].Value = dPerusahaan.email;
                report.Parameters["perusahaanNPWP"].Value = dPerusahaan.npwp;

                DataPenyesuaianPersediaan dPenyesuaianPersediaan = new DataPenyesuaianPersediaan(command, kode);
                report.Parameters["kode"].Value = kode;
                report.Parameters["tanggal"].Value = dPenyesuaianPersediaan.tanggal;
                report.Parameters["catatan"].Value = dPenyesuaianPersediaan.catatan;

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

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode") == null) {
                gridView.FocusedColumn = gridView.Columns["Kode"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Kode" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Kode"];
                return;
            }

            setCurrentDataRowBarang();
        }

        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode") == null) {
                gridView.FocusedColumn = gridView.Columns["Kode"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Kode" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Kode"];
                return;
            }

            setCurrentDataRowBarang();
        }

        private void setCurrentDataRowBarang() {
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

                if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode") == null) {
                    gridView.FocusedColumn = gridView.Columns["Kode"];
                    return;
                }

                if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode").ToString() == "") {
                    gridView.FocusedColumn = gridView.Columns["Kode"];
                    return;
                }

                String strngQtySkrg = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Qty Awal").ToString();
                String strngQtyPenyesuaian = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Qty Pny").ToString();

                double dblQtyBaru = double.Parse(strngQtySkrg) + double.Parse(strngQtyPenyesuaian);

                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Qty Baru"], dblQtyBaru);

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
            setCurrentDataRowBarang();
        }
    }
}
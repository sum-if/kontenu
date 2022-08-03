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
using Kontenu.Umum.Laporan;
using Kontenu.Umum;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.Utils.Drawing;
using Kontenu.Master;

namespace Kontenu.Design
{
    public partial class FrmPenagihanAdd : DevExpress.XtraEditors.XtraForm
    {
        private String id = "PENAGIHAN";
        private String dokumen = "PENAGIHAN";
        private String dokumenDetail = "PENAGIHAN";
        private Boolean isAdd;

        private decimal dblGrandTotal = 0;
        private decimal dblTelahDibayar = 0;
        private decimal dblDitagihkan = 0;
        private decimal dblSisa = 0;

        public FrmPenagihanAdd(bool pIsAdd)
        {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmPenagihanAdd_Load(object sender, EventArgs e)
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
                DataOswJenisDokumen dOswJenisDokumen = new DataOswJenisDokumen(command, id);
                if (!this.isAdd)
                {
                    this.dokumen = "Ubah " + dOswJenisDokumen.nama;
                }
                else
                {
                    this.dokumen = "Tambah " + dOswJenisDokumen.nama;
                }

                this.dokumenDetail = "Tambah " + dOswJenisDokumen.nama;

                this.Text = this.dokumen;

                OswControlDefaultProperties.setInput(this, id, command);
                OswControlDefaultProperties.setTanggal(deTanggal);
                OswControlDefaultProperties.setAngka(txtDitagihkan);

                this.setDefaultInput(command);

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

        private void setDefaultInput(MySqlCommand command)
        {
            if (!this.isAdd)
            {
                String strngKode = txtKode.Text;
                deTanggal.Enabled = false;
                btnCetak1.Enabled = true;

                // PENAGIHAN
                DataPenagihan dPenagihan = new DataPenagihan(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dPenagihan.tanggal);

                // Invoice
                DataInvoice dInvoice = new DataInvoice(command, dPenagihan.invoice);
                txtKodeInvoice.Text = dInvoice.kode;
                txtTanggalInvoice.Text = dInvoice.tanggal;
                txtNoQuotation.Text = dInvoice.quotation;

                // Klien
                DataKlien dKlien = new DataKlien(command, dInvoice.klien);
                txtKodeKlien.EditValue = dKlien.kode;
                txtNama.EditValue = dKlien.nama;
                txtAlamat.Text = dKlien.alamat;
                txtProvinsi.Text = dKlien.provinsi;
                txtKota.Text = dKlien.kota;
                txtKodePos.Text = dKlien.kodepos;
                txtTelepon.Text = dKlien.telp;
                txtHandphone.Text = dKlien.handphone;
                txtEmail.Text = dKlien.email;

                // Proyek
                DataProyek dProyek = new DataProyek(command, dInvoice.proyek);
                DataTujuanProyek dTujuanProyek = new DataTujuanProyek(command, dProyek.tujuanproyek);
                DataJenisProyek dJenisProyek = new DataJenisProyek(command, dProyek.jenisproyek);
                DataPIC dPIC = new DataPIC(command, dProyek.pic);

                txtProyekID.Text = dProyek.kode;
                txtProyekNama.Text = dProyek.nama;
                txtProyekAlamat.Text = dProyek.alamat;
                txtProyekKota.Text = dProyek.kota;
                txtProyekProvinsi.Text = dProyek.provinsi;
                txtProyekKodePos.Text = dProyek.kodepos;
                txtProyekTujuan.Text = dTujuanProyek.nama;
                txtProyekJenis.Text = dJenisProyek.nama;
                txtProyekPIC.Text = dPIC.nama;

                this.dblGrandTotal = Tools.getRoundMoney(decimal.Parse(dInvoice.grandtotal));
                this.dblTelahDibayar = Tools.getRoundMoney(decimal.Parse(dPenagihan.telahdibayar));
                this.dblDitagihkan = Tools.getRoundMoney(decimal.Parse(dPenagihan.ditagihkan));
                this.dblSisa = Tools.getRoundMoney(dblGrandTotal - dblTelahDibayar - dblDitagihkan);

                txtDitagihkan.EditValue = Tools.getRoundMoney(decimal.Parse(this.dblDitagihkan.ToString()));
            }
            else
            {
                OswControlDefaultProperties.resetAllInput(this);
                deTanggal.EditValue = "";

                dblGrandTotal = 0;
                dblTelahDibayar = 0;
                dblDitagihkan = 0;
                dblSisa = 0;

                btnCetak1.Enabled = false;
            }

            this.setGrid(command);
            setFooter();
            txtKode.Focus();
        }

        public void setGrid(MySqlCommand command)
        {
            String strngKode = txtKodeInvoice.Text;

            String query = @"SELECT A.no AS No, B.kode AS 'Kode Jasa', B.nama AS Jasa, A.deskripsi AS Deskripsi, 
                                    A.jumlah AS Qty, C.kode 'Kode Unit', C.nama AS Unit, A.rate AS Rate, A.subtotal AS Subtotal
                            FROM invoicedetail A
                            INNER JOIN jasa B ON A.jasa = B.kode
                            INNER JOIN unit C ON A.unit = C.kode
                            WHERE A.invoice = @kode
                            ORDER BY A.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", strngKode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 960 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 927
            widths.Add("Jasa", 255);
            widths.Add("Deskripsi", 287);
            widths.Add("Qty", 80);
            widths.Add("Unit", 90);
            widths.Add("Rate", 100);
            widths.Add("Subtotal", 110);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Qty", OswInputType.NUMBER);
            inputType.Add("Rate", OswInputType.NUMBER);
            inputType.Add("Subtotal", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { "No", "Kode Jasa", "Kode Unit" },
                                 new String[] { "Jasa", "Deskripsi", "Unit", "Subtotal", "Qty", "Rate" },
                                 false);

            setFooter();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            // validation
            dxValidationProvider1.SetValidationRule(deTanggal, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(txtKodeKlien, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(txtKodeInvoice, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(txtProyekNama, OswValidation.IsNotBlank());

            if (!dxValidationProvider1.Validate())
            {
                foreach (Control x in dxValidationProvider1.GetInvalidControls())
                {
                    dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                }
                return;
            }

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
                String strngKode = txtKode.Text;
                String strngTanggal = deTanggal.Text;
                String strngInvoice = txtKodeInvoice.Text;
                String strngKodeKlien = txtKodeKlien.Text;

                setFooter();

                DataPenagihan dPenagihan = new DataPenagihan(command, strngKode);
                dPenagihan.tanggal = strngTanggal;
                dPenagihan.invoice = strngInvoice;
                dPenagihan.klien = strngKodeKlien;
                dPenagihan.grandtotal = this.dblGrandTotal.ToString();
                dPenagihan.telahdibayar = this.dblTelahDibayar.ToString();
                dPenagihan.ditagihkan = this.dblDitagihkan.ToString();
                dPenagihan.sisa = this.dblSisa.ToString();

                if (this.isAdd)
                {
                    dPenagihan.status = Constants.STATUS_PENAGIHAN_BELUM_BAYAR;
                    dPenagihan.tambah();

                    // update kode header --> setelah generate
                    strngKode = dPenagihan.kode;
                    txtKode.Text = strngKode;

                    this.isAdd = false;
                }
                else
                {
                    dPenagihan.ubah();
                }

                // tulis log
                // OswLog.setTransaksi(command, dokumen, dPenagihan.ToString());

                // reload grid di form header
                FrmPenagihan frmPenagihan = (FrmPenagihan)this.Owner;
                frmPenagihan.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses simpan berhasil.");

                if (this.isAdd)
                {
                    setDefaultInput(command);
                }
                else
                {
                    this.Close();
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

        private void btnCetak_Click(object sender, EventArgs e)
        {
            String strngKode = txtKode.Text;
            cetak(strngKode);

        }

        private void cetak(String kode)
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

                // function code
                RptPenagihan report = new RptPenagihan();

                // PERUSAHAAN
                DataPerusahaan dPerusahaan = new DataPerusahaan(command, Constants.PERUSAHAAN_KONTENU);
                report.Parameters["PerusahaanKode"].Value = dPerusahaan.kode;
                report.Parameters["PerusahaanNama"].Value = dPerusahaan.nama;
                report.Parameters["PerusahaanAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["PerusahaanKota"].Value = dPerusahaan.kota;
                report.Parameters["PerusahaanEmail"].Value = dPerusahaan.email;
                report.Parameters["PerusahaanTelepon"].Value = "+62 811 318 6880";
                report.Parameters["PerusahaanWebsite"].Value = dPerusahaan.website;

                // TRANSAKSI
                DataPenagihan dPenagihan = new DataPenagihan(command, kode);
                report.Parameters["Kode"].Value = kode;
                report.Parameters["kodePagar"].Value = "# " + kode;
                report.Parameters["Tanggal"].Value = dPenagihan.tanggal;

                // PROYEK
                DataInvoice dInvoice = new DataInvoice(command, dPenagihan.invoice);
                DataProyek dProyek = new DataProyek(command, dInvoice.proyek);
                report.Parameters["ProyekNama"].Value = dProyek.nama;
                report.Parameters["ProyekAlamat"].Value = dProyek.alamat;
                report.Parameters["ProyekKota"].Value = dProyek.kota;
                String jenisProyek = (new DataJenisProyek(command, dProyek.jenisproyek)).nama;
                report.Parameters["ProyekJenisBefore"].Value = jenisProyek.Substring(0, jenisProyek.IndexOf("-"));
                report.Parameters["ProyekJenisAfter"].Value = jenisProyek.Substring(jenisProyek.IndexOf("-") + 2);
                report.Parameters["ProyekTanggalBerlaku"].Value = dProyek.tanggaldeal;

                // KLIEN
                DataKlien dKlien = new DataKlien(command, dPenagihan.klien);
                report.Parameters["KlienNama"].Value = dKlien.nama;
                report.Parameters["KlienAlamat"].Value = dKlien.alamat;
                report.Parameters["KlienKota"].Value = dKlien.kota;
                report.Parameters["KlienEmail"].Value = dKlien.email;
                report.Parameters["KlienTelp"].Value = dKlien.telp;


                // assign the printing system to the document viewer.
                LaporanPrintPreview laporan = new LaporanPrintPreview();
                laporan.documentViewer1.DocumentSource = report;

                //reportprinttool printtool = new reportprinttool(report);
                //printtool.print();

                OswLog.setLaporan(command, dokumen);

                laporan.Show();

                // commit transaction
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

        private void setFooter()
        {
            this.dblDitagihkan = Tools.getRoundMoney(decimal.Parse(txtDitagihkan.EditValue.ToString()));
            this.dblSisa = Tools.getRoundMoney(this.dblGrandTotal - this.dblTelahDibayar - this.dblDitagihkan);

            lblGrandTotal.Text = OswConvert.convertToRupiah(dblGrandTotal);
            lblTelahDibayar.Text = OswConvert.convertToRupiah(dblTelahDibayar);
            lblSisa.Text = OswConvert.convertToRupiah(dblSisa);
        }

        private void btnCariInvoice_Click(object sender, EventArgs e)
        {
            infoInvoice();
        }

        private void infoInvoice()
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
                String strngKodeKlien = txtKodeKlien.Text;

                String query = @"SELECT A.kode AS Kode, A.tanggal AS Tanggal, A.jenis AS Jenis, B.nama AS Proyek, 
                                        A.grandtotal AS 'Grand Total', AA.totaltagih AS 'Total Tagih', AAA.totalterima AS 'Total Terima'
                                FROM invoice A
                                INNER JOIN v_invoice_totaltagih AA ON A.kode = AA.invoice
                                INNER JOIN v_invoice_totalterima AAA ON A.kode = AAA.invoice
                                INNER JOIN proyek B ON A.proyek = B.kode
                                INNER JOIN klien C ON A.klien = C.kode
                                WHERE A.grandtotal > AA.totaltagih AND A.klien = @klien
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("klien", strngKodeKlien);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Invoice", query, parameters,
                                                                new String[] { "Kode", "Total Terima" },
                                                                new String[] { },
                                                                new String[] { "Grand Total", "Total Tagih", "Total Terima" });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if (!form.hasil.ContainsKey("Kode"))
                {
                    return;
                }

                String strngKodeInvoice = form.hasil["Kode"];
                String strngTotalTerima = form.hasil["Total Terima"];

                // Invoice
                DataInvoice dInvoice = new DataInvoice(command, strngKodeInvoice);
                txtKodeInvoice.Text = dInvoice.kode;
                txtTanggalInvoice.Text = dInvoice.tanggal;
                txtNoQuotation.Text = dInvoice.quotation;

                // Proyek
                DataProyek dProyek = new DataProyek(command, dInvoice.proyek);
                DataTujuanProyek dTujuanProyek = new DataTujuanProyek(command, dProyek.tujuanproyek);
                DataJenisProyek dJenisProyek = new DataJenisProyek(command, dProyek.jenisproyek);
                DataPIC dPIC = new DataPIC(command, dProyek.pic);

                txtProyekID.Text = dProyek.kode;
                txtProyekNama.Text = dProyek.nama;
                txtProyekAlamat.Text = dProyek.alamat;
                txtProyekKota.Text = dProyek.kota;
                txtProyekProvinsi.Text = dProyek.provinsi;
                txtProyekKodePos.Text = dProyek.kodepos;
                txtProyekTujuan.Text = dTujuanProyek.nama;
                txtProyekJenis.Text = dJenisProyek.nama;
                txtProyekPIC.Text = dPIC.nama;

                setGrid(command);

                this.dblGrandTotal = Tools.getRoundMoney(decimal.Parse(dInvoice.grandtotal));
                this.dblTelahDibayar = Tools.getRoundMoney(decimal.Parse(strngTotalTerima));
                this.dblDitagihkan = Tools.getRoundMoney(decimal.Parse(txtDitagihkan.EditValue.ToString()));

                this.dblSisa = Tools.getRoundMoney(dblGrandTotal - dblTelahDibayar - dblDitagihkan);

                setFooter();

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

        private void txtDitagihkan_EditValueChanged(object sender, EventArgs e)
        {
            setFooter();
        }

        private void btnCariKlien_Click(object sender, EventArgs e)
        {
            infoKlien();
        }

        private void infoKlien()
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
                String query = @"SELECT A.kode AS 'Kode Klien',A.nama AS Klien, A.alamat AS Alamat, A.provinsi AS Provinsi, A.kota AS Kota, A.kodepos AS 'Kode Pos', 
                                        A.telp AS Telp, A.handphone AS Handphone, A.email AS Email, A.ktp AS NIK
                                FROM klien A
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();

                InfUtamaDictionary form = new InfUtamaDictionary("Info Klien", query, parameters,
                                                                new String[] { "Kode Klien" },
                                                                new String[] { "Kode Klien" },
                                                                new String[] { });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if (!form.hasil.ContainsKey("Kode Klien"))
                {
                    return;
                }

                String strngKodeKlien = form.hasil["Kode Klien"];

                DataKlien dKlien = new DataKlien(command, strngKodeKlien);
                txtKodeKlien.EditValue = strngKodeKlien;
                txtNama.EditValue = dKlien.nama;
                txtAlamat.Text = dKlien.alamat;
                txtProvinsi.Text = dKlien.provinsi;
                txtKota.Text = dKlien.kota;
                txtKodePos.Text = dKlien.kodepos;
                txtTelepon.Text = dKlien.telp;
                txtHandphone.Text = dKlien.handphone;
                txtEmail.Text = dKlien.email;

                // RESET
                // Invoice
                txtKodeInvoice.Text = "";
                txtTanggalInvoice.Text = "";
                txtNoQuotation.Text = "";

                // Proyek
                txtProyekID.Text = "";
                txtProyekNama.Text = "";
                txtProyekAlamat.Text = "";
                txtProyekKota.Text = "";
                txtProyekProvinsi.Text = "";
                txtProyekKodePos.Text = "";
                txtProyekTujuan.Text = "";
                txtProyekJenis.Text = "";
                txtProyekPIC.Text = "";

                setGrid(command);

                this.dblGrandTotal = 0;
                this.dblTelahDibayar = 0;
                this.dblDitagihkan = 0;
                txtDitagihkan.EditValue = 0;

                this.dblSisa = Tools.getRoundMoney(dblGrandTotal - dblTelahDibayar - dblDitagihkan);

                setFooter();

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

        private void btnCetak_Click_1(object sender, EventArgs e)
        {
            String strngKode = txtKode.Text;
            cetak(strngKode);
        }

        private void btnCetak1_Click(object sender, EventArgs e)
        {
            String strngKode = txtKode.Text;
            cetak(strngKode);
        }

    }
}
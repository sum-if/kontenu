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

namespace Kontenu.Design
{
    public partial class FrmProyekAdd : DevExpress.XtraEditors.XtraForm
    {
        private String id = "PROYEK";
        private String dokumen = "PROYEK";
        private String dokumenDetail = "PROYEK";
        private Boolean isAdd;
        public Dictionary<string, DataTable> dicDetailJasa;

        public FrmProyekAdd(bool pIsAdd)
        {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmProyekAdd_Load(object sender, EventArgs e)
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

                cmbProyekTujuan = ComboQueryUmum.getTujuanProyek(cmbProyekTujuan, command);
                cmbProyekPIC = ComboQueryUmum.getPIC(cmbProyekPIC, command);

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

                // PROYEK
                DataProyek dProyek = new DataProyek(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dProyek.tanggaldeal);
                chkTutup.Checked = dProyek.status == Constants.STATUS_PROYEK_TIDAK_AKTIF;

                rdoJenisInvoiceInterior.Checked = (dProyek.kategori == Constants.JENIS_INVOICE_INTERIOR);
                rdoJenisInvoiceProduct.Checked = (dProyek.kategori == Constants.JENIS_INVOICE_PRODUCT);

                // KLIEN
                DataKlien dKlien = new DataKlien(command, dProyek.klien);
                txtKodeKlien.Text = dKlien.kode;
                txtNama.EditValue = dKlien.nama;
                txtAlamat.Text = dKlien.alamat;
                txtProvinsi.Text = dKlien.provinsi;
                txtKota.Text = dKlien.kota;                
                txtKodePos.Text = dKlien.kodepos;
                txtTelepon.Text = dKlien.telp;
                txtHandphone.Text = dKlien.handphone;
                txtEmail.Text = dKlien.email;

                // PROYEK
                txtProyekNama.Text = dProyek.nama;
                txtProyekAlamat.Text = dProyek.alamat;
                txtProyekKota.Text = dProyek.kota;
                txtProyekProvinsi.Text = dProyek.provinsi;
                txtProyekKodePos.Text = dProyek.kodepos;
                txtProyekTelepon.Text = dProyek.telepon;
                txtJabatanKontrak.Text = dProyek.jabatanklien;     

                cmbProyekTujuan.EditValue = dProyek.tujuanproyek;
                cmbProyekJenis.EditValue = dProyek.jenisproyek;
                cmbProyekPIC.EditValue = dProyek.pic;


                // BUTTON
                chkTutup.Enabled = true;

                if (chkTutup.Checked)
                {
                    chkTutup.Enabled = false;
                    btnSimpan.Enabled = false;
                    btnCariKlien.Enabled = false;
                }
            }
            else
            {
                OswControlDefaultProperties.resetAllInput(this);
                deTanggal.EditValue = "";
                rdoJenisInvoiceInterior.Checked = true;

                chkTutup.Enabled = false;
                chkTutup.Checked = false;

                cmbProyekTujuan.ItemIndex = 0;
                cmbProyekPIC.ItemIndex = 0;
            }

            txtKode.Focus();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            // validation
            dxValidationProvider1.SetValidationRule(deTanggal, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(txtKodeKlien, OswValidation.IsNotBlank());

            dxValidationProvider1.SetValidationRule(txtProyekNama, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(txtProyekAlamat, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(cmbProyekTujuan, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(cmbProyekJenis, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(cmbProyekPIC, OswValidation.IsNotBlank());

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
                String strngTutup = chkTutup.Checked ? Constants.STATUS_YA : Constants.STATUS_TIDAK;
                String strngKategori = rdoJenisInvoiceInterior.Checked ? Constants.JENIS_INVOICE_INTERIOR : Constants.JENIS_INVOICE_PRODUCT;
                String strngKlien = txtKodeKlien.Text;

                String strngProyekNama = txtProyekNama.Text;
                String strngProyekAlamat = txtProyekAlamat.Text;
                String strngProyekKota = txtProyekKota.Text;
                String strngProyekTelepon = txtProyekTelepon.Text;
                String strngProyekProvinsi = txtProyekProvinsi.Text;
                String strngProyekKodePos = txtProyekKodePos.Text;
                String strngProyekTujuan = cmbProyekTujuan.EditValue.ToString();
                String strngProyekJenis = cmbProyekJenis.EditValue.ToString();
                String strngProyekPIC = cmbProyekPIC.EditValue.ToString();
                String stringJabatanKontrak = txtJabatanKontrak.Text;

                DataProyek dProyek = new DataProyek(command, strngKode);
                dProyek.tanggaldeal = strngTanggal;

                dProyek.klien = strngKlien;
                dProyek.kategori = strngKategori;
                dProyek.nama = strngProyekNama;
                dProyek.alamat = strngProyekAlamat;
                dProyek.kota = strngProyekKota;
                dProyek.telepon = strngProyekTelepon;
                dProyek.provinsi = strngProyekProvinsi;
                dProyek.kodepos = strngProyekKodePos;
                dProyek.tujuanproyek = strngProyekTujuan;
                dProyek.jenisproyek = strngProyekJenis;
                dProyek.pic = strngProyekPIC;
                dProyek.jabatanklien = stringJabatanKontrak;


                if (this.isAdd)
                {
                    dProyek.status = Constants.STATUS_PROYEK_AKTIF;
                    dProyek.tambah();

                    // update kode header --> setelah generate
                    strngKode = dProyek.kode;
                    txtKode.Text = strngKode;
                }
                else
                {
                    if (chkTutup.Checked)
                    {
                        dProyek.status = Constants.STATUS_PROYEK_TIDAK_AKTIF;
                        dProyek.ubahStatus();
                    }
                    else
                    {
                        dProyek.ubah();
                    }
                }

                // tulis log
                // OswLog.setTransaksi(command, dokumen, dProyek.ToString());

                // reload grid di form header
                FrmProyek frmProyek = (FrmProyek)this.Owner;
                frmProyek.setGrid(command);
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

        private void cmbProyekTujuan_EditValueChanged(object sender, EventArgs e)
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
                cmbProyekJenis = ComboQueryUmum.getJenisProyek(cmbProyekJenis, command, cmbProyekTujuan.ItemIndex < 0 ? "" : cmbProyekTujuan.EditValue.ToString());
                cmbProyekJenis.ItemIndex = 0;

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

    }
}
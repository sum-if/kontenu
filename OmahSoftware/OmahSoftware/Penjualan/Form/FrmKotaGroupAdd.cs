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
using OmahSoftware.Sistem;
using DevExpress.XtraEditors.DXErrorProvider;
using System.Net;
using OmahSoftware.OswLib;
using OmahSoftware.Umum;

namespace OmahSoftware.Penjualan {
    public partial class FrmKotaGroupAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "KOTAGROUP";
        private String dokumen = "KOTAGROUP";
        private Boolean isAdd;
        public String kode;

        public FrmKotaGroupAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmKotaGroupAdd_Load(object sender, EventArgs e) {
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
                String strngKode = this.kode;

                // data
                DataKotaGroup dKotaGroup = new DataKotaGroup(command, strngKode);
                txtNama.EditValue = dKotaGroup.nama;
                chkStatus.Checked = dKotaGroup.status == Constants.STATUS_AKTIF;

            } else {
                OswControlDefaultProperties.resetAllInput(this);
                chkStatus.Checked = true;
            }

            txtNama.Focus();
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
                dxValidationProvider1.SetValidationRule(txtNama, OswValidation.IsNotBlank());
                
                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                // simpan header
                String strngKode = this.kode;
                String strngNama = txtNama.EditValue.ToString();
                String strngStatus = chkStatus.Checked ? Constants.STATUS_AKTIF : Constants.STATUS_AKTIF_TIDAK;

                DataKotaGroup dKotaGroup = new DataKotaGroup(command, strngKode);
                dKotaGroup.nama = strngNama;
                dKotaGroup.status = strngStatus;

                if(this.isAdd) {
                    dKotaGroup.tambah();
                } else {
                    dKotaGroup.ubah();
                }

                // tulis log
                OswLog.setTransaksi(command, dokumen, dKotaGroup.ToString());

                // reload grid di form header
                FrmKotaGroup frmKotaGroup = (FrmKotaGroup)this.Owner;
                frmKotaGroup.setGrid(command);

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

    }
}
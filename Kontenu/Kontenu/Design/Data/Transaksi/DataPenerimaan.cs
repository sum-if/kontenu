﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using Kontenu.OswLib;
using Kontenu.Umum;

namespace Kontenu.Design {
    class DataPenerimaan {
        private String id = "PENERIMAAN";
        public String kode = "";
        public String tanggal = "";
        public String akunkas = "";
        public String klien = "";
        public String penagihan = "";
        public String sisa = "0";
        public String grandtotal = "0";
        public String telahdibayar = "0";
        public String diterima = "0";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Akun Kas:" + akunkas + ";";
            kolom += "Jenis:" + klien + ";";
            kolom += "Proyek:" + penagihan + ";";
            kolom += "Sisa:" + sisa + ";";
            kolom += "Grand Total:" + grandtotal + ";";
            kolom += "Telah dibayar:" + telahdibayar + ";";
            kolom += "Ditagihkan:" + diterima + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataPenerimaan(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal, akunkas, klien, penagihan, sisa,grandtotal, telahdibayar, diterima,version
                             FROM penerimaan 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.akunkas = reader.GetString("akunkas");
                this.klien = reader.GetString("klien");
                this.penagihan = reader.GetString("penagihan");
                this.sisa = reader.GetString("sisa");
                this.grandtotal = reader.GetString("grandtotal");
                this.telahdibayar = reader.GetString("telahdibayar");
                this.diterima = reader.GetString("diterima");
                this.version = reader.GetInt64("version");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        private String generateKode() {
            String strngTanggalSekarang = this.tanggal;
            String strngTahun = OswDate.getTahun(strngTanggalSekarang);
            String strngTahunDuaDigit = OswDate.getTahunDuaDigit(strngTanggalSekarang);
            String strngBulan = OswDate.getBulan(strngTanggalSekarang);
            String strngTanggal = OswDate.getTanggal(strngTanggalSekarang);


            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("Tahun", strngTahun);
            parameters.Add("TahunDuaDigit", strngTahunDuaDigit);
            parameters.Add("Bulan", strngBulan);
            parameters.Add("Tanggal", strngTanggal);

            String kode = OswFormatDokumen.generate(command, id, parameters);

            // jika kode kosong, berarti tidak diseting no otomatis, sehingga harus diisi manual
            if(kode == "") {
                if(this.kode == "") {
                    throw new Exception("Kode/Nomor harus diisi, karena tidak disetting untuk digenerate otomatis");
                } else {
                    kode = this.kode;
                }
            }

            DataPenerimaan dPenerimaan = new DataPenerimaan(command, kode);
            if(dPenerimaan.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public void tambah() {
            // validasi
            valNotExist();

            this.kode = this.generateKode();
            this.version += 1;

            String query = @"INSERT INTO penerimaan(kode, tanggal,akunkas, klien, penagihan, sisa,grandtotal, telahdibayar, diterima,version,create_user) 
                             VALUES(@kode,@tanggal,@akunkas, @klien, @penagihan, @sisa,@grandtotal,@telahdibayar,@diterima,@version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("akunkas", this.akunkas);
            parameters.Add("klien", this.klien);
            parameters.Add("penagihan", this.penagihan);
            parameters.Add("sisa", this.sisa);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("telahdibayar", this.telahdibayar);
            parameters.Add("diterima", this.diterima);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
            //DataPenerimaan dPenerimaan = new DataPenerimaan(command, this.kode);
            DataPenagihan dPenagihan = new DataPenagihan(command, this.penagihan);
            dPenagihan.status = Constants.STATUS_PENAGIHAN_SUDAH_BAYAR;
            dPenagihan.ubahStatus();

        }

        public void hapus() {
            // validasi
            valExist();

            // hapus header
            String query = @"DELETE FROM penerimaan
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);

            DataPenagihan dPenagihan = new DataPenagihan(command, this.penagihan);
            dPenagihan.status = Constants.STATUS_PENAGIHAN_BELUM_BAYAR;
            dPenagihan.ubahStatus();
            //dPenagihan.kurangTotalTagih();
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE penerimaan
                             SET tanggal = @tanggal,
                                 akunkas = @akunkas,
                                 klien = @klien, 
                                 penagihan = @penagihan, 
                                 sisa = @sisa,
                                 grandtotal = @grandtotal,
                                 telahdibayar = @telahdibayar,
                                 diterima = @diterima,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("akunkas", this.akunkas);
            parameters.Add("klien", this.klien);
            parameters.Add("penagihan", this.penagihan);
            parameters.Add("sisa", this.sisa);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("telahdibayar", this.telahdibayar);
            parameters.Add("diterima", this.diterima);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void prosesJurnal()
        {
            /*
            Deskripsi : Retur Penjualan dari [NAMA CUSTOMER]																													
            Status		Akun
            Kredit		akun_piutang						kembali piutang
            Kredit		akun_uang_titipan_customer			kembali uang titipan
            Kredit		diskon_penjualan			        totaldiskon
            Debit		pajak_keluaran					    totalppn
            Debit		retur penjualan				        grand total + totaldiskon - totalppn
            Kredit		akun_HPP						    Total HPP (HPP * Jumlah)
            Debit		akun_persediaan					    Total HPP (HPP * Jumlah)
            */

//            int no = 1;
//            DataCustomer dCustomer = new DataCustomer(command, this.customer);
//            String strngKeteranganJurnal = "Retur Penjualan dari [" + dCustomer.nama + "]";

//            // Debet			ppn_keluaran					
//            String strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PAJAK_KELUARAN);
//            DataAkun dAkun = new DataAkun(command, strngAkun);
//            if (!dAkun.isExist)
//            {
//                throw new Exception("Akun Pajak Keluaran [" + strngAkun + "] tidak ditemukan.");
//            }

//            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, this.totalppn, "0");
//            dJurnal.prosesJurnal();

//            // Debet			retur_penjualan
//            decimal dblPenjualan = Tools.getRoundMoney(decimal.Parse(this.grandtotal) + decimal.Parse(this.totaldiskon) - decimal.Parse(totalppn));
//            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_RETUR_PENJUALAN);
//            dAkun = new DataAkun(command, strngAkun);
//            if (!dAkun.isExist)
//            {
//                throw new Exception("Akun Retur Penjualan [" + strngAkun + "] tidak ditemukan.");
//            }

//            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, dblPenjualan.ToString(), "0");
//            dJurnal.prosesJurnal();

//            if (decimal.Parse(this.kembalipiutang) > 0)
//            {
//                // Kredit			akun_piutang					
//                strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PIUTANG);
//                dAkun = new DataAkun(command, strngAkun);
//                if (!dAkun.isExist)
//                {
//                    throw new Exception("Akun Piutang [" + strngAkun + "] tidak ditemukan.");
//                }

//                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", this.kembalipiutang);
//                dJurnal.prosesJurnal();
//            }

//            if (decimal.Parse(this.kembaliuangtitipan) > 0)
//            {
//                // Kredit			akun_uang_titipan_customer					
//                strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_UANG_TITIPAN_CUSTOMER);
//                dAkun = new DataAkun(command, strngAkun);
//                if (!dAkun.isExist)
//                {
//                    throw new Exception("Akun Uang Titipan Customer [" + strngAkun + "] tidak ditemukan.");
//                }

//                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", this.kembaliuangtitipan);
//                dJurnal.prosesJurnal();
//            }


//            // Kredit			diskon_penjualan
//            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_DISKON_PENJUALAN);
//            dAkun = new DataAkun(command, strngAkun);
//            if (!dAkun.isExist)
//            {
//                throw new Exception("Akun Diskon Penjualan [" + strngAkun + "] tidak ditemukan.");
//            }

//            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", this.totaldiskon);
//            dJurnal.prosesJurnal();

//            // Kredit		akun_HPP						    Total HPP (HPP * Jumlah)
//            String query = @"SELECT SUM(jumlahretur * hpp) 
//                            FROM returpenjualandetail 
//                            WHERE returpenjualan = @returpenjualan";

//            Dictionary<String, String> parameters = new Dictionary<String, String>();
//            parameters.Add("returpenjualan", this.kode);

//            decimal dblHPP = Tools.getRoundMoney(decimal.Parse(OswDataAccess.executeScalarQuery(query, parameters, command)));

//            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_HPP);
//            dAkun = new DataAkun(command, strngAkun);
//            if (!dAkun.isExist)
//            {
//                throw new Exception("Akun HPP [" + strngAkun + "] tidak ditemukan.");
//            }

//            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", dblHPP.ToString());
//            dJurnal.prosesJurnal();

//            // Debit		akun_persediaan					    Total HPP (HPP * Jumlah)
//            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PERSEDIAAN);
//            dAkun = new DataAkun(command, strngAkun);
//            if (!dAkun.isExist)
//            {
//                throw new Exception("Akun Persediaan [" + strngAkun + "] tidak ditemukan.");
//            }

//            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, dblHPP.ToString(), "0");
//            dJurnal.prosesJurnal();

//            Tools.cekJurnalBalance(command, this.id, this.kode);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.kode + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.kode + "] tidak ada");
            }
        }

        private void valVersion() {
            DataPenerimaan dPenerimaan = new DataPenerimaan(command, this.kode);
            if(this.version != dPenerimaan.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        private void valDetail() {
            if(decimal.Parse(this.grandtotal) <= 0) {
                throw new Exception("Grand Total harus > 0");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using OmahSoftware.OswLib;
using OmahSoftware.Umum;
using OmahSoftware.Akuntansi;

namespace OmahSoftware.Akuntansi {
    class DataTransferKas {
        private String id = "TRANSFERKAS";
        public String kode = "";
        public String tanggal = "";
        public String akunawal = "";
        public String akunakhir = "";
        public String nominal = "0";
        public String catatan = "";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Akun Awal:" + akunawal + ";";
            kolom += "Akun Akhir:" + akunakhir + ";";
            kolom += "Nominal:" + nominal + ";";
            kolom += "Catatan:" + catatan + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataTransferKas(MySqlCommand command, String kode) {
            this.command = command;

            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal, akunawal, akunakhir,nominal,catatan, version
                             FROM transferkas 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.akunawal = reader.GetString("akunawal");
                this.akunakhir = reader.GetString("akunakhir");
                this.nominal = reader.GetString("nominal");
                this.catatan = reader.GetString("catatan");
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

            DataTransferKas dTransferKas = new DataTransferKas(command, kode);
            if(dTransferKas.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public void tambah() {
            // validasi
            valNotExist();
            valDetail();
            Tools.valAdmin(command, this.tanggal);

            this.kode = this.kode == "" ? this.generateKode() : this.kode;
            this.version += 1;

            this.prosesJurnal();

            String query = @"INSERT INTO transferkas(kode, tanggal, akunawal, akunakhir,nominal,catatan, version,create_user) 
                             VALUES(@kode,@tanggal,@akunawal,@akunakhir,@nominal,@catatan,@version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("akunawal", this.akunawal);
            parameters.Add("akunakhir", this.akunakhir);
            parameters.Add("nominal", this.nominal);
            parameters.Add("catatan", this.catatan);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();
            valVersion();
            Tools.valAdmin(command, this.tanggal);

            // hapus jurnal
            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode);
            dJurnal.hapusJurnal();

            // hapus header
            String query = @"DELETE FROM transferkas
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();
            valDetail();

            Tools.valAdmin(command, this.tanggal);

            this.version += 1;

            // hapus jurnal
            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode);
            dJurnal.hapusJurnal();

            this.prosesJurnal();

            // proses ubah
            String query = @"UPDATE transferkas
                             SET tanggal = @tanggal,
                                 akunawal = @akunawal, 
                                 akunakhir = @akunakhir, 
                                 nominal = @nominal,
                                 catatan = @catatan,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("akunawal", this.akunawal);
            parameters.Add("akunakhir", this.akunakhir);
            parameters.Add("nominal", this.nominal);
            parameters.Add("catatan", this.catatan);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
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
            DataTransferKas dTransferKas = new DataTransferKas(command, this.kode);
            if(this.version != dTransferKas.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        private void valDetail() {
            if(this.akunawal == this.akunakhir) {
                throw new Exception("Akun Asal dan Akun Tujuan tidak boleh sama");
            }
        }

        private void prosesJurnal() {
            /*
            Deskripsi : Transfer Kas / Bank dari [AWAL] ke [AKHIR]
	        Status			Nama Akun									
            Debet			Akun Akhir								    
            Kredit			Akun Awal							        
            */

            int no = 1;
            DataAkun dAkunAwal = new DataAkun(command, this.akunawal);
            DataAkun dAkunAkhir = new DataAkun(command, this.akunakhir);

            String strngKeteranganJurnal = this.catatan;

            // Debet			Akun Akhir								    
            DataAkun dAkun = new DataAkun(command, this.akunakhir);
            if(!dAkun.isExist) {
                throw new Exception("Akun [" + this.akunakhir + "] tidak ditemukan.");
            }

            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), this.akunakhir, this.nominal, "0");
            dJurnal.prosesJurnal();

            // Kredit			Akun Awal							        
            dAkun = new DataAkun(command, this.akunawal);
            if(!dAkun.isExist) {
                throw new Exception("Akun [" + this.akunawal + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), this.akunawal, "0", this.nominal);
            dJurnal.prosesJurnal();

            Tools.cekJurnalBalance(command, this.id, this.kode);
        }
    }
}

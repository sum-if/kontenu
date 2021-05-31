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

namespace OmahSoftware.Akuntansi {
    class DataAkun {
        private String id = "AKUN";
        private String kode = "";
        public String nama = "";
        public String akunkategori = "";
        public String akunsubkategori = "";
        public String akungroup = "";
        public String akunsubgroup = "";
        public String saldonormal = "";
        public String jurnalmanual = "";
        public String status = "";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Nama:" + nama + ";";
            kolom += "Akun Kategori:" + akunkategori + ";";
            kolom += "Akun Sub Kategori:" + akunsubkategori + ";";
            kolom += "Akun Group:" + akungroup + ";";
            kolom += "Akun Sub Group:" + akunsubgroup + ";";
            kolom += "Saldo Normal:" + saldonormal + ";";
            kolom += "Jurnal Manual:" + jurnalmanual + ";";
            kolom += "Status:" + status + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataAkun(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT nama,akunkategori,akunsubkategori,akungroup,akunsubgroup,saldonormal,jurnalmanual,
                                    status,version
                             FROM akun 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.nama = reader.GetString("nama");
                this.akunkategori = reader.GetString("akunkategori");
                this.akunsubkategori = reader.GetString("akunsubkategori");
                this.akungroup = reader.GetString("akungroup");
                this.akunsubgroup = reader.GetString("akunsubgroup");
                this.saldonormal = reader.GetString("saldonormal");
                this.jurnalmanual = reader.GetString("jurnalmanual");
                this.status = reader.GetString("status");
                this.version = reader.GetInt64("version");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        private String generateKode() {
            String strngTanggalSekarang = OswDate.getStringTanggalHariIni();
            String strngTahun = OswDate.getTahun(strngTanggalSekarang);
            String strngTahunDuaDigit = OswDate.getTahunDuaDigit(strngTanggalSekarang);
            String strngBulan = OswDate.getBulan(strngTanggalSekarang);
            String strngTanggal = OswDate.getTanggal(strngTanggalSekarang);
            String strngInduk = "";

            if(this.akunsubgroup != Constants.TIDAK_ADA) { // sub group ada
                strngInduk = this.akunsubgroup;
            } else if(this.akunsubgroup == Constants.TIDAK_ADA && this.akungroup != Constants.TIDAK_ADA) { // sub group tidak ada, dan group ada
                strngInduk = this.akungroup + ".00";
            } else {
                strngInduk = this.akunsubkategori + "0.00";
            }

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("Tahun", strngTahun);
            parameters.Add("TahunDuaDigit", strngTahunDuaDigit);
            parameters.Add("Bulan", strngBulan);
            parameters.Add("Tanggal", strngTanggal);
            parameters.Add("Induk", strngInduk);

            String kode = OswFormatDokumen.generate(command, id, parameters);

            // jika kode kosong, berarti tidak diseting no otomatis, sehingga harus diisi manual
            if(kode == "") {
                if(this.kode == "") {
                    throw new Exception("Kode/Nomor harus diisi, karena tidak disetting untuk digenerate otomatis");
                } else {
                    kode = this.kode;
                }
            }

            DataAkun dAkun = new DataAkun(command, kode);
            if(dAkun.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public void tambah() {
            // validasi
            valNotExist();

            // this.kode = this.kode == "" ? this.generateKode() : this.kode;
            this.version += 1;

            String query = @"INSERT INTO akun(kode,nama,akunkategori,akunsubkategori,akungroup,akunsubgroup,saldonormal,jurnalmanual,
                                    status,version,create_user) 
                             VALUES(@kode,@nama,@akunkategori,@akunsubkategori,@akungroup,@akunsubgroup,@saldonormal,@jurnalmanual,
                                    @status,@version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);
            parameters.Add("akunkategori", this.akunkategori);
            parameters.Add("akunsubkategori", this.akunsubkategori);
            parameters.Add("akungroup", this.akungroup);
            parameters.Add("akunsubgroup", this.akunsubgroup);
            parameters.Add("saldonormal", this.saldonormal);
            parameters.Add("jurnalmanual", this.jurnalmanual);
            parameters.Add("status", this.status);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();
            valVersion();

            String query = @"DELETE FROM akun
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE akun
                             SET nama = @nama,
                                 akunkategori = @akunkategori,
                                 akunsubkategori = @akunsubkategori,
                                 akungroup = @akungroup,
                                 akunsubgroup = @akunsubgroup,
                                 saldonormal = @saldonormal,
                                 jurnalmanual = @jurnalmanual,
                                 status = @status,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);
            parameters.Add("akunkategori", this.akunkategori);
            parameters.Add("akunsubkategori", this.akunsubkategori);
            parameters.Add("akungroup", this.akungroup);
            parameters.Add("akunsubgroup", this.akunsubgroup);
            parameters.Add("saldonormal", this.saldonormal);
            parameters.Add("jurnalmanual", this.jurnalmanual);
            parameters.Add("status", this.status);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.kode + "," + this.nama + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.kode + "] tidak ada");
            }
        }

        private void valVersion() {
            DataAkun dAkun = new DataAkun(command, this.kode);
            if(this.version != dAkun.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using OmahSoftware.OswLib;

namespace OmahSoftware.Master {
    class DataOutsource {
        private String id = "OUTSOURCE";
        public String kode = "";
        public String nama = "";
        public String alamat = "";
        public String provinsi = "";
        public String kota = ""; 
        public String kodepos = "";
        public String telp = "";
        public String handphone = ""; 
        public String email = "";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Nama:" + nama + ";";
            kolom += "Alamat:" + alamat + ";";
            kolom += "Provinsi:" + provinsi + ";";
            kolom += "Kota:" + kota + ";";
            kolom += "Kode Pos:" + kodepos + ";";
            kolom += "Telp:" + telp + ";";
            kolom += "Email:" + email + ";";
            kolom += "Handphone:" + handphone + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataOutsource(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT nama, alamat, provinsi, kota, kodepos, telp, email, handphone ,version
                             FROM outsource 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.nama = reader.GetString("nama");
                this.alamat = reader.GetString("alamat");
                this.provinsi = reader.GetString("provinsi");
                this.kota = reader.GetString("kota");
                this.kodepos = reader.GetString("kodepos");
                this.telp = reader.GetString("telp");
                this.email = reader.GetString("email");
                this.handphone = reader.GetString("handphone");
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

            DataOutsource dOutsource = new DataOutsource(command, kode);
            if(dOutsource.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public void tambah() {
            // validasi
            valNotExist();

            this.kode = this.kode == "" ? this.generateKode() : this.kode;
            this.version += 1;

            String query = @"INSERT INTO outsource(kode, nama, alamat, provinsi, kota, kodepos, telp, email, 
                                                handphone, version, create_user) 
                                 VALUES(@kode, @nama, @alamat, @provinsi, @kota, @kodepos, @telp, @email, 
                                        @handphone, @version, @create_user)";

            Dictionary<String, object> parameters = new Dictionary<String, object>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);
            parameters.Add("alamat", this.alamat);
            parameters.Add("provinsi", this.provinsi);
            parameters.Add("kota", this.kota);
            parameters.Add("kodepos", this.kodepos);
            parameters.Add("telp", this.telp);
            parameters.Add("email", this.email);
            parameters.Add("handphone", this.handphone);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();
            valVersion();

            String query = @"DELETE FROM outsource
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
            String query = @"UPDATE outsource
                             SET nama = @nama,
                                 alamat = @alamat,
                                 provinsi = @provinsi,
                                 kota = @kota,
                                 kodepos = @kodepos,
                                 telp = @telp,
                                 email = @email,
                                 handphone = @handphone,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, object> parameters = new Dictionary<String, object>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);
            parameters.Add("alamat", this.alamat);
            parameters.Add("provinsi", this.provinsi);
            parameters.Add("kota", this.kota);
            parameters.Add("kodepos", this.kodepos);
            parameters.Add("telp", this.telp);
            parameters.Add("email", this.email);
            parameters.Add("handphone", this.handphone);
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
            DataOutsource dOutsource = new DataOutsource(command, this.kode);
            if(this.version != dOutsource.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

    }
}

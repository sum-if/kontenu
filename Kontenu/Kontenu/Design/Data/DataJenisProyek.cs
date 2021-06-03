using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using Kontenu.OswLib;

namespace Kontenu.Master {
    class DataJenisProyek {
        private String id = "JENISPROYEK";
        public String kode = "";
        public String nama = "";
        public String tujuanproyek = "";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Nama:" + nama + ";";
            kolom += "TujuanProyek:" + tujuanproyek + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataJenisProyek(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT nama, tujuanproyek,version
                             FROM jenisproyek 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.nama = reader.GetString("nama");
                this.tujuanproyek = reader.GetString("tujuanproyek");
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

            DataJenisProyek dJenisProyek = new DataJenisProyek(command, kode);
            if(dJenisProyek.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public void tambah() {
            // validasi
            valNotExist();

            this.kode = this.kode == "" ? this.generateKode() : this.kode;
            this.version += 1;

            String query = @"INSERT INTO jenisproyek(kode, nama, tujuanproyek, version, create_user) 
                                 VALUES(@kode, @nama, @tujuanproyek, @version, @create_user)";

            Dictionary<String, object> parameters = new Dictionary<String, object>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);
            parameters.Add("tujuanproyek", this.tujuanproyek);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();
            valVersion();

            String query = @"DELETE FROM jenisproyek
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
            String query = @"UPDATE jenisproyek
                             SET nama = @nama,
                                 tujuanproyek = @tujuanproyek,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, object> parameters = new Dictionary<String, object>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);
            parameters.Add("tujuanproyek", this.tujuanproyek);
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
            DataJenisProyek dJenisProyek = new DataJenisProyek(command, this.kode);
            if(this.version != dJenisProyek.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

    }
}

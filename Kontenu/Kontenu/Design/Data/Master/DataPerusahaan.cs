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
    class DataPerusahaan {
        private String id = "PERUSAHAAN";
        public String kode = "";
        public String nama = "";
        public String alamat = "";
        public String kota = ""; 
        public String email = "";
        public String telf = "";
        public String website = "";
        public byte[] logo;
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Nama:" + nama + ";";
            kolom += "Alamat:" + alamat + ";";
            kolom += "Kota:" + kota + ";";
            kolom += "Email:" + email + ";";
            kolom += "Telf:" + telf + ";";
            kolom += "Website:" + website + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataPerusahaan(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT logo ,nama, alamat, kota, email, telf, website, version
                             FROM perusahaan 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.nama = reader.GetString("nama");
                this.alamat = reader.GetString("alamat");
                this.kota = reader.GetString("kota");
                this.email = reader.GetString("email");
                this.telf = reader.GetString("telf");
                this.website = reader.GetString("website");
                this.logo = (reader.GetValue(0) is DBNull) ? null : (byte[])reader.GetValue(0);
                this.version = reader.GetInt64("version");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE perusahaan
                             SET nama = @nama,
                                 alamat = @alamat,
                                 kota = @kota,
                                 telf = @telf,
                                 email = @email,
                                 website = @website,
                                 telf = @telf,
                                 logo = @logo,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, object> parameters = new Dictionary<String, object>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);
            parameters.Add("alamat", this.alamat);
            parameters.Add("kota", this.kota);
            parameters.Add("email", this.email);
            parameters.Add("telf", this.telf);
            parameters.Add("website", this.website);
            parameters.Add("logo", this.logo);
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
            DataPerusahaan dPerusahaan = new DataPerusahaan(command, this.kode);
            if(this.version != dPerusahaan.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;

namespace OmahSoftware.Sistem {
    class DataOswPerusahaan {
        public String nama = "";
        public String alamat = "";
        public String telp = "";
        public String email = "";
        public String pajak = "";
        public String npwp = "";
        public String alamatpajak = "";
        public String tipepajak = "";
        public byte[] icon;

        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Nama:" + nama + ";";
            kolom += "Alamat:" + alamat + ";";
            kolom += "Telp:" + telp + ";";
            kolom += "Email:" + email + ";";
            kolom += "Pajak:" + pajak + ";";
            kolom += "NPWP:" + npwp + ";";
            kolom += "Alamat Pajak:" + alamatpajak + ";";
            kolom += "Tipe Pajak:" + tipepajak + ";";
            return kolom;
        }


        public DataOswPerusahaan(MySqlCommand command) {
            this.command = command;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT icon,nama,alamat,telp,email,pajak,npwp,alamatpajak,tipepajak
                             FROM oswperusahaan";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.nama = reader.GetString("nama");
                this.alamat = reader.GetString("alamat");
                this.telp = reader.GetString("telp");
                this.email = reader.GetString("email");
                this.pajak = reader.GetString("pajak");
                this.npwp = reader.GetString("npwp");
                this.alamatpajak = reader.GetString("alamatpajak");
                this.tipepajak = reader.GetString("tipepajak");
                this.icon = (reader.GetValue(0) is DBNull) ? null : (byte[])reader.GetValue(0);
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        public void ubah() {
            // validasi
            valExist();

            // proses ubah
            String query = @"UPDATE oswperusahaan
                             SET nama = @nama,
                                 alamat = @alamat,
                                 telp = @telp,
                                 email = @email,
                                 pajak = @pajak,
                                 npwp = @npwp,
                                 alamatpajak = @alamatpajak,
                                 tipepajak = @tipepajak,
                                 icon = @icon";

            Dictionary<String, object> parameters = new Dictionary<String, object>();
            parameters.Add("nama", this.nama);
            parameters.Add("alamat", this.alamat);
            parameters.Add("telp", this.telp);
            parameters.Add("email", this.email);
            parameters.Add("pajak", this.pajak);
            parameters.Add("npwp", this.npwp);
            parameters.Add("alamatpajak", this.alamatpajak);
            parameters.Add("tipepajak", this.tipepajak);
            parameters.Add("icon", this.icon);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data Perusahaan tidak ada");
            }
        }

    }
}

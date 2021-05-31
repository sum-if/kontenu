using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;

namespace OmahSoftware.Sistem {
    class DataOswSetting {
        public String kode = "";
        public String isi = "";

        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Isi:" + isi + ";";
            return kolom;
        }


        public DataOswSetting(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT isi
                             FROM oswsetting
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.isi = reader.GetString("isi");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        public void tambah() {
            // validasi
            valNotExist();

            String query = @"INSERT INTO oswsetting(kode,isi) 
                             VALUES(@kode,@isi)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("isi", this.isi);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubah() {
            // validasi
            valExist();

            // proses ubah
            String query = @"UPDATE oswsetting
                             SET isi = @isi
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("isi", this.isi);
            
            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data Setting tidak ada");
            }
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data Setting sudah ada");
            }
        }

    }
}

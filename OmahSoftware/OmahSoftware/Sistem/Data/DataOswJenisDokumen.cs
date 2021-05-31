using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;

namespace OmahSoftware.Sistem {
    class DataOswJenisDokumen {
        private String kode = "";
        public String nama = "";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Nama:" + nama + ";";
            return kolom;
        }

        public DataOswJenisDokumen(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT nama
                             FROM oswjenisdokumen 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.nama = reader.GetString("nama");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        public void ubah() {
            // validasi
            valExist();

            if(this.nama == "") {
                throw new Exception("Nama ["+this.kode+"] harus diisi");
            }

            // proses ubah
            String query = @"UPDATE oswjenisdokumen
                             SET nama = @nama
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data Jenis Dokumen tidak ada");
            }
        }
    }
}

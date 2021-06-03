using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using Kontenu.OswLib;

namespace Kontenu.Sistem {
    class DataOswSettingMulti {
        private String kode = "";
        private String isi = "";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Isi:" + isi + ";";
            return kolom;
        }

        public DataOswSettingMulti(MySqlCommand command, String kode, String isi) {
            this.command = command;
            this.kode = kode;
            this.isi = isi;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT 1
                             FROM oswsettingmulti 
                             WHERE kode = @kode AND isi = @isi";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("isi", this.isi);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        public void tambah() {
            // validasi
            valNotExist();

            String query = @"INSERT INTO oswsettingmulti(kode,isi) 
                             VALUES(@kode,@isi)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("isi", this.isi);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapusByKode() {
            String query = @"DELETE FROM oswsettingmulti
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data Setting Multi [" + this.kode + "," + this.isi + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data Setting Multi [" + this.kode + "," + this.isi + "] tidak ada");
            }
        }
    }
}

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
    class DataKasMasukDetail {
        private String kasmasuk = "";
        private String no = "0";
        public String akun = "";
        public String keterangan = "";
        public String jumlah = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kas Masuk:" + kasmasuk + ";";
            kolom += "No:" + no + ";";
            kolom += "Akun:" + akun + ";";
            kolom += "Keterangan:" + keterangan + ";";
            kolom += "Debit:" + jumlah + ";";
            return kolom;
        }

        public DataKasMasukDetail(MySqlCommand command, String kasmasuk, String no) {
            this.command = command;
            this.kasmasuk = kasmasuk;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT akun, keterangan, jumlah
                             FROM kasmasukdetail 
                             WHERE kasmasuk = @kasmasuk AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kasmasuk", this.kasmasuk);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.akun = reader.GetString("akun");
                this.keterangan = reader.GetString("keterangan");
                this.jumlah = reader.GetString("jumlah");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        public void tambah() {
            // validasi
            valNotExist();

            // tambah detail
            String query = @"INSERT INTO kasmasukdetail(kasmasuk,no,akun,keterangan, jumlah) 
                             VALUES(@kasmasuk,@no,@akun,@keterangan,@jumlah)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kasmasuk", this.kasmasuk);
            parameters.Add("no", this.no);
            parameters.Add("akun", this.akun);
            parameters.Add("keterangan", this.keterangan);
            parameters.Add("jumlah", this.jumlah);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            // hapus detail
            String query = @"DELETE FROM kasmasukdetail WHERE kasmasuk = @kasmasuk AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kasmasuk", this.kasmasuk);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.kasmasuk + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.kasmasuk + " - " + this.no + "] tidak ada");
            }
        }
    }
}

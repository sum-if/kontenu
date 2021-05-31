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
    class DataKasKeluarDetail {
        private String kaskeluar = "";
        private String no = "0";
        public String akun = "";
        public String keterangan = "";
        public String jumlah = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            
            kolom += "Kas Keluar:" + kaskeluar + ";";
            kolom += "No:" + no + ";";
            kolom += "Akun:" + akun + ";";
            kolom += "Keterangan:" + keterangan + ";";
            kolom += "Debit:" + jumlah + ";";
            return kolom;
        }

        public DataKasKeluarDetail(MySqlCommand command, String kaskeluar, String no) {
            this.command = command;
            
            this.kaskeluar = kaskeluar;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT akun, keterangan, jumlah
                             FROM kaskeluardetail 
                             WHERE kaskeluar = @kaskeluar AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("kaskeluar", this.kaskeluar);
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
            String query = @"INSERT INTO kaskeluardetail(kaskeluar,no,akun, keterangan,jumlah) 
                             VALUES(@kaskeluar,@no,@akun,@keterangan,@jumlah)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("kaskeluar", this.kaskeluar);
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
            String query = @"DELETE FROM kaskeluardetail WHERE kaskeluar = @kaskeluar AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("kaskeluar", this.kaskeluar);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.kaskeluar + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.kaskeluar + " - " + this.no + "] tidak ada");
            }
        }
    }
}

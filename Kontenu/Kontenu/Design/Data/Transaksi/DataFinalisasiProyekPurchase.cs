using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using Kontenu.OswLib;
using Kontenu.Umum;

namespace Kontenu.Design {
    class DataFinalisasiProyekPurchase {
        private String finalisasiproyek = "";
        private String no = "";
        public String purchase = "";
        public String totalpurchase = "0";
        public String totalbayar = "0";
        public String sisa = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "FinalisasiProyek:" + finalisasiproyek + ";";
            kolom += "no:" + no + ";";
            kolom += "purchase:" + purchase + ";";
            kolom += "totalpurchase:" + totalpurchase + ";";
            kolom += "totalbayar:" + totalbayar + ";";
            kolom += "sisa:" + sisa + ";";
            return kolom;
        }

        public DataFinalisasiProyekPurchase(MySqlCommand command, String finalisasiproyek, String no) {
            this.command = command;
            this.finalisasiproyek = finalisasiproyek;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT purchase, totalpurchase,totalbayar,sisa
                             FROM finalisasiproyekpurchase 
                             WHERE finalisasiproyek = @finalisasiproyek AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("finalisasiproyek", this.finalisasiproyek);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.purchase = reader.GetString("purchase");
                this.totalpurchase = reader.GetString("totalpurchase");
                this.totalbayar = reader.GetString("totalbayar");
                this.sisa = reader.GetString("sisa");
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
            String query = @"INSERT INTO finalisasiproyekpurchase(finalisasiproyek,no,purchase,totalpurchase,totalbayar,sisa) 
                             VALUES(@finalisasiproyek,@no,@purchase,@totalpurchase,@totalbayar,@sisa)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("finalisasiproyek", this.finalisasiproyek);
            parameters.Add("no", this.no);
            parameters.Add("purchase", this.purchase);
            parameters.Add("totalpurchase", this.totalpurchase);
            parameters.Add("totalbayar", this.totalbayar);
            parameters.Add("sisa", this.sisa);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            // hapus detail
            String query = @"DELETE FROM finalisasiproyekpurchase WHERE finalisasiproyek = @finalisasiproyek AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("finalisasiproyek", this.finalisasiproyek);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.finalisasiproyek + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.finalisasiproyek + " - " + this.no + "] tidak ada");
            }
        }
    }
}

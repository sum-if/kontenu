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
    class DataFinalisasiProyekPenjualan {
        private String finalisasiproyek = "";
        private String no = "";
        public String invoice = "";
        public String totalinvoice = "0";
        public String totalditerima = "0";
        public String sisa = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "FinalisasiProyek:" + finalisasiproyek + ";";
            kolom += "no:" + no + ";";
            kolom += "invoice:" + invoice + ";";
            kolom += "totalinvoice:" + totalinvoice + ";";
            kolom += "totalditerima:" + totalditerima + ";";
            kolom += "sisa:" + sisa + ";";
            return kolom;
        }

        public DataFinalisasiProyekPenjualan(MySqlCommand command, String finalisasiproyek, String no) {
            this.command = command;
            this.finalisasiproyek = finalisasiproyek;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT invoice, totalinvoice,totalditerima,sisa
                             FROM finalisasiproyekpenjualan 
                             WHERE finalisasiproyek = @finalisasiproyek AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("finalisasiproyek", this.finalisasiproyek);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.invoice = reader.GetString("invoice");
                this.totalinvoice = reader.GetString("totalinvoice");
                this.totalditerima = reader.GetString("totalditerima");
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
            String query = @"INSERT INTO finalisasiproyekpenjualan(finalisasiproyek,no,invoice,totalinvoice,totalditerima,sisa) 
                             VALUES(@finalisasiproyek,@no,@invoice,@totalinvoice,@totalditerima,@sisa)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("finalisasiproyek", this.finalisasiproyek);
            parameters.Add("no", this.no);
            parameters.Add("invoice", this.invoice);
            parameters.Add("totalinvoice", this.totalinvoice);
            parameters.Add("totalditerima", this.totalditerima);
            parameters.Add("sisa", this.sisa);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            // hapus detail
            String query = @"DELETE FROM finalisasiproyekpenjualan WHERE finalisasiproyek = @finalisasiproyek AND no = @no";

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

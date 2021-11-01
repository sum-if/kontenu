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
    class DataPurchaseDetail {
        private String purchase = "";
        private String no = "";
        public String jasaoutsource = "";
        public String deskripsi = "";
        public String jumlah = "0";
        public String unit = "";
        public String rate = "0";
        public String subtotal = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Purchase:" + purchase + ";";
            kolom += "Id:" + no + ";";
            kolom += "Jasa Outsource:" + jasaoutsource + ";";
            kolom += "Deskripsi:" + deskripsi + ";";
            kolom += "Jumlah:" + jumlah + ";";
            kolom += "Unit:" + unit + ";";
            kolom += "Rate:" + rate + ";";
            kolom += "Subtotal:" + subtotal + ";";
            return kolom;
        }

        public DataPurchaseDetail(MySqlCommand command, String purchase, String no) {
            this.command = command;
            this.purchase = purchase;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT jasaoutsource, deskripsi,jumlah,unit,rate,subtotal
                             FROM purchasedetail 
                             WHERE purchase = @purchase AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("purchase", this.purchase);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.jasaoutsource = reader.GetString("jasaoutsource");
                this.deskripsi = reader.GetString("deskripsi");
                this.jumlah = reader.GetString("jumlah");
                this.unit = reader.GetString("unit");
                this.rate = reader.GetString("rate");
                this.subtotal = reader.GetString("subtotal");
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
            String query = @"INSERT INTO purchasedetail(purchase,no,jasaoutsource,deskripsi,jumlah,unit,rate,subtotal) 
                             VALUES(@purchase,@no,@jasaoutsource,@deskripsi,@jumlah,@unit,@rate,@subtotal)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("purchase", this.purchase);
            parameters.Add("no", this.no);
            parameters.Add("jasaoutsource", this.jasaoutsource);
            parameters.Add("deskripsi", this.deskripsi);
            parameters.Add("jumlah", this.jumlah);
            parameters.Add("unit", this.unit);
            parameters.Add("rate", this.rate);
            parameters.Add("subtotal", this.subtotal);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubah() {
            // validasi
            valExist();

            // hapus detail
            String query = @"UPDATE purchasedetail 
                             SET jasaoutsource = @jasaoutsource,
                                deskripsi = @deskripsi,
                                jumlah = @jumlah,
                                unit = @unit,
                                rate = @rate,
                                subtotal = @subtotal
                             WHERE purchase = @purchase AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("purchase", this.purchase);
            parameters.Add("no", this.no);
            parameters.Add("jasaoutsource", this.jasaoutsource);
            parameters.Add("deskripsi", this.deskripsi);
            parameters.Add("jumlah", this.jumlah);
            parameters.Add("unit", this.unit);
            parameters.Add("rate", this.rate);
            parameters.Add("subtotal", this.subtotal);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            // hapus detail
            String query = @"DELETE FROM purchasedetail WHERE purchase = @purchase AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("purchase", this.purchase);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.purchase + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.purchase + " - " + this.no + "] tidak ada");
            }
        }
    }
}

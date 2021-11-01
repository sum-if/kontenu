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
    class DataPurchasePaymentDetail {
        private String purchasepayment = "";
        private String no = "";
        public String purchase = "";
        public String grandtotal = "0";
        public String telahdibayar = "0";
        public String nominalbayar = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "PurchasePayment:" + purchasepayment + ";";
            kolom += "Id:" + no + ";";
            kolom += "Jasa Outsource:" + purchase + ";";
            kolom += "Deskripsi:" + grandtotal + ";";
            kolom += "Jumlah:" + telahdibayar + ";";
            kolom += "Unit:" + nominalbayar + ";";
            return kolom;
        }

        public DataPurchasePaymentDetail(MySqlCommand command, String purchasepayment, String no) {
            this.command = command;
            this.purchasepayment = purchasepayment;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT purchase, grandtotal,telahdibayar,nominalbayar
                             FROM purchasepaymentdetail 
                             WHERE purchasepayment = @purchasepayment AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("purchasepayment", this.purchasepayment);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.purchase = reader.GetString("purchase");
                this.grandtotal = reader.GetString("grandtotal");
                this.telahdibayar = reader.GetString("telahdibayar");
                this.nominalbayar = reader.GetString("nominalbayar");
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
            String query = @"INSERT INTO purchasepaymentdetail(purchasepayment,no,purchase,grandtotal,telahdibayar,nominalbayar) 
                             VALUES(@purchasepayment,@no,@purchase,@grandtotal,@telahdibayar,@nominalbayar)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("purchasepayment", this.purchasepayment);
            parameters.Add("no", this.no);
            parameters.Add("purchase", this.purchase);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("telahdibayar", this.telahdibayar);
            parameters.Add("nominalbayar", this.nominalbayar);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubah() {
            // validasi
            valExist();

            // hapus detail
            String query = @"UPDATE purchasepaymentdetail 
                             SET purchase = @purchase,
                                grandtotal = @grandtotal,
                                telahdibayar = @telahdibayar,
                                nominalbayar = @nominalbayar
                             WHERE purchasepayment = @purchasepayment AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("purchasepayment", this.purchasepayment);
            parameters.Add("no", this.no);
            parameters.Add("purchase", this.purchase);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("telahdibayar", this.telahdibayar);
            parameters.Add("nominalbayar", this.nominalbayar);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            // hapus detail
            String query = @"DELETE FROM purchasepaymentdetail WHERE purchasepayment = @purchasepayment AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("purchasepayment", this.purchasepayment);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.purchasepayment + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.purchasepayment + " - " + this.no + "] tidak ada");
            }
        }
    }
}

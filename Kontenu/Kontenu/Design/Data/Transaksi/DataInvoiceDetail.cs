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
    class DataInvoiceDetail {
        private String invoice = "";
        private String no = "";
        public String jasa = "";
        public String deskripsi = "";
        public String jumlah = "0";
        public String unit = "";
        public String rate = "0";
        public String subtotal = "0";
        public String quotation = "";
        public String quotationdetailno = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Invoice:" + invoice + ";";
            kolom += "Id:" + no + ";";
            kolom += "Jasa:" + jasa + ";";
            kolom += "Deskripsi:" + deskripsi + ";";
            kolom += "Jumlah:" + jumlah + ";";
            kolom += "Unit:" + unit + ";";
            kolom += "Rate:" + rate + ";";
            kolom += "Subtotal:" + subtotal + ";";
            kolom += "Quotation:" + quotation + ";";
            kolom += "Quotation Detail No:" + quotationdetailno + ";";
            return kolom;
        }

        public DataInvoiceDetail(MySqlCommand command, String invoice, String no) {
            this.command = command;
            this.invoice = invoice;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT jasa, deskripsi,jumlah,unit,rate,subtotal,quotation,quotationdetailno
                             FROM invoicedetail 
                             WHERE invoice = @invoice AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("invoice", this.invoice);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.jasa = reader.GetString("jasa");
                this.deskripsi = reader.GetString("deskripsi");
                this.jumlah = reader.GetString("jumlah");
                this.unit = reader.GetString("unit");
                this.rate = reader.GetString("rate");
                this.subtotal = reader.GetString("subtotal");
                this.quotation = reader.GetString("quotation");
                this.quotationdetailno = reader.GetString("quotationdetailno");
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
            String query = @"INSERT INTO invoicedetail(invoice,no,jasa,deskripsi,jumlah,unit,rate,subtotal,quotation,quotationdetailno) 
                             VALUES(@invoice,@no,@jasa,@deskripsi,@jumlah,@unit,@rate,@subtotal,@quotation,@quotationdetailno)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("invoice", this.invoice);
            parameters.Add("no", this.no);
            parameters.Add("jasa", this.jasa);
            parameters.Add("deskripsi", this.deskripsi);
            parameters.Add("jumlah", this.jumlah);
            parameters.Add("unit", this.unit);
            parameters.Add("rate", this.rate);
            parameters.Add("subtotal", this.subtotal);
            parameters.Add("quotation", this.quotation);
            parameters.Add("quotationdetailno", this.quotationdetailno);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubah() {
            // validasi
            valExist();

            // hapus detail
            String query = @"UPDATE invoicedetail 
                             SET jasa = @jasa,
                                deskripsi = @deskripsi,
                                jumlah = @jumlah,
                                unit = @unit,
                                rate = @rate,
                                subtotal = @subtotal,
                                quotation = @quotation,
                                quotationdetailno = @quotationdetailno,
                             WHERE invoice = @invoice AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("invoice", this.invoice);
            parameters.Add("no", this.no);
            parameters.Add("jasa", this.jasa);
            parameters.Add("deskripsi", this.deskripsi);
            parameters.Add("jumlah", this.jumlah);
            parameters.Add("unit", this.unit);
            parameters.Add("rate", this.rate);
            parameters.Add("subtotal", this.subtotal);
            parameters.Add("quotation", this.quotation);
            parameters.Add("quotationdetailno", this.quotationdetailno);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            // hapus detail
            String query = @"DELETE FROM invoicedetail WHERE invoice = @invoice AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("invoice", this.invoice);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.invoice + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.invoice + " - " + this.no + "] tidak ada");
            }
        }
    }
}

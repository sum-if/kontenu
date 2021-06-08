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
    class DataQuotationDetail {
        private String quotation = "";
        private String no = "";
        public String jasa = "";
        public String deskripsi = "";
        public String jumlah = "0";
        public String unit = "";
        public String rate = "0";
        public String subtotal = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Pesanan Penjualan:" + quotation + ";";
            kolom += "Id:" + no + ";";
            kolom += "Nama Barang:" + jasa + ";";
            kolom += "Jumlah:" + deskripsi + ";";
            kolom += "Satuan:" + jumlah + ";";
            kolom += "Harga Barang:" + unit + ";";
            kolom += "Biaya:" + rate + ";";
            kolom += "Subtotal:" + subtotal + ";";
            return kolom;
        }

        public DataQuotationDetail(MySqlCommand command, String quotation, String no) {
            this.command = command;
            this.quotation = quotation;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT jasa, deskripsi,jumlah,unit,rate,subtotal
                             FROM quotationdetail 
                             WHERE quotation = @quotation AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("quotation", this.quotation);
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
            String query = @"INSERT INTO quotationdetail(quotation,no,jasa,deskripsi,jumlah,unit,rate,subtotal) 
                             VALUES(@quotation,@no,@jasa,@deskripsi,@jumlah,@unit,@rate,@subtotal)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("quotation", this.quotation);
            parameters.Add("no", this.no);
            parameters.Add("jasa", this.jasa);
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
            String query = @"UPDATE quotationdetail 
                             SET jasa = @jasa,
                                deskripsi = @deskripsi,
                                jumlah = @jumlah,
                                unit = @unit,
                                rate = @rate,
                                subtotal = @subtotal
                             WHERE quotation = @quotation AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("quotation", this.quotation);
            parameters.Add("no", this.no);
            parameters.Add("jasa", this.jasa);
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

            this.hapusDetail();

            // hapus detail
            String query = @"DELETE FROM quotationdetail WHERE quotation = @quotation AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("quotation", this.quotation);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);

            // proses hitung
            DataQuotation dQuotation = new DataQuotation(command, this.quotation);
            dQuotation.prosesHitung();
        }

        public void hapusDetail() {
            // validasi
            valExist();

            // hapus detail
            String query = @"DELETE FROM quotationdetailjasa WHERE quotation = @quotation AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("quotation", this.quotation);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.quotation + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.quotation + " - " + this.no + "] tidak ada");
            }
        }
    }
}

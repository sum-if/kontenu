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
    class DataQuotationDetailJasa {
        private String quotation = "";
        private String quotationdetailno = "";
        private String no = "0";
        public String keterangan = "";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Pesanan Penjualan:" + quotation + ";";
            kolom += "Id:" + quotationdetailno + ";";
            kolom += "No:" + no + ";";
            kolom += "Nama:" + keterangan + ";";
            return kolom;
        }

        public DataQuotationDetailJasa(MySqlCommand command, String quotation, String quotationdetailno, String no) {
            this.command = command;
            this.quotation = quotation;
            this.quotationdetailno = quotationdetailno;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT keterangan
                             FROM quotationdetailjasa 
                             WHERE quotation = @quotation AND quotationdetailno = @quotationdetailno AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("quotation", this.quotation);
            parameters.Add("quotationdetailno", this.quotationdetailno);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.keterangan = reader.GetString("keterangan");
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
            String query = @"INSERT INTO quotationdetailjasa(quotation,quotationdetailno,no,keterangan) 
                             VALUES(@quotation,@quotationdetailno,@no,@keterangan)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("quotation", this.quotation);
            parameters.Add("quotationdetailno", this.quotationdetailno);
            parameters.Add("no", this.no);
            parameters.Add("keterangan", this.keterangan);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.quotation + " - " + this.quotationdetailno + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.quotation + " - " + this.quotationdetailno + "] tidak ada");
            }
        }

        private void valDetail() {
            if (this.keterangan != "")
            {
                throw new Exception("Keterangan harus diisi.");
            }
        }
    }
}

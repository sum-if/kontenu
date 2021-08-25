using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using Kontenu.OswLib;

namespace Kontenu.Master {
    class DataVInvoiceTotalTerima {
        public String invoice = "";
        public String totalterima = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public DataVInvoiceTotalTerima(MySqlCommand command, String invoice) {
            this.command = command;
            this.invoice = invoice;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT invoice,totalterima FROM v_invoice_totalterima WHERE invoice = @invoice";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("invoice", this.invoice);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.totalterima = reader.GetString("totalterima");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.invoice + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.invoice + "] tidak ada");
            }
        }

    }
}

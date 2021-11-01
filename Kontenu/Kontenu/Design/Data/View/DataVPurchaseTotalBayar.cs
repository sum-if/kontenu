using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using Kontenu.OswLib;

namespace Kontenu.Design
{
    class DataVPurchaseTotalBayar
    {
        public String purchase = "";
        public String totalbayar = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public DataVPurchaseTotalBayar(MySqlCommand command, String purchase)
        {
            this.command = command;
            this.purchase = purchase;
            this.getOtherAttribute();
        }

        private void getOtherAttribute()
        {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT purchase,totalbayar FROM v_purchase_totalbayar WHERE purchase = @purchase";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("purchase", this.purchase);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if (reader.Read())
            {
                this.isExist = true;
                this.totalbayar = reader.GetString("totalbayar");
                reader.Close();
            }
            else
            {
                this.isExist = false;
                reader.Close();
            }
        }

        private void valNotExist()
        {
            if (this.isExist)
            {
                throw new Exception("Data [" + this.purchase + "] sudah ada");
            }
        }

        private void valExist()
        {
            if (!this.isExist)
            {
                throw new Exception("Data [" + this.purchase + "] tidak ada");
            }
        }

    }
}

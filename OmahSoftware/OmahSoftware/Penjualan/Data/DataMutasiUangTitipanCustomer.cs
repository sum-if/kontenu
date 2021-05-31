using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using OmahSoftware.OswLib;

namespace OmahSoftware.Penjualan {
    class DataMutasiUangTitipanCustomer {
        private String oswjenisdokumen = "";
        private String noreferensi = "";
        public String tanggal = "";
        public String no = "";
        public String customer = "";
        public String jumlah = "0";
        private MySqlCommand command;

        public DataMutasiUangTitipanCustomer(MySqlCommand command, String oswjenisdokumen, String noreferensi) {
            this.command = command;
            this.oswjenisdokumen = oswjenisdokumen;
            this.noreferensi = noreferensi;
        }

        public void proses() {
            // validasi
            if(this.tanggal == "") {
                throw new Exception("Tanggal harus diisi");
            }

            if(this.no == "") {
                throw new Exception("No Item harus diisi");
            }

            if(this.customer == "") {
                throw new Exception("Customer harus diisi");
            }

            // saldo aktual
            if(double.Parse(this.jumlah) > 0) {
                DataSaldoUangTitipanCustomerAktual dSaldoUangTitipanCustomerAktual = new DataSaldoUangTitipanCustomerAktual(command, this.customer);
                dSaldoUangTitipanCustomerAktual.jumlah = this.jumlah;
                dSaldoUangTitipanCustomerAktual.tambah();
            } else if(double.Parse(this.jumlah) < 0) {
                DataSaldoUangTitipanCustomerAktual dSaldoUangTitipanCustomerAktual = new DataSaldoUangTitipanCustomerAktual(command, this.customer);
                dSaldoUangTitipanCustomerAktual.jumlah = (double.Parse(this.jumlah) * -1).ToString();
                dSaldoUangTitipanCustomerAktual.kurang();
            }

            // mutasi customer
            String query = @"INSERT INTO mutasiuangtitipancustomer(tanggal,oswjenisdokumen,noreferensi,no,customer,jumlah,create_user) 
                             VALUES(@tanggal,@oswjenisdokumen,@noreferensi,@no,@customer,@jumlah,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);
            parameters.Add("no", this.no);
            parameters.Add("customer", this.customer);
            parameters.Add("jumlah", this.jumlah);
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // kembalikan saldo sebelumnya
            String query = @"SELECT customer,jumlah
                             FROM mutasiuangtitipancustomer
                             WHERE oswjenisdokumen = @oswjenisdokumen AND noreferensi = @noreferensi";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);

            // buat penampung data itemno
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("customer");
            dataTable.Columns.Add("jumlah");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngCustomer = reader.GetString("customer");
                String strngJumlah = reader.GetString("jumlah");
                dataTable.Rows.Add(strngCustomer, strngJumlah);
            }
            reader.Close();

            // loop per detail lalu hapus
            foreach(DataRow row in dataTable.Rows) {
                String strngCustomer = row["customer"].ToString();
                String strngJumlah = row["jumlah"].ToString();

                if(double.Parse(strngJumlah) > 0) {
                    DataSaldoUangTitipanCustomerAktual dSaldoUangTitipanCustomerAktual = new DataSaldoUangTitipanCustomerAktual(command, strngCustomer);
                    dSaldoUangTitipanCustomerAktual.jumlah = strngJumlah;
                    dSaldoUangTitipanCustomerAktual.kurang();
                } else if(double.Parse(strngJumlah) < 0) {
                    DataSaldoUangTitipanCustomerAktual dSaldoUangTitipanCustomerAktual = new DataSaldoUangTitipanCustomerAktual(command, strngCustomer);
                    dSaldoUangTitipanCustomerAktual.jumlah = (double.Parse(strngJumlah) * -1).ToString();
                    dSaldoUangTitipanCustomerAktual.tambah();
                }
            }

            // proses hapus dimutasi customer
            query = @"DELETE FROM mutasiuangtitipancustomer
                      WHERE oswjenisdokumen = @oswjenisdokumen AND noreferensi = @noreferensi";

            parameters = new Dictionary<String, String>();
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

    }
}

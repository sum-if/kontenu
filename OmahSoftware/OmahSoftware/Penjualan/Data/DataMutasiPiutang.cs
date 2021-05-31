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
    class DataMutasiPiutang {
        private String oswjenisdokumen = "";
        private String noreferensi = "";
        public String tanggal = "";
        public String no = "";
        public String customer = "";
        public String jumlah = "0";
        private MySqlCommand command;

        public DataMutasiPiutang(MySqlCommand command, String oswjenisdokumen, String noreferensi) {
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
                DataSaldoPiutangAktual dSaldoPiutangAktual = new DataSaldoPiutangAktual(command, this.customer);
                dSaldoPiutangAktual.jumlah = this.jumlah;
                dSaldoPiutangAktual.tambah();
            } else if(double.Parse(this.jumlah) < 0) {
                DataSaldoPiutangAktual dSaldoPiutangAktual = new DataSaldoPiutangAktual(command, this.customer);
                dSaldoPiutangAktual.jumlah = (double.Parse(this.jumlah) * -1).ToString();
                dSaldoPiutangAktual.kurang();
            }

            // mutasi customer
            String query = @"INSERT INTO mutasipiutang(tanggal,oswjenisdokumen,noreferensi,no,customer,jumlah,create_user) 
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
                             FROM mutasipiutang
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
                    DataSaldoPiutangAktual dSaldoPiutangAktual = new DataSaldoPiutangAktual(command, strngCustomer);
                    dSaldoPiutangAktual.jumlah = strngJumlah;
                    dSaldoPiutangAktual.kurang();
                } else if(double.Parse(strngJumlah) < 0) {
                    DataSaldoPiutangAktual dSaldoPiutangAktual = new DataSaldoPiutangAktual(command, strngCustomer);
                    dSaldoPiutangAktual.jumlah = (double.Parse(strngJumlah) * -1).ToString();
                    dSaldoPiutangAktual.tambah();
                }
            }

            // proses hapus dimutasi customer
            query = @"DELETE FROM mutasipiutang
                      WHERE oswjenisdokumen = @oswjenisdokumen AND noreferensi = @noreferensi";

            parameters = new Dictionary<String, String>();
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

    }
}

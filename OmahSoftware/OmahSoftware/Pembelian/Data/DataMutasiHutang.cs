using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using OmahSoftware.OswLib;

namespace OmahSoftware.Pembelian {
    class DataMutasiHutang {
        private String oswjenisdokumen = "";
        private String noreferensi = "";
        public String tanggal = "";
        public String no = "";
        public String supplier = "";
        public String jumlah = "0";
        private MySqlCommand command;

        public DataMutasiHutang(MySqlCommand command, String oswjenisdokumen, String noreferensi) {
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

            if(this.supplier == "") {
                throw new Exception("Supplier harus diisi");
            }

            // saldo aktual
            if(double.Parse(this.jumlah) > 0) {
                DataSaldoHutangAktual dSaldoHutangAktual = new DataSaldoHutangAktual(command, this.supplier);
                dSaldoHutangAktual.jumlah = this.jumlah;
                dSaldoHutangAktual.tambah();
            } else if(double.Parse(this.jumlah) < 0) {
                DataSaldoHutangAktual dSaldoHutangAktual = new DataSaldoHutangAktual(command, this.supplier);
                dSaldoHutangAktual.jumlah = (double.Parse(this.jumlah) * -1).ToString();
                dSaldoHutangAktual.kurang();
            }

            // mutasi supplier
            String query = @"INSERT INTO mutasihutang(tanggal,oswjenisdokumen,noreferensi,no,supplier,jumlah,create_user) 
                             VALUES(@tanggal,@oswjenisdokumen,@noreferensi,@no,@supplier,@jumlah,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);
            parameters.Add("no", this.no);
            parameters.Add("supplier", this.supplier);
            parameters.Add("jumlah", this.jumlah);
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // kembalikan saldo sebelumnya
            String query = @"SELECT supplier,jumlah
                             FROM mutasihutang
                             WHERE oswjenisdokumen = @oswjenisdokumen AND noreferensi = @noreferensi";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);

            // buat penampung data itemno
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("supplier");
            dataTable.Columns.Add("jumlah");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngSupplier = reader.GetString("supplier");
                String strngJumlah = reader.GetString("jumlah");
                dataTable.Rows.Add(strngSupplier, strngJumlah);
            }
            reader.Close();

            // loop per detail lalu hapus
            foreach(DataRow row in dataTable.Rows) {
                String strngSupplier = row["supplier"].ToString();
                String strngJumlah = row["jumlah"].ToString();

                if(double.Parse(strngJumlah) > 0) {
                    DataSaldoHutangAktual dSaldoHutangAktual = new DataSaldoHutangAktual(command, strngSupplier);
                    dSaldoHutangAktual.jumlah = strngJumlah;
                    dSaldoHutangAktual.kurang();
                } else if(double.Parse(strngJumlah) < 0) {
                    DataSaldoHutangAktual dSaldoHutangAktual = new DataSaldoHutangAktual(command, strngSupplier);
                    dSaldoHutangAktual.jumlah = (double.Parse(strngJumlah) * -1).ToString();
                    dSaldoHutangAktual.tambah();
                }
            }

            // proses hapus dimutasi supplier
            query = @"DELETE FROM mutasihutang
                      WHERE oswjenisdokumen = @oswjenisdokumen AND noreferensi = @noreferensi";

            parameters = new Dictionary<String, String>();
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

    }
}

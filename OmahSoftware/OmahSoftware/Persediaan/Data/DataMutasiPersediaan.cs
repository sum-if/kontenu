using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using OmahSoftware.OswLib;

namespace OmahSoftware.Persediaan {
    class DataMutasiPersediaan {
        private String oswjenisdokumen = "";
        private String noreferensi = "";
        public String tanggal = "";
        public String barang = "";
        public String no = "0";
        public String jumlah = "0";
        private MySqlCommand command;

        public DataMutasiPersediaan(MySqlCommand command, String oswjenisdokumen, String noreferensi) {
            this.command = command;
            this.oswjenisdokumen = oswjenisdokumen;
            this.noreferensi = noreferensi;
        }

        public void proses() {
            // validasi
            if(this.tanggal == "") {
                throw new Exception("Tanggal harus diisi");
            }

            if(this.barang == "") {
                throw new Exception("Barang harus diisi");
            }


            if(this.no == "0") {
                throw new Exception("No Urut Item harus diisi");
            }

            // saldo aktual
            if(double.Parse(this.jumlah) > 0) {
                DataSaldoPersediaanAktual dSaldoPersediaanAktual = new DataSaldoPersediaanAktual(command, this.barang);
                dSaldoPersediaanAktual.jumlah = this.jumlah;
                dSaldoPersediaanAktual.tambah();
            } else if(double.Parse(this.jumlah) < 0) {
                DataSaldoPersediaanAktual dSaldoPersediaanAktual = new DataSaldoPersediaanAktual(command, this.barang);
                dSaldoPersediaanAktual.jumlah = (double.Parse(this.jumlah) * -1).ToString();
                dSaldoPersediaanAktual.kurang();
            }

            // mutasi persediaan
            String query = @"INSERT INTO mutasipersediaan(tanggal,oswjenisdokumen,noreferensi,no,barang,jumlah,create_user) 
                             VALUES(@tanggal,@oswjenisdokumen,@noreferensi,@no,@barang,@jumlah,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);
            parameters.Add("barang", this.barang);
            parameters.Add("no", this.no);
            parameters.Add("jumlah", this.jumlah);
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // kembalikan stok sebelumnya
            String query = @"SELECT barang,jumlah
                             FROM mutasipersediaan
                             WHERE oswjenisdokumen = @oswjenisdokumen AND noreferensi = @noreferensi";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);

            // buat penampung data itemno
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("barang");
            dataTable.Columns.Add("jumlah");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngBarang = reader.GetString("barang");
                String strngJumlah = reader.GetString("jumlah");
                dataTable.Rows.Add(strngBarang, strngJumlah);
            }
            reader.Close();

            // loop per detail lalu hapus
            foreach(DataRow row in dataTable.Rows) {
                String strngBarang = row["barang"].ToString();
                String strngJumlah = row["jumlah"].ToString();

                if(double.Parse(strngJumlah) > 0) {
                    DataSaldoPersediaanAktual dSaldoPersediaanAktual = new DataSaldoPersediaanAktual(command, strngBarang);
                    dSaldoPersediaanAktual.jumlah = strngJumlah;
                    dSaldoPersediaanAktual.kurang();
                } else if(double.Parse(strngJumlah) < 0) {
                    DataSaldoPersediaanAktual dSaldoPersediaanAktual = new DataSaldoPersediaanAktual(command, strngBarang);
                    dSaldoPersediaanAktual.jumlah = (double.Parse(strngJumlah) * -1).ToString();
                    dSaldoPersediaanAktual.tambah();
                }
            }

            // proses hapus dimutasi persediaan
            query = @"DELETE FROM mutasipersediaan
                      WHERE oswjenisdokumen = @oswjenisdokumen AND noreferensi = @noreferensi";

            parameters = new Dictionary<String, String>();
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

    }
}

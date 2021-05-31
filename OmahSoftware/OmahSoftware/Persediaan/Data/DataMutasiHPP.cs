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
    class DataMutasiHPP {
        private String oswjenisdokumen = "";
        private String noreferensi = "";
        public String tanggal = "";
        public String barang = "";
        public String no = "0";
        public String jumlahawal = "0";
        public String hppawal = "0";
        public String jumlahbaru = "0";
        public String hargabaru = "0";
        public String hppakhir = "0";
        private MySqlCommand command;

        public DataMutasiHPP(MySqlCommand command, String oswjenisdokumen, String noreferensi) {
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

            if(this.jumlahbaru == "0") {
                throw new Exception("Jumlah Baru harus diisi");
            }

            if(this.hargabaru == "0") {
                throw new Exception("Harga Baru harus diisi");
            }

            // HPP Baru = ((HPP skrg * qty skrg) + (Harga sudah potong diskon * Qty beli))/(qty skrg + qty beli)
            DataSaldoPersediaanHPP dSaldoPersediaanHPP = new DataSaldoPersediaanHPP(command, this.barang);
            double dblHPPSekarang = double.Parse(dSaldoPersediaanHPP.nilai);

            DataSaldoPersediaanAktual dSaldoPersediaanAktual = new DataSaldoPersediaanAktual(command, this.barang);
            double dblJumlahSekarang = double.Parse(dSaldoPersediaanAktual.jumlah);

            double dblHPPBaru = Tools.getRoundMoney(((dblHPPSekarang * dblJumlahSekarang) + (double.Parse(this.hargabaru) * double.Parse(this.jumlahbaru))) / (dblJumlahSekarang + double.Parse(this.jumlahbaru)));

            if(dblJumlahSekarang + double.Parse(this.jumlahbaru) == 0) {
                dSaldoPersediaanHPP.nilai = this.hargabaru;
                dSaldoPersediaanHPP.simpan();
            } else {
                dSaldoPersediaanHPP.nilai = dblHPPBaru.ToString();
                dSaldoPersediaanHPP.simpan();
            }

            this.hppawal = dblHPPSekarang.ToString();
            this.jumlahawal = dblJumlahSekarang.ToString();
            this.hppakhir = dblHPPBaru.ToString();

            // mutasi persediaan
            String query = @"INSERT INTO mutasihpp(tanggal,oswjenisdokumen,noreferensi,no,barang,jumlahawal,hppawal,jumlahbaru,hargabaru,hppakhir,create_user) 
                             VALUES(@tanggal,@oswjenisdokumen,@noreferensi,@no,@barang,@jumlahawal,@hppawal,@jumlahbaru,@hargabaru,@hppakhir,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);
            parameters.Add("barang", this.barang);
            parameters.Add("no", this.no);
            parameters.Add("jumlahawal", this.jumlahawal);
            parameters.Add("hppawal", this.hppawal);
            parameters.Add("jumlahbaru", this.jumlahbaru);
            parameters.Add("hargabaru", this.hargabaru);
            parameters.Add("hppakhir", this.hppakhir);
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // kembalikan stok sebelumnya
            String query = @"SELECT barang,jumlahbaru,hargabaru
                             FROM mutasihpp
                             WHERE oswjenisdokumen = @oswjenisdokumen AND noreferensi = @noreferensi";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);

            // buat penampung data itemno
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("barang");
            dataTable.Columns.Add("jumlahbaru");
            dataTable.Columns.Add("hargabaru");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngBarang = reader.GetString("barang");
                String strngJumlahBaru = reader.GetString("jumlahbaru");
                String strngHargaBaru = reader.GetString("hargabaru");
                dataTable.Rows.Add(strngBarang, strngJumlahBaru, strngHargaBaru);
            }
            reader.Close();

            // loop per detail lalu hapus
            foreach(DataRow row in dataTable.Rows) {
                String strngBarang = row["barang"].ToString();
                String strngJumlahBaru = row["jumlahbaru"].ToString();
                String strngHargaBaru = row["hargabaru"].ToString();

                // HPP Baru = ((HPP skrg * qty skrg) - (Harga sudah potong diskon * Qty beli))/(qty skrg - qty beli)
                DataSaldoPersediaanHPP dSaldoPersediaanHPP = new DataSaldoPersediaanHPP(command, strngBarang);
                double dblHPPSekarang = double.Parse(dSaldoPersediaanHPP.nilai);

                DataSaldoPersediaanAktual dSaldoPersediaanAktual = new DataSaldoPersediaanAktual(command, strngBarang);
                double dblJumlahSekarang = double.Parse(dSaldoPersediaanAktual.jumlah);

                double dblHPPBaru = Math.Floor(((dblHPPSekarang * dblJumlahSekarang) - (double.Parse(strngHargaBaru) * double.Parse(strngJumlahBaru))) / (dblJumlahSekarang - double.Parse(strngJumlahBaru)));

                if(dblJumlahSekarang - double.Parse(strngJumlahBaru) == 0) {
                    dSaldoPersediaanHPP.nilai = strngHargaBaru;
                    dSaldoPersediaanHPP.simpan();
                } else {
                    dSaldoPersediaanHPP.nilai = dblHPPBaru.ToString();
                    dSaldoPersediaanHPP.simpan();
                }
            }

            // proses hapus dimutasi persediaan
            query = @"DELETE FROM mutasihpp
                      WHERE oswjenisdokumen = @oswjenisdokumen AND noreferensi = @noreferensi";

            parameters = new Dictionary<String, String>();
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

    }
}

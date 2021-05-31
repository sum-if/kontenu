using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using OmahSoftware.OswLib;
using OmahSoftware.Umum;
using OmahSoftware.Persediaan;

namespace OmahSoftware.Penjualan {
    class DataPesananPenjualanDetail {
        private String pesananpenjualan = "";
        private String no = "0";
        public String barang = "";
        public String satuan = "";
        public String jumlahpesan = "0";
        public String jumlahbackorder = "0";
        public String jumlahfaktur = "0";
        public String hargajual = "0";
        public String diskonitempersen = "0";
        public String diskonitem = "0";
        public String diskonfaktur = "0";
        public String dpp = "0";
        public String ppn = "0";
        public String subtotal = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Pesanan Penjualan:" + pesananpenjualan + ";";
            kolom += "No:" + no + ";";
            kolom += "Barang:" + barang + ";";
            kolom += "Satuan:" + satuan + ";";
            kolom += "Jumlah Pesan:" + jumlahpesan + ";";
            kolom += "Jumlah Back Order:" + jumlahbackorder + ";";
            kolom += "Jumlah Faktur:" + jumlahfaktur + ";";
            kolom += "Harga Jual:" + hargajual + ";";
            kolom += "Diskon Item Persen:" + diskonitempersen + ";";
            kolom += "Diskon Item:" + diskonitem + ";";
            kolom += "Diskon Faktur:" + diskonfaktur + ";";
            kolom += "DPP:" + dpp + ";";
            kolom += "PPN:" + ppn + ";";
            kolom += "Subtotal:" + subtotal + ";";
            return kolom;
        }

        public DataPesananPenjualanDetail(MySqlCommand command, String pesananpenjualan, String no) {
            this.command = command;

            this.pesananpenjualan = pesananpenjualan;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT barang,satuan,jumlahpesan,jumlahbackorder,jumlahfaktur,hargajual,diskonitempersen,diskonitem,diskonfaktur,dpp,ppn,subtotal
                             FROM pesananpenjualandetail 
                             WHERE pesananpenjualan = @pesananpenjualan AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pesananpenjualan", this.pesananpenjualan);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.barang = reader.GetString("barang");
                this.satuan = reader.GetString("satuan");
                this.jumlahpesan = reader.GetString("jumlahpesan");
                this.jumlahbackorder = reader.GetString("jumlahbackorder");
                this.jumlahfaktur = reader.GetString("jumlahfaktur");
                this.hargajual = reader.GetString("hargajual");
                this.diskonitempersen = reader.GetString("diskonitempersen");
                this.diskonitem = reader.GetString("diskonitem");
                this.diskonfaktur = reader.GetString("diskonfaktur");
                this.dpp = reader.GetString("dpp");
                this.ppn = reader.GetString("ppn");
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
            valDetail();

            // tambah detail
            String query = @"INSERT INTO pesananpenjualandetail(pesananpenjualan,no,barang,satuan,jumlahpesan,jumlahbackorder,jumlahfaktur,hargajual,diskonitempersen,diskonitem,diskonfaktur,dpp,ppn,subtotal) 
                             VALUES(@pesananpenjualan,@no,@barang,@satuan,@jumlahpesan,@jumlahbackorder,@jumlahfaktur,@hargajual,@diskonitempersen,@diskonitem,@diskonfaktur,@dpp,@ppn,@subtotal)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pesananpenjualan", this.pesananpenjualan);
            parameters.Add("no", this.no);
            parameters.Add("barang", this.barang);
            parameters.Add("satuan", this.satuan);
            parameters.Add("jumlahpesan", this.jumlahpesan);
            parameters.Add("jumlahbackorder", this.jumlahbackorder);
            parameters.Add("jumlahfaktur", this.jumlahfaktur);
            parameters.Add("hargajual", this.hargajual);
            parameters.Add("diskonitempersen", this.diskonitempersen);
            parameters.Add("diskonitem", this.diskonitem);
            parameters.Add("diskonfaktur", this.diskonfaktur);
            parameters.Add("dpp", this.dpp);
            parameters.Add("ppn", this.ppn);
            parameters.Add("subtotal", this.subtotal);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void tambahJumlahFaktur() {
            // validasi
            valExist();

            // update jumlah faktur di pesanan
            DataPesananPenjualanDetail dPesananPenjualanDetail = new DataPesananPenjualanDetail(command, this.pesananpenjualan, this.no);
            double dblJumlahLama = double.Parse(dPesananPenjualanDetail.jumlahfaktur);
            double dblJumlahBaru = dblJumlahLama + double.Parse(this.jumlahfaktur);

            String query = @"UPDATE pesananpenjualandetail 
                             SET jumlahfaktur = @jumlahfaktur
                             WHERE pesananpenjualan = @pesananpenjualan AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("pesananpenjualan", this.pesananpenjualan);
            parameters.Add("no", this.no);
            parameters.Add("jumlahfaktur", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);

            // cek jumlah faktur
            this.cekJumlahFakturPesanan();
        }

        public void kurangJumlahFaktur() {
            // validasi
            valExist();

            // update jumlah faktur di pesanan
            DataPesananPenjualanDetail dPesananPenjualanDetail = new DataPesananPenjualanDetail(command, this.pesananpenjualan, this.no);
            double dblJumlahLama = double.Parse(dPesananPenjualanDetail.jumlahfaktur);
            double dblJumlahBaru = dblJumlahLama - double.Parse(this.jumlahfaktur);

            // jumlah faktur < 0
            if(dblJumlahBaru < 0) {
                DataBarang dBarang = new DataBarang(command, this.barang);
                throw new Exception("Jumlah Faktur [" + dBarang.nama + "] < 0 di Pesanan Penjualan");
            }

            String query = @"UPDATE pesananpenjualandetail 
                             SET jumlahfaktur = @jumlahfaktur
                             WHERE pesananpenjualan = @pesananpenjualan AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("pesananpenjualan", this.pesananpenjualan);
            parameters.Add("no", this.no);
            parameters.Add("jumlahfaktur", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);

            // cek jumlah faktur
            this.cekJumlahFakturPesanan();
        }

        public void tambahJumlahBackOrder() {
            // validasi
            valExist();

            // update jumlah faktur di pesanan
            DataPesananPenjualanDetail dPesananPenjualanDetail = new DataPesananPenjualanDetail(command, this.pesananpenjualan, this.no);
            double dblJumlahLama = double.Parse(dPesananPenjualanDetail.jumlahbackorder);
            double dblJumlahBaru = dblJumlahLama + double.Parse(this.jumlahbackorder);

            String query = @"UPDATE pesananpenjualandetail 
                             SET jumlahbackorder = @jumlahbackorder
                             WHERE pesananpenjualan = @pesananpenjualan AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("pesananpenjualan", this.pesananpenjualan);
            parameters.Add("no", this.no);
            parameters.Add("jumlahbackorder", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);

            // cek jumlah faktur
            this.cekJumlahFakturPesanan();
        }

        public void kurangJumlahBackOrder() {
            // validasi
            valExist();

            // update jumlah faktur di pesanan
            DataPesananPenjualanDetail dPesananPenjualanDetail = new DataPesananPenjualanDetail(command, this.pesananpenjualan, this.no);
            double dblJumlahLama = double.Parse(dPesananPenjualanDetail.jumlahbackorder);
            double dblJumlahBaru = dblJumlahLama - double.Parse(this.jumlahbackorder);

            // jumlah faktur < 0
            if(dblJumlahBaru < 0) {
                DataBarang dBarang = new DataBarang(command, this.barang);
                throw new Exception("Jumlah Faktur [" + dBarang.nama + "] < 0 di Pesanan Penjualan");
            }

            String query = @"UPDATE pesananpenjualandetail 
                             SET jumlahbackorder = @jumlahbackorder
                             WHERE pesananpenjualan = @pesananpenjualan AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("pesananpenjualan", this.pesananpenjualan);
            parameters.Add("no", this.no);
            parameters.Add("jumlahbackorder", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);

            // cek jumlah faktur
            this.cekJumlahFakturPesanan();
        }

        private void cekJumlahFakturPesanan() {
            String query = @"SELECT COUNT(*)
                             FROM pesananpenjualandetail A
                             WHERE A.pesananpenjualan = @pesananpenjualan AND A.jumlahpesan > A.jumlahfaktur + A.jumlahbackorder";

            Dictionary<String, String> parameters = new Dictionary<string, string>();

            parameters.Add("pesananpenjualan", this.pesananpenjualan);

            Double dblJumlah = Double.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlah <= 0) {
                DataPesananPenjualan dPesananPenjualan = new DataPesananPenjualan(command, this.pesananpenjualan);
                dPesananPenjualan.status = Constants.STATUS_PESANAN_PENJUALAN_SELESAI;
                dPesananPenjualan.ubahStatus();
            } else {
                DataPesananPenjualan dPesananPenjualan = new DataPesananPenjualan(command, this.pesananpenjualan);
                dPesananPenjualan.status = Constants.STATUS_PESANAN_PENJUALAN_DALAM_PROSES;
                dPesananPenjualan.ubahStatus();
            }
        }

        public void hapus() {
            // validasi
            valExist();

            // hapus detail
            String query = @"DELETE FROM pesananpenjualandetail WHERE pesananpenjualan = @pesananpenjualan AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("pesananpenjualan", this.pesananpenjualan);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.pesananpenjualan + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.pesananpenjualan + " - " + this.no + "] tidak ada");
            }
        }

        private void valDetail() {
            DataBarang dBarang = new DataBarang(command, this.barang);

            if(double.Parse(this.jumlahpesan) <= 0) {
                throw new Exception("Jumlah Pesan Barang [" + dBarang.nama + "] harus > 0");
            }

            if(double.Parse(this.hargajual) <= 0) {
                throw new Exception("Harga Jual Barang [" + dBarang.nama + "] harus > 0");
            }

            if(double.Parse(this.subtotal) <= 0) {
                throw new Exception("Subtotal Barang [" + dBarang.nama + "] harus > 0");
            }

            // dpp tidak bisa di bawah harga modal
            DataSaldoPersediaanHPP dSaldoPersediaanHPP = new DataSaldoPersediaanHPP(command, this.barang);
            if(double.Parse(this.dpp) < double.Parse(dSaldoPersediaanHPP.nilai)) {
                throw new Exception("DPP Barang [" + dBarang.nama + "] [" + OswConvert.convertToRupiah(double.Parse(this.dpp)) + "] harus lebih besar dari harga modal [" + OswConvert.convertToRupiah(double.Parse(dSaldoPersediaanHPP.nilai)) + "]");
            }

        }
    }
}

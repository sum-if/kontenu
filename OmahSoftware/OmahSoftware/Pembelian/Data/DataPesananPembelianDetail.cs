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

namespace OmahSoftware.Pembelian {
    class DataPesananPembelianDetail {
        private String pesananpembelian = "";
        private String no = "0";
        public String barang = "";
        public String satuan = "";
        public String jumlah = "0";
        public String jumlahfaktur = "0";
        public String hargabeli = "0";
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
            kolom += "Pesanan Pembelian:" + pesananpembelian + ";";
            kolom += "No:" + no + ";";
            kolom += "Barang:" + barang + ";";
            kolom += "Satuan:" + satuan + ";";
            kolom += "Jumlah:" + jumlah + ";";
            kolom += "Jumlah Faktur:" + jumlahfaktur + ";";
            kolom += "Harga Beli:" + hargabeli + ";";
            kolom += "Diskon Item Persen:" + diskonitempersen + ";";
            kolom += "Diskon Item:" + diskonitem + ";";
            kolom += "Diskon Faktur:" + diskonfaktur + ";";
            kolom += "DPP:" + dpp + ";";
            kolom += "PPN:" + ppn + ";";
            kolom += "Subtotal:" + subtotal + ";";
            return kolom;
        }

        public DataPesananPembelianDetail(MySqlCommand command, String pesananpembelian, String no) {
            this.command = command;
            this.pesananpembelian = pesananpembelian;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT barang, satuan, jumlah,jumlahfaktur,hargabeli,diskonitempersen,diskonitem,diskonfaktur,dpp,ppn,subtotal
                             FROM pesananpembeliandetail 
                             WHERE pesananpembelian = @pesananpembelian AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pesananpembelian", this.pesananpembelian);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.barang = reader.GetString("barang");
                this.satuan = reader.GetString("satuan");
                this.jumlah = reader.GetString("jumlah");
                this.jumlahfaktur = reader.GetString("jumlahfaktur");
                this.hargabeli = reader.GetString("hargabeli");
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
            String query = @"INSERT INTO pesananpembeliandetail(pesananpembelian,no,barang,satuan, jumlah,jumlahfaktur,hargabeli,diskonitempersen,diskonitem,diskonfaktur,dpp,ppn,subtotal) 
                             VALUES(@pesananpembelian,@no,@barang,@satuan,@jumlah,@jumlahfaktur,@hargabeli,@diskonitempersen,@diskonitem,@diskonfaktur,@dpp,@ppn,@subtotal)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pesananpembelian", this.pesananpembelian);
            parameters.Add("no", this.no);
            parameters.Add("barang", this.barang);
            parameters.Add("satuan", this.satuan);
            parameters.Add("jumlah", this.jumlah);
            parameters.Add("jumlahfaktur", this.jumlahfaktur);
            parameters.Add("hargabeli", this.hargabeli);
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

            // update jumlah faktur
            DataPesananPembelianDetail dPesananPembelianDetail = new DataPesananPembelianDetail(command, this.pesananpembelian, this.no);
            double dblJumlahLama = double.Parse(dPesananPembelianDetail.jumlahfaktur);
            double dblJumlahBaru = dblJumlahLama + double.Parse(this.jumlahfaktur);

            String query = @"UPDATE pesananpembeliandetail 
                             SET jumlahfaktur = @jumlahfaktur
                             WHERE pesananpembelian = @pesananpembelian AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pesananpembelian", this.pesananpembelian);
            parameters.Add("no", this.no);
            parameters.Add("jumlahfaktur", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);

            // cek status
            DataPesananPembelian dPesananPembelian = new DataPesananPembelian(command, this.pesananpembelian);
            dPesananPembelian.cekStatus();
        }

        public void kurangJumlahFaktur() {
            // validasi
            valExist();

            // update jumlah faktur
            DataPesananPembelianDetail dPesananPembelianDetail = new DataPesananPembelianDetail(command, this.pesananpembelian, this.no);
            double dblJumlahLama = double.Parse(dPesananPembelianDetail.jumlahfaktur);
            double dblJumlahBaru = dblJumlahLama - double.Parse(this.jumlahfaktur);

            // jumlah terima < 0
            if(dblJumlahBaru < 0) {
                DataBarang dBarang = new DataBarang(command, this.barang);

                throw new Exception("Jumlah Faktur [" + dBarang.nama + "] < 0 di Pesanan Pembelian");
            }

            String query = @"UPDATE pesananpembeliandetail 
                             SET jumlahfaktur = @jumlahfaktur
                             WHERE pesananpembelian = @pesananpembelian AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pesananpembelian", this.pesananpembelian);
            parameters.Add("no", this.no);
            parameters.Add("jumlahfaktur", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);

            // cek status
            DataPesananPembelian dPesananPembelian = new DataPesananPembelian(command, this.pesananpembelian);
            dPesananPembelian.cekStatus();
        }

        public void hapus() {
            // validasi
            valExist();

            // hapus detail
            String query = @"DELETE FROM pesananpembeliandetail WHERE pesananpembelian = @pesananpembelian AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pesananpembelian", this.pesananpembelian);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.pesananpembelian + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.pesananpembelian + " - " + this.no + "] tidak ada");
            }
        }

        private void valDetail() {
            DataBarang dBarang = new DataBarang(command, this.barang);

            if(double.Parse(this.jumlah) <= 0) {
                throw new Exception("Jumlah Barang [" + dBarang.nama + "] harus > 0");
            }

            if(double.Parse(this.hargabeli) <= 0) {
                throw new Exception("Harga Beli Barang [" + dBarang.nama + "] harus > 0");
            }

            if(double.Parse(this.subtotal) <= 0) {
                throw new Exception("Subtotal Barang [" + dBarang.nama + "] harus > 0");
            }
        }
    }
}

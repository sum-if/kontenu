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

namespace OmahSoftware.Pembelian {
    class DataFakturPembelianDetail {
        private String id = "FAKTURPEMBELIAN";
        private String fakturpembelian = "";
        private String no = "0";
        public String barang = "";
        public String satuan = "";
        public String jumlah = "0";
        public String jumlahretur = "0";
        public String hargabeli = "0";
        public String diskonitempersen = "0";
        public String diskonitem = "0";
        public String diskonfaktur = "0";
        public String dpp = "0";
        public String ppn = "0";
        public String subtotal = "0";
        public String pesananpembeliandetail = "";
        public String pesananpembeliandetailno = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Faktur Pembelian:" + fakturpembelian + ";";
            kolom += "No:" + no + ";";
            kolom += "Barang:" + barang + ";";
            kolom += "Satuan:" + satuan + ";";
            kolom += "Jumlah:" + jumlah + ";";
            kolom += "Jumlah Retur:" + jumlahretur + ";";
            kolom += "Harga Beli:" + hargabeli + ";";
            kolom += "Diskon Item Persen:" + diskonitempersen + ";";
            kolom += "Diskon Item:" + diskonitem + ";";
            kolom += "Diskon Faktur:" + diskonfaktur + ";";
            kolom += "DPP:" + dpp + ";";
            kolom += "PPN:" + ppn + ";";
            kolom += "Subtotal:" + subtotal + ";";
            kolom += "Pesanan Pembelian Detail:" + pesananpembeliandetail + ";";
            kolom += "Pesanan Pembelian Detail No:" + pesananpembeliandetailno + ";";
            return kolom;
        }

        public DataFakturPembelianDetail(MySqlCommand command, String fakturpembelian, String no) {
            this.command = command;
            this.fakturpembelian = fakturpembelian;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT barang, satuan, jumlah,jumlahretur,hargabeli,diskonitempersen,diskonitem,diskonfaktur,dpp,ppn,subtotal,pesananpembeliandetail,pesananpembeliandetailno
                             FROM fakturpembeliandetail 
                             WHERE fakturpembelian = @fakturpembelian AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("fakturpembelian", this.fakturpembelian);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.barang = reader.GetString("barang");
                this.satuan = reader.GetString("satuan");
                this.jumlah = reader.GetString("jumlah");
                this.jumlahretur = reader.GetString("jumlahretur");
                this.hargabeli = reader.GetString("hargabeli");
                this.diskonitempersen = reader.GetString("diskonitempersen");
                this.diskonitem = reader.GetString("diskonitem");
                this.diskonfaktur = reader.GetString("diskonfaktur");
                this.dpp = reader.GetString("dpp");
                this.ppn = reader.GetString("ppn");
                this.subtotal = reader.GetString("subtotal");
                this.pesananpembeliandetail = reader.GetString("pesananpembeliandetail");
                this.pesananpembeliandetailno = reader.GetString("pesananpembeliandetailno");
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

            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.fakturpembelian);

            // update hpp
            DataMutasiHPP dMutasiHPP = new DataMutasiHPP(command, this.id, this.fakturpembelian);
            dMutasiHPP.tanggal = dFakturPembelian.tanggal;
            dMutasiHPP.barang = this.barang;
            dMutasiHPP.jumlahbaru = this.jumlah;
            dMutasiHPP.hargabaru = this.dpp;
            dMutasiHPP.no = this.no;
            dMutasiHPP.proses();

            // mutasi persediaan
            DataMutasiPersediaan dMutasiPersediaan = new DataMutasiPersediaan(command, this.id, this.fakturpembelian);
            dMutasiPersediaan.tanggal = dFakturPembelian.tanggal;
            dMutasiPersediaan.barang = this.barang;
            dMutasiPersediaan.jumlah = this.jumlah;
            dMutasiPersediaan.no = this.no;
            dMutasiPersediaan.proses();

            // ubah harga beli terakhir di barang
            DataBarang dBarang = new DataBarang(command, this.barang);
            dBarang.hargabeliterakhir = this.dpp;
            dBarang.ubah();

            // tambah jumlah faktur di pesanan
            if(this.pesananpembeliandetail != "") {
                DataPesananPembelianDetail dPesananPembelianDetail = new DataPesananPembelianDetail(command, this.pesananpembeliandetail, this.pesananpembeliandetailno);
                dPesananPembelianDetail.jumlahfaktur = this.jumlah;
                dPesananPembelianDetail.tambahJumlahFaktur();
            }

            // tambah detail
            String query = @"INSERT INTO fakturpembeliandetail(fakturpembelian,no,barang,satuan, jumlah,jumlahretur,hargabeli,diskonitempersen,diskonitem,diskonfaktur,dpp,ppn,subtotal,pesananpembeliandetail,pesananpembeliandetailno) 
                             VALUES(@fakturpembelian,@no,@barang,@satuan,@jumlah,@jumlahretur,@hargabeli,@diskonitempersen,@diskonitem,@diskonfaktur,@dpp,@ppn,@subtotal,@pesananpembeliandetail,@pesananpembeliandetailno)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("fakturpembelian", this.fakturpembelian);
            parameters.Add("no", this.no);
            parameters.Add("barang", this.barang);
            parameters.Add("satuan", this.satuan);
            parameters.Add("jumlah", this.jumlah);
            parameters.Add("jumlahretur", this.jumlahretur);
            parameters.Add("hargabeli", this.hargabeli);
            parameters.Add("diskonitempersen", this.diskonitempersen);
            parameters.Add("diskonitem", this.diskonitem);
            parameters.Add("diskonfaktur", this.diskonfaktur);
            parameters.Add("dpp", this.dpp);
            parameters.Add("ppn", this.ppn);
            parameters.Add("subtotal", this.subtotal);
            parameters.Add("pesananpembeliandetail", this.pesananpembeliandetail);
            parameters.Add("pesananpembeliandetailno", this.pesananpembeliandetailno);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }


        public void tambahJumlahRetur() {
            // validasi
            valExist();

            // update jumlah faktur
            DataFakturPembelianDetail dFakturPembelianDetail = new DataFakturPembelianDetail(command, this.fakturpembelian, this.no);
            double dblJumlahLama = double.Parse(dFakturPembelianDetail.jumlahretur);
            double dblJumlahBaru = dblJumlahLama + double.Parse(this.jumlahretur);

            String query = @"UPDATE fakturpembeliandetail 
                             SET jumlahretur = @jumlahretur
                             WHERE fakturpembelian = @fakturpembelian AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("fakturpembelian", this.fakturpembelian);
            parameters.Add("no", this.no);
            parameters.Add("jumlahretur", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void kurangJumlahRetur() {
            // validasi
            valExist();

            // update jumlah faktur
            DataFakturPembelianDetail dFakturPembelianDetail = new DataFakturPembelianDetail(command, this.fakturpembelian, this.no);
            double dblJumlahLama = double.Parse(dFakturPembelianDetail.jumlahretur);
            double dblJumlahBaru = dblJumlahLama - double.Parse(this.jumlahretur);

            // jumlah terima < 0
            if(dblJumlahBaru < 0) {
                DataBarang dBarang = new DataBarang(command, this.barang);

                throw new Exception("Jumlah Faktur [" + dBarang.nama + "] < 0 di Faktur Pembelian");
            }

            String query = @"UPDATE fakturpembeliandetail 
                             SET jumlahretur = @jumlahretur
                             WHERE fakturpembelian = @fakturpembelian AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("fakturpembelian", this.fakturpembelian);
            parameters.Add("no", this.no);
            parameters.Add("jumlahretur", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            // tambah jumlah faktur di pesanan
            if(this.pesananpembeliandetail != "") {
                DataPesananPembelianDetail dPesananPembelianDetail = new DataPesananPembelianDetail(command, this.pesananpembeliandetail, this.pesananpembeliandetailno);
                dPesananPembelianDetail.jumlahfaktur = this.jumlah;
                dPesananPembelianDetail.kurangJumlahFaktur();
            }

            // hapus detail
            String query = @"DELETE FROM fakturpembeliandetail WHERE fakturpembelian = @fakturpembelian AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("fakturpembelian", this.fakturpembelian);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.fakturpembelian + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.fakturpembelian + " - " + this.no + "] tidak ada");
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

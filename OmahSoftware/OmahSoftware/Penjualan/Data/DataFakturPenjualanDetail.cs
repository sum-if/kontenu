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
    class DataFakturPenjualanDetail {
        private String id = "FAKTURPENJUALAN";
        private String fakturpenjualan = "";
        private String no = "0";
        public String barang = "";
        public String satuan = "";
        public String colli = "";
        public String jumlahfaktur = "0";
        public String jumlahretur = "0";
        public String hargajual = "0";
        public String diskonitempersen = "0";
        public String diskonitem = "0";
        public String diskonfaktur = "0";
        public String hpp = "0";
        public String dpp = "0";
        public String ppn = "0";
        public String subtotal = "0";
        public String pesananpenjualandetail = "";
        public String pesananpenjualandetailno = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Faktur Penjualan:" + fakturpenjualan + ";";
            kolom += "No:" + no + ";";
            kolom += "Barang:" + barang + ";";
            kolom += "Satuan:" + satuan + ";";
            kolom += "Colli:" + colli + ";";
            kolom += "Jumlah Faktur:" + jumlahfaktur + ";";
            kolom += "Jumlah Retur:" + jumlahretur + ";";
            kolom += "Harga Jual:" + hargajual + ";";
            kolom += "Diskon Item Persen:" + diskonitempersen + ";";
            kolom += "Diskon Item:" + diskonitem + ";";
            kolom += "Diskon Faktur:" + diskonfaktur + ";";
            kolom += "HPP:" + hpp + ";";
            kolom += "DPP:" + dpp + ";";
            kolom += "PPN:" + ppn + ";";
            kolom += "Subtotal:" + subtotal + ";";
            kolom += "Pesanan Penjualan Detail:" + pesananpenjualandetail + ";";
            kolom += "Pesanan Penjualan Detail No:" + pesananpenjualandetailno + ";";
            return kolom;
        }

        public DataFakturPenjualanDetail(MySqlCommand command, String fakturpenjualan, String no) {
            this.command = command;
            this.fakturpenjualan = fakturpenjualan;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT barang, satuan,colli,jumlahfaktur,jumlahretur,hargajual,diskonitempersen,diskonitem,diskonfaktur,
                                    hpp,dpp,ppn, subtotal,pesananpenjualandetail,pesananpenjualandetailno
                             FROM fakturpenjualandetail 
                             WHERE fakturpenjualan = @fakturpenjualan AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("fakturpenjualan", this.fakturpenjualan);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.barang = reader.GetString("barang");
                this.satuan = reader.GetString("satuan");
                this.colli = reader.GetString("colli");
                this.jumlahfaktur = reader.GetString("jumlahfaktur");
                this.jumlahretur = reader.GetString("jumlahretur");
                this.hargajual = reader.GetString("hargajual");
                this.diskonitempersen = reader.GetString("diskonitempersen");
                this.diskonitem = reader.GetString("diskonitem");
                this.diskonfaktur = reader.GetString("diskonfaktur");
                this.hpp = reader.GetString("hpp");
                this.dpp = reader.GetString("dpp");
                this.ppn = reader.GetString("ppn");
                this.subtotal = reader.GetString("subtotal");
                this.pesananpenjualandetail = reader.GetString("pesananpenjualandetail");
                this.pesananpenjualandetailno = reader.GetString("pesananpenjualandetailno");
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

            DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, this.fakturpenjualan);

            if(dFakturPenjualan.jenispesananpenjualan == Constants.JENIS_PESANAN_PENJUALAN_PESANAN_PENJUALAN) {
                // tambah jumlah faktur di pesanan penjualan
                DataPesananPenjualanDetail dPesananPenjualanDetail = new DataPesananPenjualanDetail(command, this.pesananpenjualandetail, this.pesananpenjualandetailno);
                dPesananPenjualanDetail.jumlahfaktur = this.jumlahfaktur;
                dPesananPenjualanDetail.tambahJumlahFaktur();
            } else if(dFakturPenjualan.jenispesananpenjualan == Constants.JENIS_PESANAN_PENJUALAN_BACK_ORDER) {
                // tambah jumlah faktur di back order
                DataBackOrderDetail dBackOrderDetail = new DataBackOrderDetail(command, this.pesananpenjualandetail, this.pesananpenjualandetailno);
                dBackOrderDetail.jumlahfaktur = this.jumlahfaktur;
                dBackOrderDetail.tambahJumlahFaktur();
            }
            
            // mutasi persediaan
            DataMutasiPersediaan dMutasiPersediaan = new DataMutasiPersediaan(command, this.id, this.fakturpenjualan);
            dMutasiPersediaan.tanggal = dFakturPenjualan.tanggal;
            dMutasiPersediaan.barang = this.barang;
            dMutasiPersediaan.jumlah = (double.Parse(this.jumlahfaktur) * -1).ToString();
            dMutasiPersediaan.no = this.no;
            dMutasiPersediaan.proses();

            // tambah detail
            String query = @"INSERT INTO fakturpenjualandetail(fakturpenjualan,no,barang,satuan,colli, jumlahfaktur,jumlahretur,hargajual,diskonitempersen,diskonitem,diskonfaktur,hpp,dpp,ppn, subtotal,pesananpenjualandetail,pesananpenjualandetailno) 
                             VALUES(@fakturpenjualan,@no,@barang,@satuan,@colli,@jumlahfaktur,@jumlahretur,@hargajual,@diskonitempersen,@diskonitem,@diskonfaktur,@hpp,@dpp,@ppn, @subtotal,@pesananpenjualandetail,@pesananpenjualandetailno)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("fakturpenjualan", this.fakturpenjualan);
            parameters.Add("no", this.no);
            parameters.Add("barang", this.barang);
            parameters.Add("satuan", this.satuan);
            parameters.Add("colli", this.colli);
            parameters.Add("jumlahfaktur", this.jumlahfaktur);
            parameters.Add("jumlahretur", this.jumlahretur);
            parameters.Add("hargajual", this.hargajual);
            parameters.Add("diskonitempersen", this.diskonitempersen);
            parameters.Add("diskonitem", this.diskonitem);
            parameters.Add("diskonfaktur", this.diskonfaktur);
            parameters.Add("hpp", this.hpp);
            parameters.Add("dpp", this.dpp);
            parameters.Add("ppn", this.ppn);
            parameters.Add("subtotal", this.subtotal);
            parameters.Add("pesananpenjualandetail", this.pesananpenjualandetail);
            parameters.Add("pesananpenjualandetailno", this.pesananpenjualandetailno);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, this.fakturpenjualan);

            if(dFakturPenjualan.jenispesananpenjualan == Constants.JENIS_PESANAN_PENJUALAN_PESANAN_PENJUALAN) {
                // tambah jumlah faktur di pesanan penjualan
                DataPesananPenjualanDetail dPesananPenjualanDetail = new DataPesananPenjualanDetail(command, this.pesananpenjualandetail, this.pesananpenjualandetailno);
                dPesananPenjualanDetail.jumlahfaktur = this.jumlahfaktur;
                dPesananPenjualanDetail.kurangJumlahFaktur();
            } else if(dFakturPenjualan.jenispesananpenjualan == Constants.JENIS_PESANAN_PENJUALAN_BACK_ORDER) {
                // tambah jumlah faktur di back order
                DataBackOrderDetail dBackOrderDetail = new DataBackOrderDetail(command, this.pesananpenjualandetail, this.pesananpenjualandetailno);
                dBackOrderDetail.jumlahfaktur = this.jumlahfaktur;
                dBackOrderDetail.kurangJumlahFaktur();
            }

            // hapus detail
            String query = @"DELETE FROM fakturpenjualandetail WHERE fakturpenjualan = @fakturpenjualan AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("fakturpenjualan", this.fakturpenjualan);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void tambahJumlahRetur() {
            // validasi
            valExist();

            // update jumlah retur di faktur
            DataFakturPenjualanDetail dFakturPenjualanDetail = new DataFakturPenjualanDetail(command, this.fakturpenjualan, this.no);
            double dblJumlahLama = double.Parse(dFakturPenjualanDetail.jumlahretur);
            double dblJumlahBaru = dblJumlahLama + double.Parse(this.jumlahretur);

            // jumlah retur > jumlah faktur
            if(double.Parse(this.jumlahfaktur) < dblJumlahBaru) {
                DataBarang dBarang = new DataBarang(command, this.barang);
                throw new Exception("Jumlah Retur [" + dBarang.nama + "] > Jumlah Faktur di Faktur Penjualan");
            }

            String query = @"UPDATE fakturpenjualandetail 
                             SET jumlahretur = @jumlahretur
                             WHERE fakturpenjualan = @fakturpenjualan AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("fakturpenjualan", this.fakturpenjualan);
            parameters.Add("no", this.no);
            parameters.Add("jumlahretur", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void kurangJumlahRetur() {
            // validasi
            valExist();

            // update jumlah kirim di faktur
            DataFakturPenjualanDetail dFakturPenjualanDetail = new DataFakturPenjualanDetail(command, this.fakturpenjualan, this.no);
            double dblJumlahLama = double.Parse(dFakturPenjualanDetail.jumlahretur);
            double dblJumlahBaru = dblJumlahLama - double.Parse(this.jumlahretur);

            // jumlah kirim < 0
            if(dblJumlahBaru < 0) {
                DataBarang dBarang = new DataBarang(command, this.barang);
                throw new Exception("Jumlah Retur [" + dBarang.nama + "] < 0 di Faktur Penjualan");
            }

            String query = @"UPDATE fakturpenjualandetail 
                             SET jumlahretur = @jumlahretur
                             WHERE fakturpenjualan = @fakturpenjualan AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("fakturpenjualan", this.fakturpenjualan);
            parameters.Add("no", this.no);
            parameters.Add("jumlahretur", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                
                throw new Exception("Data [" + this.fakturpenjualan + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                
                throw new Exception("Data [" + this.fakturpenjualan + " - " + this.no + "] tidak ada");
            }
        }

        private void valDetail() {
            DataBarang dBarang = new DataBarang(command, this.barang);

            if(double.Parse(this.jumlahfaktur) <= 0) {
                throw new Exception("Jumlah Faktur Barang [" + dBarang.nama + "] harus > 0");
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

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
    class DataReturPenjualanDetail {
        private String id = "RETURPENJUALAN";
        private String returpenjualan = "";
        private String no = "0";
        public String barang = "";
        public String satuan = "";
        public String jumlahretur = "0";
        public String hargajual = "0";
        public String diskonitempersen = "0";
        public String diskonitem = "0";
        public String diskonfaktur = "0";
        public String hpp = "0";
        public String dpp = "0";
        public String ppn = "0";
        public String subtotal = "0";
        public String fakturpenjualandetail = "";
        public String fakturpenjualandetailno = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Retur Penjualan:" + returpenjualan + ";";
            kolom += "No:" + no + ";";
            kolom += "Barang:" + barang + ";";
            kolom += "Satuan:" + satuan + ";";
            kolom += "Jumlah Retur:" + jumlahretur + ";";
            kolom += "Harga Jual:" + hargajual + ";";
            kolom += "Diskon Item Persen:" + diskonitempersen + ";";
            kolom += "Diskon Item:" + diskonitem + ";";
            kolom += "Diskon Faktur:" + diskonfaktur + ";";
            kolom += "HPP:" + hpp + ";";
            kolom += "dpp:" + dpp + ";";
            kolom += "ppn:" + ppn + ";";
            kolom += "Subtotal:" + subtotal + ";";
            kolom += "Faktur Penjualan Detail:" + fakturpenjualandetail + ";";
            kolom += "Faktur Penjualan Detail No:" + fakturpenjualandetailno + ";";
            return kolom;
        }

        public DataReturPenjualanDetail(MySqlCommand command, String returpenjualan, String no) {
            this.command = command;
            this.returpenjualan = returpenjualan;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT  barang, jumlahretur, hargajual, hpp, diskonitempersen, diskonitem, subtotal,satuan,diskonfaktur,dpp,ppn,fakturpenjualandetail,fakturpenjualandetailno
                             FROM returpenjualandetail 
                             WHERE returpenjualan = @returpenjualan AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("returpenjualan", this.returpenjualan);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.barang = reader.GetString("barang");
                this.jumlahretur = reader.GetString("jumlahretur");
                this.hargajual = reader.GetString("hargajual");
                this.hpp = reader.GetString("hpp");
                this.diskonitempersen = reader.GetString("diskonitempersen");
                this.diskonitem = reader.GetString("diskonitem");
                this.subtotal = reader.GetString("subtotal");
                this.satuan = reader.GetString("satuan");
                this.diskonfaktur = reader.GetString("diskonfaktur");
                this.dpp = reader.GetString("dpp");
                this.ppn = reader.GetString("ppn");
                this.fakturpenjualandetail = reader.GetString("fakturpenjualandetail");
                this.fakturpenjualandetailno = reader.GetString("fakturpenjualandetailno");
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

            DataReturPenjualan dReturPenjualan = new DataReturPenjualan(command, this.returpenjualan);

            // tambah jumlah retur di faktur penjualan
            DataFakturPenjualanDetail dFakturPenjualanDetail = new DataFakturPenjualanDetail(command, this.fakturpenjualandetail, this.fakturpenjualandetailno);
            dFakturPenjualanDetail.jumlahretur = this.jumlahretur;
            dFakturPenjualanDetail.tambahJumlahRetur();

            // update hpp
            DataMutasiHPP dMutasiHPP = new DataMutasiHPP(command, this.id, this.returpenjualan);
            dMutasiHPP.tanggal = dReturPenjualan.tanggal;
            dMutasiHPP.barang = this.barang;
            dMutasiHPP.jumlahbaru = this.jumlahretur;
            dMutasiHPP.hargabaru = this.hpp;
            dMutasiHPP.no = this.no;
            dMutasiHPP.proses();

            // tambah mutasi
            // mutasi persediaan
            DataMutasiPersediaan dMutasiPersediaan = new DataMutasiPersediaan(command, this.id, this.returpenjualan);
            dMutasiPersediaan.tanggal = dReturPenjualan.tanggal;
            dMutasiPersediaan.barang = this.barang;
            dMutasiPersediaan.jumlah = this.jumlahretur;
            dMutasiPersediaan.no = this.no;
            dMutasiPersediaan.proses();

            
            
            // tambah detail
            String query = @"INSERT INTO returpenjualandetail( returpenjualan, no, barang, jumlahretur, hargajual, hpp, diskonitempersen, diskonitem, subtotal,satuan,diskonfaktur,dpp,ppn,fakturpenjualandetail,fakturpenjualandetailno) 
                             VALUES(@returpenjualan, @no, @barang, @jumlahretur, @hargajual, @hpp, @diskonitempersen, @diskonitem, @subtotal,@satuan,@diskonfaktur,@dpp,@ppn,@fakturpenjualandetail,@fakturpenjualandetailno)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("returpenjualan", this.returpenjualan);
            parameters.Add("no", this.no);
            parameters.Add("barang", this.barang);
            parameters.Add("jumlahretur", this.jumlahretur);
            parameters.Add("hargajual", this.hargajual);
            parameters.Add("hpp", this.hpp);
            parameters.Add("diskonitempersen", this.diskonitempersen);
            parameters.Add("diskonitem", this.diskonitem);
            parameters.Add("subtotal", this.subtotal);
            parameters.Add("satuan", this.satuan);
            parameters.Add("diskonfaktur", this.diskonfaktur);
            parameters.Add("dpp", this.dpp);
            parameters.Add("ppn", this.ppn);
            parameters.Add("fakturpenjualandetail", this.fakturpenjualandetail);
            parameters.Add("fakturpenjualandetailno", this.fakturpenjualandetailno);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            DataReturPenjualan dReturPenjualan = new DataReturPenjualan(command, this.returpenjualan);

            // tambah jumlah retur di faktur penjualan
            DataFakturPenjualanDetail dFakturPenjualanDetail = new DataFakturPenjualanDetail(command, this.fakturpenjualandetail, this.fakturpenjualandetailno);
            dFakturPenjualanDetail.jumlahretur = this.jumlahretur;
            dFakturPenjualanDetail.kurangJumlahRetur();
            
            // hapus detail
            String query = @"DELETE FROM returpenjualandetail WHERE returpenjualan = @returpenjualan AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("returpenjualan", this.returpenjualan);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                
                throw new Exception("Data [" + this.returpenjualan + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                
                throw new Exception("Data [" + this.returpenjualan + " - " + this.no + "] tidak ada");
            }
        }

        private void valDetail() {
            DataBarang dBarang = new DataBarang(command, this.barang);

            if(double.Parse(this.jumlahretur) <= 0) {
                throw new Exception("Jumlah Retur Barang [" + dBarang.nama + "] harus > 0");
            }

            if(double.Parse(this.hargajual) <= 0) {
                throw new Exception("Harga Jual Barang [" + dBarang.nama + "] harus > 0");
            }

            if(double.Parse(this.subtotal) <= 0) {
                throw new Exception("Subtotal Barang [" + dBarang.nama + "] harus > 0");
            }
        }
    }
}

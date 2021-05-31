using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using OmahSoftware.OswLib;
using OmahSoftware.Persediaan;
using OmahSoftware.Umum;

namespace OmahSoftware.Pembelian {
    class DataReturPembelianDetail {
        private String id = "RETURPEMBELIAN";
        private String returpembelian = "";
        private String no = "0";
        public String barang = "";
        public String satuan = "";
        public String jumlahfaktur = "0";
        public String jumlahretur = "0";
        public String hargabeli = "0";
        public String diskonitempersen = "0";
        public String diskonitem = "0";
        public String diskonfaktur = "0";
        public String dpp = "0";
        public String ppn = "0";
        public String subtotal = "0";
        public String fakturpembeliandetail = "";
        public String fakturpembeliandetailno = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Retur Pembelian:" + returpembelian + ";";
            kolom += "No:" + no + ";";
            kolom += "Barang:" + barang + ";";
            kolom += "Satuan:" + satuan + ";";
            kolom += "Jumlah Faktur:" + jumlahfaktur + ";";
            kolom += "Jumlah Retur:" + jumlahretur + ";";
            kolom += "Harga Beli:" + hargabeli + ";";
            kolom += "Diskon Persen:" + diskonitempersen + ";";
            kolom += "Diskon Item:" + diskonitem + ";";
            kolom += "Diskon Nilai:" + diskonfaktur + ";";
            kolom += "DPP:" + dpp + ";";
            kolom += "PPN:" + ppn + ";";
            kolom += "Subtotal:" + subtotal + ";";
            kolom += "Faktur Pembelian Detail:" + fakturpembeliandetail+ ";";
            kolom += "Faktur Pembelian Detail No:" + fakturpembeliandetailno + ";";
            return kolom;
        }

        public DataReturPembelianDetail(MySqlCommand command, String returpembelian, String no) {
            this.command = command;
            this.returpembelian = returpembelian;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT barang, jumlahfaktur,jumlahretur,hargabeli,diskonitempersen,diskonitem,diskonfaktur,dpp, satuan,ppn,subtotal,fakturpembeliandetail,fakturpembeliandetailno
                             FROM returpembeliandetail 
                             WHERE returpembelian = @returpembelian AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("returpembelian", this.returpembelian);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.barang = reader.GetString("barang");
                this.jumlahfaktur = reader.GetString("jumlahfaktur");
                this.jumlahretur = reader.GetString("jumlahretur");
                this.hargabeli = reader.GetString("hargabeli");
                this.diskonitempersen = reader.GetString("diskonitempersen");
                this.diskonitem = reader.GetString("diskonitem");
                this.diskonfaktur = reader.GetString("diskonfaktur");
                this.dpp = reader.GetString("dpp");
                this.satuan = reader.GetString("satuan");
                this.ppn = reader.GetString("ppn");
                this.subtotal = reader.GetString("subtotal");
                this.fakturpembeliandetail = reader.GetString("fakturpembeliandetail");
                this.fakturpembeliandetailno = reader.GetString("fakturpembeliandetailno");
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

            DataReturPembelian dReturPembelian = new DataReturPembelian(command, this.returpembelian);

            // tambah jumlah retur di faktur pembelian
            DataFakturPembelianDetail dFakturPembelianDetail = new DataFakturPembelianDetail(command, this.fakturpembeliandetail, this.fakturpembeliandetailno);
            dFakturPembelianDetail.jumlahretur = this.jumlahretur;
            dFakturPembelianDetail.tambahJumlahRetur();

            // tambah mutasi
            // mutasi persediaan
            DataMutasiPersediaan dMutasiPersediaan = new DataMutasiPersediaan(command, this.id, this.returpembelian);
            dMutasiPersediaan.tanggal = dReturPembelian.tanggal;
            dMutasiPersediaan.barang = this.barang;
            dMutasiPersediaan.jumlah = (double.Parse(this.jumlahretur) * -1).ToString();
            dMutasiPersediaan.no = this.no;
            dMutasiPersediaan.proses();

            // tambah detail
            String query = @"INSERT INTO returpembeliandetail(returpembelian,no,barang,jumlahfaktur,jumlahretur,hargabeli,diskonitempersen,diskonitem,diskonfaktur,dpp, satuan,ppn,subtotal,fakturpembeliandetail,fakturpembeliandetailno) 
                             VALUES(@returpembelian,@no,@barang,@jumlahfaktur,@jumlahretur,@hargabeli,@diskonitempersen,@diskonitem,@diskonfaktur,@dpp, @satuan,@ppn,@subtotal,@fakturpembeliandetail,@fakturpembeliandetailno)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("returpembelian", this.returpembelian);
            parameters.Add("no", this.no);
            parameters.Add("barang", this.barang);
            parameters.Add("satuan", this.satuan);
            parameters.Add("jumlahfaktur", this.jumlahfaktur);
            parameters.Add("jumlahretur", this.jumlahretur);
            parameters.Add("hargabeli", this.hargabeli);
            parameters.Add("diskonitempersen", this.diskonitempersen);
            parameters.Add("diskonitem", this.diskonitem);
            parameters.Add("diskonfaktur", this.diskonfaktur);
            parameters.Add("dpp", this.dpp);
            parameters.Add("ppn", this.ppn);
            parameters.Add("subtotal", this.subtotal);
            parameters.Add("fakturpembeliandetail", this.fakturpembeliandetail);
            parameters.Add("fakturpembeliandetailno", this.fakturpembeliandetailno);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            DataReturPembelian dReturPembelian = new DataReturPembelian(command, this.returpembelian);
            
            // tambah jumlah retur di faktur pembelian
            DataFakturPembelianDetail dFakturPembelianDetail = new DataFakturPembelianDetail(command, this.fakturpembeliandetail, this.fakturpembeliandetailno);
            dFakturPembelianDetail.jumlahretur = this.jumlahretur;
            dFakturPembelianDetail.kurangJumlahRetur();

            // hapus detail
            String query = @"DELETE FROM returpembeliandetail WHERE returpembelian = @returpembelian AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("returpembelian", this.returpembelian);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.returpembelian + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.returpembelian + " - " + this.no + "] tidak ada");
            }
        }

        private void valDetail() {
            DataBarang dBarang = new DataBarang(command, this.barang);

            if(double.Parse(this.jumlahretur) <= 0) {
                throw new Exception("Jumlah Retur Barang [" + dBarang.nama + "] harus > 0");
            }

            if(double.Parse(this.jumlahretur) > double.Parse(this.jumlahfaktur)) {
                throw new Exception("Jumlah Retur Barang [" + dBarang.nama + "] tidak boleh > Jumlah Faktur");
            }

            if(double.Parse(this.hargabeli) <= 0) {
                throw new Exception("Harga Beli Barang [" + dBarang.nama + "] harus > 0");
            }

            if(double.Parse(this.dpp) <= 0) {
                throw new Exception("Subtotal Barang [" + dBarang.nama + "] harus > 0");
            }
        }
    }
}

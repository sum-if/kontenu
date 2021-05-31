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
    class DataPenerimaanPenjualanDetail {
        private String id = "PENERIMAANPENJUALAN";
        private String penerimaanpenjualan = "";
        private String no = "0";
        public String fakturpenjualan = "";
        public String totalfaktur = "0";
        public String totalbayar = "0";
        public String totalretur = "0";
        public String jumlah = "0";
        public String selisih = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Penerimaan Penjualan:" + penerimaanpenjualan + ";";
            kolom += "No:" + no + ";";
            kolom += "Faktur Penjualan:" + fakturpenjualan + ";";
            kolom += "Total Faktur:" + totalfaktur + ";";
            kolom += "Total Bayar:" + totalbayar + ";";
            kolom += "Total Retur:" + totalretur + ";";
            kolom += "Jumlah:" + jumlah + ";";
            kolom += "Selisih:" + selisih + ";";
            return kolom;
        }

        public DataPenerimaanPenjualanDetail(MySqlCommand command, String penerimaanpenjualan, String no) {
            this.command = command;
            this.penerimaanpenjualan = penerimaanpenjualan;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT fakturpenjualan, totalfaktur,totalbayar,totalretur, jumlah, selisih
                             FROM penerimaanpenjualandetail 
                             WHERE penerimaanpenjualan = @penerimaanpenjualan AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("penerimaanpenjualan", this.penerimaanpenjualan);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.fakturpenjualan = reader.GetString("fakturpenjualan");
                this.totalfaktur = reader.GetString("totalfaktur");
                this.totalbayar = reader.GetString("totalbayar");
                this.totalretur = reader.GetString("totalretur");
                this.jumlah = reader.GetString("jumlah");
                this.selisih = reader.GetString("selisih");
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

            // tambah jumlah bayar di faktur
            DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, this.fakturpenjualan);
            double dblTotalBayar = Tools.getRoundMoney(double.Parse(this.jumlah) + double.Parse(this.selisih));
            dFakturPenjualan.totalbayar = dblTotalBayar.ToString();
            dFakturPenjualan.tambahTotalBayar();

            // tambah detail
            String query = @"INSERT INTO penerimaanpenjualandetail(penerimaanpenjualan,no,fakturpenjualan, totalfaktur,totalbayar,totalretur,jumlah,selisih) 
                             VALUES(@penerimaanpenjualan,@no,@fakturpenjualan, @totalfaktur,@totalbayar,@totalretur,@jumlah,@selisih)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("penerimaanpenjualan", this.penerimaanpenjualan);
            parameters.Add("no", this.no);
            parameters.Add("fakturpenjualan", this.fakturpenjualan);
            parameters.Add("totalfaktur", this.totalfaktur);
            parameters.Add("totalbayar", this.totalbayar);
            parameters.Add("totalretur", this.totalretur);
            parameters.Add("jumlah", this.jumlah);
            parameters.Add("selisih", this.selisih);    

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            // kurang jumlah bayar di faktur
            DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, this.fakturpenjualan);
            double dblTotalBayar = Tools.getRoundMoney(double.Parse(this.jumlah) + double.Parse(this.selisih));
            dFakturPenjualan.totalbayar = dblTotalBayar.ToString();
            dFakturPenjualan.kurangTotalBayar();
            
            // hapus detail
            String query = @"DELETE FROM penerimaanpenjualandetail WHERE penerimaanpenjualan = @penerimaanpenjualan AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("penerimaanpenjualan", this.penerimaanpenjualan);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.penerimaanpenjualan + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.penerimaanpenjualan + " - " + this.no + "] tidak ada");
            }
        }

        private void valDetail() {
            DataPenerimaanPenjualan dPenerimaanPenjualan = new DataPenerimaanPenjualan(command, this.penerimaanpenjualan);
            DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, this.fakturpenjualan);

            if(OswDate.getDateTimeFromStringTanggal(dPenerimaanPenjualan.tanggal) < OswDate.getDateTimeFromStringTanggal(dFakturPenjualan.tanggal)) {
                throw new Exception("Tanggal Penerimaan Penjualan harus >= Tanggal Faktur Penjualan [" + this.fakturpenjualan + " - " + dFakturPenjualan.tanggal + "]");
            }

            if(double.Parse(this.jumlah) <= 0) {
                throw new Exception("Jumlah Bayar Faktur Pembelian [" + this.fakturpenjualan + "] harus > 0");
            }
        }
    }
}

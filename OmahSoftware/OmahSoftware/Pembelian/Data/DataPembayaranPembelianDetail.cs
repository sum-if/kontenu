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
    class DataPembayaranPembelianDetail {
        private String id = "PEMBAYARANPEMBELIAN";
        private String pembayaranpembelian = "";
        private String no = "0";
        public String fakturpembelian = "";
        public String totalfaktur = "0";
        public String totalbayar = "0";
        public String totalretur = "0";
        public String jumlah = "0";
        public String selisih = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Pembayaran Pembelian:" + pembayaranpembelian + ";";
            kolom += "No:" + no + ";";
            kolom += "Faktur Pembelian:" + fakturpembelian + ";";
            kolom += "Total Faktur:" + totalfaktur + ";";
            kolom += "Total Bayar:" + totalbayar + ";";
            kolom += "Total Retur:" + totalretur + ";";
            kolom += "Jumlah:" + jumlah + ";";
            kolom += "Selisih:" + selisih + ";";
            return kolom;
        }

        public DataPembayaranPembelianDetail(MySqlCommand command, String pembayaranpembelian, String no) {
            this.command = command;
            this.pembayaranpembelian = pembayaranpembelian;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT fakturpembelian, totalfaktur,totalbayar,totalretur, jumlah,selisih
                             FROM pembayaranpembeliandetail 
                             WHERE pembayaranpembelian = @pembayaranpembelian AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pembayaranpembelian", this.pembayaranpembelian);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.fakturpembelian = reader.GetString("fakturpembelian");
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
            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.fakturpembelian);
            double dblTotalBayar = Tools.getRoundMoney(double.Parse(this.jumlah) + double.Parse(this.selisih));
            dFakturPembelian.totalbayar = dblTotalBayar.ToString();
            dFakturPembelian.tambahTotalBayar();

            // tambah detail
            String query = @"INSERT INTO pembayaranpembeliandetail(pembayaranpembelian,no,fakturpembelian, totalfaktur,totalbayar,totalretur,jumlah,selisih) 
                             VALUES(@pembayaranpembelian,@no,@fakturpembelian, @totalfaktur,@totalbayar,@totalretur,@jumlah,@selisih)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pembayaranpembelian", this.pembayaranpembelian);
            parameters.Add("no", this.no);
            parameters.Add("fakturpembelian", this.fakturpembelian);
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
            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.fakturpembelian);
            double dblTotalBayar = Tools.getRoundMoney(double.Parse(this.jumlah) + double.Parse(this.selisih));
            dFakturPembelian.totalbayar = dblTotalBayar.ToString();
            dFakturPembelian.kurangTotalBayar();

            // hapus detail
            String query = @"DELETE FROM pembayaranpembeliandetail WHERE pembayaranpembelian = @pembayaranpembelian AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pembayaranpembelian", this.pembayaranpembelian);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.pembayaranpembelian + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.pembayaranpembelian + " - " + this.no + "] tidak ada");
            }
        }

        private void valDetail() {
            DataPembayaranPembelian dPembayaranPembelian = new DataPembayaranPembelian(command, this.pembayaranpembelian);
            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.fakturpembelian);

            if(OswDate.getDateTimeFromStringTanggal(dPembayaranPembelian.tanggal) < OswDate.getDateTimeFromStringTanggal(dFakturPembelian.tanggal)) {
                throw new Exception("Tanggal Pembayaran Pembelian harus >= Tanggal Faktur Pembelien [" + this.fakturpembelian + " - " + dFakturPembelian.tanggal + "]");
            }

            if(double.Parse(this.jumlah) <= 0) {
                throw new Exception("Jumlah Bayar Faktur Pembelian [" + this.fakturpembelian + "] harus > 0");
            }
        }
    }
}

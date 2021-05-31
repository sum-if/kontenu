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
    class DataPembayaranHutangEkspedisiDetail {
        private String id = "PEMBAYARANHUTANGEKSPEDISI";
        private String pembayaranhutangekspedisi = "";
        private String no = "0";
        public String fakturpembelian = "";
        public String jumlah = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Pembayaran Pembelian:" + pembayaranhutangekspedisi + ";";
            kolom += "No:" + no + ";";
            kolom += "Faktur Pembelian:" + fakturpembelian + ";";
            kolom += "Jumlah:" + jumlah + ";";
            return kolom;
        }

        public DataPembayaranHutangEkspedisiDetail(MySqlCommand command, String pembayaranhutangekspedisi, String no) {
            this.command = command;
            this.pembayaranhutangekspedisi = pembayaranhutangekspedisi;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT fakturpembelian, jumlah
                             FROM pembayaranhutangekspedisidetail 
                             WHERE pembayaranhutangekspedisi = @pembayaranhutangekspedisi AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pembayaranhutangekspedisi", this.pembayaranhutangekspedisi);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.fakturpembelian = reader.GetString("fakturpembelian");
                this.jumlah = reader.GetString("jumlah");
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
            double dblTotalBayar = Tools.getRoundMoney(double.Parse(this.jumlah));
            dFakturPembelian.totalbayarekspedisi = dblTotalBayar.ToString();
            dFakturPembelian.tambahTotalBayarEkspedisi();

            // tambah detail
            String query = @"INSERT INTO pembayaranhutangekspedisidetail(pembayaranhutangekspedisi,no,fakturpembelian, jumlah) 
                             VALUES(@pembayaranhutangekspedisi,@no,@fakturpembelian,@jumlah)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pembayaranhutangekspedisi", this.pembayaranhutangekspedisi);
            parameters.Add("no", this.no);
            parameters.Add("fakturpembelian", this.fakturpembelian);
            parameters.Add("jumlah", this.jumlah);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            // kurang jumlah bayar di faktur
            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.fakturpembelian);
            double dblTotalBayar = Tools.getRoundMoney(double.Parse(this.jumlah));
            dFakturPembelian.totalbayarekspedisi = dblTotalBayar.ToString();
            dFakturPembelian.kurangTotalBayarEkspedisi();

            // hapus detail
            String query = @"DELETE FROM pembayaranhutangekspedisidetail WHERE pembayaranhutangekspedisi = @pembayaranhutangekspedisi AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pembayaranhutangekspedisi", this.pembayaranhutangekspedisi);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.pembayaranhutangekspedisi + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.pembayaranhutangekspedisi + " - " + this.no + "] tidak ada");
            }
        }

        private void valDetail() {
            DataPembayaranHutangEkspedisi dPembayaranHutangEkspedisi = new DataPembayaranHutangEkspedisi(command, this.pembayaranhutangekspedisi);
            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.fakturpembelian);

            if(OswDate.getDateTimeFromStringTanggal(dPembayaranHutangEkspedisi.tanggal) < OswDate.getDateTimeFromStringTanggal(dFakturPembelian.tanggal)) {
                throw new Exception("Tanggal Pembayaran Pembelian harus >= Tanggal Faktur Pembelien [" + this.fakturpembelian + " - " + dFakturPembelian.tanggal + "]");
            }

            if(double.Parse(this.jumlah) <= 0) {
                throw new Exception("Jumlah Bayar Faktur Pembelian [" + this.fakturpembelian + "] harus > 0");
            }
        }
    }
}

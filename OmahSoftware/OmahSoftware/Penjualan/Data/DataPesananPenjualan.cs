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

namespace OmahSoftware.Penjualan {
    class DataPesananPenjualan {
        private String id = "PESANANPENJUALAN";
        public String kode = "";
        public String tanggal = "";
        public String customer = "";
        public String sales = "";
        public String jenisppn = "";
        public String total = "0";
        public String diskon = "0";
        public String diskonnilai = "0";
        public String totaldiskon = "0";
        public String totaldpp = "0";
        public String totalppn = "0";
        public String grandtotal = "0";
        public String catatan = "";
        public String status = "";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Customer:" + customer + ";";
            kolom += "Sales:" + sales + ";";
            kolom += "Jenis PPN:" + jenisppn + ";";
            kolom += "Total:" + total + ";";
            kolom += "Diskon:" + diskon + ";";
            kolom += "Diskon Nilai:" + diskonnilai + ";";
            kolom += "Total Diskon:" + totaldiskon + ";";
            kolom += "Total DPP:" + totaldpp + ";";
            kolom += "Total PPN:" + totalppn + ";";
            kolom += "Grand Total:" + grandtotal + ";";
            kolom += "Catatan:" + catatan + ";";
            kolom += "Status:" + status + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataPesananPenjualan(MySqlCommand command, String kode) {
            this.command = command;

            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal, customer, sales,jenisppn,total,diskon,diskonnilai,totaldiskon,totaldpp,totalppn,grandtotal,catatan, status, version
                             FROM pesananpenjualan 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.customer = reader.GetString("customer");
                this.sales = reader.GetString("sales");
                this.jenisppn = reader.GetString("jenisppn");
                this.total = reader.GetString("total");
                this.diskon = reader.GetString("diskon");
                this.diskonnilai = reader.GetString("diskonnilai");
                this.totaldiskon = reader.GetString("totaldiskon");
                this.totaldpp = reader.GetString("totaldpp");
                this.totalppn = reader.GetString("totalppn");
                this.grandtotal = reader.GetString("grandtotal");
                this.catatan = reader.GetString("catatan");
                this.status = reader.GetString("status");
                this.version = reader.GetInt64("version");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        private String generateKode() {
            String strngTanggalSekarang = this.tanggal;
            String strngTahun = OswDate.getTahun(strngTanggalSekarang);
            String strngTahunDuaDigit = OswDate.getTahunDuaDigit(strngTanggalSekarang);
            String strngBulan = OswDate.getBulan(strngTanggalSekarang);
            String strngTanggal = OswDate.getTanggal(strngTanggalSekarang);


            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("Tahun", strngTahun);
            parameters.Add("TahunDuaDigit", strngTahunDuaDigit);
            parameters.Add("Bulan", strngBulan);
            parameters.Add("Tanggal", strngTanggal);

            String kode = OswFormatDokumen.generate(command, id, parameters);

            // jika kode kosong, berarti tidak diseting no otomatis, sehingga harus diisi manual
            if(kode == "") {
                if(this.kode == "") {
                    throw new Exception("Kode/Nomor harus diisi, karena tidak disetting untuk digenerate otomatis");
                } else {
                    kode = this.kode;
                }
            }

            DataPesananPenjualan dPesananPenjualan = new DataPesananPenjualan(command, kode);
            if(dPesananPenjualan.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public void tambah() {
            // validasi
            valNotExist();
            Tools.valAdmin(command, this.tanggal);

            this.kode = this.kode == "" ? this.generateKode() : this.kode;
            this.version += 1;

            String query = @"INSERT INTO pesananpenjualan(kode, tanggal, customer, sales,jenisppn,total,diskon,diskonnilai,totaldiskon,totaldpp,totalppn,grandtotal,catatan, status, version,create_user) 
                             VALUES(@kode,@tanggal,@customer, @sales,@jenisppn,@total,@diskon,@diskonnilai,@totaldiskon,@totaldpp,@totalppn,@grandtotal,@catatan,@status, @version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("customer", this.customer);
            parameters.Add("sales", this.sales);
            parameters.Add("jenisppn", this.jenisppn);
            parameters.Add("total", this.total);
            parameters.Add("diskon", this.diskon);
            parameters.Add("diskonnilai", this.diskonnilai);
            parameters.Add("totaldiskon", this.totaldiskon);
            parameters.Add("totaldpp", this.totaldpp);
            parameters.Add("totalppn", this.totalppn);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("catatan", this.catatan);
            parameters.Add("status", this.status);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();
            valVersion();
            Tools.valAdmin(command, this.tanggal);

            // hapus detail
            this.hapusDetail();

            // hapus header
            String query = @"DELETE FROM pesananpenjualan
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapusDetail() {
            // validasi
            Tools.valAdmin(command, this.tanggal);

            String query = @"SELECT no
                             FROM pesananpenjualandetail
                             WHERE pesananpenjualan = @pesananpenjualan";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("pesananpenjualan", this.kode);

            // buat penampung data itemno
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("no");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngItemNo = reader.GetString("no");
                dataTable.Rows.Add(strngItemNo);
            }
            reader.Close();

            // loop per detail lalu hapus
            foreach(DataRow row in dataTable.Rows) {
                DataPesananPenjualanDetail dPesananPenjualanDetail = new DataPesananPenjualanDetail(command, this.kode, row["no"].ToString());
                dPesananPenjualanDetail.hapus();
            }
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();
            valFakturPenjualan();
            valDetail();

            Tools.valAdmin(command, this.tanggal);

            this.version += 1;

            // proses ubah
            String query = @"UPDATE pesananpenjualan
                             SET tanggal = @tanggal,
                                 customer = @customer, 
                                 sales = @sales,
                                 jenisppn = @jenisppn,
                                 total = @total,
                                 diskon = @diskon,
                                 diskonnilai = @diskonnilai,
                                 totaldiskon = @totaldiskon,
                                 totaldpp = @totaldpp,
                                 totalppn = @totalppn,
                                 grandtotal = @grandtotal,
                                 catatan = @catatan,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("customer", this.customer);
            parameters.Add("sales", this.sales);
            parameters.Add("jenisppn", this.jenisppn);
            parameters.Add("total", this.total);
            parameters.Add("diskon", this.diskon);
            parameters.Add("diskonnilai", this.diskonnilai);
            parameters.Add("totaldiskon", this.totaldiskon);
            parameters.Add("totaldpp", this.totaldpp);
            parameters.Add("totalppn", this.totalppn);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("catatan", this.catatan);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubahStatus() {
            // validasi
            valExist();
            valVersion();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE pesananpenjualan
                             SET status = @status,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);
            parameters.Add("status", this.status);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.kode + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.kode + "] tidak ada");
            }
        }

        private void valVersion() {
            DataPesananPenjualan dPesananPenjualan = new DataPesananPenjualan(command, this.kode);
            if(this.version != dPesananPenjualan.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        public void valJumlahDetail() {
            String query = @"SELECT COUNT(*)
                             FROM pesananpenjualandetail A
                             WHERE A.pesananpenjualan = @pesananpenjualan";

            Dictionary<String, String> parameters = new Dictionary<string, string>();

            parameters.Add("pesananpenjualan", this.kode);

            Double dblJumlahDetailBarang = Double.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlahDetailBarang <= 0) {
                throw new Exception("Jumlah Item detail harus lebih dari 0");
            }
        }

        public void valLimitPiutang() {
            DataCustomer dCustomer = new DataCustomer(command, this.customer);
            DataSaldoPiutangAktual dSaldoPiutangAktual = new DataSaldoPiutangAktual(command, dCustomer.kode);

            double limitpiutang = double.Parse(dCustomer.limitpiutang);
            double piutangaktual = double.Parse(dSaldoPiutangAktual.jumlah);

            if(limitpiutang < piutangaktual + double.Parse(this.grandtotal)) {
                throw new Exception("Limit Piutang Customer melewati batas");
            }
        }

        public void valFakturPenjualan() {
            // cek sudah ada pengiriman penjualan atau belum di barang
            String query = @"SELECT COUNT(*)
                             FROM pesananpenjualandetail A
                             WHERE A.pesananpenjualan = @kode AND A.jumlahfaktur > 0";

            Dictionary<String, String> parameters = new Dictionary<string, string>();
            parameters.Add("kode", this.kode);

            Double dblJumlah = Double.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlah > 0) {
                throw new Exception("Faktur Penjualan telah di lakukan");
            }
        }

        private void valDetail() {
            if(double.Parse(this.grandtotal) <= 0) {
                throw new Exception("Grand Total harus > 0");
            }
        }
    }
}

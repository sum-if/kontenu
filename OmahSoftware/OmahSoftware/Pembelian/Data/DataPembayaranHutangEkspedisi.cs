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
using OmahSoftware.Akuntansi;
using OmahSoftware.Sistem;
using OmahSoftware.Penjualan;

namespace OmahSoftware.Pembelian {
    class DataPembayaranHutangEkspedisi {
        private String id = "PEMBAYARANHUTANGEKSPEDISI";
        public String kode = "";
        public String tanggal = "";
        public String ekspedisi = "";
        public String akunpembayaran = "";
        public String catatan = "";
        public String total = "0";
        public String nourut = "0";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Ekspedisi:" + ekspedisi + ";";
            kolom += "Akun Pembayaran:" + akunpembayaran + ";";
            kolom += "Catatan:" + catatan + ";";
            kolom += "Total:" + total + ";";
            kolom += "No Urut:" + nourut + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataPembayaranHutangEkspedisi(MySqlCommand command, String kode) {
            this.command = command;
            
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal, ekspedisi, akunpembayaran,catatan,total,nourut, version
                             FROM pembayaranhutangekspedisi 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.ekspedisi = reader.GetString("ekspedisi");
                this.akunpembayaran = reader.GetString("akunpembayaran");
                this.catatan = reader.GetString("catatan");
                this.total = reader.GetString("total");
                this.nourut = reader.GetString("nourut");
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

            DataPembayaranHutangEkspedisi dPembayaranHutangEkspedisi = new DataPembayaranHutangEkspedisi(command, kode);
            if(dPembayaranHutangEkspedisi.isExist) {
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

            String query = @"INSERT INTO pembayaranhutangekspedisi(kode, tanggal, ekspedisi, akunpembayaran,catatan,total,nourut,version,create_user) 
                             VALUES(@kode,@tanggal, @ekspedisi,@akunpembayaran,@catatan,@total,@nourut,@version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("ekspedisi", this.ekspedisi);
            parameters.Add("akunpembayaran", this.akunpembayaran);
            parameters.Add("catatan", this.catatan);
            parameters.Add("total", this.total);
            parameters.Add("nourut", this.nourut);
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
            String query = @"DELETE FROM pembayaranhutangekspedisi
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("kode", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();

            Tools.valAdmin(command, this.tanggal);

            this.version += 1;

            // proses ubah
            String query = @"UPDATE pembayaranhutangekspedisi
                             SET tanggal = @tanggal,
                                 ekspedisi = @ekspedisi, 
                                 akunpembayaran = @akunpembayaran, 
                                 catatan = @catatan,
                                 total = @total,
                                 nourut = @nourut,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("ekspedisi", this.ekspedisi);
            parameters.Add("akunpembayaran", this.akunpembayaran);
            parameters.Add("catatan", this.catatan);
            parameters.Add("total", this.total);
            parameters.Add("nourut", this.nourut);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapusDetail() {
            // validasi
            Tools.valAdmin(command, this.tanggal);

            // hapus jurnal 
            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode);
            dJurnal.hapusJurnal();

            String query = @"SELECT no
                             FROM pembayaranhutangekspedisidetail
                             WHERE pembayaranhutangekspedisi = @pembayaranhutangekspedisi";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("pembayaranhutangekspedisi", this.kode);

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
                DataPembayaranHutangEkspedisiDetail dPembayaranHutangEkspedisiDetail = new DataPembayaranHutangEkspedisiDetail(command, this.kode, row["no"].ToString());
                dPembayaranHutangEkspedisiDetail.hapus();
            }
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

        public void valJumlahDetail() {
            String query = @"SELECT COUNT(*)
                             FROM pembayaranhutangekspedisidetail A
                             WHERE A.pembayaranhutangekspedisi = @pembayaranhutangekspedisi";

            Dictionary<String, String> parameters = new Dictionary<string, string>();
            parameters.Add("pembayaranhutangekspedisi", this.kode);

            Double dblJumlahDetail = Double.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlahDetail <= 0) {
                throw new Exception("Jumlah Item detail harus lebih dari 0");
            }
        }

        private void valVersion() {
            DataPembayaranHutangEkspedisi dPembayaranHutangEkspedisi = new DataPembayaranHutangEkspedisi(command, this.kode);
            if(this.version != dPembayaranHutangEkspedisi.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        public void prosesJurnal() {
            /*
            Deskripsi : Pembayaran Pembelian ke [NAMA SUPPLIER]
	        Status			Akun									Nilai
	        Debet			akun_hutang_ekspedisi			        Total Bayar
	        Kredit			akun_pembayaran_pembelian				Total Bayar
            */

            int no = 1;
            DataEkspedisi dEkspedisi = new DataEkspedisi(command, this.ekspedisi);
            String strngKeteranganJurnal = "Pembayaran Hutang Ekspedisi ke [" + dEkspedisi.nama + "]";

            String strngTanggal = this.tanggal;

            String mutasi = Tools.getRoundMoney(double.Parse(this.total)).ToString();

            // Debet			akun_hutang					            Total Bayar
            String strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_HUTANG_EKSPEDISI);
            DataAkun dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Hutang Ekspedisi [" + strngAkun + "] tidak ditemukan.");
            }

            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, mutasi, "0");
            dJurnal.prosesJurnal();

            // Kredit			akun_pembayaran_pembelian				Total Bayar
            dAkun = new DataAkun(command, this.akunpembayaran);
            if(!dAkun.isExist) {
                throw new Exception("Akun Pembayaran Hutang Ekspedisi [" + this.akunpembayaran + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, strngTanggal, strngKeteranganJurnal, (no++).ToString(), this.akunpembayaran, "0", this.total);
            dJurnal.prosesJurnal();

            Tools.cekJurnalBalance(command, this.id, this.kode);
        }
    }
}

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

namespace OmahSoftware.Pembelian {
    class DataPembayaranPembelian {
        private String id = "PEMBAYARANPEMBELIAN";
        public String kode = "";
        public String tanggal = "";
        public String jenis = "";
        public String supplier = "";
        public String akunpembayaran = "";
        public String nocek = "";
        public String tanggalcek = "";
        public String statuscek = "";
        public String catatan = "";
        public String total = "0";
        public String lunas = "";
        public String totalselisih = "0";
        public String nourut = "0";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Jenis:" + jenis + ";";
            kolom += "Supplier:" + supplier + ";";
            kolom += "Akun Pembayaran:" + akunpembayaran + ";";
            kolom += "No Cek:" + nocek + ";";
            kolom += "Tanggal Cek:" + tanggalcek + ";";
            kolom += "Status Cek:" + statuscek + ";";
            kolom += "Catatan:" + catatan + ";";
            kolom += "Total:" + total + ";";
            kolom += "Lunas:" + lunas + ";";
            kolom += "Total Selisih:" + totalselisih + ";";
            kolom += "No Urut:" + nourut + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataPembayaranPembelian(MySqlCommand command, String kode) {
            this.command = command;
            
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal, jenis, supplier, akunpembayaran,nocek, tanggalcek, statuscek, catatan,total,lunas,totalselisih,nourut, version
                             FROM pembayaranpembelian 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.jenis = reader.GetString("jenis");
                this.supplier = reader.GetString("supplier");
                this.akunpembayaran = reader.GetString("akunpembayaran");
                this.nocek = reader.GetString("nocek");
                this.tanggalcek = reader.GetString("tanggalcek");
                this.statuscek = reader.GetString("statuscek");
                this.catatan = reader.GetString("catatan");
                this.total = reader.GetString("total");
                this.lunas = reader.GetString("lunas");
                this.totalselisih = reader.GetString("totalselisih");
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

            DataPembayaranPembelian dPembayaranPembelian = new DataPembayaranPembelian(command, kode);
            if(dPembayaranPembelian.isExist) {
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

            String query = @"INSERT INTO pembayaranpembelian(kode, tanggal, jenis, supplier, akunpembayaran,nocek, tanggalcek, statuscek, catatan,total,lunas,totalselisih,nourut,version,create_user) 
                             VALUES(@kode,@tanggal,@jenis, @supplier,@akunpembayaran,@nocek, @tanggalcek, @statuscek, @catatan,@total,@lunas,@totalselisih,@nourut,@version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("jenis", this.jenis);
            parameters.Add("supplier", this.supplier);
            parameters.Add("akunpembayaran", this.akunpembayaran);
            parameters.Add("nocek", this.nocek);
            parameters.Add("tanggalcek", this.tanggalcek);
            parameters.Add("statuscek", this.statuscek);
            parameters.Add("catatan", this.catatan);
            parameters.Add("total", this.total);
            parameters.Add("lunas", this.lunas);
            parameters.Add("totalselisih", this.totalselisih);
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
            String query = @"DELETE FROM pembayaranpembelian
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
            String query = @"UPDATE pembayaranpembelian
                             SET tanggal = @tanggal,
                                 jenis = @jenis,
                                 supplier = @supplier, 
                                 akunpembayaran = @akunpembayaran, 
                                 nocek = @nocek,
                                 tanggalcek = @tanggalcek,
                                 statuscek = @statuscek,
                                 catatan = @catatan,
                                 total = @total,
                                 totalselisih = @totalselisih,
                                 lunas = @lunas,
                                 nourut = @nourut,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("jenis", this.jenis);
            parameters.Add("supplier", this.supplier);
            parameters.Add("akunpembayaran", this.akunpembayaran);
            parameters.Add("nocek", this.nocek);
            parameters.Add("tanggalcek", this.tanggalcek);
            parameters.Add("statuscek", this.statuscek);
            parameters.Add("catatan", this.catatan);
            parameters.Add("total", this.total);
            parameters.Add("lunas", this.lunas);
            parameters.Add("totalselisih", this.totalselisih);
            parameters.Add("nourut", this.nourut);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapusDetail() {
            // validasi
            Tools.valAdmin(command, this.tanggal);

            this.hapusMutasi();

            String query = @"SELECT no
                             FROM pembayaranpembeliandetail
                             WHERE pembayaranpembelian = @pembayaranpembelian";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("pembayaranpembelian", this.kode);

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
                DataPembayaranPembelianDetail dPembayaranPembelianDetail = new DataPembayaranPembelianDetail(command, this.kode, row["no"].ToString());
                dPembayaranPembelianDetail.hapus();
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
                             FROM pembayaranpembeliandetail A
                             WHERE A.pembayaranpembelian = @pembayaranpembelian";

            Dictionary<String, String> parameters = new Dictionary<string, string>();
            parameters.Add("pembayaranpembelian", this.kode);

            Double dblJumlahDetail = Double.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlahDetail <= 0) {
                throw new Exception("Jumlah Item detail harus lebih dari 0");
            }
        }

        private void valVersion() {
            DataPembayaranPembelian dPembayaranPembelian = new DataPembayaranPembelian(command, this.kode);
            if(this.version != dPembayaranPembelian.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        public void updateMutasi() {
            String mutasi = Tools.getRoundMoney(((double.Parse(this.total) + double.Parse(this.totalselisih)) * -1)).ToString();

            // mutasi hutang
            DataMutasiHutang dMutasiHutang = new DataMutasiHutang(command, this.id, this.kode);
            dMutasiHutang.no = "1";
            dMutasiHutang.tanggal = this.tanggal;
            dMutasiHutang.supplier = this.supplier;
            dMutasiHutang.jumlah = mutasi;
            dMutasiHutang.proses();

            DataOswSetting dOswSetting = new DataOswSetting(command, Constants.AKUN_UANG_TITIPAN_SUPPLIER);
            String strngAkunUangTitipanSupplier = dOswSetting.isi;

            if(this.akunpembayaran == strngAkunUangTitipanSupplier) {
                DataMutasiUangTitipanSupplier dMutasiUangTitipanSupplier = new DataMutasiUangTitipanSupplier(command, this.id, this.kode);
                dMutasiUangTitipanSupplier.no = "1";
                dMutasiUangTitipanSupplier.tanggal = this.tanggal;
                dMutasiUangTitipanSupplier.supplier = this.supplier;
                dMutasiUangTitipanSupplier.jumlah = mutasi;
                dMutasiUangTitipanSupplier.proses();
            }
        }

        private void hapusMutasi() {
            // hapus jurnal
            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode);
            dJurnal.hapusJurnal();

            DataMutasiHutang dMutasiHutang = new DataMutasiHutang(command, this.id, this.kode);
            dMutasiHutang.hapus();

            DataMutasiUangTitipanSupplier dMutasiUangTitipanSupplier = new DataMutasiUangTitipanSupplier(command, this.id, this.kode);
            dMutasiUangTitipanSupplier.hapus();
        }

        public void ubahStatusCek() {
            String query = @"UPDATE pembayaranpembelian
                             SET statuscek = @statuscek
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("statuscek", this.statuscek);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubahDataCek() {
            String query = @"UPDATE pembayaranpembelian
                             SET nocek = @nocek, 
                                 tanggalcek = @tanggalcek,
                                 statuscek = @statuscek
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("nocek", this.nocek);
            parameters.Add("tanggalcek", this.tanggalcek);
            parameters.Add("statuscek", this.statuscek);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void tambahTotalBayarCekDitolak() {
            String query = @"SELECT fakturpembelian,jumlah,selisih
                             FROM pembayaranpembeliandetail
                             WHERE pembayaranpembelian = @pembayaranpembelian";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pembayaranpembelian", this.kode);

            // buat penampung data itemno
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("fakturpembelian");
            dataTable.Columns.Add("jumlah");
            dataTable.Columns.Add("selisih");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngFakturPembelian = reader.GetString("fakturpembelian");
                String strngJumlah = reader.GetString("jumlah");
                String strngSelisih = reader.GetString("selisih");
                dataTable.Rows.Add(strngFakturPembelian, strngJumlah, strngSelisih);
            }
            reader.Close();

            // loop per detail lalu hapus
            foreach(DataRow row in dataTable.Rows) {
                String strngFakturPembelian = row["fakturpembelian"].ToString();
                String strngJumlah = row["jumlah"].ToString();
                String strngSelisih = row["selisih"].ToString();

                String mutasi = Tools.getRoundMoney((double.Parse(strngJumlah) + double.Parse(strngSelisih))).ToString();

                // kurang total bayar di faktur penjualan
                DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, strngFakturPembelian);
                dFakturPembelian.totalbayar = mutasi;
                dFakturPembelian.tambahTotalBayar();
            }
        }

        public void kurangTotalBayarCekDitolak() {
            String query = @"SELECT fakturpembelian,jumlah,selisih
                             FROM pembayaranpembeliandetail
                             WHERE pembayaranpembelian = @pembayaranpembelian";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pembayaranpembelian", this.kode);

            // buat penampung data itemno
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("fakturpembelian");
            dataTable.Columns.Add("jumlah");
            dataTable.Columns.Add("selisih");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngFakturPembelian = reader.GetString("fakturpembelian");
                String strngJumlah = reader.GetString("jumlah");
                String strngSelisih = reader.GetString("selisih");

                dataTable.Rows.Add(strngFakturPembelian, strngJumlah, strngSelisih);
            }
            reader.Close();

            // loop per detail lalu hapus
            foreach(DataRow row in dataTable.Rows) {
                String strngFakturPembelian = row["fakturpembelian"].ToString();
                String strngJumlah = row["jumlah"].ToString();
                String strngSelisih = row["selisih"].ToString();

                String mutasi = Tools.getRoundMoney((double.Parse(strngJumlah) + double.Parse(strngSelisih))).ToString();

                // kurang total bayar di faktur penjualan
                DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, strngFakturPembelian);
                dFakturPembelian.totalbayar = mutasi;
                dFakturPembelian.kurangTotalBayar();
            }
        }

        public void valStatusCek() {
            if(this.statuscek == Constants.STATUS_CEK_DIBATALKAN || this.statuscek == Constants.STATUS_CEK_SELESAI) {
                throw new Exception("Cheque sudah di settlement");
            }
        }

        public void prosesJurnal() {
            /*
            Deskripsi : Pembayaran Pembelian ke [NAMA SUPPLIER]
	        Status			Akun									Nilai
	        Debet			akun_hutang					            Total Bayar
	        Kredit			akun_pembayaran_pembelian				Total Bayar
            Debit/Kredit	selisih pembayaran				        Selisih
            */

            int no = 1;
            DataSupplier dSupplier = new DataSupplier(command, this.supplier);
            String strngKeteranganJurnal = "Pembayaran Pembelian ke [" + dSupplier.nama + "]";

            String strngTanggal = this.tanggal;

            String mutasi = Tools.getRoundMoney((double.Parse(this.total) + double.Parse(this.totalselisih))).ToString();

            // Debet			akun_hutang					            Total Bayar
            String strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_HUTANG);
            DataAkun dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Hutang [" + strngAkun + "] tidak ditemukan.");
            }

            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, mutasi, "0");
            dJurnal.prosesJurnal();

            // Kredit			akun_pembayaran_pembelian				Total Bayar
            dAkun = new DataAkun(command, this.akunpembayaran);
            if(!dAkun.isExist) {
                throw new Exception("Akun [" + this.akunpembayaran + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, strngTanggal, strngKeteranganJurnal, (no++).ToString(), this.akunpembayaran, "0", this.total);
            dJurnal.prosesJurnal();

            // Debit/Kredit	selisih pembayaran				        Selisih
            if(double.Parse(this.totalselisih) < 0) {
                strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PEMBULATAN_BIAYA);
                dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Pembulatan Biaya [" + strngAkun + "] tidak ditemukan.");
                }

                double dblTotalSelisih = double.Parse(totalselisih) * -1;
                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, dblTotalSelisih.ToString(), "0");
                dJurnal.prosesJurnal();
            } else if(double.Parse(totalselisih) > 0) {
                strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PEMBULATAN_PENDAPATAN);
                dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Pembulatan Pendapatan [" + strngAkun + "] tidak ditemukan.");
                }

                
                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", this.totalselisih);
                dJurnal.prosesJurnal();
            }

            Tools.cekJurnalBalance(command, this.id, this.kode);
        }
    }
}

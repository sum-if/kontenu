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
using OmahSoftware.Persediaan;
using OmahSoftware.Sistem;

namespace OmahSoftware.Penjualan {
    class DataPenerimaanPenjualan {
        private String id = "PENERIMAANPENJUALAN";
        public String kode = "";
        public String tanggal = "";
        public String jenis = "";
        public String customer = "";
        public String akunpenerimaan = "";
        public String nocek = "";
        public String tanggalcek = "";
        public String statuscek = "";
        public String catatan = "";
        public String total = "0";
        public String lunas = "0";
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
            kolom += "Customer:" + customer + ";";
            kolom += "Akun Penerimaan:" + akunpenerimaan + ";";
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

        public DataPenerimaanPenjualan(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal, jenis, customer, akunpenerimaan, nocek, tanggalcek, statuscek, catatan, 
                                    total,lunas,totalselisih, nourut, version
                             FROM penerimaanpenjualan 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.jenis = reader.GetString("jenis");
                this.customer = reader.GetString("customer");
                this.akunpenerimaan = reader.GetString("akunpenerimaan");
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

            DataPenerimaanPenjualan dPenerimaanPenjualan = new DataPenerimaanPenjualan(command, kode);
            if(dPenerimaanPenjualan.isExist) {
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

            String query = @"INSERT INTO penerimaanpenjualan( kode, tanggal, jenis, customer, akunpenerimaan, nocek, tanggalcek, statuscek, total,lunas,totalselisih, nourut, catatan, version,create_user) 
                             VALUES(@kode, @tanggal, @jenis, @customer, @akunpenerimaan, @nocek, @tanggalcek, @statuscek, @total,@lunas,@totalselisih, @nourut, @catatan, @version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("jenis", this.jenis);
            parameters.Add("customer", this.customer);
            parameters.Add("akunpenerimaan", this.akunpenerimaan);
            parameters.Add("nocek", this.nocek);
            parameters.Add("tanggalcek", this.tanggalcek);
            parameters.Add("statuscek", this.statuscek);
            parameters.Add("total", this.total);
            parameters.Add("lunas", this.lunas);
            parameters.Add("totalselisih", this.totalselisih);
            parameters.Add("nourut", this.nourut);
            parameters.Add("catatan", this.catatan);
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
            String query = @"DELETE FROM penerimaanpenjualan
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();
            valDetail();

            Tools.valAdmin(command, this.tanggal);

            this.version += 1;
            
            // proses ubah
            String query = @"UPDATE penerimaanpenjualan
                             SET tanggal = @tanggal,
                                 jenis = @jenis,
                                 customer = @customer,  
                                 akunpenerimaan = @akunpenerimaan,
                                 nocek = @nocek,
                                 tanggalcek = @tanggalcek,
                                 statuscek = @statuscek,
                                 total = @total,
                                 lunas = @lunas,
                                 totalselisih = @totalselisih,
                                 nourut = @nourut,
                                 catatan = @catatan,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("jenis", this.jenis);
            parameters.Add("customer", this.customer);
            parameters.Add("akunpenerimaan", this.akunpenerimaan);
            parameters.Add("nocek", this.nocek);
            parameters.Add("tanggalcek", this.tanggalcek);
            parameters.Add("statuscek", this.statuscek);
            parameters.Add("total", this.total);
            parameters.Add("lunas", this.lunas);
            parameters.Add("totalselisih", this.totalselisih);
            parameters.Add("nourut", this.nourut);
            parameters.Add("catatan", this.catatan);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapusDetail() {
            // validasi
            Tools.valAdmin(command, this.tanggal);

            this.hapusMutasi();

            String query = @"SELECT no
                             FROM penerimaanpenjualandetail
                             WHERE penerimaanpenjualan = @penerimaanpenjualan";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("penerimaanpenjualan", this.kode);

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
                DataPenerimaanPenjualanDetail dPenerimaaPenjualanDetail = new DataPenerimaanPenjualanDetail(command, this.kode, row["no"].ToString());
                dPenerimaaPenjualanDetail.hapus();
            }
        }

        public void updateMutasi() {
            String mutasi = Tools.getRoundMoney(((double.Parse(this.total) + double.Parse(this.totalselisih)) * -1)).ToString();

            // mutasi piutang
            DataMutasiPiutang dMutasiPiutang = new DataMutasiPiutang(command, this.id, this.kode);
            dMutasiPiutang.no = "1";
            dMutasiPiutang.tanggal = this.tanggal;
            dMutasiPiutang.customer = this.customer;
            dMutasiPiutang.jumlah = mutasi;
            dMutasiPiutang.proses();

            DataOswSetting dOswSetting = new DataOswSetting(command, Constants.AKUN_UANG_TITIPAN_CUSTOMER);
            String strngAkunUangTitipanCustomer = dOswSetting.isi;

            if(this.akunpenerimaan == strngAkunUangTitipanCustomer) {
                DataMutasiUangTitipanCustomer dMutasiUangTitipanCustomer = new DataMutasiUangTitipanCustomer(command, this.id, this.kode);
                dMutasiUangTitipanCustomer.no = "1";
                dMutasiUangTitipanCustomer.tanggal = this.tanggal;
                dMutasiUangTitipanCustomer.customer = this.customer;
                dMutasiUangTitipanCustomer.jumlah = mutasi;
                dMutasiUangTitipanCustomer.proses();
            }
        }

        private void hapusMutasi() {
            // hapus jurnal
            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode);
            dJurnal.hapusJurnal();

            DataMutasiPiutang dMutasiPiutang = new DataMutasiPiutang(command, this.id, this.kode);
            dMutasiPiutang.hapus();
            
            DataMutasiUangTitipanCustomer dMutasiUangTitipanCustomer = new DataMutasiUangTitipanCustomer(command, this.id, this.kode);
            dMutasiUangTitipanCustomer.hapus();
        }

        public void ubahStatusCek() {
            String query = @"UPDATE penerimaanpenjualan
                             SET statuscek = @statuscek
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("statuscek", this.statuscek);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubahDataCek() {
            String query = @"UPDATE penerimaanpenjualan
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

        public void kurangTotalBayarCekDitolak() {
            String query = @"SELECT fakturpenjualan,jumlah,selisih
                             FROM penerimaanpenjualandetail
                             WHERE penerimaanpenjualan = @penerimaanpenjualan";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("penerimaanpenjualan", this.kode);

            // buat penampung data itemno
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("fakturpenjualan");
            dataTable.Columns.Add("jumlah");
            dataTable.Columns.Add("selisih");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngFakturPenjualan = reader.GetString("fakturpenjualan");
                String strngJumlah = reader.GetString("jumlah");
                String strngSelisih = reader.GetString("selisih");

                dataTable.Rows.Add(strngFakturPenjualan, strngJumlah, strngSelisih);
            }
            reader.Close();

            // loop per detail lalu hapus
            foreach(DataRow row in dataTable.Rows) {
                String strngFakturPenjualan = row["fakturpenjualan"].ToString();
                String strngJumlah = row["jumlah"].ToString();
                String strngSelisih = row["selisih"].ToString();

                String mutasi = Tools.getRoundMoney((double.Parse(strngJumlah) + double.Parse(strngSelisih))).ToString();

                // kurang total bayar di faktur penjualan
                DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, strngFakturPenjualan);
                dFakturPenjualan.totalbayar = mutasi;
                dFakturPenjualan.kurangTotalBayar();
            }
        }

        public void tambahTotalBayarCekDitolak() {
            String query = @"SELECT fakturpenjualan,jumlah,selisih
                             FROM penerimaanpenjualandetail
                             WHERE penerimaanpenjualan = @penerimaanpenjualan";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("penerimaanpenjualan", this.kode);

            // buat penampung data itemno
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("fakturpenjualan");
            dataTable.Columns.Add("jumlah");
            dataTable.Columns.Add("selisih");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngFakturPenjualan = reader.GetString("fakturpenjualan");
                String strngJumlah = reader.GetString("jumlah");
                String strngSelisih = reader.GetString("selisih");
                dataTable.Rows.Add(strngFakturPenjualan, strngJumlah, strngSelisih);
            }
            reader.Close();

            // loop per detail lalu hapus
            foreach(DataRow row in dataTable.Rows) {
                String strngFakturPenjualan = row["no"].ToString();
                String strngJumlah = row["jumlah"].ToString();
                String strngSelisih = row["selisih"].ToString();

                String mutasi = Tools.getRoundMoney((double.Parse(strngJumlah) + double.Parse(strngSelisih))).ToString();

                // kurang total bayar di faktur penjualan
                DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, strngFakturPenjualan);
                dFakturPenjualan.totalbayar = mutasi;
                dFakturPenjualan.tambahTotalBayar();
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

        private void valVersion() {
            DataPenerimaanPenjualan dPenerimaanPenjualan = new DataPenerimaanPenjualan(command, this.kode);
            if(this.version != dPenerimaanPenjualan.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        private void valDetail() {
            if(double.Parse(this.total) <= 0) {
                throw new Exception("Total harus > 0");
            }
        }

        public void valJumlahDetail() {
            String query = @"SELECT COUNT(*)
                             FROM penerimaanpenjualandetail A
                             WHERE A.penerimaanpenjualan = @penerimaanpenjualan";

            Dictionary<String, String> parameters = new Dictionary<string, string>();
            parameters.Add("penerimaanpenjualan", this.kode);

            Double dblJumlahDetail = Double.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlahDetail <= 0) {
                throw new Exception("Jumlah Item detail harus lebih dari 0");
            }
        }

        public void valStatusCek() {
            if(this.statuscek == Constants.STATUS_CEK_DIBATALKAN || this.statuscek == Constants.STATUS_CEK_SELESAI) {
                throw new Exception("Cheque sudah di settlement");
            }
        }

        public void prosesJurnal() {
            /*
            Deskripsi : Penerimaan  Penjualan dari [NAMA CUSTOMER]
	        Status			Akun													Nilai													
            Debet			form.akun_penerimaan							        Total Terima
            Kredit			akun_piutang									        Total Terima
            Debit/Kredit	selisih pembayaran
            */

            int no = 1;
            DataCustomer dCustomer = new DataCustomer(command, this.customer);
            String strngKeteranganJurnal = "Penerimaan Penjualan dari [" + dCustomer.nama + "]";

            String mutasi = Tools.getRoundMoney((double.Parse(this.total) + double.Parse(this.totalselisih))).ToString();

            // Debet			akun deposit ke yang terpilih					    Total Terima
            DataAkun dAkun = new DataAkun(command, this.akunpenerimaan);
            if(!dAkun.isExist) {
                throw new Exception("Akun Penerimaan Penjualan [" + this.akunpenerimaan + "] tidak ditemukan.");
            }

            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), this.akunpenerimaan, this.total, "0");
            dJurnal.prosesJurnal();

            // Kredit			akun_piutang								Total Terima
            String strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PIUTANG);
            dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Piutang [" + strngAkun + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", mutasi);
            dJurnal.prosesJurnal();

            // Debit/Kredit	selisih pembayaran				        Selisih
            if(double.Parse(this.totalselisih) > 0) {
                strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PEMBULATAN_BIAYA);
                dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Pembulatan Biaya [" + strngAkun + "] tidak ditemukan.");
                }

                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, this.totalselisih, "0");
                dJurnal.prosesJurnal();
            } else if(double.Parse(totalselisih) < 0) {
                strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PEMBULATAN_PENDAPATAN);
                dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Pembulatan Pendapatan [" + strngAkun + "] tidak ditemukan.");
                }

                double dblTotalSelisih = double.Parse(totalselisih) * -1;
                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", dblTotalSelisih.ToString());
                dJurnal.prosesJurnal();
            }

            Tools.cekJurnalBalance(command, this.id, this.kode);
        }
    }
}

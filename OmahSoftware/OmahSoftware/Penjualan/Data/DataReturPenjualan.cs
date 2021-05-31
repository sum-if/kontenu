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
using OmahSoftware.Sistem;
using OmahSoftware.Akuntansi;
using OmahSoftware.Persediaan;

namespace OmahSoftware.Penjualan {
    class DataReturPenjualan {
        private String id = "RETURPENJUALAN";
        public String kode = "";
        public String tanggal = "";
        public String customer = "";
        public String fakturpenjualan = "";
        public String jenisppn = "";
        public String total = "0";
        public String diskon = "0";
        public String diskonnilai = "0";
        public String totaldiskon = "0";
        public String totaldpp = "0";
        public String totalppn = "0";
        public String grandtotal = "0";
        public String totalfaktur = "0";
        public String sisabayar = "0";
        public String kembalipiutang = "0";
        public String kembaliuangtitipan = "0";
        public String catatan = "";
        public String nofakturpajak = "";
        public String tanggalfakturpajak = "";
        public String masapajak = "";
        public String tanggalupload = "";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Customer:" + customer + ";";
            kolom += "Faktur Penjualan:" + fakturpenjualan + ";";
            kolom += "Jenis PPN:" + jenisppn + ";";
            kolom += "Total:" + total + ";";
            kolom += "Diskon:" + diskon + ";";
            kolom += "Diskon Nilai:" + diskonnilai + ";";
            kolom += "Total Diskon:" + totaldiskon + ";";
            kolom += "Total DPP:" + totaldpp + ";";
            kolom += "Total PPN:" + totalppn + ";";
            kolom += "Grand Total:" + grandtotal + ";";
            kolom += "Total Faktur:" + totalfaktur + ";";
            kolom += "Sisa Bayar:" + sisabayar + ";";
            kolom += "Kembali Piutang:" + kembalipiutang + ";";
            kolom += "Kembali Uang Titipan:" + kembaliuangtitipan + ";";
            kolom += "Catatan:" + catatan + ";";
            kolom += "No Faktur Pajak:" + nofakturpajak + ";";
            kolom += "Tanggal Faktur Pajak:" + tanggalfakturpajak + ";";
            kolom += "Masa Pajak:" + masapajak + ";";
            kolom += "Tanggal Upload:" + tanggalupload + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataReturPenjualan(MySqlCommand command, String kode) {
            this.command = command;

            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal, customer, fakturpenjualan,jenisppn, total, diskon, diskonnilai, totaldiskon,totaldpp,totalppn,
                                    grandtotal,totalfaktur,sisabayar,kembalipiutang,kembaliuangtitipan, catatan, nofakturpajak,tanggalfakturpajak,masapajak, tanggalupload, version
                             FROM returpenjualan 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.customer = reader.GetString("customer");
                this.fakturpenjualan = reader.GetString("fakturpenjualan");
                this.jenisppn = reader.GetString("jenisppn");
                this.total = reader.GetString("total");
                this.diskon = reader.GetString("diskon");
                this.diskonnilai = reader.GetString("diskonnilai");
                this.totaldiskon = reader.GetString("totaldiskon");
                this.totaldpp = reader.GetString("totaldpp");
                this.totalppn = reader.GetString("totalppn");
                this.grandtotal = reader.GetString("grandtotal");
                this.totalfaktur = reader.GetString("totalfaktur");
                this.sisabayar = reader.GetString("sisabayar");
                this.kembalipiutang = reader.GetString("kembalipiutang");
                this.kembaliuangtitipan = reader.GetString("kembaliuangtitipan");
                this.catatan = reader.GetString("catatan");
                this.nofakturpajak = reader.GetString("nofakturpajak");
                this.tanggalfakturpajak = reader.GetString("tanggalfakturpajak");
                this.masapajak = reader.GetString("masapajak");
                this.tanggalupload = reader.GetString("tanggalupload");
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

            DataReturPenjualan dReturPenjualan = new DataReturPenjualan(command, kode);
            if(dReturPenjualan.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public void tambah() {
            // validasi
            valNotExist();
            valTanggal();
            Tools.valAdmin(command, this.tanggal);

            this.kode = this.kode == "" ? this.generateKode() : this.kode;
            this.version += 1;

            String query = @"INSERT INTO returpenjualan( kode, tanggal, customer, fakturpenjualan,jenisppn, total, diskon, diskonnilai,totaldiskon, totaldpp, totalppn, grandtotal,totalfaktur,sisabayar,kembalipiutang,kembaliuangtitipan, catatan, nofakturpajak,tanggalfakturpajak,masapajak, tanggalupload, version, create_user) 
                             VALUES(@kode, @tanggal, @customer, @fakturpenjualan,@jenisppn, @total, @diskon, @diskonnilai,@totaldiskon, @totaldpp, @totalppn, @grandtotal,@totalfaktur,@sisabayar,@kembalipiutang,@kembaliuangtitipan, @catatan, @nofakturpajak,@tanggalfakturpajak,@masapajak,@tanggalupload, @version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("customer", this.customer);
            parameters.Add("fakturpenjualan", this.fakturpenjualan);
            parameters.Add("jenisppn", this.jenisppn);
            parameters.Add("total", this.total);
            parameters.Add("diskon", this.diskon);
            parameters.Add("diskonnilai", this.diskonnilai);
            parameters.Add("totaldiskon", this.totaldiskon);
            parameters.Add("totaldpp", this.totaldpp);
            parameters.Add("totalppn", this.totalppn);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("totalfaktur", this.totalfaktur);
            parameters.Add("sisabayar", this.sisabayar);
            parameters.Add("kembalipiutang", this.kembalipiutang);
            parameters.Add("kembaliuangtitipan", this.kembaliuangtitipan);
            parameters.Add("catatan", this.catatan);
            parameters.Add("nofakturpajak", this.nofakturpajak);
            parameters.Add("tanggalfakturpajak", this.tanggalfakturpajak);
            parameters.Add("masapajak", this.masapajak);
            parameters.Add("tanggalupload", this.tanggalupload);
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
            String query = @"DELETE FROM returpenjualan
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapusDetail() {
            // validasi
            Tools.valAdmin(command, this.tanggal);

            // hapus jurnal
            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode);
            dJurnal.hapusJurnal();

            DataMutasiHPP dMutasiHPP = new DataMutasiHPP(command, this.id, this.kode);
            dMutasiHPP.hapus();

            DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, this.fakturpenjualan);
            dFakturPenjualan.totalretur = this.grandtotal;
            dFakturPenjualan.kurangTotalRetur();

            // hapus mutasi
            DataMutasiPersediaan dMutasiPersediaan = new DataMutasiPersediaan(command, this.id, this.kode);
            dMutasiPersediaan.hapus();

            DataMutasiPiutang dMutasiPiutang = new DataMutasiPiutang(command, this.id, this.kode);
            dMutasiPiutang.hapus();

            DataMutasiUangTitipanCustomer dMutasiUangTitipanCustomer = new DataMutasiUangTitipanCustomer(command, this.id, this.kode);
            dMutasiUangTitipanCustomer.hapus();

            String query = @"SELECT no
                             FROM returpenjualandetail
                             WHERE returpenjualan = @returpenjualan";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("returpenjualan", this.kode);

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
                DataReturPenjualanDetail dReturPenjualanDetail = new DataReturPenjualanDetail(command, this.kode, row["no"].ToString());
                dReturPenjualanDetail.hapus();
            }
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();
            valDetail();
            valTanggal();

            Tools.valAdmin(command, this.tanggal);

            this.version += 1;

            // proses ubah
            String query = @"UPDATE returpenjualan
                             SET tanggal = @tanggal,
                                 customer = @customer, 
                                 fakturpenjualan = @fakturpenjualan,
                                 jenisppn = @jenisppn,
                                 total = @total,
                                 diskon = @diskon,
                                 diskonnilai = @diskonnilai,
                                 totaldpp = @totaldpp,
                                 totaldiskon = @totaldiskon,
                                 totalppn = @totalppn,
                                 grandtotal = @grandtotal,
                                 totalfaktur = @totalfaktur,
                                 sisabayar = @sisabayar,
                                 kembalipiutang = @kembalipiutang,
                                 kembaliuangtitipan = @kembaliuangtitipan,
                                 catatan = @catatan,
                                 nofakturpajak = @nofakturpajak,
                                 tanggalfakturpajak = @tanggalfakturpajak,
                                 masapajak = @masapajak,
                                 tanggalupload = @tanggalupload,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("customer", this.customer);
            parameters.Add("fakturpenjualan", this.fakturpenjualan);
            parameters.Add("jenisppn", this.jenisppn);
            parameters.Add("total", this.total);
            parameters.Add("diskon", this.diskon);
            parameters.Add("diskonnilai", this.diskonnilai);
            parameters.Add("totaldpp", this.totaldpp);
            parameters.Add("totaldiskon", this.totaldiskon);
            parameters.Add("totalppn", this.totalppn);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("totalfaktur", this.totalfaktur);
            parameters.Add("sisabayar", this.sisabayar);
            parameters.Add("kembalipiutang", this.kembalipiutang);
            parameters.Add("kembaliuangtitipan", this.kembaliuangtitipan);
            parameters.Add("catatan", this.catatan);
            parameters.Add("nofakturpajak", this.nofakturpajak);
            parameters.Add("tanggalfakturpajak", this.tanggalfakturpajak);
            parameters.Add("masapajak", this.masapajak);
            parameters.Add("tanggalupload", this.tanggalupload);
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
            DataReturPenjualan dReturPenjualan = new DataReturPenjualan(command, this.kode);
            if(this.version != dReturPenjualan.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        public void valJumlahDetail() {
            String query = @"SELECT COUNT(*)
                             FROM returpenjualandetail A
                             WHERE A.returpenjualan = @returpenjualan";

            Dictionary<String, String> parameters = new Dictionary<string, string>();

            parameters.Add("returpenjualan", this.kode);

            Double dblJumlahDetailBarang = Double.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlahDetailBarang <= 0) {
                throw new Exception("Jumlah Item detail harus lebih dari 0");
            }
        }

        private void valDetail() {
            if(double.Parse(this.grandtotal) <= 0) {
                throw new Exception("Grand Total harus > 0");
            }

            if(double.Parse(this.totalfaktur) < 0) {
                throw new Exception("Total Faktur harus >= 0");
            }

            if(double.Parse(this.sisabayar) < 0) {
                throw new Exception("Sisa Bayar harus >= 0");
            }

            if(double.Parse(this.kembalipiutang) < 0) {
                throw new Exception("Kembali Piutang harus >= 0");
            }

            if(double.Parse(this.kembaliuangtitipan) < 0) {
                throw new Exception("Kembali Uang Titipan harus >= 0");
            }
        }

        private void valTanggal() {
            DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, this.fakturpenjualan);

            if(OswDate.getDateTimeFromStringTanggal(this.tanggal) < OswDate.getDateTimeFromStringTanggal(dFakturPenjualan.tanggal)) {
                throw new Exception("Tanggal Retur Penjualan harus >= Tanggal Faktur Penjualan [" + this.fakturpenjualan + " - " + dFakturPenjualan.tanggal + "]");
            }
        }

        public void updateMutasi() {
            if(double.Parse(this.kembalipiutang) > 0) {
                DataMutasiPiutang dMutasiPiutang = new DataMutasiPiutang(command, this.id, this.kode);
                dMutasiPiutang.no = "1";
                dMutasiPiutang.tanggal = this.tanggal;
                dMutasiPiutang.customer = this.customer;
                dMutasiPiutang.jumlah = (double.Parse(this.kembalipiutang) * -1).ToString();
                dMutasiPiutang.proses();
            }

            if(double.Parse(this.kembaliuangtitipan) > 0) {
                DataMutasiUangTitipanCustomer dMutasiUangTitipanCustomer = new DataMutasiUangTitipanCustomer(command, this.id, this.kode);
                dMutasiUangTitipanCustomer.no = "1";
                dMutasiUangTitipanCustomer.tanggal = this.tanggal;
                dMutasiUangTitipanCustomer.customer = this.customer;
                dMutasiUangTitipanCustomer.jumlah = this.kembaliuangtitipan;
                dMutasiUangTitipanCustomer.proses();
            }

            //update total retur di faktur penjualan
            DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, this.fakturpenjualan);
            dFakturPenjualan.totalretur = this.grandtotal;
            dFakturPenjualan.tambahTotalRetur();
        }

        public void ubahFakturPajak() {
            // validasi
            valExist();
            valVersion();
            valDetailPajak();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE returpenjualan
                             SET nofakturpajak = @nofakturpajak,
                                 tanggalfakturpajak = @tanggalfakturpajak, 
                                 masapajak = @masapajak, 
                                 tanggalupload = @tanggalupload,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("nofakturpajak", this.nofakturpajak);
            parameters.Add("tanggalfakturpajak", this.tanggalfakturpajak);
            parameters.Add("masapajak", this.masapajak);
            parameters.Add("tanggalupload", this.tanggalupload);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valDetailPajak() {
            if(this.tanggalfakturpajak != "" || this.masapajak != "" || this.nofakturpajak != "") {
                if(this.nofakturpajak == "") {
                    throw new Exception("No Faktur Pajak harus diisi, untuk faktur [" + this.kode + "]");
                }

                if(!OswDate.isDate(this.tanggalfakturpajak)) {
                    throw new Exception("Tanggal Faktur Pajak harus valid (dd/mm/yyyy), untuk faktur [" + this.kode + "]");
                }

                if(!OswDate.isTahunBulan(this.masapajak)) {
                    throw new Exception("Masa Pajak harus valid (yyyymm), untuk faktur [" + this.kode + "]");
                }
            }

            if(this.tanggalupload != "") {
                if(this.nofakturpajak == "") {
                    throw new Exception("No Faktur Pajak harus diisi, untuk faktur [" + this.kode + "]");
                }

                if(!OswDate.isDate(this.tanggalfakturpajak)) {
                    throw new Exception("Tanggal Faktur Pajak harus valid (dd/mm/yyyy), untuk faktur [" + this.kode + "]");
                }

                if(!OswDate.isTahunBulan(this.masapajak)) {
                    throw new Exception("Masa Pajak harus valid (yyyymm), untuk faktur [" + this.kode + "]");
                }

                if(!OswDate.isDate(this.tanggalupload)) {
                    throw new Exception("Tanggal Upload Pajak harus valid (dd/mm/yyyy), untuk faktur [" + this.kode + "]");
                }
            }

        }

        public void prosesJurnal() {
            /*
            Deskripsi : Retur Penjualan dari [NAMA CUSTOMER]																													
            Status		Akun
            Kredit		akun_piutang						kembali piutang
            Kredit		akun_uang_titipan_customer			kembali uang titipan
            Kredit		diskon_penjualan			        totaldiskon
            Debit		pajak_keluaran					    totalppn
            Debit		retur penjualan				        grand total + totaldiskon - totalppn
            Kredit		akun_HPP						    Total HPP (HPP * Jumlah)
            Debit		akun_persediaan					    Total HPP (HPP * Jumlah)
            */

            int no = 1;
            DataCustomer dCustomer = new DataCustomer(command, this.customer);
            String strngKeteranganJurnal = "Retur Penjualan dari [" + dCustomer.nama + "]";

            // Debet			ppn_keluaran					
            String strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PAJAK_KELUARAN);
            DataAkun dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Pajak Keluaran [" + strngAkun + "] tidak ditemukan.");
            }

            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, this.totalppn, "0");
            dJurnal.prosesJurnal();

            // Debet			retur_penjualan
            double dblPenjualan = Tools.getRoundMoney(double.Parse(this.grandtotal) + double.Parse(this.totaldiskon) - double.Parse(totalppn));		
            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_RETUR_PENJUALAN);
            dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Retur Penjualan [" + strngAkun + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, dblPenjualan.ToString(), "0");
            dJurnal.prosesJurnal();

            if(double.Parse(this.kembalipiutang) > 0) {
                // Kredit			akun_piutang					
                strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PIUTANG);
                dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Piutang [" + strngAkun + "] tidak ditemukan.");
                }

                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", this.kembalipiutang);
                dJurnal.prosesJurnal();
            }

            if(double.Parse(this.kembaliuangtitipan) > 0) {
                // Kredit			akun_uang_titipan_customer					
                strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_UANG_TITIPAN_CUSTOMER);
                dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Uang Titipan Customer [" + strngAkun + "] tidak ditemukan.");
                }

                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", this.kembaliuangtitipan);
                dJurnal.prosesJurnal();
            }
            

            // Kredit			diskon_penjualan
            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_DISKON_PENJUALAN);
            dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Diskon Penjualan [" + strngAkun + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", this.totaldiskon);
            dJurnal.prosesJurnal();

            // Kredit		akun_HPP						    Total HPP (HPP * Jumlah)
            String query = @"SELECT SUM(jumlahretur * hpp) 
                            FROM returpenjualandetail 
                            WHERE returpenjualan = @returpenjualan";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("returpenjualan", this.kode);

            double dblHPP = Tools.getRoundMoney(double.Parse(OswDataAccess.executeScalarQuery(query, parameters, command)));

            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_HPP);
            dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun HPP [" + strngAkun + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", dblHPP.ToString());
            dJurnal.prosesJurnal();

            // Debit		akun_persediaan					    Total HPP (HPP * Jumlah)
            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PERSEDIAAN);
            dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Persediaan [" + strngAkun + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, dblHPP.ToString(), "0");
            dJurnal.prosesJurnal();

            Tools.cekJurnalBalance(command, this.id, this.kode);
        }

    }
}

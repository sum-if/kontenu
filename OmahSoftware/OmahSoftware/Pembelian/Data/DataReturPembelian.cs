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

namespace OmahSoftware.Pembelian {
    class DataReturPembelian {
        private String id = "RETURPEMBELIAN";
        public String kode = "";
        public String tanggal = "";
        public String supplier = "";
        public String fakturpembelian = "";
        public String catatan = "";
        public String jenisppn = "";
        public String nofakturpajak = "";
        public String tanggalfakturpajak = "";
        public String masapajak = "";
        public String tanggalupload = "";
        public String total = "0";
        public String diskon = "0";
        public String diskonnilai = "0";
        public String totaldiskon = "0";
        public String totaldpp = "0";
        public String totalppn = "0";
        public String grandtotal = "0";
        public String totalfaktur = "0";
        public String sisabayar = "0";
        public String kembalihutang = "0";
        public String kembaliuangtitipan = "0";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Supplier:" + supplier + ";";
            kolom += "Faktur Pembelian:" + fakturpembelian + ";";
            kolom += "Catatan:" + catatan + ";";
            kolom += "Jenis PPN:" + jenisppn + ";";
            kolom += "No Faktur Pajak:" + nofakturpajak + ";";
            kolom += "Tangggal Faktur Pajak:" + tanggalfakturpajak + ";";
            kolom += "Masa Pajak:" + masapajak + ";";
            kolom += "Tanggal Upload:" + tanggalupload + ";";
            kolom += "Total:" + total + ";";
            kolom += "Diskon:" + diskon + ";";
            kolom += "Diskon Nilai:" + diskonnilai + ";";
            kolom += "Total Diskon:" + totaldiskon + ";";
            kolom += "Total DPP:" + totaldpp + ";";
            kolom += "Total PPN:" + totalppn + ";";
            kolom += "Grand Total:" + grandtotal + ";";
            kolom += "Total Faktur:" + totalfaktur + ";";
            kolom += "Sisa Bayar:" + sisabayar + ";";
            kolom += "Kembali Hutang:" + kembalihutang + ";";
            kolom += "Kembali Uang Titipan:" + kembaliuangtitipan + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataReturPembelian(MySqlCommand command, String kode) {
            this.command = command;

            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal,supplier,fakturpembelian,catatan,jenisppn,nofakturpajak, tanggalfakturpajak, masapajak, tanggalupload, total,diskon,diskonnilai,totaldiskon,totaldpp,totalppn,grandtotal,totalfaktur,sisabayar,kembalihutang,kembaliuangtitipan, version
                             FROM returpembelian 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.supplier = reader.GetString("supplier");
                this.fakturpembelian = reader.GetString("fakturpembelian");
                this.catatan = reader.GetString("catatan");
                this.jenisppn = reader.GetString("jenisppn");
                this.nofakturpajak = reader.GetString("nofakturpajak");
                this.tanggalfakturpajak = reader.GetString("tanggalfakturpajak");
                this.masapajak = reader.GetString("masapajak");
                this.tanggalupload = reader.GetString("tanggalupload");
                this.total = reader.GetString("total");
                this.diskon = reader.GetString("diskon");
                this.diskonnilai = reader.GetString("diskonnilai");
                this.totaldiskon = reader.GetString("totaldiskon");
                this.totaldpp = reader.GetString("totaldpp");
                this.totalppn = reader.GetString("totalppn");
                this.grandtotal = reader.GetString("grandtotal");
                this.totalfaktur = reader.GetString("totalfaktur");
                this.sisabayar = reader.GetString("sisabayar");
                this.kembalihutang = reader.GetString("kembalihutang");
                this.kembaliuangtitipan = reader.GetString("kembaliuangtitipan");
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

            DataReturPembelian dReturPembelian = new DataReturPembelian(command, kode);
            if(dReturPembelian.isExist) {
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

            String query = @"INSERT INTO returpembelian(kode, tanggal, supplier,fakturpembelian,catatan,jenisppn,nofakturpajak, tanggalfakturpajak, masapajak, tanggalupload,total,diskon,diskonnilai,totaldiskon,totaldpp,totalppn,grandtotal,totalfaktur,sisabayar,kembalihutang,kembaliuangtitipan, version,create_user) 
                             VALUES(@kode,@tanggal,@supplier,@fakturpembelian,@catatan,@jenisppn,@nofakturpajak, @tanggalfakturpajak, @masapajak, @tanggalupload, @total,@diskon,@diskonnilai,@totaldiskon,@totaldpp,@totalppn,@grandtotal,@totalfaktur,@sisabayar,@kembalihutang,@kembaliuangtitipan, @version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("supplier", this.supplier);
            parameters.Add("fakturpembelian", this.fakturpembelian);
            parameters.Add("catatan", this.catatan);
            parameters.Add("jenisppn", this.jenisppn);
            parameters.Add("nofakturpajak", this.nofakturpajak);
            parameters.Add("tanggalfakturpajak", this.tanggalfakturpajak);
            parameters.Add("masapajak", this.masapajak);
            parameters.Add("tanggalupload", this.tanggalupload);
            parameters.Add("total", this.total);
            parameters.Add("diskon", this.diskon);
            parameters.Add("diskonnilai", this.diskonnilai);
            parameters.Add("totaldiskon", this.totaldiskon);
            parameters.Add("totaldpp", this.totaldpp);
            parameters.Add("totalppn", this.totalppn);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("totalfaktur", this.totalfaktur);
            parameters.Add("sisabayar", this.sisabayar);
            parameters.Add("kembalihutang", this.kembalihutang);
            parameters.Add("kembaliuangtitipan", this.kembaliuangtitipan);
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
            String query = @"DELETE FROM returpembelian
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

            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.fakturpembelian);
            dFakturPembelian.totalretur = this.grandtotal;
            dFakturPembelian.kurangTotalRetur();

            // hapus mutasi
            DataMutasiPersediaan dMutasiPersediaan = new DataMutasiPersediaan(command, this.id, this.kode);
            dMutasiPersediaan.hapus();

            DataMutasiHutang dMutasiHutang = new DataMutasiHutang(command, this.id, this.kode);
            dMutasiHutang.hapus();

            DataMutasiUangTitipanSupplier dMutasiUangTitipanSupplier = new DataMutasiUangTitipanSupplier(command, this.id, this.kode);
            dMutasiUangTitipanSupplier.hapus();

            String query = @"SELECT no
                             FROM returpembeliandetail
                             WHERE returpembelian = @returpembelian";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("returpembelian", this.kode);

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
                DataReturPembelianDetail dReturPembelianDetail = new DataReturPembelianDetail(command, this.kode, row["no"].ToString());
                dReturPembelianDetail.hapus();
            }
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();
            valDetail();

            Tools.valAdmin(command, this.tanggal);

            this.version += 1;

            // proses ubah
            String query = @"UPDATE returpembelian
                             SET tanggal = @tanggal,
                                 supplier = @supplier, 
                                 fakturpembelian = @fakturpembelian,
                                 catatan = @catatan,
                                 jenisppn = @jenisppn,
                                 nofakturpajak = @nofakturpajak,
                                 tanggalfakturpajak = @tanggalfakturpajak, 
                                 masapajak = @masapajak, 
                                 tanggalupload = @tanggalupload,
                                 total = @total,
                                 diskon = @diskon,
                                 diskonnilai = @diskonnilai,
                                 totaldiskon = @totaldiskon,
                                 totaldpp = @totaldpp,
                                 totalppn = @totalppn,
                                 grandtotal = @grandtotal,
                                 totalfaktur = @totalfaktur,
                                 sisabayar = @sisabayar,
                                 kembalihutang = @kembalihutang,
                                 kembaliuangtitipan = @kembaliuangtitipan,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("supplier", this.supplier);
            parameters.Add("fakturpembelian", this.fakturpembelian);
            parameters.Add("catatan", this.catatan);
            parameters.Add("jenisppn", this.jenisppn);
            parameters.Add("nofakturpajak", this.nofakturpajak);
            parameters.Add("tanggalfakturpajak", this.tanggalfakturpajak);
            parameters.Add("masapajak", this.masapajak);
            parameters.Add("tanggalupload", this.tanggalupload);
            parameters.Add("total", this.total);
            parameters.Add("diskon", this.diskon);
            parameters.Add("diskonnilai", this.diskonnilai);
            parameters.Add("totaldiskon", this.totaldiskon);
            parameters.Add("totaldpp", this.totaldpp);
            parameters.Add("totalppn", this.totalppn);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("totalfaktur", this.totalfaktur);
            parameters.Add("sisabayar", this.sisabayar);
            parameters.Add("kembalihutang", this.kembalihutang);
            parameters.Add("kembaliuangtitipan", this.kembaliuangtitipan);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubahFakturPajak() {
            // validasi
            valExist();
            valVersion();
            valDetailPajak();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE returpembelian
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
            parameters.Add("tanggalfakturpajak", this.tanggalfakturpajak);
            parameters.Add("masapajak", this.masapajak);
            parameters.Add("tanggalupload", this.tanggalupload);
            parameters.Add("nofakturpajak", this.nofakturpajak);
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

        public void updateMutasi() {
            if(double.Parse(this.kembalihutang) > 0) {
                DataMutasiHutang dMutasiHutang = new DataMutasiHutang(command, this.id, this.kode);
                dMutasiHutang.no = "1";
                dMutasiHutang.tanggal = this.tanggal;
                dMutasiHutang.supplier = this.supplier;
                dMutasiHutang.jumlah = (double.Parse(this.kembalihutang) * -1).ToString();
                dMutasiHutang.proses();
            }

            if(double.Parse(this.kembaliuangtitipan) > 0) {
                DataMutasiUangTitipanSupplier dMutasiUangTitipanSupplier = new DataMutasiUangTitipanSupplier(command, this.id, this.kode);
                dMutasiUangTitipanSupplier.no = "1";
                dMutasiUangTitipanSupplier.tanggal = this.tanggal;
                dMutasiUangTitipanSupplier.supplier = this.supplier;
                dMutasiUangTitipanSupplier.jumlah = this.kembaliuangtitipan;
                dMutasiUangTitipanSupplier.proses();
            }

            //update total retur di faktur pembelian
            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.fakturpembelian);
            dFakturPembelian.totalretur = this.grandtotal;
            dFakturPembelian.tambahTotalRetur();
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
            DataReturPembelian dReturPembelian = new DataReturPembelian(command, this.kode);
            if(this.version != dReturPembelian.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        private void valTanggal() {
            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.fakturpembelian);

            if(OswDate.getDateTimeFromStringTanggal(this.tanggal) < OswDate.getDateTimeFromStringTanggal(dFakturPembelian.tanggal)) {
                throw new Exception("Tanggal Retur Pembelian harus >= Tanggal Faktur Pembelien [" + this.fakturpembelian + " - " + dFakturPembelian.tanggal + "]");
            }
        }

        public void valJumlahDetail() {
            String query = @"SELECT COUNT(*)
                             FROM returpembeliandetail A
                             WHERE A.returpembelian = @returpembelian";

            Dictionary<String, String> parameters = new Dictionary<string, string>();

            parameters.Add("returpembelian", this.kode);

            Double dblJumlahDetail = Double.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlahDetail <= 0) {
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

            if(double.Parse(this.kembalihutang) < 0) {
                throw new Exception("Kembali Hutang harus >= 0");
            }

            if(double.Parse(this.kembaliuangtitipan) < 0) {
                throw new Exception("Kembali Uang Titipan harus >= 0");
            }
        }

        public void prosesJurnal() {
            /*
            Deskripsi : Retur Pembelian ke [NAMA SUPPLIER]
	        Status			Akun									            Keterangan
            Debit			akun_hutang						    grand total
            Kredit			akun_pajak_masukan					total pajak
	        Kredit          akun_persediaan						subtotal per jenis setelah dipotong diskon & ppn
            */

            int no = 1;
            DataSupplier dSupplier = new DataSupplier(command, this.supplier);
            String strngKeteranganJurnal = "Retur Pembelian ke [" + dSupplier.nama + "]";

            String strngAkun;
            DataJurnal dJurnal;
            DataAkun dAkun;

            if(double.Parse(this.kembalihutang) > 0) {
                // Debit			akun_hutang						    kembali hutang
                strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_HUTANG);
                dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Hutang [" + strngAkun + "] tidak ditemukan.");
                }

                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, this.kembalihutang, "0");
                dJurnal.prosesJurnal();
            }

            if(double.Parse(this.kembaliuangtitipan) > 0) {
                // Debit			akun_uang_titipan_supplier						    kembali uang titipan
                strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_UANG_TITIPAN_SUPPLIER);
                dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Uang Titipan Supplier [" + strngAkun + "] tidak ditemukan.");
                }

                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, this.kembaliuangtitipan, "0");
                dJurnal.prosesJurnal();
            }

            // Kredit			akun_pajak_masukan					total pajak
            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PAJAK_MASUKAN);
            dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Pajak Masukan [" + strngAkun + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", this.totalppn);
            dJurnal.prosesJurnal();

            // Kredit			akun_persediaan						subtotal per jenis setelah dipotong diskon & ppn
            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PERSEDIAAN);
            dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Persediaan [" + strngAkun + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", this.totaldpp);
            dJurnal.prosesJurnal();

            Tools.cekJurnalBalance(command, this.id, this.kode);
        }
    }
}

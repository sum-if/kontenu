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
    class DataFakturPembelian {
        private String id = "FAKTURPEMBELIAN";
        public String kode = "";
        public String tanggal = "";
        public String supplier = "";
        public String pesananpembelian = "";
        public String faktursupplier = "";
        public String jatuhtempo = "";
        public String jenisppn = "";
        public String nofakturpajak = "";
        public String tanggalfakturpajak = "";
        public String masapajak = "";
        public String tanggalupload = "";
        public String catatan = "";
        public String total = "0";
        public String diskon = "0";
        public String diskonnilai = "0";
        public String totaldiskon = "0";
        public String totaldpp = "0";
        public String totalppn = "0";
        public String pembulatan = "0";
        public String grandtotal = "0";
        public String totalretur = "0";
        public String totalbayar = "0";
        public String ekspedisi = "";
        public String biayaekspedisi = "0";
        public String totalbayarekspedisi = "0";
        public String status = "";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Supplier:" + supplier + ";";
            kolom += "Pesanan Pembelian:" + pesananpembelian + ";";
            kolom += "Invoice Supplier:" + faktursupplier + ";";
            kolom += "Jatuh Tempo:" + jatuhtempo + ";";
            kolom += "Jenis PPN:" + jenisppn + ";";
            kolom += "No Faktur Pajak:" + nofakturpajak + ";";
            kolom += "Tangggal Faktur Pajak:" + tanggalfakturpajak + ";";
            kolom += "Masa Pajak:" + masapajak + ";";
            kolom += "Tanggal Upload:" + tanggalupload + ";";
            kolom += "Catatan:" + catatan + ";";
            kolom += "Total:" + total + ";";
            kolom += "Diskon:" + diskon + ";";
            kolom += "Diskon Nilai:" + diskonnilai + ";";
            kolom += "Total Diskon:" + totaldiskon + ";";
            kolom += "Total DPP:" + totaldpp + ";";
            kolom += "Total PPN:" + totalppn + ";";
            kolom += "Pembulatan:" + pembulatan + ";";
            kolom += "Grand Total:" + grandtotal + ";";
            kolom += "Total Retur:" + totalretur + ";";
            kolom += "Total Bayar:" + totalbayar + ";";
            kolom += "Ekspedisi:" + ekspedisi + ";";
            kolom += "Biaya Ekspedisi:" + biayaekspedisi + ";";
            kolom += "Total Bayar Ekspedisi:" + totalbayarekspedisi + ";";
            kolom += "Status:" + status + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataFakturPembelian(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal, supplier, pesananpembelian, faktursupplier, jatuhtempo, jenisppn, nofakturpajak, tanggalfakturpajak, masapajak, tanggalupload, catatan, total, 
                                    diskon, diskonnilai, totaldiskon,totaldpp, totalppn, pembulatan, grandtotal, totalretur, totalbayar, ekspedisi, biayaekspedisi, totalbayarekspedisi, status, version
                             FROM fakturpembelian 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.supplier = reader.GetString("supplier");
                this.pesananpembelian = reader.GetString("pesananpembelian");
                this.faktursupplier = reader.GetString("faktursupplier");
                this.jatuhtempo = reader.GetString("jatuhtempo");
                this.jenisppn = reader.GetString("jenisppn");
                this.nofakturpajak = reader.GetString("nofakturpajak");
                this.tanggalfakturpajak = reader.GetString("tanggalfakturpajak");
                this.masapajak = reader.GetString("masapajak");
                this.tanggalupload = reader.GetString("tanggalupload");
                this.catatan = reader.GetString("catatan");
                this.total = reader.GetString("total");
                this.diskon = reader.GetString("diskon");
                this.diskonnilai = reader.GetString("diskonnilai");
                this.totaldiskon = reader.GetString("totaldiskon");
                this.totaldpp = reader.GetString("totaldpp");
                this.totalppn = reader.GetString("totalppn");
                this.pembulatan = reader.GetString("pembulatan");
                this.grandtotal = reader.GetString("grandtotal");
                this.totalretur = reader.GetString("totalretur");
                this.totalbayar = reader.GetString("totalbayar");
                this.ekspedisi = reader.GetString("ekspedisi");
                this.biayaekspedisi = reader.GetString("biayaekspedisi");
                this.totalbayarekspedisi = reader.GetString("totalbayarekspedisi");
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

            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, kode);
            if(dFakturPembelian.isExist) {
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

            String query = @"INSERT INTO fakturpembelian( kode, tanggal, supplier, pesananpembelian, faktursupplier, jatuhtempo, jenisppn, nofakturpajak, tanggalfakturpajak, masapajak, tanggalupload, catatan, 
                                                         total, diskon, diskonnilai, totaldiskon,totaldpp, totalppn,pembulatan, grandtotal, totalretur, totalbayar, ekspedisi, biayaekspedisi, totalbayarekspedisi, status, version, create_user) 
                             VALUES(@kode, @tanggal, @supplier, @pesananpembelian, @faktursupplier, @jatuhtempo, @jenisppn, @nofakturpajak, @tanggalfakturpajak, @masapajak, @tanggalupload, @catatan, 
                                    @total, @diskon, @diskonnilai, @totaldiskon,@totaldpp, @totalppn,@pembulatan, @grandtotal, @totalretur, @totalbayar, @ekspedisi, @biayaekspedisi, @totalbayarekspedisi, @status, @version, @create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("supplier", this.supplier);
            parameters.Add("pesananpembelian", this.pesananpembelian);
            parameters.Add("faktursupplier", this.faktursupplier);
            parameters.Add("jatuhtempo", this.jatuhtempo);
            parameters.Add("jenisppn", this.jenisppn);
            parameters.Add("nofakturpajak", this.nofakturpajak);
            parameters.Add("tanggalfakturpajak", this.tanggalfakturpajak);
            parameters.Add("masapajak", this.masapajak);
            parameters.Add("tanggalupload", this.tanggalupload);
            parameters.Add("catatan", this.catatan);
            parameters.Add("total", this.total);
            parameters.Add("diskon", this.diskon);
            parameters.Add("diskonnilai", this.diskonnilai);
            parameters.Add("totaldiskon", this.totaldiskon);
            parameters.Add("totaldpp", this.totaldpp);
            parameters.Add("totalppn", this.totalppn);
            parameters.Add("pembulatan", this.pembulatan);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("totalretur", this.totalretur);
            parameters.Add("totalbayar", this.totalbayar);
            parameters.Add("ekspedisi", this.ekspedisi);
            parameters.Add("biayaekspedisi", this.biayaekspedisi);
            parameters.Add("totalbayarekspedisi", this.totalbayarekspedisi);
            parameters.Add("status", this.status);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();
            valVersion();
            valPembayaranPembelian();
            valReturPembelian();

            // hapus detail
            this.hapusDetail();

            // hapus header
            String query = @"DELETE FROM fakturpembelian
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

            // hapus mutasi
            DataMutasiHutang dMutasiHutang = new DataMutasiHutang(command, this.id, this.kode);
            dMutasiHutang.hapus();

            DataMutasiHPP dMutasiHPP = new DataMutasiHPP(command, this.id, this.kode);
            dMutasiHPP.hapus();

            DataMutasiPersediaan dMutasiPersediaan = new DataMutasiPersediaan(command, this.id, this.kode);
            dMutasiPersediaan.hapus();

            String query = @"SELECT no
                             FROM fakturpembeliandetail
                             WHERE fakturpembelian = @fakturpembelian";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("fakturpembelian", this.kode);

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
                DataFakturPembelianDetail dFakturPembelianDetail = new DataFakturPembelianDetail(command, this.kode, row["no"].ToString());
                dFakturPembelianDetail.hapus();
            }
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();
            valPembayaranPembelian();
            valUbahEkspedisi();
            valReturPembelian();
            valDetail();
            valTanggal();

            Tools.valAdmin(command, this.tanggal);

            this.version += 1;

            // proses ubah
            String query = @"UPDATE fakturpembelian
                             SET tanggal = @tanggal,
                                 supplier = @supplier, 
                                 pesananpembelian = @pesananpembelian,
                                 faktursupplier = @faktursupplier,
                                 jatuhtempo = @jatuhtempo,
                                 jenisppn = @jenisppn, 
                                 nofakturpajak = @nofakturpajak,
                                 tanggalfakturpajak = @tanggalfakturpajak, 
                                 masapajak = @masapajak, 
                                 tanggalupload = @tanggalupload,
                                 catatan = @catatan,
                                 total = @total,
                                 diskon = @diskon,
                                 diskonnilai = @diskonnilai,
                                 totaldiskon = @totaldiskon,
                                 totaldpp = @totaldpp,
                                 totalppn = @totalppn,
                                 pembulatan = @pembulatan,
                                 grandtotal = @grandtotal,
                                 totalretur = @totalretur,
                                 totalbayar = @totalbayar,
                                 ekspedisi = @ekspedisi,
                                 biayaekspedisi = @biayaekspedisi,
                                 totalbayarekspedisi = @totalbayarekspedisi,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("supplier", this.supplier);
            parameters.Add("pesananpembelian", this.pesananpembelian);
            parameters.Add("faktursupplier", this.faktursupplier);
            parameters.Add("jatuhtempo", this.jatuhtempo);
            parameters.Add("jenisppn", this.jenisppn);
            parameters.Add("nofakturpajak", this.nofakturpajak);
            parameters.Add("tanggalfakturpajak", this.tanggalfakturpajak);
            parameters.Add("masapajak", this.masapajak);
            parameters.Add("tanggalupload", this.tanggalupload);
            parameters.Add("catatan", this.catatan);
            parameters.Add("total", this.total);
            parameters.Add("diskon", this.diskon);
            parameters.Add("diskonnilai", this.diskonnilai);
            parameters.Add("totaldiskon", this.totaldiskon);
            parameters.Add("totaldpp", this.totaldpp);
            parameters.Add("totalppn", this.totalppn);
            parameters.Add("pembulatan", this.pembulatan);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("totalretur", this.totalretur);
            parameters.Add("totalbayar", this.totalbayar);
            parameters.Add("ekspedisi", this.ekspedisi);
            parameters.Add("biayaekspedisi", this.biayaekspedisi);
            parameters.Add("totalbayarekspedisi", this.totalbayarekspedisi);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubahEkspedisi() {
            // validasi
            valExist();
            valVersion();
            valUbahEkspedisi();

            // hapus jurnal 
            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode);
            dJurnal.hapusJurnal();

            Tools.valAdmin(command, this.tanggal);

            this.version += 1;

            // proses ubah
            String query = @"UPDATE fakturpembelian
                             SET ekspedisi = @ekspedisi,
                                 biayaekspedisi = @biayaekspedisi,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("ekspedisi", this.ekspedisi);
            parameters.Add("biayaekspedisi", this.biayaekspedisi);
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
            String query = @"UPDATE fakturpembelian
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

        public void ubahFakturPajak() {
            // validasi
            valExist();
            valVersion();
            valDetailPajak();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE fakturpembelian
                             SET nofakturpajak = @nofakturpajak,
                                 tanggalfakturpajak = @tanggalfakturpajak, 
                                 masapajak = @masapajak, 
                                 tanggalupload = @tanggalupload,
                                 faktursupplier = @faktursupplier,
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
            parameters.Add("faktursupplier", this.faktursupplier);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void tambahTotalRetur() {
            // validasi
            valExist();
            valVersion();

            // update total retur
            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.kode);
            double dblJumlahLama = double.Parse(dFakturPembelian.totalretur);
            double dblJumlahBaru = dblJumlahLama + double.Parse(this.totalretur);

            String query = @"UPDATE fakturpembelian 
                             SET totalretur = @totalretur
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);
            parameters.Add("totalretur", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);

            setStatusFaktur();
        }

        public void kurangTotalRetur() {
            // validasi
            valExist();
            valVersion();

            // update jumlah terima di faktur
            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.kode);
            double dblJumlahLama = double.Parse(dFakturPembelian.totalretur);
            double dblJumlahBaru = dblJumlahLama - double.Parse(this.totalretur);

            // jumlah terima < 0
            if(dblJumlahBaru < 0) {
                throw new Exception("Total Retur [" + this.kode + "] < 0 di Invoice Pembelian");
            }

            String query = @"UPDATE fakturpembelian 
                             SET totalretur = @totalretur
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);
            parameters.Add("totalretur", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);

            setStatusFaktur();
        }

        public void tambahTotalBayar() {
            // validasi
            valExist();
            valVersion();

            // update total bayar di faktur
            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.kode);
            double dblJumlahLama = double.Parse(dFakturPembelian.totalbayar);
            double dblJumlahBaru = dblJumlahLama + double.Parse(this.totalbayar);

            // total bayar > grand total
            if(double.Parse(this.grandtotal) < (dblJumlahBaru + double.Parse(this.totalretur))) {
                throw new Exception("Total Bayar [" + this.kode + "] > Sisa Bayar di Invoice Pembelian");
            }

            String query = @"UPDATE fakturpembelian
                             SET totalbayar = @totalbayar
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);
            parameters.Add("totalbayar", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);

            setStatusFaktur();
        }

        public void kurangTotalBayar() {
            // validasi
            valExist();
            valVersion();

            // update jumlah terima di faktur
            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.kode);
            double dblJumlahLama = double.Parse(dFakturPembelian.totalbayar);
            double dblJumlahBaru = dblJumlahLama - double.Parse(this.totalbayar);

            // total bayar < 0
            if(dblJumlahBaru < 0) {
                throw new Exception("Total Bayar [" + this.kode + "] < 0 di Invoice Pembelian");
            }

            String query = @"UPDATE fakturpembelian
                             SET totalbayar = @totalbayar
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);
            parameters.Add("totalbayar", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);

            setStatusFaktur();
        }

        public void tambahTotalBayarEkspedisi() {
            // validasi
            valExist();
            valVersion();

            // update total bayar di faktur
            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.kode);
            double dblJumlahLama = double.Parse(dFakturPembelian.totalbayarekspedisi);
            double dblJumlahBaru = dblJumlahLama + double.Parse(this.totalbayarekspedisi);

            // total bayar > grand total
            if(double.Parse(this.biayaekspedisi) < dblJumlahBaru) {
                throw new Exception("Total Bayar Ekspedisi [" + this.kode + "] > Biaya Ekspedisi di Invoice Pembelian");
            }

            String query = @"UPDATE fakturpembelian
                             SET totalbayarekspedisi = @totalbayarekspedisi
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("totalbayarekspedisi", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void kurangTotalBayarEkspedisi() {
            // validasi
            valExist();
            valVersion();

            // update jumlah terima di faktur
            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.kode);
            double dblJumlahLama = double.Parse(dFakturPembelian.totalbayarekspedisi);
            double dblJumlahBaru = dblJumlahLama - double.Parse(this.totalbayarekspedisi);

            // total bayar < 0
            if(dblJumlahBaru < 0) {
                throw new Exception("Total Bayar [" + this.kode + "] < 0 di Invoice Pembelian");
            }

            String query = @"UPDATE fakturpembelian
                             SET totalbayarekspedisi = @totalbayarekspedisi
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("totalbayarekspedisi", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void setStatusFaktur() {
            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.kode);

            // jika grand total > total bayar + total retur --> status faktur jadi 'Belum Lunas'
            if(double.Parse(dFakturPembelian.grandtotal) > (double.Parse(dFakturPembelian.totalbayar) + double.Parse(dFakturPembelian.totalretur))) {
                dFakturPembelian.status = Constants.STATUS_FAKTUR_PEMBELIAN_BELUM_LUNAS;
            } else {
                dFakturPembelian.status = Constants.STATUS_FAKTUR_PEMBELIAN_LUNAS;
            }

            dFakturPembelian.ubahStatus();
        }

        public void updateMutasi() {
            // validasi
            valPembayaranPembelian();
            valReturPembelian();

            // mutasi hutang
            DataMutasiHutang dMutasiHutang = new DataMutasiHutang(command, this.id, this.kode);
            dMutasiHutang.no = "1";
            dMutasiHutang.tanggal = this.tanggal;
            dMutasiHutang.supplier = this.supplier;
            dMutasiHutang.jumlah = this.grandtotal;
            dMutasiHutang.proses();
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
            DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, this.kode);
            if(this.version != dFakturPembelian.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        public void valJumlahDetail() {
            String query = @"SELECT COUNT(*)
                             FROM fakturpembeliandetail A
                             WHERE A.fakturpembelian = @fakturpembelian";

            Dictionary<String, String> parameters = new Dictionary<string, string>();

            parameters.Add("fakturpembelian", this.kode);

            Double dblJumlahDetail = Double.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlahDetail <= 0) {
                throw new Exception("Jumlah Item detail harus lebih dari 0");
            }
        }

        public void valPembayaranPembelian() {
            // cek sudah ada pembayaran pembelian atau belum
            if(double.Parse(this.totalbayar) > 0) {
                throw new Exception("Pembayaran Pembelian telah di lakukan");
            }
        }

        public void valReturPembelian() {
            // cek sudah ada retur pembelian atau belum
            if(double.Parse(this.totalretur) > 0) {
                throw new Exception("Retur Pembelian telah di lakukan");
            }
        }

        private void valDetail() {
            if(double.Parse(this.grandtotal) <= 0) {
                throw new Exception("Grand Total harus > 0");
            }

            if(this.ekspedisi == "" && double.Parse(this.biayaekspedisi) > 0) {
                throw new Exception("Silahkan pilih ekspedisi");
            }

            if(this.ekspedisi != "" && double.Parse(this.biayaekspedisi) <= 0) {
                throw new Exception("Biaya ekspedisi harus > 0");
            }
        }

        private void valUbahEkspedisi() {
            // cek sudah ada pembayaran hutang ekspedisi
            if(double.Parse(this.biayaekspedisi) > 0 && double.Parse(this.totalbayarekspedisi) > 0) {
                throw new Exception("Pembayaran Hutang Ekspedisi telah di lakukan");
            }
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

        private void valTanggal() {
            if(this.pesananpembelian != "") {
                DataPesananPembelian dPesananPembelian = new DataPesananPembelian(command, this.pesananpembelian);
                if(OswDate.getDateTimeFromStringTanggal(this.tanggal) < OswDate.getDateTimeFromStringTanggal(dPesananPembelian.tanggal)) {
                    throw new Exception("Tanggal Faktur Pembelian harus >= Tanggal Pesanan Pembelien [" + this.pesananpembelian + " - " + dPesananPembelian.tanggal + "]");
                }
            }
        }

        public void prosesJurnal() {
            /*
            Deskripsi : Invoice Pembelian dari [NAMA SUPPLIER]																														
	        Status			Akun									    Keterangan
	        Debit			akun_persediaan						        total dpp
            Debit			akun_pajak_masukan						    total ppn
            Debit			akun_pembulatan_biaya						Selisih Kredit - Debit
            Kredit			akun_hutang						            grand total
            
            Kredit			akun_hutang_ekspedisi						biaya ekspedisi
            Debit			akun_biaya_ekspedisi						biaya ekspedisi
            */

            int no = 1;
            DataSupplier dSupplier = new DataSupplier(command, this.supplier);
            String strngKeteranganJurnal = "Invoice Pembelian dari [" + dSupplier.nama + "]";

            // Debit			barang.akun_persediaan 
            String strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PERSEDIAAN);
            DataAkun dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Persediaan [" + strngAkun + "] tidak ditemukan.");
            }

            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, this.totaldpp, "0");
            dJurnal.prosesJurnal();

            // Debit			akun_pajak_masukan						total pajak									
            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PAJAK_MASUKAN);
            dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Pajak Masukan [" + strngAkun + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, this.totalppn, "0");
            dJurnal.prosesJurnal();

            // Pembulatan
            if(double.Parse(this.pembulatan) > 0) {
                strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PEMBULATAN_BIAYA);
                dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Pembulatan Biaya [" + strngAkun + "] tidak ditemukan.");
                }

                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, this.pembulatan, "0");
                dJurnal.prosesJurnal();
            } else if(double.Parse(this.pembulatan) < 0) {
                strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PEMBULATAN_PENDAPATAN);
                dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Pembulatan Pendapatan [" + strngAkun + "] tidak ditemukan.");
                }

                double dblPembulatan = double.Parse(this.pembulatan) * -1;
                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", dblPembulatan.ToString());
                dJurnal.prosesJurnal();
            }

            // Kredit			akun_hutang						grand total
            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_HUTANG);
            dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Hutang [" + strngAkun + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", this.grandtotal);
            dJurnal.prosesJurnal();

            // hutang ekspedisi
            if(double.Parse(this.biayaekspedisi) > 0) {
                // Kredit			akun_hutang_ekspedisi						biaya ekspedisi
                strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_HUTANG_EKSPEDISI);
                dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Hutang Ekspedisi [" + strngAkun + "] tidak ditemukan.");
                }

                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", this.biayaekspedisi);
                dJurnal.prosesJurnal();

                // Debit			akun_biaya_ekspedisi						biaya ekspedisi
                strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_BIAYA_EKSPEDISI);
                dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Biaya Ekspedisi [" + strngAkun + "] tidak ditemukan.");
                }

                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, this.biayaekspedisi, "0");
                dJurnal.prosesJurnal();
            }


            Tools.cekJurnalBalance(command, this.id, this.kode);
        }
    }
}

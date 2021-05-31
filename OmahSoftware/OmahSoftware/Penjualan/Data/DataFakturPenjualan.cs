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
    class DataFakturPenjualan {
        private String id = "FAKTURPENJUALAN";
        public String kode = "";
        public String tanggal = "";
        public String customer = "";
        public String jenispesananpenjualan = "";
        public String pesananpenjualan = "";
        public String ekspedisi = "";
        public String nofakturpajak = "";
        public String tanggalfakturpajak = "";
        public String masapajak = "";
        public String tanggalupload = "";
        public String tanggaljatuhtempo = "";
        public String jenisppn = "";
        public String catatan = "";
        public String total = "0";
        public String diskon = "0";
        public String diskonnilai = "0";
        public String totaldiskon = "0";
        public String totaldpp = "0";
        public String totalppn = "0";
        public String grandtotal = "0";
        public String totalbayar = "0";
        public String totalretur = "0";
        public String gunggung = "";
        public String status = "";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";

            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Customer:" + customer + ";";
            kolom += "No Faktur Pajak:" + nofakturpajak + ";";
            kolom += "Tangggal Faktur Pajak:" + tanggalfakturpajak + ";";
            kolom += "Masa Pajak:" + masapajak + ";";
            kolom += "Tanggal Upload:" + tanggalupload + ";";
            kolom += "Tanggal Jatuh Tempo:" + tanggaljatuhtempo + ";";
            kolom += "Jenis Pesanan Penjualan:" + jenispesananpenjualan + ";";
            kolom += "Pesanan Penjualan:" + pesananpenjualan + ";";
            kolom += "Ekspedisi:" + ekspedisi + ";";
            kolom += "PPN:" + jenisppn + ";";
            kolom += "Total:" + total + ";";
            kolom += "Diskon:" + diskon + ";";
            kolom += "Diskon Nilai:" + diskonnilai + ";";
            kolom += "Total Diskon:" + totaldiskon + ";";
            kolom += "Total DPP:" + totaldpp + ";";
            kolom += "Total PPN:" + totalppn + ";";
            kolom += "Grand Total:" + grandtotal + ";";
            kolom += "Total Bayar:" + totalbayar + ";";
            kolom += "Total Retur:" + totalretur + ";";
            kolom += "Catatan:" + catatan + ";";
            kolom += "Gunggung:" + gunggung + ";";
            kolom += "Status:" + status + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataFakturPenjualan(MySqlCommand command, String kode) {
            this.command = command;

            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal, customer,nofakturpajak, tanggalfakturpajak, masapajak, tanggalupload, tanggaljatuhtempo,jenispesananpenjualan,pesananpenjualan,jenisppn,total,diskon,diskonnilai,totaldiskon,totaldpp,totalppn,
                                    grandtotal,totalbayar,totalretur,catatan, gunggung, status, version,ekspedisi
                             FROM fakturpenjualan 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.customer = reader.GetString("customer");
                this.nofakturpajak = reader.GetString("nofakturpajak");
                this.tanggalfakturpajak = reader.GetString("tanggalfakturpajak");
                this.masapajak = reader.GetString("masapajak");
                this.tanggalupload = reader.GetString("tanggalupload");
                this.tanggaljatuhtempo = reader.GetString("tanggaljatuhtempo");
                this.jenispesananpenjualan = reader.GetString("jenispesananpenjualan");
                this.pesananpenjualan = reader.GetString("pesananpenjualan");
                this.jenisppn = reader.GetString("jenisppn");
                this.total = reader.GetString("total");
                this.diskon = reader.GetString("diskon");
                this.diskonnilai = reader.GetString("diskonnilai");
                this.totaldiskon = reader.GetString("totaldiskon");
                this.totaldpp = reader.GetString("totaldpp");
                this.totalppn = reader.GetString("totalppn");
                this.grandtotal = reader.GetString("grandtotal");
                this.totalbayar = reader.GetString("totalbayar");
                this.totalretur = reader.GetString("totalretur");
                this.catatan = reader.GetString("catatan");
                this.gunggung = reader.GetString("gunggung");
                this.status = reader.GetString("status");
                this.version = reader.GetInt64("version");
                this.ekspedisi = reader.GetString("ekspedisi");
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

            DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, kode);
            if(dFakturPenjualan.isExist) {
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

            String query = @"INSERT INTO fakturpenjualan(kode, tanggal, customer,nofakturpajak, tanggalfakturpajak, masapajak, tanggalupload, tanggaljatuhtempo,jenispesananpenjualan,pesananpenjualan,jenisppn,total,diskon,diskonnilai,totaldiskon,totaldpp,totalppn,grandtotal,totalbayar,totalretur,catatan, gunggung, status, ekspedisi, version,create_user) 
                             VALUES(@kode,@tanggal,@customer,@nofakturpajak, @tanggalfakturpajak, @masapajak, @tanggalupload, @tanggaljatuhtempo,@jenispesananpenjualan,@pesananpenjualan,@jenisppn,@total,@diskon,@diskonnilai,@totaldiskon,@totaldpp,@totalppn,@grandtotal,@totalbayar,@totalretur,@catatan,@gunggung, @status, @ekspedisi, @version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("customer", this.customer);
            parameters.Add("nofakturpajak", this.nofakturpajak);
            parameters.Add("tanggalfakturpajak", this.tanggalfakturpajak);
            parameters.Add("masapajak", this.masapajak);
            parameters.Add("tanggalupload", this.tanggalupload);
            parameters.Add("tanggaljatuhtempo", this.tanggaljatuhtempo);
            parameters.Add("jenispesananpenjualan", this.jenispesananpenjualan);
            parameters.Add("pesananpenjualan", this.pesananpenjualan);
            parameters.Add("jenisppn", this.jenisppn);
            parameters.Add("total", this.total);
            parameters.Add("diskon", this.diskon);
            parameters.Add("diskonnilai", this.diskonnilai);
            parameters.Add("totaldiskon", this.totaldiskon);
            parameters.Add("totaldpp", this.totaldpp);
            parameters.Add("totalppn", this.totalppn);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("totalbayar", this.totalbayar);
            parameters.Add("totalretur", this.totalretur);
            parameters.Add("catatan", this.catatan);
            parameters.Add("gunggung", this.gunggung);
            parameters.Add("status", this.status);
            parameters.Add("ekspedisi", this.ekspedisi);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();
            valVersion();
            valReturPenjualan();
            valPenerimaanPenjualan();

            // hapus detail
            this.hapusDetail();

            // hapus header
            String query = @"DELETE FROM fakturpenjualan
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
            DataMutasiPiutang dMutasiPiutang = new DataMutasiPiutang(command, this.id, this.kode);
            dMutasiPiutang.hapus();

            DataMutasiPersediaan dMutasiPersediaan = new DataMutasiPersediaan(command, this.id, this.kode);
            dMutasiPersediaan.hapus();

            String query = @"SELECT no
                             FROM fakturpenjualandetail
                             WHERE fakturpenjualan = @fakturpenjualan";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("fakturpenjualan", this.kode);

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
                DataFakturPenjualanDetail dFakturPenjualanDetail = new DataFakturPenjualanDetail(command, this.kode, row["no"].ToString());
                dFakturPenjualanDetail.hapus();
            }
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();
            valReturPenjualan();
            valPenerimaanPenjualan();
            valDetail();
            valTanggal();

            Tools.valAdmin(command, this.tanggal);

            this.version += 1;

            // proses ubah
            String query = @"UPDATE fakturpenjualan
                             SET tanggal = @tanggal,
                                 customer = @customer, 
                                 nofakturpajak = @nofakturpajak,
                                 tanggalfakturpajak = @tanggalfakturpajak, 
                                 masapajak = @masapajak, 
                                 tanggalupload = @tanggalupload,
                                 tanggaljatuhtempo = @tanggaljatuhtempo,
                                 jenispesananpenjualan = @jenispesananpenjualan,
                                 pesananpenjualan = @pesananpenjualan,
                                 jenisppn = @jenisppn,
                                 total = @total,
                                 diskon = @diskon,
                                 diskonnilai = @diskonnilai,
                                 totaldiskon = @totaldiskon,
                                 totaldpp = @totaldpp,
                                 totalppn = @totalppn,
                                 grandtotal = @grandtotal,
                                 totalbayar = @totalbayar,
                                 totalretur = @totalretur,
                                 catatan = @catatan,
                                 ekspedisi = @ekspedisi,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("customer", this.customer);
            parameters.Add("nofakturpajak", this.nofakturpajak);
            parameters.Add("tanggalfakturpajak", this.tanggalfakturpajak);
            parameters.Add("masapajak", this.masapajak);
            parameters.Add("tanggalupload", this.tanggalupload);
            parameters.Add("tanggaljatuhtempo", this.tanggaljatuhtempo);
            parameters.Add("jenispesananpenjualan", this.jenispesananpenjualan);
            parameters.Add("pesananpenjualan", this.pesananpenjualan);
            parameters.Add("jenisppn", this.jenisppn);
            parameters.Add("total", this.total);
            parameters.Add("diskon", this.diskon);
            parameters.Add("diskonnilai", this.diskonnilai);
            parameters.Add("totaldiskon", this.totaldiskon);
            parameters.Add("totaldpp", this.totaldpp);
            parameters.Add("totalppn", this.totalppn);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("totalbayar", this.totalbayar);
            parameters.Add("totalretur", this.totalretur);
            parameters.Add("catatan", this.catatan);
            parameters.Add("ekspedisi", this.ekspedisi);
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
            String query = @"UPDATE fakturpenjualan
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

        public void ubahStatusGunggung() {
            // validasi
            valExist();
            valVersion();
            valGunggung();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE fakturpenjualan
                             SET gunggung = @gunggung,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("gunggung", this.gunggung);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void tambahTotalRetur() {
            // validasi
            valExist();
            valVersion();

            // update total retur
            DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, this.kode);
            double dblJumlahLama = double.Parse(dFakturPenjualan.totalretur);
            double dblJumlahBaru = dblJumlahLama + double.Parse(this.totalretur);

            String query = @"UPDATE fakturpenjualan 
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
            DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, this.kode);
            double dblJumlahLama = double.Parse(dFakturPenjualan.totalretur);
            double dblJumlahBaru = dblJumlahLama - double.Parse(this.totalretur);

            // jumlah terima < 0
            if(dblJumlahBaru < 0) {
                throw new Exception("Total Retur [" + this.kode + "] < 0 di Invoice Penjualan");
            }

            String query = @"UPDATE fakturpenjualan 
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
            DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, this.kode);
            double dblJumlahLama = double.Parse(dFakturPenjualan.totalbayar);
            double dblJumlahBaru = dblJumlahLama + double.Parse(this.totalbayar);

            // total bayar > grand total
            double dblGrandTotal = Tools.getRoundMoney(double.Parse(this.grandtotal));
            double dblTotalBayar = Tools.getRoundMoney((dblJumlahBaru + double.Parse(this.totalretur)));
            if(dblGrandTotal < dblTotalBayar) {
                throw new Exception("Total Bayar [" + this.kode + "] > Sisa Bayar di Invoice Penjualan");
            }

            String query = @"UPDATE fakturpenjualan
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
            DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, this.kode);
            double dblJumlahLama = double.Parse(dFakturPenjualan.totalbayar);
            double dblJumlahBaru = dblJumlahLama - double.Parse(this.totalbayar);

            // total bayar < 0
            if(dblJumlahBaru < 0) {
                throw new Exception("Total Bayar [" + this.kode + "] < 0 di Invoice Penjualan");
            }

            String query = @"UPDATE fakturpenjualan
                             SET totalbayar = @totalbayar
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);
            parameters.Add("totalbayar", dblJumlahBaru.ToString());

            OswDataAccess.executeVoidQuery(query, parameters, command);

            setStatusFaktur();
        }

        private void setStatusFaktur() {
            DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, this.kode);

            // jika grand total > total bayar + total retur --> status faktur jadi 'Belum Lunas'
            if(double.Parse(dFakturPenjualan.grandtotal) > (double.Parse(dFakturPenjualan.totalbayar) + double.Parse(dFakturPenjualan.totalretur))) {
                dFakturPenjualan.status = Constants.STATUS_FAKTUR_PENJUALAN_BELUM_LUNAS;
            } else {
                dFakturPenjualan.status = Constants.STATUS_FAKTUR_PENJUALAN_LUNAS;
            }

            dFakturPenjualan.ubahStatus();
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
            DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, this.kode);
            if(this.version != dFakturPenjualan.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        public void valJumlahDetail() {
            String query = @"SELECT COUNT(*)
                             FROM fakturpenjualandetail A
                             WHERE A.fakturpenjualan = @fakturpenjualan";

            Dictionary<String, String> parameters = new Dictionary<string, string>();

            parameters.Add("fakturpenjualan", this.kode);

            Double dblJumlahDetailBarang = Double.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlahDetailBarang <= 0) {
                throw new Exception("Jumlah Item detail harus lebih dari 0");
            }
        }

        public void valReturPenjualan() {
            // cek sudah ada retur penjualan atau blm
            if(double.Parse(this.totalretur) > 0) {
                throw new Exception("Retur Penjualan telah di lakukan");
            }
        }

        public void valPenerimaanPenjualan() {
            // cek sudah ada penerimaan penjualan atau belum
            if(double.Parse(this.totalbayar) > 0) {
                throw new Exception("Penerimaan Penjualan telah di lakukan");
            }
        }

        private void valDetail() {
            if(double.Parse(this.grandtotal) <= 0) {
                throw new Exception("Grand Total harus > 0");
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

        private void valDetailPajakGunggung() {
            if(this.masapajak != "") {
                if(!OswDate.isTahunBulan(this.masapajak)) {
                    throw new Exception("Masa Pajak harus valid (yyyymm), untuk faktur [" + this.kode + "]");
                }
            }
        }

        private void valTanggal() {
            if(this.jenispesananpenjualan == Constants.JENIS_PESANAN_PENJUALAN_PESANAN_PENJUALAN) {
                DataPesananPenjualan dPesananPenjualan = new DataPesananPenjualan(command, this.pesananpenjualan);

                if(OswDate.getDateTimeFromStringTanggal(this.tanggal) < OswDate.getDateTimeFromStringTanggal(dPesananPenjualan.tanggal)) {
                    throw new Exception("Tanggal Faktur Penjualan harus >= Tanggal Pesanan Penjualan [" + this.pesananpenjualan + " - " + dPesananPenjualan.tanggal + "]");
                }
            } else if(this.pesananpenjualan == Constants.JENIS_PESANAN_PENJUALAN_BACK_ORDER) {
                DataBackOrder dBackOrder = new DataBackOrder(command, this.pesananpenjualan);

                if(OswDate.getDateTimeFromStringTanggal(this.tanggal) < OswDate.getDateTimeFromStringTanggal(dBackOrder.tanggal)) {
                    throw new Exception("Tanggal Faktur Penjualan harus >= Tanggal Back Order Penjualan [" + this.pesananpenjualan + " - " + dBackOrder.tanggal + "]");
                }
            }

        }

        private void valGunggung() {
            if(this.nofakturpajak != "" && this.gunggung == Constants.STATUS_YA){
                throw new Exception("No Faktur Pajak telah diisi.");
            }
        }

        public void prosesJurnal() {
            /*
            Deskripsi : Faktur Penjualan ke [NAMA CUSTOMER]																													
            Status		Akun									    Nilai
            Debet		akun_piutang						grand total
            Debet		diskon_penjualan			        totaldiskon
            Kredit		pajak_keluaran					    totalppn
            Kredit		penjualan				            grandtotal + diskon - ppn
            Debet		akun_HPP						    Total HPP (HPP * Jumlah)
            Kredit		akun_persediaan					    Total HPP (HPP * Jumlah)
            */

            int no = 1;
            DataCustomer dCustomer = new DataCustomer(command, this.customer);
            String strngKeteranganJurnal = "Invoice Penjualan ke [" + dCustomer.nama + "]";

            // Debet		akun_piutang						grand total
            String strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PIUTANG);
            DataAkun dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Piutang [" + strngAkun + "] tidak ditemukan.");
            }

            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, this.grandtotal, "0");
            dJurnal.prosesJurnal();

            // Debet		diskon_penjualan			        totaldiskon
            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_DISKON_PENJUALAN);
            dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Diskon Penjualan [" + strngAkun + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, this.totaldiskon, "0");
            dJurnal.prosesJurnal();

            // Kredit		pajak_keluaran					    totalppn
            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PAJAK_KELUARAN);
            dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Pajak Keluaran [" + strngAkun + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", this.totalppn);
            dJurnal.prosesJurnal();

            // Kredit		penjualan				            grandtotal + diskon - ppn
            double dblPenjualan = Tools.getRoundMoney(double.Parse(this.grandtotal) + double.Parse(this.totaldiskon) - double.Parse(totalppn));
            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PENJUALAN);
            dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Pajak Keluaran [" + strngAkun + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", dblPenjualan.ToString());
            dJurnal.prosesJurnal();

            // Debet		akun_HPP						    Total HPP
            String query = @"SELECT SUM(jumlahfaktur * hpp) 
                            FROM fakturpenjualandetail 
                            WHERE fakturpenjualan = @fakturpenjualan";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("fakturpenjualan", this.kode);

            double dblHPP = Tools.getRoundMoney(double.Parse(OswDataAccess.executeScalarQuery(query, parameters, command)));

            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_HPP);
            dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun HPP [" + strngAkun + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, dblHPP.ToString(), "0");
            dJurnal.prosesJurnal();

            // Kredit		akun_persediaan					    Total HPP dari masing akun sejenis
            strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PERSEDIAAN);
            dAkun = new DataAkun(command, strngAkun);
            if(!dAkun.isExist) {
                throw new Exception("Akun Persediaan [" + strngAkun + "] tidak ditemukan.");
            }

            dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", dblHPP.ToString());
            dJurnal.prosesJurnal();

            Tools.cekJurnalBalance(command, this.id, this.kode);
        }

        public void updateMutasi() {
            // validasi
            valPenerimaanPenjualan();
            valReturPenjualan();

            // mutasi piutang
            DataMutasiPiutang dMutasiPiutang = new DataMutasiPiutang(command, this.id, this.kode);
            dMutasiPiutang.no = "1";
            dMutasiPiutang.tanggal = this.tanggal;
            dMutasiPiutang.customer = this.customer;
            dMutasiPiutang.jumlah = this.grandtotal;
            dMutasiPiutang.proses();
        }

        public void ubahFakturPajak() {
            // validasi
            valExist();
            valVersion();
            valDetailPajak();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE fakturpenjualan
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

        public void ubahFakturPajakGunggung() {
            // validasi
            valExist();
            valVersion();
            valDetailPajakGunggung();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE fakturpenjualan
                             SET masapajak = @masapajak, 
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("masapajak", this.masapajak);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }
    }
}

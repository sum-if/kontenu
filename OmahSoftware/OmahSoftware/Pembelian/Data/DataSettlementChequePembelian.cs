using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using OmahSoftware.OswLib;
using OmahSoftware.Akuntansi;

namespace OmahSoftware.Pembelian {
    class DataSettlementChequePembelian {
        private String id = "SETTLEMENTCHEQUEPEMBELIAN";
        public String kode = "";
        public String tanggal = "";
        public String supplier = "";
        public String pembayaranpembelian = "";
        public String jenis = "";
        public String akundeposit = "";
        public String akuncheque = "";
        public String catatan = "";
        public String nourut = "0";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";

            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Supplier:" + supplier + ";";
            kolom += "Penerimaan Penjualan:" + pembayaranpembelian + ";";
            kolom += "Jenis:" + jenis + ";";
            kolom += "Akun Deposit:" + akundeposit + ";";
            kolom += "Akun Cheque:" + akuncheque + ";";
            kolom += "Catatan:" + catatan + ";";
            kolom += "No Urut:" + nourut + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataSettlementChequePembelian(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;

            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT kode, tanggal, supplier, pembayaranpembelian pembayaranpembelian, jenis, akundeposit, akuncheque, catatan, nourut, version
                             FROM settlementchequepembelian 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;

                this.tanggal = reader.GetString("tanggal");
                this.supplier = reader.GetString("supplier");
                this.pembayaranpembelian = reader.GetString("pembayaranpembelian");
                this.jenis = reader.GetString("jenis");
                this.akundeposit = reader.GetString("akundeposit");
                this.akuncheque = reader.GetString("akuncheque");
                this.catatan = reader.GetString("catatan");
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

            DataSettlementChequePembelian dSettlementChequePembelian = new DataSettlementChequePembelian(command, kode);
            if(dSettlementChequePembelian.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public void tambah() {
            // validasi
            valNotExist();

            this.kode = this.kode == "" ? this.generateKode() : this.kode;
            this.version += 1;

            this.prosesJurnal();
            this.updateMutasi();

            String query = @"INSERT INTO settlementchequepembelian( kode, tanggal, supplier, pembayaranpembelian,
                                    jenis, akundeposit, akuncheque, catatan, nourut, version, create_user) 
                             VALUES(@kode, @tanggal, @supplier, @pembayaranpembelian, 
                                    @jenis, @akundeposit, @akuncheque, @catatan, @nourut, @version, @create_user)";

            Dictionary<String, object> parameters = new Dictionary<String, object>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("supplier", this.supplier);
            parameters.Add("pembayaranpembelian", this.pembayaranpembelian);
            parameters.Add("jenis", this.jenis);
            parameters.Add("akundeposit", this.akundeposit);
            parameters.Add("akuncheque", this.akuncheque);
            parameters.Add("catatan", this.catatan);
            parameters.Add("nourut", this.nourut);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();
            valVersion();
            valStatus();

            this.hapusMutasi();

            String query = @"DELETE FROM settlementchequepembelian
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();

            this.version += 1;

            this.hapusMutasi();
            this.prosesJurnal();
            this.updateMutasi();

            // proses ubah
            String query = @"UPDATE settlementchequepembelian
                             SET tanggal = @tanggal,
                                 supplier = @supplier,
                                 pembayaranpembelian = @pembayaranpembelian,
                                 jenis = @jenis,
                                 akundeposit = @akundeposit,
                                 akuncheque = @akuncheque,
                                 catatan = @catatan,
                                 nourut = @nourut,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, object> parameters = new Dictionary<String, object>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("supplier", this.supplier);
            parameters.Add("pembayaranpembelian", this.pembayaranpembelian);
            parameters.Add("jenis", this.jenis);
            parameters.Add("akundeposit", this.akundeposit);
            parameters.Add("akuncheque", this.akuncheque);
            parameters.Add("catatan", this.catatan);
            parameters.Add("nourut", this.nourut);
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
            DataSettlementChequePembelian dSettlementChequePembelian = new DataSettlementChequePembelian(command, this.kode);
            if(this.version != dSettlementChequePembelian.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        private void valStatus() {
            
            if(this.jenis == Constants.JENIS_SETTLEMENT_CHEQUE_TOLAK) {
                throw new Exception("Status Settlement Cheque Tolak (Tidak Bisa dihapus)");
            }
        }

        public void prosesJurnal() {
            int no = 1;

            if(this.jenis == Constants.JENIS_SETTLEMENT_CHEQUE_TERIMA) {
                /*
                Deskripsi : Settlement Cheque [NAMA CUSTOMER]																													
	            Status		Akun									    Nilai
	            Debet		form.akun_deposit_ke						Total dari form
                Kredit		form.akun_cheque							Total dari form
                */
                DataSupplier dSupplier = new DataSupplier(command, this.supplier);
                DataPembayaranPembelian dPembayaranPembelian = new DataPembayaranPembelian(command, this.pembayaranpembelian);
                String strngKeteranganJurnal = "Settlement Cheque [" + dSupplier.nama + "]";

                // Debet		form.akun_deposit_ke						Total dari form
                DataAkun dAkun = new DataAkun(command, this.akundeposit);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Deposit [" + this.akundeposit + "] tidak ditemukan.");
                }

                DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), this.akundeposit, dPembayaranPembelian.total, "0");
                dJurnal.prosesJurnal();

                // Kredit		form.akun_cheque							Total dari form
                dAkun = new DataAkun(command, this.akuncheque);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Post Dated Cheque [" + this.akuncheque + "] tidak ditemukan.");
                }

                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), this.akuncheque, "0", dPembayaranPembelian.total);
                dJurnal.prosesJurnal();
            } else if(this.jenis == Constants.JENIS_SETTLEMENT_CHEQUE_TOLAK) {
                /*
                Deskripsi : Pembatalan Cheque [NAMA CUSTOMER]																													
	            Status		Akun									    Nilai
                Debit		akun_hutang						            Total dari form
	            Kredit		form.akun_cheque							Total dari form
                */
                DataSupplier dSupplier = new DataSupplier(command, this.supplier);
                DataPembayaranPembelian dPembayaranPembelian = new DataPembayaranPembelian(command, this.pembayaranpembelian);
                String strngKeteranganJurnal = "Pembatalan Cheque [" + dSupplier.nama + "]";

                // Debit		akun_hutang						        Total dari form
                String strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PIUTANG);
                DataAkun dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Hutang [" + strngAkun + "] tidak ditemukan.");
                }

                DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, dPembayaranPembelian.total, "0");
                dJurnal.prosesJurnal();

                // Kredit		form.akun_cheque							Total dari form
                dAkun = new DataAkun(command, this.akuncheque);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Post Dated Cheque [" + this.akuncheque + "] tidak ditemukan.");
                }

                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), this.akuncheque, "0", dPembayaranPembelian.total);
                dJurnal.prosesJurnal();
            }

            Tools.cekJurnalBalance(command, this.id, this.kode);
        }

        private void updateMutasi() {
            // mutasi hutang
            if(this.jenis == Constants.JENIS_SETTLEMENT_CHEQUE_TOLAK) {
                DataPembayaranPembelian dPembayaranPembelian = new DataPembayaranPembelian(command, this.pembayaranpembelian);
                DataMutasiHutang dMutasiHutang = new DataMutasiHutang(command, this.id, this.kode);
                dMutasiHutang.no = "1";
                dMutasiHutang.tanggal = this.tanggal;
                dMutasiHutang.supplier = this.supplier;
                String mutasi = Tools.getRoundMoney((double.Parse(dPembayaranPembelian.total) + double.Parse(dPembayaranPembelian.totalselisih))).ToString();
                dMutasiHutang.jumlah = mutasi;
                dMutasiHutang.proses();

                dPembayaranPembelian.kurangTotalBayarCekDitolak();
            }
        }

        private void hapusMutasi() {
            // hapus jurnal
            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode);
            dJurnal.hapusJurnal();

            DataMutasiHutang dMutasiHutang = new DataMutasiHutang(command, this.id, this.kode);
            dMutasiHutang.hapus();

            DataPembayaranPembelian dPembayaranPembelian = new DataPembayaranPembelian(command, this.pembayaranpembelian);

            if(this.jenis == Constants.JENIS_SETTLEMENT_CHEQUE_TOLAK) {
                // tambah total bayar di faktur penjualan
                dPembayaranPembelian.tambahTotalBayarCekDitolak();
            }

            // kembalikan status cheque ke "Menunggu"
            dPembayaranPembelian.statuscek = Constants.STATUS_CEK_MENUNGGU;
            dPembayaranPembelian.ubahStatusCek();
        }
    }
}

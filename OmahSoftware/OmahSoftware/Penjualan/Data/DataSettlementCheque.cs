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

namespace OmahSoftware.Penjualan {
    class DataSettlementCheque {
        private String id = "SETTLEMENTCHEQUE";
        public String kode = "";
        public String tanggal = "";
        public String customer = "";
        public String penerimaanpenjualan = "";
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
            kolom += "Customer:" + customer + ";";
            kolom += "Penerimaan Penjualan:" + penerimaanpenjualan + ";";
            kolom += "Jenis:" + jenis + ";";
            kolom += "Akun Deposit:" + akundeposit + ";";
            kolom += "Akun Cheque:" + akuncheque + ";";
            kolom += "Catatan:" + catatan + ";";
            kolom += "No Urut:" + nourut + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataSettlementCheque(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;

            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT kode, tanggal, customer, penerimaanpenjualan penerimaanpenjualan, jenis, akundeposit, akuncheque, catatan, nourut, version
                             FROM settlementchecque 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;

                this.tanggal = reader.GetString("tanggal");
                this.customer = reader.GetString("customer");
                this.penerimaanpenjualan = reader.GetString("penerimaanpenjualan");
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

            DataSettlementCheque dSettlementCheque = new DataSettlementCheque(command, kode);
            if(dSettlementCheque.isExist) {
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

            String query = @"INSERT INTO settlementchecque( kode, tanggal, customer, penerimaanpenjualan,
                                    jenis, akundeposit, akuncheque, catatan, nourut, version, create_user) 
                             VALUES(@kode, @tanggal, @customer, @penerimaanpenjualan, 
                                    @jenis, @akundeposit, @akuncheque, @catatan, @nourut, @version, @create_user)";

            Dictionary<String, object> parameters = new Dictionary<String, object>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("customer", this.customer);
            parameters.Add("penerimaanpenjualan", this.penerimaanpenjualan);
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

            String query = @"DELETE FROM settlementchecque
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
            String query = @"UPDATE settlementchecque
                             SET tanggal = @tanggal,
                                 customer = @customer,
                                 penerimaanpenjualan = @penerimaanpenjualan,
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
            parameters.Add("customer", this.customer);
            parameters.Add("penerimaanpenjualan", this.penerimaanpenjualan);
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
            DataSettlementCheque dSettlementCheque = new DataSettlementCheque(command, this.kode);
            if(this.version != dSettlementCheque.version) {
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
                DataCustomer dCustomer = new DataCustomer(command, this.customer);
                DataPenerimaanPenjualan dPenerimaanPenjualan = new DataPenerimaanPenjualan(command, this.penerimaanpenjualan);
                String strngKeteranganJurnal = "Settlement Cheque [" + dCustomer.nama + "]";

                // Debet		form.akun_deposit_ke						Total dari form
                DataAkun dAkun = new DataAkun(command, this.akundeposit);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Deposit [" + this.akundeposit + "] tidak ditemukan.");
                }

                DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), this.akundeposit, dPenerimaanPenjualan.total, "0");
                dJurnal.prosesJurnal();

                // Kredit		form.akun_cheque							Total dari form
                dAkun = new DataAkun(command, this.akuncheque);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Post Dated Cheque [" + this.akuncheque + "] tidak ditemukan.");
                }

                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), this.akuncheque, "0", dPenerimaanPenjualan.total);
                dJurnal.prosesJurnal();
            } else if(this.jenis == Constants.JENIS_SETTLEMENT_CHEQUE_TOLAK) {
                /*
                Deskripsi : Pembatalan Cheque [NAMA CUSTOMER]																													
	            Status		Akun									    Nilai
                Debit		akun_piutang						        Total dari form
	            Kredit		form.akun_cheque							Total dari form
                */
                DataCustomer dCustomer = new DataCustomer(command, this.customer);
                DataPenerimaanPenjualan dPenerimaanPenjualan = new DataPenerimaanPenjualan(command, this.penerimaanpenjualan);
                String strngKeteranganJurnal = "Pembatalan Cheque [" + dCustomer.nama + "]";

                String mutasi = Tools.getRoundMoney((double.Parse(dPenerimaanPenjualan.total) + double.Parse(dPenerimaanPenjualan.totalselisih))).ToString();

                // Debit		akun_piutang						        Total dari form
                String strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PIUTANG);
                DataAkun dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Piutang [" + strngAkun + "] tidak ditemukan.");
                }

                DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, mutasi, "0");
                dJurnal.prosesJurnal();

                // Kredit		form.akun_cheque							Total dari form
                dAkun = new DataAkun(command, this.akuncheque);
                if(!dAkun.isExist) {
                    throw new Exception("Akun Post Dated Cheque [" + this.akuncheque + "] tidak ditemukan.");
                }

                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), this.akuncheque, "0", dPenerimaanPenjualan.total);
                dJurnal.prosesJurnal();

                // Kredit		pembulatan							Total dari form
                if(double.Parse(dPenerimaanPenjualan.totalselisih) > 0) {
                    strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PEMBULATAN_BIAYA);
                    dAkun = new DataAkun(command, strngAkun);
                    if(!dAkun.isExist) {
                        throw new Exception("Akun Pembulatan Biaya [" + strngAkun + "] tidak ditemukan.");
                    }

                    dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", dPenerimaanPenjualan.totalselisih);
                    dJurnal.prosesJurnal();
                } else if(double.Parse(dPenerimaanPenjualan.totalselisih) < 0) {
                    strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_PEMBULATAN_PENDAPATAN);
                    dAkun = new DataAkun(command, strngAkun);
                    if(!dAkun.isExist) {
                        throw new Exception("Akun Pembulatan Pendapatan [" + strngAkun + "] tidak ditemukan.");
                    }

                    double dblTotalSelisih = double.Parse(dPenerimaanPenjualan.totalselisih) * -1;
                    dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, dblTotalSelisih.ToString(), "0");
                    dJurnal.prosesJurnal();
                }
            }

            Tools.cekJurnalBalance(command, this.id, this.kode);
        }

        private void updateMutasi() {
            // mutasi piutang
            if(this.jenis == Constants.JENIS_SETTLEMENT_CHEQUE_TOLAK) {
                DataPenerimaanPenjualan dPenerimaanPenjualan = new DataPenerimaanPenjualan(command, this.penerimaanpenjualan);
                DataMutasiPiutang dMutasiPiutang = new DataMutasiPiutang(command, this.id, this.kode);
                dMutasiPiutang.no = "1";
                dMutasiPiutang.tanggal = this.tanggal;
                dMutasiPiutang.customer = this.customer;
                String mutasi = Tools.getRoundMoney((double.Parse(dPenerimaanPenjualan.total) + double.Parse(dPenerimaanPenjualan.totalselisih))).ToString();
                dMutasiPiutang.jumlah = mutasi;
                dMutasiPiutang.proses();

                dPenerimaanPenjualan.kurangTotalBayarCekDitolak();
            }
        }

        private void hapusMutasi() {
            // hapus jurnal
            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode);
            dJurnal.hapusJurnal();

            DataMutasiPiutang dMutasiPiutang = new DataMutasiPiutang(command, this.id, this.kode);
            dMutasiPiutang.hapus();

            DataPenerimaanPenjualan dPenerimaanPenjualan = new DataPenerimaanPenjualan(command, this.penerimaanpenjualan);

            if(this.jenis == Constants.JENIS_SETTLEMENT_CHEQUE_TOLAK) {
                // tambah total bayar di faktur penjualan
                dPenerimaanPenjualan.tambahTotalBayarCekDitolak();
            }

            // kembalikan status cheque ke "Menunggu"
            dPenerimaanPenjualan.statuscek = Constants.STATUS_CEK_MENUNGGU;
            dPenerimaanPenjualan.ubahStatusCek();
        }
    }
}

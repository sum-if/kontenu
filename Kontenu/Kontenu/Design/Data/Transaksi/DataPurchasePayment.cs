using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using Kontenu.OswLib;
using Kontenu.Umum;
using Kontenu.Akuntansi;

namespace Kontenu.Design {
    class DataPurchasePayment {
        private String id = "PURCHASEPAYMENT";
        public String kode = "";
        public String tanggal = "";
        public String outsource = "";
        public String akunkas = "";
        public String grandtotal = "0";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Outsource:" + outsource + ";";
            kolom += "Akun Kas:" + akunkas + ";";
            kolom += "Grand Total:" + grandtotal + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataPurchasePayment(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal, outsource, akunkas,grandtotal, version
                             FROM purchasepayment 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.outsource = reader.GetString("outsource");
                this.akunkas = reader.GetString("akunkas");
                this.grandtotal = reader.GetString("grandtotal");
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

            DataPurchasePayment dPurchasePayment = new DataPurchasePayment(command, kode);
            if(dPurchasePayment.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public void tambah() {
            // validasi
            valNotExist();

            this.kode = this.generateKode();
            this.version += 1;

            String query = @"INSERT INTO purchasepayment(kode, tanggal, outsource, akunkas,grandtotal, version,create_user) 
                             VALUES(@kode,@tanggal,@outsource, @akunkas,@grandtotal, @version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("outsource", this.outsource);
            parameters.Add("akunkas", this.akunkas);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            // hapus detail
            this.hapusDetail();

            // hapus header
            String query = @"DELETE FROM purchasepayment
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapusDetail() {
            String query = @"SELECT no
                             FROM purchasepaymentdetail
                             WHERE purchasepayment = @purchasepayment";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("purchasepayment", this.kode);

            // buat penampung data itemno
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("no");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngNo = reader.GetString("no");
                dataTable.Rows.Add(strngNo);
            }
            reader.Close();

            // loop per detail lalu hapus
            foreach(DataRow row in dataTable.Rows) {
                DataPurchasePaymentDetail dPurchasePaymentDetail = new DataPurchasePaymentDetail(command, this.kode, row["no"].ToString());
                dPurchasePaymentDetail.hapus();
            }
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE purchasepayment
                             SET tanggal = @tanggal,
                                 outsource = @outsource, 
                                 akunkas = @akunkas, 
                                 grandtotal = @grandtotal,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("outsource", this.outsource);
            parameters.Add("akunkas", this.akunkas);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void prosesJurnal()
        {
            ///*
            //PurchasePayment [NAMA PROJECT] dari [NAMA CLIENT]																																				
            //No	Status				Akun						Nilai														Contoh di atas											
            //1	Debet				konstanta.design_akunpiutang						grand total														 9.180.000
            //2	Kredit				konstanta.design_akununearned						grand total														 9.180.000
            //*/

            //int no = 1;
            //DataKlien dKlien = new DataKlien(command, this.klien);
            //DataProyek dProyek = new DataProyek(command, this.akunkas);

            //String strngKeteranganJurnal = "PurchasePayment ["+ dProyek.nama +"] dari ["+ dKlien.nama +"]";

            //// Debet				konstanta.design_akunpiutang						grand total														 9.180.000
            //String strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_DESIGN_AKUN_PIUTANG);
            //DataAkun dAkun = new DataAkun(command, strngAkun);
            //if (!dAkun.isExist)
            //{
            //    throw new Exception("Akun Design Piutang [" + strngAkun + "] tidak ditemukan.");
            //}

            //DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, this.grandtotal, "0");
            //dJurnal.prosesJurnal();

            //// Kredit				konstanta.design_akununearned						grand total														 9.180.000
            //strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_DESIGN_AKUN_EARNED);
            //dAkun = new DataAkun(command, strngAkun);
            //if (!dAkun.isExist)
            //{
            //    throw new Exception("Akun Design Unearned [" + strngAkun + "] tidak ditemukan.");
            //}

            //dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", this.grandtotal);
            //dJurnal.prosesJurnal();

            //Tools.cekJurnalBalance(command, this.id, this.kode);
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
            DataPurchasePayment dPurchasePayment = new DataPurchasePayment(command, this.kode);
            if(this.version != dPurchasePayment.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        public void valJumlahDetail() {
            String query = @"SELECT COUNT(*)
                             FROM purchasepaymentdetail A
                             WHERE A.purchasepayment = @purchasepayment";

            Dictionary<String, String> parameters = new Dictionary<string, string>();
            parameters.Add("purchasepayment", this.kode);

            decimal dblJumlahDetailBarang = decimal.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlahDetailBarang <= 0) {
                throw new Exception("Jumlah Item detail harus lebih dari 0");
            }
        }

        private void valDetail() {
            if(decimal.Parse(this.grandtotal) <= 0) {
                throw new Exception("Grand Total harus > 0");
            }
        }
    }
}

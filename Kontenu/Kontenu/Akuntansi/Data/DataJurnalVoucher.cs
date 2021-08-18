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

namespace Kontenu.Akuntansi {
    class DataJurnalVoucher {
        private String id = "JURNALVOUCHER";
        
        public String kode = "";
        public String tanggal = "";
        public String catatan = "";
        public String totaldebit = "0";
        public String totalkredit = "0";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            
            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Catatan:" + catatan + ";";
            kolom += "Total Debit:" + totaldebit + ";";
            kolom += "Total Kredit:" + totalkredit + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataJurnalVoucher(MySqlCommand command, String kode) {
            this.command = command;
            
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal, catatan, totaldebit, totalkredit, version
                             FROM jurnalvoucher 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.catatan = reader.GetString("catatan");
                this.totaldebit = reader.GetString("totaldebit");
                this.totalkredit = reader.GetString("totalkredit");
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

            DataJurnalVoucher dJurnalVoucher = new DataJurnalVoucher(command, kode);
            if(dJurnalVoucher.isExist) {
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

            String query = @"INSERT INTO jurnalvoucher(kode, tanggal, catatan, totaldebit, totalkredit,version,create_user) 
                             VALUES(@kode,@tanggal,@catatan,@totaldebit, @totalkredit,@version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("catatan", this.catatan);
            parameters.Add("totaldebit", this.totaldebit);
            parameters.Add("totalkredit", this.totalkredit);
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
            String query = @"DELETE FROM jurnalvoucher
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

            String query = @"SELECT no
                             FROM jurnalvoucherdetail
                             WHERE jurnalvoucher = @jurnalvoucher";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("jurnalvoucher", this.kode);

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
                DataJurnalVoucherDetail dJurnalVoucherDetail = new DataJurnalVoucherDetail(command, this.kode, row["no"].ToString());
                dJurnalVoucherDetail.hapus();
            }

        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();
            Tools.valAdmin(command, this.tanggal);

            this.version += 1;

            // proses ubah
            String query = @"UPDATE jurnalvoucher
                             SET tanggal = @tanggal,
                                 catatan = @catatan, 
                                 totaldebit = @totaldebit,
                                 totalkredit = @totalkredit,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("catatan", this.catatan);
            parameters.Add("totaldebit", this.totaldebit);
            parameters.Add("totalkredit", this.totalkredit);
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
            DataJurnalVoucher dJurnalVoucher = new DataJurnalVoucher(command, this.kode);
            if(this.version != dJurnalVoucher.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        public void valJumlahDetail() {
            String query = @"SELECT COUNT(*)
                             FROM jurnalvoucherdetail A
                             WHERE A.jurnalvoucher = @jurnalvoucher";

            Dictionary<String, String> parameters = new Dictionary<string, string>();
            
            parameters.Add("jurnalvoucher", this.kode);

            decimal dblJumlahDetail = decimal.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlahDetail <= 0) {
                throw new Exception("Jumlah Item detail harus lebih dari 0");
            }
        }

        public void valBalance() {
            // cek apakah jumlah total kredit dan debit sama
            decimal dblTotalDebit = decimal.Parse(this.totaldebit);
            decimal dblTotalKredit = decimal.Parse(this.totalkredit);

            if(dblTotalDebit != dblTotalKredit) {
                throw new Exception("Jurnal Voucher Harus Balance");
            }

            // cek apakah ada yang kredit dan debitnya 0
            String query = @"SELECT COUNT(*)
                             FROM jurnalvoucherdetail A
                             WHERE A.jurnalvoucher = @jurnalvoucher AND A.debit = 0 AND A.kredit = 0";

            Dictionary<String, String> parameters = new Dictionary<string, string>();
            
            parameters.Add("jurnalvoucher", this.kode);

            decimal dblJumlahDetail = decimal.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlahDetail > 0) {
                throw new Exception("Terdapat Item detail yang debit dan kreditnya = 0");
            }
        }

        public void prosesJurnal() {
            int no = 1;

            // grupkan untuk akun yang kembar2
            String query = @"SELECT A.akun, A.keterangan, A.debit,A.kredit
                             FROM jurnalvoucherdetail A
                             WHERE jurnalvoucher = @jurnalvoucher";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("jurnalvoucher", this.kode);

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("akun");
            dataTable.Columns.Add("keterangan");
            dataTable.Columns.Add("debit");
            dataTable.Columns.Add("kredit");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngAkun = reader.GetString("akun");
                String strngKeterangan = reader.GetString("keterangan");
                String strngDebit = reader.GetString("debit");
                String strngKredit = reader.GetString("kredit");
                dataTable.Rows.Add(strngAkun, strngKeterangan, strngDebit, strngKredit);
            }
            reader.Close();

            foreach(DataRow row in dataTable.Rows) {
                String strngAkun = row["akun"].ToString();
                String strngKeterangan = row["keterangan"].ToString();
                String strngDebit = row["debit"].ToString();
                String strngKredit = row["kredit"].ToString();

                DataAkun dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun [" + strngAkun + "] tidak ditemukan.");
                }

                String keterangan = strngKeterangan;

                DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, keterangan, (no++).ToString(), strngAkun, strngDebit, strngKredit);
                dJurnal.prosesJurnal();
            }

            Tools.cekJurnalBalance(command, this.id, this.kode);
        }
    }
}

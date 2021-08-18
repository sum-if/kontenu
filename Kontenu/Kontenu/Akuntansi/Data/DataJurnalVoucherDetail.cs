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
using Kontenu.Pembelian;
using Kontenu.Penjualan;

namespace Kontenu.Akuntansi {
    class DataJurnalVoucherDetail {
        private String jurnalvoucher = "";
        private String no = "0";
        public String akun = "";
        public String keterangan = "";
        public String debit = "0";
        public String kredit = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Jurnal Voucher:" + jurnalvoucher + ";";
            kolom += "No:" + no + ";";
            kolom += "Akun:" + akun + ";";
            kolom += "Keterangan:" + keterangan + ";";
            kolom += "Debit:" + debit + ";";
            kolom += "Kredit:" + kredit + ";";
            return kolom;
        }

        public DataJurnalVoucherDetail(MySqlCommand command, String jurnalvoucher, String no) {
            this.command = command;

            this.jurnalvoucher = jurnalvoucher;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT akun, keterangan, debit, kredit
                             FROM jurnalvoucherdetail 
                             WHERE jurnalvoucher = @jurnalvoucher AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("jurnalvoucher", this.jurnalvoucher);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.akun = reader.GetString("akun");
                this.keterangan = reader.GetString("keterangan");
                this.debit = reader.GetString("debit");
                this.kredit = reader.GetString("kredit");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        public void tambah() {
            // validasi
            valNotExist();
            valAkun();

            // tambah detail
            String query = @"INSERT INTO jurnalvoucherdetail(jurnalvoucher,no,akun, keterangan, debit, kredit) 
                             VALUES(@jurnalvoucher,@no,@akun, @keterangan,@debit,@kredit)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("jurnalvoucher", this.jurnalvoucher);
            parameters.Add("no", this.no);
            parameters.Add("akun", this.akun);
            parameters.Add("keterangan", this.keterangan);
            parameters.Add("debit", this.debit);
            parameters.Add("kredit", this.kredit);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            // hapus detail
            String query = @"DELETE FROM jurnalvoucherdetail WHERE jurnalvoucher = @jurnalvoucher AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("jurnalvoucher", this.jurnalvoucher);
            parameters.Add("no", this.no);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {

                throw new Exception("Data [" + this.jurnalvoucher + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.jurnalvoucher + " - " + this.no + "] tidak ada");
            }
        }

        private void valAkun() {
            DataAkun dAkun = new DataAkun(command, this.akun);
            if(!dAkun.isExist || dAkun.status == Constants.STATUS_AKTIF_TIDAK) {
                throw new Exception("Akun [" + this.akun + "] tidak ditemukan");
            }
        }
    }
}

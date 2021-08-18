using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using Kontenu.OswLib;

namespace Kontenu.Akuntansi {
    class DataJurnal {
        private String oswjenisdokumen = "";
        private String noreferensi = "";
        private String tanggal = "";
        private String keterangan = "";
        private String no = "0";
        private String akun = "";
        private String debit = "0";
        private String kredit = "0";
        private MySqlCommand command;

        public DataJurnal(MySqlCommand command, String oswjenisdokumen, String noreferensi, String tanggal = "", String keterangan = "", String no = "0", String akun = "", String debit = "0", String kredit = "0") {
            this.command = command;
            this.oswjenisdokumen = oswjenisdokumen;
            this.noreferensi = noreferensi;
            this.no = no;
            this.tanggal = tanggal;
            this.keterangan = keterangan;
            this.akun = akun;
            this.debit = debit;
            this.kredit = kredit;
        }

        public void prosesJurnal() {
            // validasi
            Tools.valAdmin(command, this.tanggal);

            DataAkun dAkun = new DataAkun(command, this.akun);
            if(!dAkun.isExist) {
                throw new Exception("Akun [" + this.akun + "] tidak ditemukan.");
            }

            // jika debit dan kredit = 0, maka tidak usa di jurnal
            this.debit = Tools.getRoundMoney(this.debit);
            this.kredit = Tools.getRoundMoney(this.kredit);

            if(decimal.Parse(this.debit) == 0 && decimal.Parse(this.kredit) == 0) {
                return;
            }

            // set saldo akun aktual
            decimal selisih = 0;
            
            if(dAkun.saldonormal == "Debit") {
                selisih = Tools.getRoundMoney(decimal.Parse(this.debit) - decimal.Parse(this.kredit));
            } else {
                selisih = Tools.getRoundMoney(decimal.Parse(this.kredit) - decimal.Parse(this.debit));
            }

            DataSaldoAkunAktual dSaldoAkunAktual = new DataSaldoAkunAktual(command, this.akun);
            dSaldoAkunAktual.jumlah = Tools.getRoundMoney((decimal.Parse(dSaldoAkunAktual.jumlah) + selisih).ToString());

            if(dSaldoAkunAktual.isExist) {
                dSaldoAkunAktual.ubah();
            } else {
                dSaldoAkunAktual.tambah();
            }

            // tambahkan jurnal yang baru
            String query = @"INSERT INTO jurnal(oswjenisdokumen, noreferensi,tanggal, keterangan, no, akun, debit, kredit, status, jurnalpenutup, tanggalrekonsiliasi, version,create_user) 
                             VALUES(@oswjenisdokumen,@noreferensi,@tanggal,@keterangan,@no,@akun,@debit,@kredit,@status,@jurnalpenutup,  @tanggalrekonsiliasi,@version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);
            parameters.Add("no", this.no);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("keterangan", this.keterangan);
            parameters.Add("akun", this.akun);
            parameters.Add("debit", this.debit);
            parameters.Add("kredit", this.kredit);
            parameters.Add("status", Constants.STATUS_AKTIF);
            parameters.Add("jurnalpenutup", Constants.STATUS_TIDAK);
            parameters.Add("tanggalrekonsiliasi", "");
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapusJurnal() {
            // validasi
            valAdminHapus();

            // set saldo akun aktual --> ambil jurnal2 yang akan di hapus, untuk di kembalikan saldonya
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("akun");
            dataTable.Columns.Add("debit");
            dataTable.Columns.Add("kredit");

            String query = @"SELECT akun,debit,kredit 
                             FROM jurnal 
                             WHERE oswjenisdokumen = @oswjenisdokumen AND noreferensi = @noreferensi AND status = @status";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);
            parameters.Add("status", Constants.STATUS_AKTIF);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                string akunJurnal = reader.GetString("akun");
                decimal debitJurnal = decimal.Parse(reader.GetString("debit"));
                decimal kreditJurnal = decimal.Parse(reader.GetString("kredit"));

                dataTable.Rows.Add(akunJurnal, debitJurnal, kreditJurnal);
            }
            reader.Close();

            foreach(DataRow row in dataTable.Rows) {
                string akunJurnal = row["akun"].ToString();
                decimal debitJurnal = decimal.Parse(row["debit"].ToString());
                decimal kreditJurnal = decimal.Parse(row["kredit"].ToString());

                DataAkun dAkun = new DataAkun(command, akunJurnal);
                if(!dAkun.isExist) {
                    throw new Exception("Akun [" + akunJurnal + "] tidak ditemukan.");
                }

                decimal selisih = 0;

                if(dAkun.saldonormal == "Debit") {
                    selisih = debitJurnal - kreditJurnal;
                } else {
                    selisih = kreditJurnal - debitJurnal;
                }

                DataSaldoAkunAktual dSaldoAkunAktual = new DataSaldoAkunAktual(command, akunJurnal);
                dSaldoAkunAktual.jumlah = (decimal.Parse(dSaldoAkunAktual.jumlah) - selisih).ToString();
                if(dSaldoAkunAktual.isExist) {
                    dSaldoAkunAktual.ubah();
                } else {
                    dSaldoAkunAktual.tambah();
                }

            }

            // jurnal sebelumnya di non aktifkan statusnya
            query = @"UPDATE jurnal
                             SET status = @status,
                                 version = version + 1,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE oswjenisdokumen = @oswjenisdokumen AND noreferensi = @noreferensi AND status = @statuslama";

            parameters = new Dictionary<String, String>();
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);
            parameters.Add("status", Constants.STATUS_AKTIF_TIDAK);
            parameters.Add("statuslama", Constants.STATUS_AKTIF);
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void prosesRekonsiliasi() {
            // validasi
            valRekonsiliasi();

            String query = @"UPDATE jurnal
                             SET tanggalrekonsiliasi = @tanggalrekonsiliasi,
                                 version = version + 1,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE oswjenisdokumen = @oswjenisdokumen AND noreferensi = @noreferensi AND akun = @akun AND status = @status";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);
            parameters.Add("akun", this.akun);
            parameters.Add("status", Constants.STATUS_AKTIF);
            parameters.Add("update_user", OswConstants.KODEUSER);
            parameters.Add("tanggalrekonsiliasi", OswDate.getStringTanggalHariIni());

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapusRekonsiliasi() {
            // validasi
            valRekonsiliasi();

            String query = @"UPDATE jurnal
                             SET tanggalrekonsiliasi = @tanggalrekonsiliasi,
                                 version = version + 1,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE oswjenisdokumen = @oswjenisdokumen AND noreferensi = @noreferensi AND akun = @akun AND status = @status";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);
            parameters.Add("akun", this.akun);
            parameters.Add("status", Constants.STATUS_AKTIF);
            parameters.Add("update_user", OswConstants.KODEUSER);
            parameters.Add("tanggalrekonsiliasi", "");

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valAdminHapus() {
            // cek apakah jurnal sudah proses tutup buku
            String query = @"SELECT tanggal 
                             FROM jurnal 
                             WHERE oswjenisdokumen = @oswjenisdokumen AND noreferensi = @noreferensi";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);

            String tanggalJurnal = OswDataAccess.executeScalarQuery(query, parameters, command);

            // jika pernah di jurnal (untuk ubah)
            if(tanggalJurnal != "") {
                Tools.valAdmin(command, tanggalJurnal);
            }

        }

        private void valRekonsiliasi() {
            // cek apakah ada jurnal
            String query = @"SELECT COUNT(*) 
                             FROM jurnal 
                             WHERE oswjenisdokumen = @oswjenisdokumen AND noreferensi = @noreferensi AND akun = @akun AND status = @status";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("oswjenisdokumen", this.oswjenisdokumen);
            parameters.Add("noreferensi", this.noreferensi);
            parameters.Add("akun", this.akun);
            parameters.Add("status", Constants.STATUS_AKTIF);

            int jumlah = int.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));
            if(jumlah == 0) {
                throw new Exception("Jurnal yang akan di rekonsiliasi tidak ditemukan.");
            }
        }
    }
}

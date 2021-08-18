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
    class DataSaldoAkunAktual {
        public String akun = "";
        public String jumlah = "0";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Akun:" + akun + ";";
            kolom += "Jumlah:" + jumlah + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataSaldoAkunAktual(MySqlCommand command, String akun) {
            this.command = command;
            
            this.akun = akun;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT jumlah,version
                             FROM saldoakunaktual 
                             WHERE akun = @akun";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("akun", this.akun);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.jumlah = reader.GetString("jumlah");
                this.version = reader.GetInt64("version");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        public void tambah() {
            // validasi
            valNotExist();

            if(decimal.Parse(this.jumlah) == 0) {
                if(this.isExist) {
                    this.hapus();
                }
            } else {
                this.version += 1;

                String query = @"INSERT INTO saldoakunaktual(akun,jumlah,version,create_user) 
                             VALUES(@akun,@jumlah,@version,@create_user)";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                
                parameters.Add("akun", this.akun);
                parameters.Add("jumlah", this.jumlah);
                parameters.Add("version", "1");
                parameters.Add("create_user", OswConstants.KODEUSER);

                OswDataAccess.executeVoidQuery(query, parameters, command);
            }
        }

        private void hapus() {
            // validasi
            valExist();
            valVersion();

            String query = @"DELETE FROM saldoakunaktual
                             WHERE akun = @akun";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("akun", this.akun);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();

            // proses ubah
            // jika saldo 0 --> hapus
            if(decimal.Parse(this.jumlah) == 0) {
                this.hapus();
            } else {
                this.version += 1;
                this.version += 1;
                String query = @"UPDATE saldoakunaktual
                             SET jumlah = @jumlah,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE akun = @akun";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                
                parameters.Add("akun", this.akun);
                parameters.Add("jumlah", this.jumlah);
                parameters.Add("version", this.version.ToString());
                parameters.Add("update_user", OswConstants.KODEUSER);

                OswDataAccess.executeVoidQuery(query, parameters, command);
            }
        }

        private void valNotExist() {
            if(this.isExist) {
                DataAkun dAkun = new DataAkun(command, this.akun);
                throw new Exception("Data [" + this.akun + "," + dAkun.nama + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                DataAkun dAkun = new DataAkun(command, this.akun);
                throw new Exception("Data [" + this.akun + "," + dAkun.nama + "] tidak ada");
            }
        }

        private void valVersion() {
            DataSaldoAkunAktual dSaldoAkunAktual = new DataSaldoAkunAktual(command, this.akun);
            if(this.version != dSaldoAkunAktual.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

    }
}

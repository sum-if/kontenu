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

namespace OmahSoftware.Persediaan {
    class DataSaldoPersediaanHPP {
        private String barang = "";
        public String nilai = "0";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Barang:" + barang + ";";
            kolom += "Nilai:" + nilai + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataSaldoPersediaanHPP(MySqlCommand command, String barang) {
            this.command = command;
            this.barang = barang;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT nilai,version
                             FROM saldopersediaanhpp 
                             WHERE barang = @barang";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("barang", this.barang);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.nilai = reader.GetString("nilai");
                this.version = reader.GetInt64("version");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        public void simpan() {
            if(this.isExist) {
                this.ubah();
            } else {
                this.tambah();
            }
        }
        private void tambah() {
            valNotExist();
            valVersion();
            valNilai();

            this.version += 1;

            String query = @"INSERT INTO saldopersediaanhpp(barang,nilai,version,create_user) 
                             VALUES(@barang,@nilai,@version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("barang", this.barang);
            parameters.Add("nilai", this.nilai);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void hapus() {
            valExist();
            valVersion();

            String query = @"DELETE FROM saldopersediaanhpp 
                             WHERE barang = @barang";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("barang", this.barang);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void ubah() {
            valExist();
            valVersion();
            valNilai();

            this.version += 1;

            String query = @"UPDATE saldopersediaanhpp 
                             SET nilai = @nilai,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE barang = @barang";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("barang", this.barang);
            parameters.Add("nilai", this.nilai);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                DataBarang dBarang = new DataBarang(command, this.barang);
                throw new Exception("Data [" + this.barang + "," + dBarang.nama + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                DataBarang dBarang = new DataBarang(command, this.barang);
                throw new Exception("Data [" + this.barang + "," + dBarang.nama + "] tidak ada");
            }
        }

        private void valVersion() {
            DataSaldoPersediaanHPP dSaldoPersediaanHPP = new DataSaldoPersediaanHPP(command, this.barang);
            if(this.version != dSaldoPersediaanHPP.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        private void valNilai() {
            if(double.Parse(this.nilai) <= 0) {
                DataBarang dBarang = new DataBarang(command, this.barang);
                throw new Exception("Nilai HPP [" + this.barang + "," + dBarang.nama + "] tidak boleh <= 0");
            }
        }
    }
}

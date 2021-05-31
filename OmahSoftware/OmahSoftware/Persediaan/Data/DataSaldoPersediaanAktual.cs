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
using OmahSoftware.Sistem;

namespace OmahSoftware.Persediaan {
    class DataSaldoPersediaanAktual {
        public String barang = "";
        public String jumlah = "0";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Barang:" + barang + ";";
            kolom += "Jumlah:" + jumlah + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataSaldoPersediaanAktual(MySqlCommand command, String barang) {
            this.command = command;
            this.barang = barang;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT jumlah,version
                             FROM saldopersediaanaktual 
                             WHERE barang = @barang";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("barang", this.barang);

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
            valVersion();
            
            if(this.isExist) {
                // jika sudah pernah ada, maka ambil jumlah lama lalu tambahkan ke yg sekarang
                DataSaldoPersediaanAktual dSaldoPersediaanAktual = new DataSaldoPersediaanAktual(command, this.barang);
                double jumlahLama = double.Parse(dSaldoPersediaanAktual.jumlah);
                double jumlahBaru = jumlahLama + double.Parse(this.jumlah);

                if(jumlahBaru == 0) {
                    // habis stoknya, maka harus di hapus dari saldo
                    this.hapus();
                } else if(jumlahBaru < 0) {
                    // tidak boleh < 0
                    DataBarang dBarang = new DataBarang(command, this.barang);
                    throw new Exception("Saldo Persediaan Aktual [" + this.barang + "," + dBarang.nama + "] < 0");
                } else {
                    this.version += 1;

                    // masih sisa
                    String query = @"UPDATE saldopersediaanaktual
                                 SET jumlah = @jumlah,
                                     version = @version,
                                     update_at = CURRENT_TIMESTAMP(),
                                     update_user = @update_user
                                 WHERE barang = @barang";

                    Dictionary<String, String> parameters = new Dictionary<String, String>();
                    parameters.Add("barang", this.barang);
                    parameters.Add("jumlah", jumlahBaru.ToString());
                    parameters.Add("version", this.version.ToString());
                    parameters.Add("update_user", OswConstants.KODEUSER);

                    OswDataAccess.executeVoidQuery(query, parameters, command);
                }
            } else {
                if(double.Parse(this.jumlah) > 0) {
                    // jika blm ada langsung di tambahkan saja
                    String query = @"INSERT INTO saldopersediaanaktual(barang,jumlah,version,create_user) 
                             VALUES(@barang,@jumlah,@version,@create_user)";

                    Dictionary<String, String> parameters = new Dictionary<String, String>();
                    parameters.Add("barang", this.barang);
                    parameters.Add("jumlah", this.jumlah);
                    parameters.Add("version", "1");
                    parameters.Add("create_user", OswConstants.KODEUSER);

                    OswDataAccess.executeVoidQuery(query, parameters, command);
                } else if(double.Parse(this.jumlah) == 0) {
                    // tidak diapa2kan
                } else {
                    // tidak boleh < 0
                    DataBarang dBarang = new DataBarang(command, this.barang);
                    throw new Exception("Saldo Persediaan Aktual [" + this.barang + "," + dBarang.nama + "] <= 0");
                }
            }
        }

        public void kurang() {
            valVersion();

            // proses ubah
            // cari jumlah lama, lalu kurangi dengan jumlah sekarang
            DataSaldoPersediaanAktual dSaldoPersediaanAktual = new DataSaldoPersediaanAktual(command, this.barang);
            double jumlahLama = double.Parse(dSaldoPersediaanAktual.jumlah);
            double jumlahBaru = jumlahLama - double.Parse(this.jumlah);

            if(jumlahBaru == 0) {
                // habis stoknya, maka harus di hapus dari saldo
                this.hapus();
            } else if(jumlahBaru < 0) {
                // tidak boleh < 0
                DataBarang dBarang = new DataBarang(command, this.barang);
                throw new Exception("Saldo Persediaan Aktual [" + this.barang + "," + dBarang.nama + "] < 0");
            } else {
                this.version += 1;

                if(!this.isExist) {
                    // jika blm ada langsung di tambahkan saja
                    String query = @"INSERT INTO saldopersediaanaktual(barang,jumlah,version,create_user) 
                             VALUES(@barang,@jumlah,@version,@create_user)";

                    Dictionary<String, String> parameters = new Dictionary<String, String>();
                    parameters.Add("barang", this.barang);
                    parameters.Add("jumlah", jumlahBaru.ToString());
                    parameters.Add("version", "1");
                    parameters.Add("create_user", OswConstants.KODEUSER);

                    OswDataAccess.executeVoidQuery(query, parameters, command);
                } else {
                    // masih sisa
                    String query = @"UPDATE saldopersediaanaktual
                                     SET jumlah = @jumlah,
                                         version = @version,
                                         update_at = CURRENT_TIMESTAMP(),
                                         update_user = @update_user
                                     WHERE barang = @barang";

                    Dictionary<String, String> parameters = new Dictionary<String, String>();
                    parameters.Add("barang", this.barang);
                    parameters.Add("jumlah", jumlahBaru.ToString());
                    parameters.Add("version", this.version.ToString());
                    parameters.Add("update_user", OswConstants.KODEUSER);

                    OswDataAccess.executeVoidQuery(query, parameters, command);
                }
            }
        }

        private void hapus() {
            // validasi
            valExist();
            valVersion();

            String query = @"DELETE FROM saldopersediaanaktual
                             WHERE barang = @barang";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("barang", this.barang);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                DataBarang dBarang = new DataBarang(command, this.barang);
                throw new Exception("Data Saldo Persediaan Aktual [" + this.barang + "," + dBarang.nama + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                DataBarang dBarang = new DataBarang(command, this.barang);
                throw new Exception("Data Saldo Persediaan Aktual [" + this.barang + "," + dBarang.nama + "] tidak ada");
            }
        }

        private void valVersion() {
            DataSaldoPersediaanAktual dSaldoPersediaanAktual = new DataSaldoPersediaanAktual(command, this.barang);
            if(this.version != dSaldoPersediaanAktual.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

    }
}

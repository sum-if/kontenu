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

namespace OmahSoftware.Pembelian {
    class DataSaldoUangTitipanSupplierAktual {
        public String supplier = "";
        public String jumlah = "0";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Supplier:" + supplier + ";";
            kolom += "Jumlah:" + jumlah + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataSaldoUangTitipanSupplierAktual(MySqlCommand command, String supplier) {
            this.command = command;

            this.supplier = supplier;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT jumlah,version
                             FROM saldouangtitipansupplieraktual 
                             WHERE supplier = @supplier";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("supplier", this.supplier);

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
                DataSaldoUangTitipanSupplierAktual dSaldoUangTitipanSupplierAktual = new DataSaldoUangTitipanSupplierAktual(command, this.supplier);
                double jumlahLama = double.Parse(dSaldoUangTitipanSupplierAktual.jumlah);
                double jumlahBaru = jumlahLama + double.Parse(this.jumlah);

                if(jumlahBaru == 0) {
                    // habis uangtitipansuppliernya, maka harus di hapus dari saldo
                    this.hapus();
                } else if(jumlahBaru < 0) {
                    DataSupplier dSupplier = new DataSupplier(command, this.supplier);
                    throw new Exception("Saldo UangTitipanSupplier Aktual [" + this.supplier + "," + dSupplier.nama + "] < 0");
                } else {
                    this.version += 1;

                    // masih sisa
                    String query = @"UPDATE saldouangtitipansupplieraktual
                                     SET jumlah = @jumlah,
                                         version = @version,
                                         update_at = CURRENT_TIMESTAMP(),
                                         update_user = @update_user
                                     WHERE supplier = @supplier";

                    Dictionary<String, String> parameters = new Dictionary<String, String>();

                    parameters.Add("supplier", this.supplier);
                    parameters.Add("jumlah", jumlahBaru.ToString());
                    parameters.Add("version", this.version.ToString());
                    parameters.Add("update_user", OswConstants.KODEUSER);

                    OswDataAccess.executeVoidQuery(query, parameters, command);
                }
            } else {
                if(double.Parse(this.jumlah) > 0) {
                    // jika blm ada langsung di tambahkan saja
                    String query = @"INSERT INTO saldouangtitipansupplieraktual(supplier,jumlah,version,create_user) 
                                     VALUES(@supplier,@jumlah,@version,@create_user)";

                    Dictionary<String, String> parameters = new Dictionary<String, String>();

                    parameters.Add("supplier", this.supplier);
                    parameters.Add("jumlah", this.jumlah);
                    parameters.Add("version", "1");
                    parameters.Add("create_user", OswConstants.KODEUSER);

                    OswDataAccess.executeVoidQuery(query, parameters, command);
                } else {

                    DataSupplier dSupplier = new DataSupplier(command, this.supplier);
                    throw new Exception("Saldo UangTitipanSupplier Aktual [" + this.supplier + "," + dSupplier.nama + "] < 0");
                }
            }
        }

        public void kurang() {
            valVersion();

            // proses ubah
            // cari jumlah lama, lalu kurangi dengan jumlah sekarang
            DataSaldoUangTitipanSupplierAktual dSaldoUangTitipanSupplierAktual = new DataSaldoUangTitipanSupplierAktual(command, this.supplier);
            double jumlahLama = double.Parse(dSaldoUangTitipanSupplierAktual.jumlah);
            double jumlahBaru = jumlahLama - double.Parse(this.jumlah);

            if(jumlahBaru == 0) {
                // habis uangtitipansuppliernya, maka harus di hapus dari saldo
                this.hapus();
            } else if(jumlahBaru < 0) {
                DataSupplier dSupplier = new DataSupplier(command, this.supplier);
                throw new Exception("Saldo Uang Titipan Supplier Aktual [" + this.supplier + "," + dSupplier.nama + "] < 0");
            } else {
                this.version += 1;

                if(this.isExist) {
                    // masih sisa
                    String query = @"UPDATE saldouangtitipansupplieraktual
                                 SET jumlah = @jumlah,
                                     version = @version,
                                     update_at = CURRENT_TIMESTAMP(),
                                     update_user = @update_user
                                 WHERE supplier = @supplier";

                    Dictionary<String, String> parameters = new Dictionary<String, String>();

                    parameters.Add("supplier", this.supplier);
                    parameters.Add("jumlah", jumlahBaru.ToString());
                    parameters.Add("version", this.version.ToString());
                    parameters.Add("update_user", OswConstants.KODEUSER);

                    OswDataAccess.executeVoidQuery(query, parameters, command);
                } else {
                    // jika blm ada langsung di tambahkan saja
                    String query = @"INSERT INTO saldouangtitipansupplieraktual(supplier,jumlah,version,create_user) 
                                     VALUES(@supplier,@jumlah,@version,@create_user)";

                    Dictionary<String, String> parameters = new Dictionary<String, String>();

                    parameters.Add("supplier", this.supplier);
                    parameters.Add("jumlah", jumlahBaru.ToString());
                    parameters.Add("version", "1");
                    parameters.Add("create_user", OswConstants.KODEUSER);

                    OswDataAccess.executeVoidQuery(query, parameters, command);
                }
            }
        }

        private void hapus() {
            // validasi
            valExist();
            valVersion();

            String query = @"DELETE FROM saldouangtitipansupplieraktual
                             WHERE supplier = @supplier";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("supplier", this.supplier);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {

                DataSupplier dSupplier = new DataSupplier(command, this.supplier);
                throw new Exception("Data Saldo Uang Titipan Supplier Aktual [" + this.supplier + "," + dSupplier.nama + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {

                DataSupplier dSupplier = new DataSupplier(command, this.supplier);
                throw new Exception("Data Saldo Uang Titipan Supplier Aktual [" + this.supplier + "," + dSupplier.nama + "] tidak ada");
            }
        }

        private void valVersion() {
            DataSaldoUangTitipanSupplierAktual dSaldoUangTitipanSupplierAktual = new DataSaldoUangTitipanSupplierAktual(command, this.supplier);
            if(this.version != dSaldoUangTitipanSupplierAktual.version) {
                throw new Exception("Data Saldo Uang Titipan Supplier Aktual yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

    }
}

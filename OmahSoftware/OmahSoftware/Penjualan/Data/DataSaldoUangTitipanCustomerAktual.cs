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

namespace OmahSoftware.Penjualan {
    class DataSaldoUangTitipanCustomerAktual {
        public String customer = "";
        public String jumlah = "0";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";

            kolom += "Customer:" + customer + ";";
            kolom += "Jumlah:" + jumlah + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataSaldoUangTitipanCustomerAktual(MySqlCommand command, String customer) {
            this.command = command;

            this.customer = customer;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT jumlah,version
                             FROM saldouangtitipancustomeraktual 
                             WHERE customer = @customer";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("customer", this.customer);

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
                DataSaldoUangTitipanCustomerAktual dSaldoUangTitipanCustomerAktual = new DataSaldoUangTitipanCustomerAktual(command, this.customer);
                double jumlahLama = double.Parse(dSaldoUangTitipanCustomerAktual.jumlah);
                double jumlahBaru = jumlahLama + double.Parse(this.jumlah);

                if(jumlahBaru == 0) {
                    // habis uangtitipancustomernya, maka harus di hapus dari saldo
                    this.hapus();
                } else if(jumlahBaru < 0) {
                    DataCustomer dCustomer = new DataCustomer(command, this.customer);
                    throw new Exception("Saldo UangTitipanCustomer Customer Aktual [" + this.customer + "," + dCustomer.nama + "] < 0");
                } else {
                    this.version += 1;

                    // masih sisa
                    String query = @"UPDATE saldouangtitipancustomeraktual
                                     SET jumlah = @jumlah,
                                         version = @version,
                                         update_at = CURRENT_TIMESTAMP(),
                                         update_user = @update_user
                                     WHERE customer = @customer";

                    Dictionary<String, String> parameters = new Dictionary<String, String>();

                    parameters.Add("customer", this.customer);
                    parameters.Add("jumlah", jumlahBaru.ToString());
                    parameters.Add("version", this.version.ToString());
                    parameters.Add("update_user", OswConstants.KODEUSER);

                    OswDataAccess.executeVoidQuery(query, parameters, command);
                }
            } else {
                if(double.Parse(this.jumlah) > 0) {
                    // jika blm ada langsung di tambahkan saja
                    String query = @"INSERT INTO saldouangtitipancustomeraktual(customer,jumlah,version,create_user) 
                                     VALUES(@customer,@jumlah,@version,@create_user)";

                    Dictionary<String, String> parameters = new Dictionary<String, String>();

                    parameters.Add("customer", this.customer);
                    parameters.Add("jumlah", this.jumlah);
                    parameters.Add("version", "1");
                    parameters.Add("create_user", OswConstants.KODEUSER);

                    OswDataAccess.executeVoidQuery(query, parameters, command);
                } else {
                    DataCustomer dCustomer = new DataCustomer(command, this.customer);
                    throw new Exception("Saldo Uang Titipan Customer Aktual [" + this.customer + "," + dCustomer.nama + "] < 0");
                }
            }
        }

        public void kurang() {
            valVersion();


            // proses ubah
            // cari jumlah lama, lalu kurangi dengan jumlah sekarang
            DataSaldoUangTitipanCustomerAktual dSaldoUangTitipanCustomerAktual = new DataSaldoUangTitipanCustomerAktual(command, this.customer);
            double jumlahLama = double.Parse(dSaldoUangTitipanCustomerAktual.jumlah);
            double jumlahBaru = jumlahLama - double.Parse(this.jumlah);

            if(jumlahBaru == 0) {
                // habis uangtitipancustomernya, maka harus di hapus dari saldo
                this.hapus();
            } else if(jumlahBaru < 0) {
                DataCustomer dCustomer = new DataCustomer(command, this.customer);
                throw new Exception("Saldo Uang Titipan Customer Aktual [" + this.customer + "," + dCustomer.nama + "] < 0");
            } else {
                this.version += 1;

                if(this.isExist) {
                    // masih sisa
                    String query = @"UPDATE saldouangtitipancustomeraktual
                                 SET jumlah = @jumlah,
                                     version = @version,
                                     update_at = CURRENT_TIMESTAMP(),
                                     update_user = @update_user
                                 WHERE customer = @customer";

                    Dictionary<String, String> parameters = new Dictionary<String, String>();

                    parameters.Add("customer", this.customer);
                    parameters.Add("jumlah", jumlahBaru.ToString());
                    parameters.Add("version", this.version.ToString());
                    parameters.Add("update_user", OswConstants.KODEUSER);

                    OswDataAccess.executeVoidQuery(query, parameters, command);
                } else {
                    // jika blm ada langsung di tambahkan saja
                    String query = @"INSERT INTO saldouangtitipancustomeraktual(customer,jumlah,version,create_user) 
                                     VALUES(@customer,@jumlah,@version,@create_user)";

                    Dictionary<String, String> parameters = new Dictionary<String, String>();

                    parameters.Add("customer", this.customer);
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

            String query = @"DELETE FROM saldouangtitipancustomeraktual
                             WHERE customer = @customer";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("customer", this.customer);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                DataCustomer dCustomer = new DataCustomer(command, this.customer);
                throw new Exception("Data Saldo Uang Titipan Customer Aktual [" + this.customer + "," + dCustomer.nama + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                DataCustomer dCustomer = new DataCustomer(command, this.customer);
                throw new Exception("Data Saldo Uang Titipan Customer Aktual [" + this.customer + "," + dCustomer.nama + "] tidak ada");
            }
        }

        private void valVersion() {
            DataSaldoUangTitipanCustomerAktual dSaldoUangTitipanCustomerAktual = new DataSaldoUangTitipanCustomerAktual(command, this.customer);
            if(this.version != dSaldoUangTitipanCustomerAktual.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

    }
}

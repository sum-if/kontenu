using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;

namespace OmahSoftware.Sistem {
    class DataOswUser {
        private String id = "USER";
        private String kode = "";
        public String nama = "";
        public String usergroup = "";
        public String username = "";
        public String password = "";
        
        public String status = "";
        public String lastlogin = "";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Nama:" + nama + ";";
            kolom += "Usergroup:" + usergroup + ";";
            kolom += "Username:" + username + ";";
            kolom += "Password:" + OswConvert.convertToBCrypt(this.password) + ";";
            
            kolom += "Status:" + status + ";";
            kolom += "LastLogin:" + lastlogin + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        private String generateKode() {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("Usergroup", this.usergroup);

            String kode = OswFormatDokumen.generate(command, id, parameters);

            // jika kode kosong, berarti tidak diseting no otomatis, sehingga harus diisi manual
            if(kode == "") {
                if(this.kode == "") {
                    throw new Exception("Kode harus diisi, karena tidak disetting untuk digenerate otomatis");
                } else {
                    kode = this.kode;
                }
            }

            DataOswUser dOswUser = new DataOswUser(command, kode);
            if(dOswUser.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public DataOswUser(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT nama,usergroup,username,password,status,lastlogin,version
                             FROM oswuser 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.nama = reader.GetString("nama");
                this.usergroup = reader.GetString("usergroup");
                this.username = reader.GetString("username");
                this.password = reader.GetString("password");
                
                this.status = reader.GetString("status");
                this.lastlogin = reader.IsDBNull(5) ? null : reader.GetString("lastlogin");
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
            valUsernameTersedia();
            valPasswordNotBlank();

            //this.kode = this.kode == "" ? this.generateKode() : this.kode;
            this.version += 1;

            String query = @"INSERT INTO oswuser(kode,nama,usergroup,username,password,status,lastlogin,version,create_user) 
                             VALUES(@kode,@nama,@usergroup,@username,@password,@status,@lastlogin,@version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);
            parameters.Add("usergroup", this.usergroup);
            parameters.Add("username", this.username);
            parameters.Add("password", this.password);
            
            parameters.Add("status", this.status);
            parameters.Add("lastlogin", this.lastlogin);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();
            valVersion();

            String query = @"DELETE FROM oswuser
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();
            valUsernameTersedia();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE oswuser
                             SET nama = @nama,
                                 usergroup = @usergroup,
                                 username = @username,
                                 password = @password,
                                 status = @status,
                                 lastlogin = @lastlogin,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);
            parameters.Add("usergroup", this.usergroup);
            parameters.Add("username", this.username);
            parameters.Add("password", this.password);
            
            parameters.Add("status", this.status);
            parameters.Add("lastlogin", this.lastlogin);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.kode + "," + this.nama + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.kode + "] tidak ada");
            }
        }

        private void valVersion() {
            // ambil data user yang terbaru
            DataOswUser dOswUser = new DataOswUser(command, this.kode);
            if(this.version != dOswUser.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        private void valPasswordNotBlank() {
            if(this.password == "") {
                throw new Exception("Password harus diisi.");
            }
        }

        private void valUsernameTersedia() {
            String query = "SELECT COUNT(*) FROM oswuser WHERE username = @username AND kode <> @kode";
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("username", this.username);
            parameters.Add("kode", this.kode);

            int username = int.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(username > 0) {
                throw new Exception("Username tidak tersedia");
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;

namespace OmahSoftware.Sistem {
    class DataOswUserGroup {
        private String id = "USERGROUP";
        private String kode = "";
        public String nama = "";
        public String keterangan = "";
        public String status = "";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Nama:" + nama + ";";
            kolom += "Keterangan:" + keterangan + ";";
            kolom += "Status:" + status + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        private String generateKode() {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("Kode", this.kode);

            String kode = OswFormatDokumen.generate(command, id, parameters);

            // jika kode kosong, berarti tidak diseting no otomatis, sehingga harus diisi manual
            if(kode == "") {
                if(this.kode == "") {
                    throw new Exception("Kode harus diisi, karena tidak disetting untuk digenerate otomatis");
                } else {
                    kode = this.kode;
                }
            }

            DataOswUserGroup dOswUserGroup = new DataOswUserGroup(command, kode);
            if(dOswUserGroup.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public DataOswUserGroup(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT nama,keterangan,status,version
                             FROM oswusergroup 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.nama = reader.GetString("nama");
                this.keterangan = reader.GetString("keterangan");
                this.status = reader.GetString("status");
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

            this.kode = this.kode == "" ? this.generateKode() : this.kode;
            this.version += 1;

            String query = @"INSERT INTO oswusergroup(kode,nama,keterangan,status,version,create_user) 
                             VALUES(@kode,@nama,@keterangan,@status,@version,@create_user)";


            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);
            parameters.Add("keterangan", this.keterangan);
            parameters.Add("status", this.status);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();
            valVersion();

            String query = @"DELETE FROM oswusergroup
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE oswusergroup
                             SET nama = @nama,
                                 keterangan = @keterangan,
                                 status = @status,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);
            parameters.Add("keterangan", this.keterangan);
            parameters.Add("status", this.status);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data User Group [" + this.kode + "," + this.nama + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data User Group [" + this.kode + "] tidak ada");
            }
        }

        private void valVersion() {
            // ambil data user yang terbaru
            DataOswUserGroup dOswUserGroup = new DataOswUserGroup(command, this.kode);
            if(this.version != dOswUserGroup.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }



    }
}

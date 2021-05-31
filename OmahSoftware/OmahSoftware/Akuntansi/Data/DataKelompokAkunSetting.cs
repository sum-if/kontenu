using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;

namespace OmahSoftware.Akuntansi {
    class DataKelompokAkunSetting {
        private String kelompokakun = "";
        public String no = "";
        public String kategori = "";
        public String subkategori = "";
        public String group = "";
        public String subgroup = "";
        public String akun = "";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kelompok Akun:" + kelompokakun + ";";
            kolom += "No:" + no + ";";
            kolom += "Kategori:" + kategori + ";";
            kolom += "Sub Kategori:" + subkategori + ";";
            kolom += "Group:" + this.group + ";";
            kolom += "Sub Group:" + subgroup + ";";
            kolom += "Akun:" + akun + ";";
            return kolom;
        }


        public DataKelompokAkunSetting(MySqlCommand command, String kelompokakun, String no) {
            this.command = command;
            this.kelompokakun = kelompokakun;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT A.kategori,A.subkategori,A.group,A.subgroup,A.akun
                             FROM kelompokakunsetting A
                             WHERE kelompokakun = @kelompokakun AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kelompokakun", this.kelompokakun);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.kategori = reader.GetString("kategori");
                this.subkategori = reader.GetString("subkategori");
                this.group = reader.GetString("group");
                this.subgroup = reader.GetString("subgroup");
                this.akun = reader.GetString("akun");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        public void tambah() {
            // validasi
            valNotExist();

            String query = @"INSERT INTO kelompokakunsetting
                             VALUES(@kelompokakun,@no,@kategori,@subkategori,@group,@subgroup,@akun)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kelompokakun", this.kelompokakun);
            parameters.Add("no", this.no);
            parameters.Add("kategori", this.kategori);
            parameters.Add("subkategori", this.subkategori);
            parameters.Add("group", this.group);
            parameters.Add("subgroup", this.subgroup);
            parameters.Add("akun", this.akun);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus(bool hapusHeader = false) {

            String query = "";
            Dictionary<String, String> parameters = null;

            if(hapusHeader) {
                query = @"DELETE FROM kelompokakunsetting
                             WHERE kelompokakun = @kelompokakun";

                parameters = new Dictionary<String, String>();
                parameters.Add("kelompokakun", this.kelompokakun);
            } else {
                // validasi
                valExist();

                query = @"DELETE FROM kelompokakunsetting
                             WHERE kelompokakun = @kelompokakun AND no = @no";

                parameters = new Dictionary<String, String>();
                parameters.Add("kelompokakun", this.kelompokakun);
                parameters.Add("no", this.no);
            }

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubah() {
            // validasi
            valExist();

            // proses ubah
            String query = @"UPDATE kelompokakunsetting
                             SET kategori = @kategori,
                                 subkategori = @subkategori,
                                 group = @group,
                                 subgroup = @subgroup,
                                 akun = @akun
                             WHERE kelompokakun = @kelompokakun AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kelompokakun", this.kelompokakun);
            parameters.Add("no", this.no);
            parameters.Add("kategori", this.kategori);
            parameters.Add("subkategori", this.subkategori);
            parameters.Add("group", this.group);
            parameters.Add("subgroup", this.subgroup);
            parameters.Add("akun", this.akun);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                DataKelompokAkun dKelompokAkun = new DataKelompokAkun(command, this.kelompokakun);
                throw new Exception("Data [" + this.kelompokakun + "" + dKelompokAkun.nama + " - " + this.no + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                DataKelompokAkun dKelompokAkun = new DataKelompokAkun(command, this.kelompokakun);
                throw new Exception("Data[ " + this.kelompokakun + "" + dKelompokAkun.nama + " - " + this.no + "] tidak ada");
            }
        }

    }
}

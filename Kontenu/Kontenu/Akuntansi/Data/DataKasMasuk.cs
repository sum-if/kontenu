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
    class DataKasMasuk {
        private String id = "KASMASUK";
        public String kode = "";
        public String tanggal = "";
        public String catatan = "";
        public String akun = "";
        public String total = "0";
        public String nourut = "0";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Catatan:" + catatan + ";";
            kolom += "Akun:" + akun + ";";
            kolom += "Total:" + total + ";";
            kolom += "No Urut:" + nourut + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataKasMasuk(MySqlCommand command, String kode) {
            this.command = command;

            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal, catatan, akun, total, nourut, version
                             FROM kasmasuk 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.catatan = reader.GetString("catatan");
                this.akun = reader.GetString("akun");
                this.total = reader.GetString("total");
                this.nourut = reader.GetString("nourut");
                this.version = reader.GetInt64("version");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        private String generateKode() {
            String strngTanggalSekarang = this.tanggal;
            String strngTahun = OswDate.getTahun(strngTanggalSekarang);
            String strngTahunDuaDigit = OswDate.getTahunDuaDigit(strngTanggalSekarang);
            String strngBulan = OswDate.getBulan(strngTanggalSekarang);
            String strngTanggal = OswDate.getTanggal(strngTanggalSekarang);


            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("Tahun", strngTahun);
            parameters.Add("TahunDuaDigit", strngTahunDuaDigit);
            parameters.Add("Bulan", strngBulan);
            parameters.Add("Tanggal", strngTanggal);

            String kode = OswFormatDokumen.generate(command, id, parameters);

            // jika kode kosong, berarti tidak diseting no otomatis, sehingga harus diisi manual
            if(kode == "") {
                if(this.kode == "") {
                    throw new Exception("Kode/Nomor harus diisi, karena tidak disetting untuk digenerate otomatis");
                } else {
                    kode = this.kode;
                }
            }

            DataKasMasuk dKasMasuk = new DataKasMasuk(command, kode);
            if(dKasMasuk.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public void tambah() {
            // validasi
            valNotExist();
            Tools.valAdmin(command, this.tanggal);

            this.kode = this.kode == "" ? this.generateKode() : this.kode;
            this.version += 1;

            String query = @"INSERT INTO kasmasuk(kode, tanggal, catatan, akun, total,nourut,version,create_user) 
                             VALUES(@kode,@tanggal,@catatan,@akun, @total, @nourut,@version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("catatan", this.catatan);
            parameters.Add("akun", this.akun);
            parameters.Add("total", this.total);
            parameters.Add("nourut", this.nourut);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);

        }

        public void hapus() {
            // validasi
            valExist();
            valVersion();
            Tools.valAdmin(command, this.tanggal);

            // hapus detail
            this.hapusDetail();

            // hapus header
            String query = @"DELETE FROM kasmasuk
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapusDetail() {
            // validasi
            Tools.valAdmin(command, this.tanggal);

            // hapus jurnal
            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode);
            dJurnal.hapusJurnal();

            String query = @"SELECT no
                             FROM kasmasukdetail
                             WHERE kasmasuk = @kasmasuk";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kasmasuk", this.kode);

            // buat penampung data itemno
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("no");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngItemNo = reader.GetString("no");
                dataTable.Rows.Add(strngItemNo);
            }
            reader.Close();

            // loop per detail lalu hapus
            foreach(DataRow row in dataTable.Rows) {
                DataKasMasukDetail dKasMasukDetail = new DataKasMasukDetail(command, this.kode, row["no"].ToString());
                dKasMasukDetail.hapus();
            }

        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();
            Tools.valAdmin(command, this.tanggal);

            this.version += 1;

            // proses ubah
            String query = @"UPDATE kasmasuk
                             SET tanggal = @tanggal,
                                 catatan = @catatan, 
                                 akun = @akun,
                                 total = @total,
                                 nourut = @nourut,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("catatan", this.catatan);
            parameters.Add("akun", this.akun);
            parameters.Add("total", this.total);
            parameters.Add("nourut", this.nourut);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                throw new Exception("Data [" + this.kode + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + this.kode + "] tidak ada");
            }
        }

        private void valVersion() {
            DataKasMasuk dKasMasuk = new DataKasMasuk(command, this.kode);
            if(this.version != dKasMasuk.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        public void valJumlahDetail() {
            String query = @"SELECT COUNT(*)
                             FROM kasmasukdetail A
                             WHERE A.kasmasuk = @kasmasuk";

            Dictionary<String, String> parameters = new Dictionary<string, string>();

            parameters.Add("kasmasuk", this.kode);

            Double dblJumlahDetail = Double.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlahDetail <= 0) {
                throw new Exception("Jumlah Item detail harus lebih dari 0");
            }
        }

        public void prosesJurnal() {
            int no = 1;
            String strngKeteranganHeader = this.catatan;

            // jurnal di akun yang di header
            DataAkun dAkun = new DataAkun(command, this.akun);

            if(!dAkun.isExist) {
                throw new Exception("Akun [" + this.akun + "] tidak ditemukan.");
            }

            DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganHeader, (no++).ToString(), this.akun, this.total, "0");
            dJurnal.prosesJurnal();

            // jurnal di akun yang di detail
            String query = @"SELECT A.akun, A.keterangan, A.jumlah
                             FROM kasmasukdetail A
                             WHERE kasmasuk = @kasmasuk";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kasmasuk", this.kode);

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("akun");
            dataTable.Columns.Add("keterangan");
            dataTable.Columns.Add("jumlah");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngAkun = reader.GetString("akun");
                String strngKeterangan = reader.GetString("keterangan");
                String strngJumlah = reader.GetString("jumlah");
                dataTable.Rows.Add(strngAkun, strngKeterangan, strngJumlah);
            }
            reader.Close();

            foreach(DataRow row in dataTable.Rows) {
                String strngAkun = row["akun"].ToString();
                String strngKeterangan = row["keterangan"].ToString();
                String strngJumlah = row["jumlah"].ToString();

                String strngKeteranganDetail = strngKeterangan;

                dAkun = new DataAkun(command, strngAkun);
                if(!dAkun.isExist) {
                    throw new Exception("Akun [" + strngAkun + "] tidak ditemukan.");
                }

                dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganDetail, (no++).ToString(), strngAkun, "0", strngJumlah);
                dJurnal.prosesJurnal();
            }

            Tools.cekJurnalBalance(command, this.id, this.kode);
        }
    }
}

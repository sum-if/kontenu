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

namespace Kontenu.Design {
    class DataProyek {
        private String id = "PROYEK";
        public String kode = "";
        public String tanggaldeal = "";
        public String klien = "";
        public String kategori = "";
        public String nama = "";
        public String alamat = "";
        public String provinsi = "";
        public String kota = "";
        public String kodepos = "";
        public String tujuanproyek = "";
        public String jenisproyek = "";
        public String pic = "";
        public String status = "";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggaldeal + ";";
            kolom += "Klien:" + klien + ";";
            kolom += "Kategori:" + kategori + ";";
            kolom += "Proyek Nama:" + nama + ";";
            kolom += "Proyek Alamat:" + alamat + ";";
            kolom += "Proyek Provinsi:" + provinsi + ";";
            kolom += "Proyek Kota:" + kota + ";";
            kolom += "Proyek Kode Pos:" + kodepos + ";";
            kolom += "Tujuan Proyek:" + tujuanproyek + ";";
            kolom += "Jenis Proyek:" + jenisproyek + ";";
            kolom += "PIC:" + pic + ";";
            kolom += "Status:" + status + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataProyek(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggaldeal, klien, kategori, nama, alamat,provinsi,kota,kodepos,tujuanproyek,jenisproyek,pic,status, version
                             FROM proyek 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggaldeal = reader.GetString("tanggaldeal");
                this.klien = reader.GetString("klien");
                this.kategori = reader.GetString("kategori");
                this.nama = reader.GetString("nama");
                this.alamat = reader.GetString("alamat");
                this.provinsi = reader.GetString("provinsi");
                this.kota = reader.GetString("kota");
                this.kodepos = reader.GetString("kodepos");
                this.tujuanproyek = reader.GetString("tujuanproyek");
                this.jenisproyek = reader.GetString("jenisproyek");
                this.pic = reader.GetString("pic");
                this.status = reader.GetString("status");
                this.version = reader.GetInt64("version");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        private String generateKode() {
            String strngTanggalSekarang = this.tanggaldeal;
            String strngTahun = OswDate.getTahun(strngTanggalSekarang);
            String strngTahunDuaDigit = OswDate.getTahunDuaDigit(strngTanggalSekarang);
            String strngBulan = OswDate.getBulan(strngTanggalSekarang);
            String strngTanggal = OswDate.getTanggal(strngTanggalSekarang);
            String strngKategori = this.kategori;

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("Kategori", strngKategori);
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

            DataProyek dProyek = new DataProyek(command, kode);
            if(dProyek.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public void tambah() {
            // validasi
            valNotExist();

            this.kode = this.generateKode();
            this.version += 1;

            String query = @"INSERT INTO proyek(kode, tanggaldeal, klien,kategori, nama, alamat,provinsi,kota,kodepos,tujuanproyek,jenisproyek,pic, status, version,create_user) 
                             VALUES(@kode,@tanggaldeal,@klien,@kategori,@nama,@alamat,@provinsi,@kota,@kodepos,@tujuanproyek,@jenisproyek,@pic, @status, @version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggaldeal", this.tanggaldeal);
            parameters.Add("klien", this.klien);
            parameters.Add("kategori", this.kategori);
            parameters.Add("nama", this.nama);
            parameters.Add("alamat", this.alamat);
            parameters.Add("provinsi", this.provinsi);
            parameters.Add("kota", this.kota);
            parameters.Add("kodepos", this.kodepos);
            parameters.Add("tujuanproyek", this.tujuanproyek);
            parameters.Add("jenisproyek", this.jenisproyek);
            parameters.Add("pic", this.pic);
            parameters.Add("status", this.status);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            // hapus header
            String query = @"DELETE FROM proyek
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
            String query = @"UPDATE proyek
                             SET tanggaldeal = @tanggaldeal,
                                 klien = @klien,
                                 kategori = @kategori,
                                 nama = @nama,
                                 alamat = @alamat, 
                                 provinsi = @provinsi,
                                 kota = @kota,
                                 kodepos = @kodepos,
                                 tujuanproyek = @tujuanproyek,
                                 jenisproyek = @jenisproyek,
                                 pic = @pic, 
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggaldeal", this.tanggaldeal);
            parameters.Add("klien", this.klien);
            parameters.Add("kategori", this.kategori);
            parameters.Add("nama", this.nama);
            parameters.Add("alamat", this.alamat);
            parameters.Add("provinsi", this.provinsi);
            parameters.Add("kota", this.kota);
            parameters.Add("kodepos", this.kodepos);
            parameters.Add("tujuanproyek", this.tujuanproyek);
            parameters.Add("jenisproyek", this.jenisproyek);
            parameters.Add("pic", this.pic);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubahStatus()
        {
            // validasi
            valExist();
            valVersion();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE proyek
                             SET status = @status,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("status", this.status);
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
            DataProyek dProyek = new DataProyek(command, this.kode);
            if(this.version != dProyek.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }
    }
}

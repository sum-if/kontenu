using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using OmahSoftware.OswLib;

namespace OmahSoftware.Umum {
    class DataBarang {
        private String id = "BARANG";
        public String kode = "";
        public String nama = "";
        public String standarsatuan = "";
        public String nopart = "";
        public String hargajual1 = "0"; 
        public String hargajual2 = "0";
        public String tanggalhargaupdate = "0";
        public String rak = ""; 
        public String hargabeliterakhir = "0";
        public String keterangan = ""; 
        public String barangkategori = ""; 
        public String stokminimum = "0"; 
        public String status = "";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Nama:" + nama + ";";
            kolom += "Standar Satuan:" + standarsatuan + ";";
            kolom += "No Part:" + nopart + ";";
            kolom += "Harga Jual 1:" + hargajual1 + ";";
            kolom += "Harga Jual 2:" + hargajual2 + ";";
            kolom += "Tanggal Harga Update:" + tanggalhargaupdate + ";";
            kolom += "Harga Beli Terakhir:" + hargabeliterakhir + ";";
            kolom += "Rak:" + rak + ";";
            kolom += "Keterangan:" + keterangan + ";";
            kolom += "Kategori Barang:" + barangkategori + ";";
            kolom += "Minimum Stok:" + stokminimum + ";";
            kolom += "Status:" + status + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataBarang(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT nama, standarsatuan, nopart, hargajual1, hargajual2, tanggalhargaupdate, hargabeliterakhir, rak, 
                                    keterangan, barangkategori, stokminimum, status ,version
                             FROM barang 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.nama = reader.GetString("nama");
                this.standarsatuan = reader.GetString("standarsatuan");
                this.nopart = reader.GetString("nopart");
                this.hargajual1 = reader.GetString("hargajual1");
                this.hargajual2 = reader.GetString("hargajual2");
                this.tanggalhargaupdate = reader.GetString("tanggalhargaupdate");
                this.hargabeliterakhir = reader.GetString("hargabeliterakhir");
                this.rak = reader.GetString("rak");
                this.keterangan = reader.GetString("keterangan");
                this.barangkategori = reader.GetString("barangkategori");
                this.stokminimum = reader.GetString("stokminimum");
                this.status = reader.GetString("status");
                this.version = reader.GetInt64("version");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        private String generateKode() {
            String strngTanggalSekarang = OswDate.getStringTanggalHariIni();
            String strngTahun = OswDate.getTahun(strngTanggalSekarang);
            String strngTahunDuaDigit = OswDate.getTahunDuaDigit(strngTanggalSekarang);
            String strngBulan = OswDate.getBulan(strngTanggalSekarang);
            String strngTanggal = OswDate.getTanggal(strngTanggalSekarang);

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("Tahun", strngTahun);
            parameters.Add("TahunDuaDigit", strngTahunDuaDigit);
            parameters.Add("Bulan", strngBulan);
            parameters.Add("Tanggal", strngTanggal);
            parameters.Add("Kategori", this.barangkategori);

            String kode = OswFormatDokumen.generate(command, id, parameters);

            // jika kode kosong, berarti tidak diseting no otomatis, sehingga harus diisi manual
            if(kode == "") {
                if(this.kode == "") {
                    throw new Exception("Kode/Nomor harus diisi, karena tidak disetting untuk digenerate otomatis");
                } else {
                    kode = this.kode;
                }
            }

            DataBarang dBarang = new DataBarang(command, kode);
            if(dBarang.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public void tambah() {
            // validasi
            valNotExist();

            this.kode = this.kode == "" ? this.generateKode() : this.kode;
            this.version += 1;

            this.tanggalhargaupdate = OswDate.getStringTanggalHariIni();

            String query = @"INSERT INTO barang(kode, nama, standarsatuan, nopart, hargajual1, hargajual2, tanggalhargaupdate, hargabeliterakhir, 
                                                rak, keterangan, barangkategori, stokminimum, 
                                                status, version, create_user) 
                                 VALUES(@kode, @nama, @standarsatuan, @nopart, @hargajual1, @hargajual2, @tanggalhargaupdate, @hargabeliterakhir, 
                                        @rak, @keterangan, @barangkategori, @stokminimum, 
                                        @status, @version, @create_user)";

            Dictionary<String, object> parameters = new Dictionary<String, object>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);
            parameters.Add("standarsatuan", this.standarsatuan);
            parameters.Add("nopart", this.nopart);
            parameters.Add("hargajual1", this.hargajual1);
            parameters.Add("hargajual2", this.hargajual2);
            parameters.Add("tanggalhargaupdate", this.tanggalhargaupdate);
            parameters.Add("hargabeliterakhir", this.hargabeliterakhir);
            parameters.Add("rak", this.rak);
            parameters.Add("keterangan", this.keterangan);
            parameters.Add("barangkategori", this.barangkategori);
            parameters.Add("stokminimum", this.stokminimum);
            parameters.Add("status", this.status);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();
            valVersion();

            String query = @"DELETE FROM barang
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

            // cek tanggal harga update
            DataBarang dBarangLama = new DataBarang(command, this.kode);
            if(dBarangLama.hargajual1 != this.hargajual1 || dBarangLama.hargajual2 != this.hargajual2) {
                this.tanggalhargaupdate = OswDate.getStringTanggalHariIni();
            }

            // proses ubah
            String query = @"UPDATE barang
                             SET nama = @nama,
                                 standarsatuan = @standarsatuan,
                                 nopart = @nopart,
                                 hargajual1 = @hargajual1,
                                 hargajual2 = @hargajual2,
                                 tanggalhargaupdate = @tanggalhargaupdate,
                                 hargabeliterakhir = @hargabeliterakhir,
                                 rak = @rak,
                                 keterangan = @keterangan,
                                 barangkategori = @barangkategori,
                                 stokminimum = @stokminimum,
                                 status = @status,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, object> parameters = new Dictionary<String, object>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);
            parameters.Add("standarsatuan", this.standarsatuan);
            parameters.Add("nopart", this.nopart);
            parameters.Add("hargajual1", this.hargajual1);
            parameters.Add("hargajual2", this.hargajual2);
            parameters.Add("tanggalhargaupdate", this.tanggalhargaupdate);
            parameters.Add("hargabeliterakhir", this.hargabeliterakhir);
            parameters.Add("rak", this.rak);
            parameters.Add("keterangan", this.keterangan);
            parameters.Add("barangkategori", this.barangkategori);
            parameters.Add("stokminimum", this.stokminimum);
            parameters.Add("status", this.status);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubahHargaBeliTerakhir() {
            // validasi
            valExist();
            valVersion();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE barang
                             SET hargabeliterakhir = @hargabeliterakhir,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, object> parameters = new Dictionary<String, object>();
            parameters.Add("kode", this.kode);
            parameters.Add("hargabeliterakhir", this.hargabeliterakhir);
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
            DataBarang dBarang = new DataBarang(command, this.kode);
            if(this.version != dBarang.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using OswLib;
using OmahSoftware.OswLib;

namespace OmahSoftware.Pembelian {
    class DataSupplier {
        private String id = "SUPPLIER";
        public String kode = "";
        public String nama = "";
        public String kota = "";
        public String alamat = "";
        public String cp = "";
        public String telp = "";
        public String email = "";
        public String catatan = "";
        public String pajak = "";
        public String bank = "";
        public String norek = "";
        public String atasnama = "";
        public String maksjatuhtempo = "";
        public String npwpkode = "";
        public String npwpnama = "";
        public String npwpjalan = "";
        public String npwpblok = "";
        public String npwpnomor = "";
        public String npwprt = "";
        public String npwprw = "";
        public String npwpkelurahan = "";
        public String npwpkecamatan = "";
        public String npwpkabupaten = "";
        public String npwpprovinsi = "";
        public String npwpkodepos = "";
        public String npwptelp = "";
        public String status = "";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Nama:" + nama + ";";
            kolom += "Kota:" + kota + ";";
            kolom += "Alamat:" + alamat + ";";
            kolom += "CP:" + cp + ";";
            kolom += "Telp:" + telp + ";";
            kolom += "Email:" + email + ";";
            kolom += "Catatan:" + catatan + ";";
            kolom += "Pajak:" + pajak + ";";
            kolom += "Bank:" + bank + ";";
            kolom += "No Rek:" + norek + ";";
            kolom += "Atas Nama:" + atasnama + ";";
            kolom += "Maksimum Jth.Tempo:" + maksjatuhtempo + ";";
            kolom += "Kode NPWP:" + npwpkode + ";";
            kolom += "Nama NPWP:" + npwpnama + ";";
            kolom += "Jalan NPWP:" + npwpjalan + ";";
            kolom += "Blok NPWP:" + npwpblok + ";";
            kolom += "Nomor NPWP:" + npwpnomor + ";";
            kolom += "RT NPWP:" + npwprt + ";";
            kolom += "RW NPWP:" + npwprw + ";";
            kolom += "Kelurahan NPWP:" + npwpkelurahan + ";";
            kolom += "Kelurahan NPWP:" + npwpkecamatan + ";";
            kolom += "Kabupaten NPWP:" + npwpkabupaten + ";";
            kolom += "Provinsi NPWP:" + npwpprovinsi + ";";
            kolom += "Kode Pos NPWP:" + npwpkodepos + ";";
            kolom += "Telepon NPWP:" + npwptelp + ";";
            kolom += "Status:" + status + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataSupplier(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT kode, nama, kota, alamat, cp,telp,  email, catatan, pajak, bank, 
                                    norek, atasnama, maksjatuhtempo, npwpkode, npwpnama, npwpjalan, npwpblok, npwpnomor, npwprt, npwprw, 
                                    npwpkelurahan, npwpkecamatan, npwpkabupaten, npwpprovinsi, npwpkodepos, npwptelp, status, version
                             FROM supplier 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.nama = reader.GetString("nama");
                this.kota = reader.GetString("kota");
                this.alamat = reader.GetString("alamat");
                this.cp = reader.GetString("cp");
                this.telp = reader.GetString("telp");
                this.email = reader.GetString("email");
                this.catatan = reader.GetString("catatan");
                this.pajak = reader.GetString("pajak");
                this.bank = reader.GetString("bank");
                this.norek = reader.GetString("norek");
                this.atasnama = reader.GetString("atasnama");
                this.maksjatuhtempo = reader.GetString("maksjatuhtempo");
                this.npwpkode = reader.GetString("npwpkode");
                this.npwpnama = reader.GetString("npwpnama");
                this.npwpjalan = reader.GetString("npwpjalan");
                this.npwpblok = reader.GetString("npwpblok");
                this.npwpnomor = reader.GetString("npwpnomor");
                this.npwprt = reader.GetString("npwprt");
                this.npwprw = reader.GetString("npwprw");
                this.npwpkelurahan = reader.GetString("npwpkelurahan");
                this.npwpkecamatan = reader.GetString("npwpkecamatan");
                this.npwpkabupaten = reader.GetString("npwpkabupaten");
                this.npwpprovinsi = reader.GetString("npwpprovinsi");
                this.npwpkodepos = reader.GetString("npwpkodepos");
                this.npwptelp = reader.GetString("npwptelp");
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

            String kode = OswFormatDokumen.generate(command, id, parameters);

            // jika kode kosong, berarti tidak diseting no otomatis, sehingga harus diisi manual
            if(kode == "") {
                if(this.kode == "") {
                    throw new Exception("Kode/Nomor harus diisi, karena tidak disetting untuk digenerate otomatis");
                } else {
                    kode = this.kode;
                }
            }

            DataSupplier dSupplier = new DataSupplier(command, kode);
            if(dSupplier.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public void tambah() {
            // validasi
            valNotExist();

            this.kode = this.kode == "" ? this.generateKode() : this.kode;
            this.version += 1;

            String query = @"INSERT INTO supplier(kode, nama, kota, alamat, cp,telp,email, catatan, pajak, bank, 
                                                  norek, atasnama, maksjatuhtempo, npwpkode, npwpnama, npwpjalan, npwpblok, npwpnomor, npwprt, npwprw, 
                                                  npwpkelurahan, npwpkecamatan, npwpkabupaten, npwpprovinsi, npwpkodepos, npwptelp, 
                                                  status, version, create_user) 
                                         VALUES(@kode,@nama,@kota,@alamat,@cp,@telp,@email,@catatan,@pajak,@bank,
                                                @norek,@atasnama,@maksjatuhtempo,@npwpkode,@npwpnama,@npwpjalan,@npwpblok,@npwpnomor,@npwprt,@npwprw,
                                                @npwpkelurahan,@npwpkecamatan,@npwpkabupaten,@npwpprovinsi,@npwpkodepos,@npwptelp,
                                                @status,@version,@create_user)";

            Dictionary<String, object> parameters = new Dictionary<String, object>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);
            parameters.Add("kota", this.kota);
            parameters.Add("alamat", this.alamat);
            parameters.Add("cp", this.cp);
            parameters.Add("telp", this.telp);
            parameters.Add("email", this.email);
            parameters.Add("catatan", this.catatan);
            parameters.Add("pajak", this.pajak);
            parameters.Add("bank", this.bank);
            parameters.Add("norek", this.norek);
            parameters.Add("atasnama", this.atasnama);
            parameters.Add("maksjatuhtempo", this.maksjatuhtempo);
            parameters.Add("npwpkode", this.npwpkode);
            parameters.Add("npwpnama", this.npwpnama);
            parameters.Add("npwpjalan", this.npwpjalan);
            parameters.Add("npwpblok", this.npwpblok);
            parameters.Add("npwpnomor", this.npwpnomor);
            parameters.Add("npwprt", this.npwprt);
            parameters.Add("npwprw", this.npwprw);
            parameters.Add("npwpkelurahan", this.npwpkelurahan);
            parameters.Add("npwpkecamatan", this.npwpkecamatan);
            parameters.Add("npwpkabupaten", this.npwpkabupaten);
            parameters.Add("npwpprovinsi", this.npwpprovinsi);
            parameters.Add("npwpkodepos", this.npwpkodepos);
            parameters.Add("npwptelp", this.npwptelp);
            parameters.Add("status", this.status);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();
            valVersion();

            String query = @"DELETE FROM supplier
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
            String query = @"UPDATE supplier
                             SET nama = @nama,
                                 kota = @kota,
                                 alamat = @alamat,
                                 cp = @cp,
                                 telp = @telp,
                                 email = @email,
                                 catatan = @catatan,                                 
                                 pajak = @pajak,
                                 bank = @bank,
                                 norek = @norek,
                                 atasnama = @atasnama,
                                 maksjatuhtempo = @maksjatuhtempo,
                                 npwpkode = @npwpkode,
                                 npwpnama = @npwpnama,
                                 npwpjalan = @npwpjalan,
                                 npwpblok = @npwpblok,
                                 npwpnomor = @npwpnomor,
                                 npwprt = @npwprt,
                                 npwprw = @npwprw,
                                 npwpkelurahan = @npwpkelurahan,
                                 npwpkecamatan = @npwpkecamatan,
                                 npwpkabupaten = @npwpkabupaten,
                                 npwpprovinsi = @npwpprovinsi,
                                 npwpkodepos = @npwpkodepos,
                                 npwptelp = @npwptelp,
                                 status = @status,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, object> parameters = new Dictionary<String, object>();
            parameters.Add("kode", this.kode);
            parameters.Add("nama", this.nama);
            parameters.Add("kota", this.kota);
            parameters.Add("alamat", this.alamat);
            parameters.Add("cp", this.cp);
            parameters.Add("telp", this.telp);
            parameters.Add("email", this.email);
            parameters.Add("catatan", this.catatan);
            parameters.Add("pajak", this.pajak);
            parameters.Add("bank", this.bank);
            parameters.Add("norek", this.norek);
            parameters.Add("atasnama", this.atasnama);
            parameters.Add("maksjatuhtempo", this.maksjatuhtempo);
            parameters.Add("npwpkode", this.npwpkode);
            parameters.Add("npwpnama", this.npwpnama);
            parameters.Add("npwpjalan", this.npwpjalan);
            parameters.Add("npwpblok", this.npwpblok);
            parameters.Add("npwpnomor", this.npwpnomor);
            parameters.Add("npwprt", this.npwprt);
            parameters.Add("npwprw", this.npwprw);
            parameters.Add("npwpkelurahan", this.npwpkelurahan);
            parameters.Add("npwpkecamatan", this.npwpkecamatan);
            parameters.Add("npwpkabupaten", this.npwpkabupaten);
            parameters.Add("npwpprovinsi", this.npwpprovinsi);
            parameters.Add("npwpkodepos", this.npwpkodepos);
            parameters.Add("npwptelp", this.npwptelp);
            parameters.Add("status", this.status);
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
            DataSupplier dSupplier = new DataSupplier(command, this.kode);
            if(this.version != dSupplier.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

    }
}

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
using OmahSoftware.Akuntansi;

namespace OmahSoftware.Persediaan {
    class DataPenyesuaianPersediaan {
        private String id = "PENYESUAIANBARANG";
        public String kode = "";
        public String tanggal = "";
        public String catatan = "";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            
            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Catatan:" + catatan + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataPenyesuaianPersediaan(MySqlCommand command, String kode) {
            this.command = command;
            
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal, catatan, version
                             FROM penyesuaianpersediaan 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.catatan = reader.GetString("catatan");
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

            DataPenyesuaianPersediaan dPenyesuaianPersediaan = new DataPenyesuaianPersediaan(command, kode);
            if(dPenyesuaianPersediaan.isExist) {
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

            String query = @"INSERT INTO penyesuaianpersediaan(kode, tanggal, catatan, version,create_user) 
                             VALUES(@kode,@tanggal,@catatan, @version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("catatan", this.catatan);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

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
            DataPenyesuaianPersediaan dPenyesuaianPersediaan = new DataPenyesuaianPersediaan(command, this.kode);
            if(this.version != dPenyesuaianPersediaan.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        public void valJumlahDetail() {
            String query = @"SELECT COUNT(*)
                             FROM penyesuaianpersediaandetail A
                             WHERE A.penyesuaianpersediaan = @penyesuaianpersediaan";

            Dictionary<String, String> parameters = new Dictionary<string, string>();
            
            parameters.Add("penyesuaianpersediaan", this.kode);

            Double dblJumlahDetail = Double.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlahDetail <= 0) {
                throw new Exception("Jumlah Item detail harus lebih dari 0");
            }
        }


        public void prosesJurnal() {
            /*
            Kurang
            711.02.003				Kerugian Penyesuaian Persediaan Stok										Debet			Ambil aja dari total kredit (Total dari tiap akun2 barang yang disesuaikan)													
            Akun di setting			Akun Persediaan Barang (Bisa Jadi/Bahan Baku/Stngh Jadi)			Kredit			(qty skrg - qty baru) * HPP Baru													

            Tambah
            Akun di setting			Akun Persediaan Barang (Bisa Jadi/Bahan Baku/Stngh Jadi)			Debet			(qty skrg - qty baru) * HPP Baru													
            711.01.003				Keuntungan Penyesuaian Persediaan Stok									Kredit			Ambil aja dari total debit (Total dari tiap akun2 barang yang disesuaikan)													

            */
            int no = 1;
            // jurnal yang untung
            String strngKeteranganJurnal = "Penyesuaian Barang (Tambah) : [" + this.catatan + "]";

            String query = @"SELECT B.akunpersediaan, SUM(A.jumlahpenyesuaian * COALESCE(D.nilai,0)) AS jumlah
                             FROM penyesuaianpersediaandetail A
                             INNER JOIN barang B ON A.barang = B.kode
                             LEFT JOIN saldopersediaanhpp D ON B.kode = D.barang
                             WHERE penyesuaianpersediaan = @penyesuaianpersediaan AND A.jumlahpenyesuaian > 0
                             GROUP BY B.akunpersediaan";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("penyesuaianpersediaan", this.kode);

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("akunpersediaan");
            dataTable.Columns.Add("jumlah");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngAkunPersediaan = reader.GetString("akunpersediaan");
                String strngJumlah = reader.GetString("jumlah");
                dataTable.Rows.Add(strngAkunPersediaan, strngJumlah);
            }
            reader.Close();

            double dblTotalUntung = 0;
            foreach(DataRow row in dataTable.Rows) {
                String strngAkunPersediaan = row["akunpersediaan"].ToString();
                String strngJumlah = row["jumlah"].ToString();

                DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkunPersediaan, strngJumlah, "0");
                dJurnal.prosesJurnal();

                dblTotalUntung += Tools.getRoundMoney(double.Parse(strngJumlah));
                dblTotalUntung = Tools.getRoundMoney(dblTotalUntung);
            }

            // jurnal yang rugi
            strngKeteranganJurnal = "Penyesuaian Barang (Kurang) : [" + this.catatan + "]";

            query = @"SELECT B.akunpersediaan, SUM(A.jumlahpenyesuaian * -1 * COALESCE(D.nilai,0)) AS jumlah
                             FROM penyesuaianpersediaandetail A
                             INNER JOIN barang B ON A.barang = B.kode
                             LEFT JOIN saldopersediaanhpp D ON B.kode = D.barang
                             WHERE penyesuaianpersediaan = @penyesuaianpersediaan AND A.jumlahpenyesuaian < 0
                             GROUP BY B.akunpersediaan";

            parameters = new Dictionary<String, String>();
            
            parameters.Add("penyesuaianpersediaan", this.kode);

            dataTable = new DataTable();
            dataTable.Columns.Add("akunpersediaan");
            dataTable.Columns.Add("jumlah");

            reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngAkunPersediaan = reader.GetString("akunpersediaan");
                String strngJumlah = reader.GetString("jumlah");
                dataTable.Rows.Add(strngAkunPersediaan, strngJumlah);
            }
            reader.Close();

            double dblTotalRugi = 0;
            foreach(DataRow row in dataTable.Rows) {
                String strngAkunPersediaan = row["akunpersediaan"].ToString();
                String strngJumlah = row["jumlah"].ToString();

                DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkunPersediaan, "0", strngJumlah);
                dJurnal.prosesJurnal();

                dblTotalRugi += Tools.getRoundMoney(double.Parse(strngJumlah));
                dblTotalRugi = Tools.getRoundMoney(dblTotalRugi);
            }

            // Akun Penyesuaian
            strngKeteranganJurnal = "Penyesuaian Barang (Selisih Penyesuaian) : [" + this.catatan + "]";

            double dblTotal = Tools.getRoundMoney(dblTotalUntung - dblTotalRugi);
            String strngAkunPenyesuaian = OswConstants.getIsiSettingDB(command, Constants.AKUN_PENYESUAIAN_PERSEDIAAN);
            DataAkun dAkunPenyesuaian = new DataAkun(command, strngAkunPenyesuaian);
            if(!dAkunPenyesuaian.isExist) {
                throw new Exception("Akun Penyesuaian Barang [" + strngAkunPenyesuaian + "] tidak ditemukan.");
            }

            if(dblTotal > 0) {
                DataJurnal dJurnalPenyesuaian = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkunPenyesuaian, "0", dblTotal.ToString());
                dJurnalPenyesuaian.prosesJurnal();
            } else if(dblTotal < 0) {
                dblTotal *= -1;
                DataJurnal dJurnalPenyesuaian = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkunPenyesuaian, dblTotal.ToString(), "0");
                dJurnalPenyesuaian.prosesJurnal();
            }

            Tools.cekJurnalBalance(command, this.id, this.kode);
        }
    }
}

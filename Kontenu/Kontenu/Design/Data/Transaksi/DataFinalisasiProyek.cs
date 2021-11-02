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
using Kontenu.Akuntansi;

namespace Kontenu.Design {
    class DataFinalisasiProyek {
        private String id = "FINALISASIPROYEK";
        public String kode = "";
        public String tanggal = "";
        public String proyek = "";
        public String penjualantotalinvoice = "0";
        public String penjualantotalditerima = "0";
        public String penjualansisa = "0";
        public String purchasetotalinvoice = "0";
        public String purchasetotalbayar = "0";
        public String purchasesisa = "0";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Proyek:" + proyek + ";";
            kolom += "Penjualan Total Invoice:" + penjualantotalinvoice + ";";
            kolom += "Penjualan Total Diterima:" + penjualantotalditerima + ";";
            kolom += "penjualansisa:" + penjualansisa + ";";
            kolom += "purchasetotalinvoice:" + purchasetotalinvoice + ";";
            kolom += "purchasetotalbayar:" + purchasetotalbayar + ";";
            kolom += "purchasesisa:" + purchasesisa + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataFinalisasiProyek(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal, proyek, penjualantotalinvoice,penjualantotalditerima, penjualansisa, purchasetotalinvoice, purchasetotalbayar, purchasesisa, version
                             FROM finalisasiproyek 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.proyek = reader.GetString("proyek");
                this.penjualantotalinvoice = reader.GetString("penjualantotalinvoice");
                this.penjualantotalditerima = reader.GetString("penjualantotalditerima");
                this.penjualansisa = reader.GetString("penjualansisa");
                this.purchasetotalinvoice = reader.GetString("purchasetotalinvoice");
                this.purchasetotalbayar = reader.GetString("purchasetotalbayar");
                this.purchasesisa = reader.GetString("purchasesisa");
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

            DataFinalisasiProyek dFinalisasiProyek = new DataFinalisasiProyek(command, kode);
            if(dFinalisasiProyek.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public void tambah() {
            // validasi
            valNotExist();

            this.kode = this.generateKode();
            this.version += 1;

            String query = @"INSERT INTO finalisasiproyek(kode, tanggal, proyek, penjualantotalinvoice,penjualantotalditerima, penjualansisa, purchasetotalinvoice, purchasetotalbayar, purchasesisa, version,create_user) 
                             VALUES(@kode,@tanggal,@proyek, @penjualantotalinvoice,@penjualantotalditerima, @penjualansisa, @purchasetotalinvoice, @purchasetotalbayar, @purchasesisa, @version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("proyek", this.proyek);
            parameters.Add("penjualantotalinvoice", this.penjualantotalinvoice);
            parameters.Add("penjualantotalditerima", this.penjualantotalditerima);
            parameters.Add("penjualansisa", this.penjualansisa);
            parameters.Add("purchasetotalinvoice", this.purchasetotalinvoice);
            parameters.Add("purchasetotalbayar", this.purchasetotalbayar);
            parameters.Add("purchasesisa", this.purchasesisa);
            parameters.Add("version", "1");
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            // hapus detail
            this.hapusDetail();

            // hapus header
            String query = @"DELETE FROM finalisasiproyek
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapusDetail() {
            // PENJUALAN
            String query = @"SELECT no
                             FROM finalisasiproyekpenjualan
                             WHERE finalisasiproyek = @finalisasiproyek";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("finalisasiproyek", this.kode);

            // buat penampung data itemno
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("no");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngNo = reader.GetString("no");
                dataTable.Rows.Add(strngNo);
            }
            reader.Close();

            // loop per detail lalu hapus
            foreach(DataRow row in dataTable.Rows) {
                DataFinalisasiProyekPenjualan dFinalisasiProyekPenjualan = new DataFinalisasiProyekPenjualan(command, this.kode, row["no"].ToString());
                dFinalisasiProyekPenjualan.hapus();
            }

            // PURCHASE
             query = @"SELECT no
                        FROM finalisasiproyekpurchase
                        WHERE finalisasiproyek = @finalisasiproyek";

            parameters = new Dictionary<String, String>();
            parameters.Add("finalisasiproyek", this.kode);

            // buat penampung data itemno
            dataTable = new DataTable();
            dataTable.Columns.Add("no");

            reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngNo = reader.GetString("no");
                dataTable.Rows.Add(strngNo);
            }
            reader.Close();

            // loop per detail lalu hapus
            foreach(DataRow row in dataTable.Rows) {
                DataFinalisasiProyekPurchase dFinalisasiProyekPurchase = new DataFinalisasiProyekPurchase(command, this.kode, row["no"].ToString());
                dFinalisasiProyekPurchase.hapus();
            }
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE finalisasiproyek
                             SET tanggal = @tanggal,
                                 proyek = @proyek, 
                                 penjualantotalinvoice = @penjualantotalinvoice, 
                                 penjualantotalditerima = @penjualantotalditerima,
                                 penjualansisa = @penjualansisa,
                                 purchasetotalinvoice = @purchasetotalinvoice,
                                 purchasetotalbayar = @purchasetotalbayar,
                                 purchasesisa = @purchasesisa,
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("proyek", this.proyek);
            parameters.Add("penjualantotalinvoice", this.penjualantotalinvoice);
            parameters.Add("penjualantotalditerima", this.penjualantotalditerima);
            parameters.Add("penjualansisa", this.penjualansisa);
            parameters.Add("purchasetotalinvoice", this.purchasetotalinvoice);
            parameters.Add("purchasetotalbayar", this.purchasetotalbayar);
            parameters.Add("purchasesisa", this.purchasesisa);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void prosesJurnal()
        {
            ///*
            //FinalisasiProyek [NAMA PROJECT] dari [NAMA CLIENT]																																				
            //No	Status				Akun						Nilai														Contoh di atas											
            //1	Debet				konstanta.design_akunpiutang						grand total														 9.180.000
            //2	Kredit				konstanta.design_akununearned						grand total														 9.180.000
            //*/

            //int no = 1;
            //DataKlien dKlien = new DataKlien(command, this.klien);
            //DataProyek dProyek = new DataProyek(command, this.penjualantotalinvoice);

            //String strngKeteranganJurnal = "FinalisasiProyek ["+ dProyek.nama +"] dari ["+ dKlien.nama +"]";

            //// Debet				konstanta.design_akunpiutang						grand total														 9.180.000
            //String strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_DESIGN_AKUN_PIUTANG);
            //DataAkun dAkun = new DataAkun(command, strngAkun);
            //if (!dAkun.isExist)
            //{
            //    throw new Exception("Akun Design Piutang [" + strngAkun + "] tidak ditemukan.");
            //}

            //DataJurnal dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, this.penjualantotalditerima, "0");
            //dJurnal.prosesJurnal();

            //// Kredit				konstanta.design_akununearned						grand total														 9.180.000
            //strngAkun = OswConstants.getIsiSettingDB(command, Constants.AKUN_DESIGN_AKUN_EARNED);
            //dAkun = new DataAkun(command, strngAkun);
            //if (!dAkun.isExist)
            //{
            //    throw new Exception("Akun Design Unearned [" + strngAkun + "] tidak ditemukan.");
            //}

            //dJurnal = new DataJurnal(command, this.id, this.kode, this.tanggal, strngKeteranganJurnal, (no++).ToString(), strngAkun, "0", this.penjualantotalditerima);
            //dJurnal.prosesJurnal();

            //Tools.cekJurnalBalance(command, this.id, this.kode);
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
            DataFinalisasiProyek dFinalisasiProyek = new DataFinalisasiProyek(command, this.kode);
            if(this.version != dFinalisasiProyek.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        public void valJumlahDetail() {
            // PENJUALAN
            String query = @"SELECT COUNT(*)
                             FROM finalisasiproyekpenjualan A
                             WHERE A.finalisasiproyek = @finalisasiproyek";

            Dictionary<String, String> parameters = new Dictionary<string, string>();
            parameters.Add("finalisasiproyek", this.kode);

            decimal dblJumlahDetailBarang = decimal.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlahDetailBarang <= 0) {
                throw new Exception("Jumlah Item detail harus lebih dari 0");
            }
        }

        private void valDetail() {
            if(decimal.Parse(this.penjualansisa) != 0) {
                throw new Exception("Sisa Penjualan tidak sama dengan 0");
            }

            if (decimal.Parse(this.purchasesisa) != 0)
            {
                throw new Exception("Sisa Purchase tidak sama dengan 0");
            }
        }
    }
}

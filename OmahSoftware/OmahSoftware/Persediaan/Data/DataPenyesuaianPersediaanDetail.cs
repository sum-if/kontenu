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

namespace OmahSoftware.Persediaan {
    class DataPenyesuaianPersediaanDetail {
        private String id = "PENYESUAIANBARANG";
        private String penyesuaianpersediaan = "";
        private String no = "0";
        public String barang = "";
        public String jumlahsekarang = "0";
        public String jumlahpenyesuaian = "0";
        public String jumlahbaru = "0";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Penyesuaian Persediaan Stok:" + penyesuaianpersediaan + ";";
            kolom += "No:" + no + ";";
            kolom += "Barang:" + barang + ";";
            kolom += "Jumlah Sekarang:" + jumlahsekarang + ";";
            kolom += "Jumlah Penyesuaian:" + jumlahpenyesuaian + ";";
            kolom += "Jumlah Baru:" + jumlahbaru + ";";
            return kolom;
        }

        public DataPenyesuaianPersediaanDetail(MySqlCommand command, String penyesuaianpersediaan, String no) {
            this.command = command;
            
            this.penyesuaianpersediaan = penyesuaianpersediaan;
            this.no = no;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            String query = @"SELECT barang, jumlahsekarang,jumlahpenyesuaian,jumlahbaru
                             FROM penyesuaianpersediaandetail 
                             WHERE penyesuaianpersediaan = @penyesuaianpersediaan AND no = @no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("penyesuaianpersediaan", this.penyesuaianpersediaan);
            parameters.Add("no", this.no);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.barang = reader.GetString("barang");
                this.jumlahsekarang = reader.GetString("jumlahsekarang");
                this.jumlahpenyesuaian = reader.GetString("jumlahpenyesuaian");
                this.jumlahbaru = reader.GetString("jumlahbaru");
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        public void tambah() {
            // validasi
            valNotExist();
            valDetail();

            // mutasi persediaan
            DataPenyesuaianPersediaan dPenyesuaianPersediaan = new DataPenyesuaianPersediaan(command, this.penyesuaianpersediaan);
            DataMutasiPersediaan dMutasiPersediaan = new DataMutasiPersediaan(command, this.id, this.penyesuaianpersediaan);
            dMutasiPersediaan.tanggal = dPenyesuaianPersediaan.tanggal;
            dMutasiPersediaan.no = this.no;
            dMutasiPersediaan.barang = this.barang;
            dMutasiPersediaan.jumlah = this.jumlahpenyesuaian;
            dMutasiPersediaan.proses();

            // tambah detail
            String query = @"INSERT INTO penyesuaianpersediaandetail(penyesuaianpersediaan,no,barang,jumlahsekarang,jumlahpenyesuaian,jumlahbaru) 
                             VALUES(@penyesuaianpersediaan,@no,@barang,@jumlahsekarang,@jumlahpenyesuaian,@jumlahbaru)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("penyesuaianpersediaan", this.penyesuaianpersediaan);
            parameters.Add("no", this.no);
            parameters.Add("barang", this.barang);
            parameters.Add("jumlahsekarang", this.jumlahsekarang);
            parameters.Add("jumlahpenyesuaian", this.jumlahpenyesuaian);
            parameters.Add("jumlahbaru", this.jumlahbaru);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                
                throw new Exception("Data Penyesuaian Persediaan Stok [" + this.penyesuaianpersediaan + " - " + this.no + "] sudah ada");
            }
        }

        private void valDetail() {
            

            if(double.Parse(this.jumlahpenyesuaian) == 0) {
                throw new Exception("Data Penyesuaian Persediaan Stok [" + this.penyesuaianpersediaan + " - " + this.no + "] jumlah penyesuaian = 0");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                
                throw new Exception("Data Penyesuaian Persediaan Stok [" + this.penyesuaianpersediaan + " - " + this.no + "] tidak ada");
            }
        }
    }
}

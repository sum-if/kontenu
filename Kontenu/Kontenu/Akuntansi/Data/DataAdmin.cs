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
    public class DataAdmin {
        public String periode = "";
        public String proses = "";
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            
            kolom += "Periode:" + periode + ";";
            kolom += "Proses:" + proses + ";";
            return kolom;
        }

        public DataAdmin(MySqlCommand command, String periode, String proses) {
            this.command = command;
            
            this.periode = periode;
            this.proses = proses;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT 1
                             FROM admin 
                             WHERE periode = @periode AND proses = @proses";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("periode", this.periode);
            parameters.Add("proses", this.proses);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                reader.Close();
            } else {
                this.isExist = false;
                reader.Close();
            }
        }

        public void tambah() {
            // validasi
            valNotExist();

            String query = @"INSERT INTO admin(periode,proses,create_user) 
                             VALUES(@periode,@proses,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("periode", this.periode);
            parameters.Add("proses", this.proses);
            parameters.Add("create_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapus() {
            // validasi
            valExist();

            String query = @"DELETE FROM admin
                             WHERE periode = @periode AND proses = @proses";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("periode", this.periode);
            parameters.Add("proses", this.proses);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valNotExist() {
            if(this.isExist) {
                
                throw new Exception("Data [" + OswConvert.toNamaPeriode(this.periode) + " - " + this.proses + "] sudah ada");
            }
        }

        private void valExist() {
            if(!this.isExist) {
                throw new Exception("Data [" + OswConvert.toNamaPeriode(this.periode) + " - " + this.proses + "] tidak ada");
            }
        }


        /// <summary>
        /// Mengetahui apakah proses tersebut sudah di lakukan atau belum
        /// </summary>
        /// <returns></returns>
        public bool isProcessed() {
            return this.isExist;
        }

        /// <summary>
        /// Mengetahui apakah proses berikutnya sudah di lakukan atau belum
        /// </summary>
        /// <returns></returns>
        public bool isNextPeriodProcessed() {
            bool hasil = false;

            String query = @"SELECT COUNT(*)
	                        FROM admin
	                       WHERE periode > @periode AND proses = @proses";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("periode", this.periode);
            parameters.Add("proses", this.proses);

            int jumlah = int.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(jumlah > 0) {
                hasil = true;
            }

            return hasil;
        }
    }


}

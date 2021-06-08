﻿using System;
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
    class DataQuotation {
        private String id = "QUOTATION";
        public String kode = "";
        public String tanggal = "";
        public String tanggalberlaku = "";
        public String klien = "";
        public String proyeknama = "";
        public String proyekalamat = "";
        public String proyekprovinsi = "";
        public String proyekkota = "";
        public String proyekkodepos = "";
        public String tujuanproyek = "";
        public String jenisproyek = "";
        public String pic = "";
        public String grandtotal = "0";
        public String status = "";
        public Int64 version = 0;
        public Boolean isExist = false;
        private MySqlCommand command;

        public override string ToString() {
            String kolom = "";
            kolom += "Kode:" + kode + ";";
            kolom += "Tanggal:" + tanggal + ";";
            kolom += "Tanggal Berlaku:" + tanggalberlaku + ";";
            kolom += "Klien:" + klien + ";";
            kolom += "Proyek Nama:" + proyeknama + ";";
            kolom += "Proyek Alamat:" + proyekalamat + ";";
            kolom += "Proyek Provinsi:" + proyekprovinsi + ";";
            kolom += "Proyek Kota:" + proyekkota + ";";
            kolom += "Proyek Kode Pos:" + proyekkodepos + ";";
            kolom += "Tujuan Proyek:" + tujuanproyek + ";";
            kolom += "Jenis Proyek:" + jenisproyek + ";";
            kolom += "PIC:" + pic + ";";
            kolom += "Grand Total:" + grandtotal + ";";
            kolom += "Status:" + status + ";";
            kolom += "Version:" + version + ";";
            return kolom;
        }

        public DataQuotation(MySqlCommand command, String kode) {
            this.command = command;
            this.kode = kode;
            this.getOtherAttribute();
        }

        private void getOtherAttribute() {
            // cek apakah ada di database berdasarkan PK
            String query = @"SELECT tanggal, tanggalberlaku, klien,proyeknama, proyekalamat,proyekprovinsi,proyekkota,proyekkodepos,tujuanproyek,jenisproyek,pic, grandtotal,grandtotal,catatan, status, version
                             FROM quotation 
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            if(reader.Read()) {
                this.isExist = true;
                this.tanggal = reader.GetString("tanggal");
                this.tanggalberlaku = reader.GetString("tanggalberlaku");
                this.klien = reader.GetString("klien");
                this.proyeknama = reader.GetString("proyeknama");
                this.proyekalamat = reader.GetString("proyekalamat");
                this.proyekprovinsi = reader.GetString("proyekprovinsi");
                this.proyekkota = reader.GetString("proyekkota");
                this.proyekkodepos = reader.GetString("proyekkodepos");
                this.tujuanproyek = reader.GetString("tujuanproyek");
                this.jenisproyek = reader.GetString("jenisproyek");
                this.pic = reader.GetString("pic");
                this.grandtotal = reader.GetString("grandtotal");
                this.status = reader.GetString("status");
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

            DataQuotation dQuotation = new DataQuotation(command, kode);
            if(dQuotation.isExist) {
                kode = generateKode();
            }

            return kode;
        }

        public void tambah() {
            // validasi
            valNotExist();

            this.kode = this.generateKode();
            this.version += 1;

            String query = @"INSERT INTO quotation(kode, tanggal, tanggalberlaku, klien,proyeknama, proyekalamat,proyekprovinsi,proyekkota,proyekkodepos,tujuanproyek,jenisproyek,pic, grandtotal, status, version,create_user) 
                             VALUES(@kode,@tanggal,@tanggalberlaku,  @klien,@proyeknama,@proyekalamat,@proyekprovinsi,@proyekkota,@proyekkodepos,@tujuanproyek,@jenisproyek,@pic, @grandtotal, @status, @version,@create_user)";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("tanggalberlaku", this.tanggalberlaku);
            parameters.Add("klien", this.klien);
            parameters.Add("proyeknama", this.proyeknama);
            parameters.Add("proyekalamat", this.proyekalamat);
            parameters.Add("proyekprovinsi", this.proyekprovinsi);
            parameters.Add("proyekkota", this.proyekkota);
            parameters.Add("proyekkodepos", this.proyekkodepos);
            parameters.Add("tujuanproyek", this.tujuanproyek);
            parameters.Add("jenisproyek", this.jenisproyek);
            parameters.Add("pic", this.pic);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("status", this.status);
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
            String query = @"DELETE FROM quotation
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void hapusDetail() {
            String query = @"SELECT id
                             FROM quotationdetail
                             WHERE quotation = @quotation";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("quotation", this.kode);

            // buat penampung data itemno
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("id");

            MySqlDataReader reader = OswDataAccess.executeReaderQuery(query, parameters, command);
            while(reader.Read()) {
                String strngId = reader.GetString("id");
                dataTable.Rows.Add(strngId);
            }
            reader.Close();

            // loop per detail lalu hapus
            foreach(DataRow row in dataTable.Rows) {
                DataQuotationDetail dQuotationDetail = new DataQuotationDetail(command, this.kode, row["id"].ToString());
                dQuotationDetail.hapus();
            }
        }

        public void ubah() {
            // validasi
            valExist();
            valVersion();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE quotation
                             SET tanggal = @tanggal,
                                 tanggalberlaku = @tanggalberlaku, 
                                 klien = @klien,
                                 proyeknama = @proyeknama,
                                 proyekalamat = @proyekalamat, 
                                 proyekprovinsi = @proyekprovinsi,
                                 proyekkota = @proyekkota,
                                 proyekkodepos = @proyekkodepos,
                                 tujuanproyek = @tujuanproyek,
                                 jenisproyek = @jenisproyek,
                                 pic = @pic, 
                                 grandtotal = @grandtotal, 
                                 version = @version,
                                 update_at = CURRENT_TIMESTAMP(),
                                 update_user = @update_user
                             WHERE kode = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", this.kode);
            parameters.Add("tanggal", this.tanggal);
            parameters.Add("tanggalberlaku", this.tanggalberlaku);
            parameters.Add("klien", this.klien);
            parameters.Add("proyeknama", this.proyeknama);
            parameters.Add("proyekalamat", this.proyekalamat);
            parameters.Add("proyekprovinsi", this.proyekprovinsi);
            parameters.Add("proyekkota", this.proyekkota);
            parameters.Add("proyekkodepos", this.proyekkodepos);
            parameters.Add("tujuanproyek", this.tujuanproyek);
            parameters.Add("jenisproyek", this.jenisproyek);
            parameters.Add("pic", this.pic);
            parameters.Add("grandtotal", this.grandtotal);
            parameters.Add("version", this.version.ToString());
            parameters.Add("update_user", OswConstants.KODEUSER);

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        public void ubahStatus() {
            // validasi
            valExist();
            valVersion();

            this.version += 1;

            // proses ubah
            String query = @"UPDATE quotation
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
            DataQuotation dQuotation = new DataQuotation(command, this.kode);
            if(this.version != dQuotation.version) {
                throw new Exception("Data yang dimiliki bukan yang terbaru, silahkan tutup dan lakukan proses ulang");
            }
        }

        public void valJumlahDetail() {
            String query = @"SELECT COUNT(*)
                             FROM quotationdetail A
                             WHERE A.quotation = @quotation";

            Dictionary<String, String> parameters = new Dictionary<string, string>();
            parameters.Add("quotation", this.kode);

            Double dblJumlahDetailBarang = Double.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            if(dblJumlahDetailBarang <= 0) {
                throw new Exception("Jumlah Item detail harus lebih dari 0");
            }
        }

        public void prosesHitung() {
            // detail untuk kolom harga dan biaya
            String query = @"UPDATE quotationdetail A
                            LEFT JOIN (
	                            SELECT quotation,id,SUM(subtotal) AS total
	                            FROM quotationdetailbarang A
                                WHERE A.quotation = @quotation
	                            GROUP BY quotation,id
                            ) B ON A.quotation = B.quotation AND A.id = B.id
                            LEFT JOIN (
	                            SELECT quotation,id,SUM(subtotal) AS total
	                            FROM quotationdetailbiaya A
                                WHERE A.quotation = @quotation
	                            GROUP BY quotation,id
                            ) C ON A.quotation = C.quotation AND A.id = C.id
                            SET A.hargabarang = COALESCE(B.total,0), 
	                            A.biaya = COALESCE(C.total,0)
                            WHERE A.quotation = @quotation";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("quotation", this.kode);

            OswDataAccess.executeVoidQuery(query, parameters, command);

            // detail untuk kolom harga dasar
            query = @"UPDATE quotationdetail A
                    SET A.subhargadasar = (A.hargabarang + A.biaya + A.profit)
                    WHERE A.quotation = @quotation";

            OswDataAccess.executeVoidQuery(query, parameters, command);

            // detail untuk kolom pic
            query = @"UPDATE quotationdetail A
                    INNER JOIN quotation B ON A.quotation = B.kode
                    SET A.pic = ROUND((A.subhargadasar / (100 - B.pic)) * B.pic)
                    WHERE A.quotation = @quotation";

            OswDataAccess.executeVoidQuery(query, parameters, command);

            // detail untuk kolom harga jenisproyek
            query = @"UPDATE quotationdetail A
                    SET A.hargajenisproyek = CEILING((A.subhargadasar + A.pic) / 100) * 100
                    WHERE A.quotation = @quotation";

            OswDataAccess.executeVoidQuery(query, parameters, command);

            // detail untuk kolom subtotal
            query = @"UPDATE quotationdetail A
                    SET A.subtotal = A.hargajenisproyek * A.jumlah
                    WHERE A.quotation = @quotation";

            OswDataAccess.executeVoidQuery(query, parameters, command);


            // update header untuk totalbiaya,totalprofit,totalpic, totalhargadasar,dpp,ppn
            query = @"UPDATE quotation A
                    LEFT JOIN (
	                    SELECT A.quotation, SUM(A.pic * A.jumlah) AS totalpic, SUM(A.biaya * A.jumlah) AS totalbiaya, SUM(A.profit * A.jumlah) AS totalprofit, SUM(A.hargabarang * A.jumlah) AS subhargadasar, SUM(A.subtotal) AS total
	                    FROM quotationdetail A
	                    WHERE A.quotation = @quotation
	                    GROUP BY A.quotation
                    ) B ON A.kode = B.quotation
                    SET A.totalbiaya = COALESCE(B.totalbiaya,0), 
	                    A.totalprofit = COALESCE(B.totalprofit,0),
                        A.totalhargadasar = COALESCE(B.subhargadasar,0),
                        A.totalpic = COALESCE(B.totalpic,0), 
                        A.dpp = COALESCE(B.total,0)
                    WHERE A.kode = @quotation";

            OswDataAccess.executeVoidQuery(query, parameters, command);
        }

        private void valDetail() {
            if(double.Parse(this.grandtotal) <= 0) {
                throw new Exception("Grand Total harus > 0");
            }
        }
    }
}

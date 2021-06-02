using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using System.Data;
using OswLib;
using OmahSoftware.OswLib;
using MySql.Data.MySqlClient;
using OmahSoftware.Sistem;

namespace OmahSoftware.Umum {
    public class ComboQueryUmum {
        public static SearchLookUpEdit getUserGroup(SearchLookUpEdit combo, MySqlCommand command, Boolean pilihanSemua = false) {
            String query = @"SELECT 100 AS Urutan, kode AS Kode,nama AS Nama 
                            FROM oswusergroup";

            String queryTambahan = "";

            if(pilihanSemua) {
                queryTambahan += "SELECT 10 AS Urutan, '%' AS Kode, '[Semua]' AS Nama UNION ";
            }

            query = queryTambahan + query;

            query = @"SELECT * FROM (" + query + ") Z ORDER BY Urutan, Nama";

            return OswCombo.getComboQuery(combo, command, query, new Dictionary<String, String>(), new String[] { "Kode", "Urutan" }, "Kode", "Nama");

        }

        public static LookUpEdit getUserGroup(LookUpEdit combo, MySqlCommand command, Boolean pilihanSemua = false) {
            String query = @"SELECT 100 AS Urutan, kode AS Kode,nama AS Nama 
                            FROM oswusergroup";

            String queryTambahan = "";

            if(pilihanSemua) {
                queryTambahan += "SELECT 10 AS Urutan, '%' AS Kode, '[Semua]' AS Nama UNION ";
            }

            query = queryTambahan + query;

            query = @"SELECT * FROM (" + query + ") Z ORDER BY Urutan, Nama";

            return OswCombo.getComboQuery(combo, command, query, new Dictionary<String, String>(), new String[] { "Kode", "Urutan" }, "Kode", "Nama");

        }

        public static SearchLookUpEdit getUser(SearchLookUpEdit combo, MySqlCommand command, Boolean pilihanSemua = false) {
            String query = @"SELECT 100 AS Urutan, kode AS Kode,nama AS Nama 
                            FROM oswuser";

            String queryTambahan = "";

            if(pilihanSemua) {
                queryTambahan += "SELECT 10 AS Urutan, '%' AS Kode, '[Semua]' AS Nama UNION ";
            }

            query = queryTambahan + query;

            query = @"SELECT * FROM (" + query + ") Z ORDER BY Urutan, Nama";

            return OswCombo.getComboQuery(combo, command, query, new Dictionary<String, String>(), new String[] { "Kode", "Urutan" }, "Kode", "Nama");
        }

        public static SearchLookUpEdit getJenisDokumen(SearchLookUpEdit combo, MySqlCommand command, Boolean pilihanSemua = false, Boolean belumDibuatFormatnya = false) {
            String query = "";
            if(belumDibuatFormatnya) {
                query = @"SELECT 100 AS Urutan, kode AS Kode,nama AS Nama 
                            FROM oswjenisdokumen
                            WHERE kode NOT IN (SELECT DISTINCT jenis FROM oswformatdokumen)";

                String queryTambahan = "";

                if(pilihanSemua) {
                    queryTambahan += "SELECT 10 AS Urutan, '%' AS Kode, '[Semua]' AS Nama UNION ";
                }

                query = queryTambahan + query;
            } else {
                query = @"SELECT 100 AS Urutan, kode AS Kode,nama AS Nama 
                            FROM oswjenisdokumen";

                String queryTambahan = "";

                if(pilihanSemua) {
                    queryTambahan += "SELECT 10 AS Urutan, '%' AS Kode, '[Semua]' AS Nama UNION ";
                }

                query = queryTambahan + query;
            }

            query = @"SELECT * FROM (" + query + ") Z ORDER BY Urutan, Nama";


            return OswCombo.getComboQuery(combo, command, query, new Dictionary<String, String>(), new String[] { "Kode", "Urutan" }, "Kode", "Nama");
        }

        public static SearchLookUpEdit getAkun(SearchLookUpEdit combo, MySqlCommand command, String kelompokAkun, Boolean pilihanSemua = false) {
            String query = @"SELECT 100 AS Urutan, A.kode AS Kode,A.nama AS Nama 
                              FROM akun A
                              INNER JOIN kelompokakunsetting B ON A.kode LIKE B.akun AND A.akunkategori LIKE B.kategori AND A.akunsubkategori LIKE B.subkategori AND A.akungroup LIKE B.group AND A.akunsubgroup LIKE B.subgroup
                              WHERE B.kelompokakun = @kelompokakun AND A.status = @status";

            String queryTambahan = "";

            if(pilihanSemua) {
                queryTambahan += "SELECT 10 AS Urutan, '%' AS Kode, '[Semua]' AS Nama UNION ";
            }

            query = queryTambahan + query;

            query = @"SELECT * FROM (" + query + ") Z ORDER BY Urutan, Kode";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("status", Constants.STATUS_AKTIF);
            parameters.Add("kelompokakun", kelompokAkun);

            return OswCombo.getComboQuery(combo, command, query, parameters, new String[] { "Urutan" }, "Kode", "Nama");
        }

        public static SearchLookUpEdit getAkunKategori(SearchLookUpEdit combo, MySqlCommand command, Boolean pilihanSemua = false) {
            String query = @"SELECT 100 AS Urutan, kode AS Kode,nama AS Nama 
                            FROM akunkategori";

            String queryTambahan = "";

            if(pilihanSemua) {
                queryTambahan += "SELECT 10 AS Urutan, '%' AS Kode, '[Semua]' AS Nama UNION ";
            }

            query = queryTambahan + query;

            query = @"SELECT * FROM (" + query + ") Z ORDER BY Urutan, Kode";

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            return OswCombo.getComboQuery(combo, command, query, parameters, new String[] { "Kode", "Urutan" }, "Kode", "Nama");
        }

        public static SearchLookUpEdit getAkunSubKategori(SearchLookUpEdit combo, MySqlCommand command, String akunkategori, Boolean pilihanSemua = false) {
            String query = @"SELECT 100 AS Urutan, kode AS Kode,nama AS Nama 
                            FROM akunsubkategori
                            WHERE akunkategori LIKE @akunkategori";

            String queryTambahan = "";

            if(pilihanSemua) {
                queryTambahan += "SELECT 10 AS Urutan, '%' AS Kode, '[Semua]' AS Nama UNION ";
            }

            query = queryTambahan + query;

            query = @"SELECT * FROM (" + query + ") Z ORDER BY Urutan, Kode";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("akunkategori", akunkategori);

            return OswCombo.getComboQuery(combo, command, query, parameters, new String[] { "Kode", "Urutan" }, "Kode", "Nama");
        }

        public static SearchLookUpEdit getAkunGroup(SearchLookUpEdit combo, MySqlCommand command, String akunkategori, String akunsubkategori, Boolean pilihanSemua = false, Boolean pilihanTidakAda = false) {
            String query = @"SELECT 100 AS Urutan, A.kode AS Kode,A.nama AS Nama 
                            FROM akungroup A
                            INNER JOIN akunsubkategori B ON A.akunsubkategori = B.kode
                            WHERE A.akunsubkategori LIKE @akunsubkategori AND B.akunkategori LIKE @akunkategori";

            String queryTambahan = "";

            if(pilihanSemua) {
                queryTambahan += "SELECT 10 AS Urutan, '%' AS Kode, '[Semua]' AS Nama UNION ";
            }

            if(pilihanTidakAda) {
                queryTambahan += "SELECT 20 AS Urutan, '[Tidak Ada]' AS Kode, '[Tidak Ada]' AS Nama UNION ";
            }

            query = queryTambahan + query;

            query = @"SELECT * FROM (" + query + ") Z ORDER BY Urutan, Kode";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("akunsubkategori", akunsubkategori);
            parameters.Add("akunkategori", akunkategori);

            return OswCombo.getComboQuery(combo, command, query, parameters, new String[] { "Kode", "Urutan" }, "Kode", "Nama");
        }

        public static SearchLookUpEdit getAkunGroupAll(SearchLookUpEdit combo, MySqlCommand command, Boolean pilihanSemua = false, Boolean pilihanTidakAda = false) {
            String query = @"SELECT 100 AS Urutan, A.kode AS Kode,A.nama AS Nama
                            FROM akungroup A";

            String queryTambahan = "";

            if(pilihanSemua) {
                queryTambahan += "SELECT 10 AS Urutan, '%' AS Kode, '[Semua]' AS Nama UNION ";
            }

            if(pilihanTidakAda) {
                queryTambahan += "SELECT 20 AS Urutan, '[Tidak Ada]' AS Kode, '[Tidak Ada]' AS Nama UNION ";
            }

            query = queryTambahan + query;

            query = @"SELECT * FROM (" + query + ") Z ORDER BY Urutan, Kode";

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            return OswCombo.getComboQuery(combo, command, query, parameters, new String[] { "Kode", "Urutan" }, "Kode", "Nama");
        }

        public static SearchLookUpEdit getAkunSubGroup(SearchLookUpEdit combo, MySqlCommand command, String akunkategori, String akunsubkategori, String akungroup, Boolean pilihanSemua = false, Boolean pilihanTidakAda = false) {
            String query = @"SELECT 100 AS Urutan, A.kode AS Kode,A.nama AS Nama 
                            FROM akunsubgroup A
                            INNER JOIN akungroup B ON A.akungroup = B.kode
                            INNER JOIN akunsubkategori C ON B.akunsubkategori = C.kode
                            WHERE B.akunsubkategori LIKE @akunsubkategori AND C.akunkategori LIKE @akunkategori AND A.akungroup LIKE @akungroup";

            String queryTambahan = "";

            if(pilihanSemua) {
                queryTambahan += "SELECT 10 AS Urutan, '%' AS Kode, '[Semua]' AS Nama UNION ";
            }

            if(pilihanTidakAda) {
                queryTambahan += "SELECT 20 AS Urutan, '[Tidak Ada]' AS Kode, '[Tidak Ada]' AS Nama UNION ";
            }

            query = queryTambahan + query;

            query = @"SELECT * FROM (" + query + ") Z ORDER BY Urutan, Kode";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("akunsubkategori", akunsubkategori);
            parameters.Add("akunkategori", akunkategori);
            parameters.Add("akungroup", akungroup);

            return OswCombo.getComboQuery(combo, command, query, parameters, new String[] { "Kode", "Urutan" }, "Kode", "Nama");
        }

        public static SearchLookUpEdit getKelompokAkun(SearchLookUpEdit combo, MySqlCommand command, Boolean pilihanSemua = false, Boolean belumDibuatSettingnya = false) {
            String query = "";
            if(belumDibuatSettingnya) {
                query = @"SELECT 100 AS Urutan, kode AS Kode,nama AS Nama 
                            FROM kelompokakun
                            WHERE kode NOT IN (SELECT DISTINCT kelompokakun FROM kelompokakunsetting)";
            } else {
                query = @"SELECT 100 AS Urutan, kode AS Kode,nama AS Nama 
                            FROM kelompokakun";
            }

            String queryTambahan = "";

            if(pilihanSemua) {
                queryTambahan += "SELECT 10 AS Urutan, '%' AS Kode, '[Semua]' AS Nama UNION ";
            }

            query = queryTambahan + query;

            query = @"SELECT * FROM (" + query + ") Z ORDER BY Urutan, Nama";

            return OswCombo.getComboQuery(combo, command, query, new Dictionary<String, String>(), new String[] { "Kode", "Urutan" }, "Kode", "Nama");
        }


        public static LookUpEdit getJabatan(LookUpEdit combo, MySqlCommand command, Boolean pilihanSemua = false) {
            String query = @"SELECT 100 AS Urutan, kode AS Kode,nama AS Nama 
                            FROM jabatan";

            String queryTambahan = "";

            if(pilihanSemua) {
                queryTambahan += "SELECT 10 AS Urutan, '%' AS Kode, '[Semua]' AS Nama UNION ";
            }

            query = queryTambahan + query;

            query = @"SELECT * FROM (" + query + ") Z ORDER BY Urutan, Nama";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("status", Constants.STATUS_AKTIF);

            return OswCombo.getComboQuery(combo, command, query, parameters, new String[] { "Kode", "Urutan" }, "Kode", "Nama");
        }

        public static LookUpEdit getUnit(LookUpEdit combo, MySqlCommand command, Boolean pilihanSemua = false)
        {
            String query = @"SELECT 100 AS Urutan, kode AS Kode,nama AS Nama 
                            FROM unit";

            String queryTambahan = "";

            if (pilihanSemua)
            {
                queryTambahan += "SELECT 10 AS Urutan, '%' AS Kode, '[Semua]' AS Nama UNION ";
            }

            query = queryTambahan + query;

            query = @"SELECT * FROM (" + query + ") Z ORDER BY Urutan, Nama";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("status", Constants.STATUS_AKTIF);

            return OswCombo.getComboQuery(combo, command, query, parameters, new String[] { "Kode", "Urutan" }, "Kode", "Nama");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using System.Data;
using OswLib;
using MySql.Data.MySqlClient;
using Kontenu.OswLib;
using DevExpress.XtraEditors.Repository;

namespace Kontenu.Umum {
    class ComboConstantUmum {
        public static LookUpEdit getStatusAktif(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add("1", "Aktif");
            isi.Add("0", "Tidak Aktif");

            return OswCombo.getComboConstant(combo, isi);
        }

        public static LookUpEdit getStatusYa(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add(Constants.STATUS_YA, Constants.STATUS_YA);
            isi.Add(Constants.STATUS_TIDAK, Constants.STATUS_TIDAK);

            return OswCombo.getComboConstant(combo, isi);
        }

        public static LookUpEdit getStatusSelesai(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add("Selesai", "Selesai");
            isi.Add("Belum", "Belum");

            return OswCombo.getComboConstant(combo, isi);
        }

        public static LookUpEdit getBulan(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add("01", "Januari");
            isi.Add("02", "Februari");
            isi.Add("03", "Maret");
            isi.Add("04", "April");
            isi.Add("05", "Mei");
            isi.Add("06", "Juni");
            isi.Add("07", "Juli");
            isi.Add("08", "Agustus");
            isi.Add("09", "September");
            isi.Add("10", "Oktober");
            isi.Add("11", "November");
            isi.Add("12", "Desember");

            return OswCombo.getComboConstant(combo, isi);
        }

        public static LookUpEdit getTahun(LookUpEdit combo, int kurangTahun = 0, int tambahTahun = 0) {
            Dictionary<String, String> isi = new Dictionary<String, String>();

            int tahunSekarang = int.Parse(OswDate.getTahun(OswDate.getStringTanggalHariIni()));
            int tahunAwal = tahunSekarang - kurangTahun;
            int tahunAkhir = tahunSekarang + tambahTahun;

            for(int i = tahunAwal; i <= tahunAkhir; i++) {
                isi.Add(i.ToString(), i.ToString());
            }

            return OswCombo.getComboConstant(combo, isi);
        }

        public static LookUpEdit getJenisSaldoAkun(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add("Debit", "Debit");
            isi.Add("Kredit", "Kredit");

            return OswCombo.getComboConstant(combo, isi);
        }

        public static LookUpEdit getJenisCosting(LookUpEdit combo, Boolean pilihanSemua = false)
        {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if (pilihanSemua)
            {
                isi.Add("%", "[Semua]");
            }

            isi.Add("Indirect", "Indirect");
            isi.Add("Direct", "Direct");

            return OswCombo.getComboConstant(combo, isi);
        }
    }
}

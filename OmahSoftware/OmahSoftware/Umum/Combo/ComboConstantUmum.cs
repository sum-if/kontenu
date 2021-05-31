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
using OmahSoftware.OswLib;
using DevExpress.XtraEditors.Repository;

namespace OmahSoftware.Umum {
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

        public static LookUpEdit getAgama(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add("Islam", "Islam");
            isi.Add("Kristen", "Kristen");
            isi.Add("Katolik", "Katolik");
            isi.Add("Hindu", "Hindu");
            isi.Add("Budha", "Budha");
            isi.Add("Konghucu", "Konghucu");

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

        public static RepositoryItemLookUpEdit getTipeFormatDokumen(RepositoryItemLookUpEdit combo) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            isi.Add("1", "Variable");
            isi.Add("2", "Konstanta");
            isi.Add("3", "Counter");

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

        public static LookUpEdit getStatusFakturPembelian(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add(Constants.STATUS_FAKTUR_PEMBELIAN_BELUM_LUNAS, Constants.STATUS_FAKTUR_PEMBELIAN_BELUM_LUNAS);
            isi.Add(Constants.STATUS_FAKTUR_PEMBELIAN_LUNAS, Constants.STATUS_FAKTUR_PEMBELIAN_LUNAS);

            return OswCombo.getComboConstant(combo, isi);
        }

        public static LookUpEdit getStatusPesananPembelian(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add(Constants.STATUS_PESANAN_PEMBELIAN_DALAM_PROSES, Constants.STATUS_PESANAN_PEMBELIAN_DALAM_PROSES);
            isi.Add(Constants.STATUS_PESANAN_PEMBELIAN_SELESAI, Constants.STATUS_PESANAN_PEMBELIAN_SELESAI);

            return OswCombo.getComboConstant(combo, isi);
        }

        public static LookUpEdit getStatusPesananPenjualan(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add(Constants.STATUS_PESANAN_PENJUALAN_DALAM_PROSES, Constants.STATUS_PESANAN_PENJUALAN_DALAM_PROSES);
            isi.Add(Constants.STATUS_PESANAN_PENJUALAN_SELESAI, Constants.STATUS_PESANAN_PENJUALAN_SELESAI);

            return OswCombo.getComboConstant(combo, isi);
        }

        public static LookUpEdit getStatusFakturPenjualan(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add(Constants.STATUS_FAKTUR_PENJUALAN_BELUM_LUNAS, Constants.STATUS_FAKTUR_PENJUALAN_BELUM_LUNAS);
            isi.Add(Constants.STATUS_FAKTUR_PENJUALAN_LUNAS, Constants.STATUS_FAKTUR_PENJUALAN_LUNAS);

            return OswCombo.getComboConstant(combo, isi);
        }

        public static LookUpEdit getStatusBackOrder(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add(Constants.STATUS_BACK_ORDER_DALAM_PROSES, Constants.STATUS_BACK_ORDER_DALAM_PROSES);
            isi.Add(Constants.STATUS_BACK_ORDER_SELESAI, Constants.STATUS_BACK_ORDER_SELESAI);

            return OswCombo.getComboConstant(combo, isi);
        }

        public static LookUpEdit getStatusBarangAktif(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add("Aktif", "Aktif");
            isi.Add("Tidak Aktif", "Tidak Aktif");

            return OswCombo.getComboConstant(combo, isi);
        }

        public static LookUpEdit getJenisPenerimaanPenjualan(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add(Constants.JENIS_PENERIMAAN_PENJUALAN_TUNAI, Constants.JENIS_PENERIMAAN_PENJUALAN_TUNAI);
            isi.Add(Constants.JENIS_PENERIMAAN_PENJUALAN_CEK, Constants.JENIS_PENERIMAAN_PENJUALAN_CEK);

            return OswCombo.getComboConstant(combo, isi);
        }

        public static LookUpEdit getJenisPPN(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add(Constants.JENIS_PPN_EXCLUDE_PPN, Constants.JENIS_PPN_EXCLUDE_PPN);
            isi.Add(Constants.JENIS_PPN_INCLUDE_PPN, Constants.JENIS_PPN_INCLUDE_PPN);
            isi.Add(Constants.JENIS_PPN_NON_PPN, Constants.JENIS_PPN_NON_PPN);

            return OswCombo.getComboConstant(combo, isi);
        }

        public static LookUpEdit getJenisSettlementCheque(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add(Constants.JENIS_SETTLEMENT_CHEQUE_TERIMA, Constants.JENIS_SETTLEMENT_CHEQUE_TERIMA);
            isi.Add(Constants.JENIS_SETTLEMENT_CHEQUE_TOLAK, Constants.JENIS_SETTLEMENT_CHEQUE_TOLAK);

            return OswCombo.getComboConstant(combo, isi);
        }

        public static LookUpEdit getStatusFakturPajak(LookUpEdit combo, Boolean pilihanSemua = false) {
            Dictionary<String, String> isi = new Dictionary<String, String>();
            if(pilihanSemua) {
                isi.Add("%", "[Semua]");
            }

            isi.Add(Constants.STATUS_FAKTUR_PAJAK_SUDAH, Constants.STATUS_FAKTUR_PAJAK_SUDAH);
            isi.Add(Constants.STATUS_FAKTUR_PAJAK_BELUM, Constants.STATUS_FAKTUR_PAJAK_BELUM);

            return OswCombo.getComboConstant(combo, isi);
        }
    }
}

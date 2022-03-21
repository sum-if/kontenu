using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontenu.OswLib {
    class Constants {
        public static String AES_PASSWORD = "Kontenu123";

        public static String TIDAK_MUNGKIN = "!@&^%$#^(&^!*&^";

        public static String FORMAT_TANGGAL_BULAN_TAHUN_TRANSAKSI = "MMyy";
        public static String FORMAT_TANGGAL_TAHUN = "yyyy";

        public static String PERUSAHAAN_KONTENU = "KONTENU";

        // Jangan lupa ubah di tutup periode
        public static String PROSES_TUTUP_PERIODE = "Tutup Periode";

        public static String TIDAK_ADA = "[Tidak Ada]";

        public static String STATUS_SELESAI = "Selesai";
        public static String STATUS_SELESAI_BELUM = "Belum Selesai";

        public static String STATUS_YA = "Ya";
        public static String STATUS_TIDAK = "Tidak";

        public static String STATUS_AKTIF = "Aktif";
        public static String STATUS_AKTIF_TIDAK = "Tidak Aktif";

        public static String STATUS_SUDAH = "Sudah";
        public static String STATUS_SUDAH_BELUM = "Belum";

        public static String STATUS_QUOTATION_PROSES = "Proses";
        public static String STATUS_QUOTATION_TUTUP = "Tutup";

        public static String STATUS_INVOICE_PROSES = "Proses";
        public static String STATUS_INVOICE_TUTUP = "Tutup";

        public static String STATUS_PROYEK_AKTIF = "Aktif";
        public static String STATUS_PROYEK_TIDAK_AKTIF = "Tidak Aktif";

        public static String JENIS_INVOICE_INTERIOR = "Interior";
        public static String JENIS_INVOICE_PRODUCT = "Product";

        public static String JENIS_SUBCON_INTERNAL = "Internal";
        public static String JENIS_SUBCON_EXTERNAL = "External";

        public static String STATUS_PENAGIHAN_BELUM_BAYAR = "Belum Bayar";
        public static String STATUS_PENAGIHAN_SUDAH_BAYAR = "Sudah Bayar";

        public static String STATUS_PURCHASE_BELUM_LUNAS = "Belum Lunas";
        public static String STATUS_PURCHASE_LUNAS = "Lunas";


        // AKUN KONSTAN
        public static String AKUN_DESIGN_AKUN_PIUTANG = "design_akunpiutang";
        public static String AKUN_DESIGN_AKUN_EARNED = "design_akununearned";
        public static String AKUN_DESIGN_AKUN_ACCRUED = "design_akunaccrued";
        public static String AKUN_DESIGN_AKUN_HUTANG_OUTSOURCE = "design_akunhutangoutsource";


        // AKUN LIST
        public static String AKUN_LIST_SEMUA = "akun_semua";
        public static String AKUN_LIST_PENERIMAAN_INVOICE = "akun_penerimaan_invoice";
        public static String AKUN_LIST_PURCHASE_PAYMENT = "akun_purchase_payment";
    }
}

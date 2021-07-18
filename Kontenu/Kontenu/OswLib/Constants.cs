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


        // TIDAK BOLEH DI ComboQueryUmum.getAkun
        // TIDAK BOLEH DI Tools.isKelompokAkun
        public static String AKUN_PERSEDIAAN = "akun_persediaan";
        public static String AKUN_PENJUALAN = "akun_penjualan";
        public static String AKUN_HPP = "akun_hpp";
        public static String AKUN_RETUR_PENJUALAN = "akun_retur_penjualan";
        public static String AKUN_DISKON_PENJUALAN = "akun_diskon_penjualan";
        public static String AKUN_HUTANG = "akun_hutang";
        public static String AKUN_PAJAK_MASUKAN = "akun_pajak_masukan";
        public static String AKUN_PAJAK_KELUARAN = "akun_pajak_keluaran";
        public static String AKUN_PENYESUAIAN_PERSEDIAAN = "akun_penyesuaian_persediaan";
        public static String AKUN_PIUTANG = "akun_piutang";
        public static String AKUN_POST_DATED_CHEQUE = "akun_post_dated_cheque";
        public static String AKUN_UANG_TITIPAN_SUPPLIER = "akun_uang_titipan_supplier";
        public static String AKUN_UANG_TITIPAN_CUSTOMER = "akun_uang_titipan_customer";
        public static String AKUN_PEMBULATAN_BIAYA = "akun_pembulatan_biaya";
        public static String AKUN_PEMBULATAN_PENDAPATAN = "akun_pembulatan_pendapatan";
        public static String AKUN_HUTANG_EKSPEDISI = "akun_hutang_ekspedisi";
        public static String AKUN_BIAYA_EKSPEDISI = "akun_biaya_ekspedisi";
    }
}

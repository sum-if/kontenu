using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmahSoftware.OswLib {
    class Constants {
        public static String AES_PASSWORD = "OmahSoftware123";

        public static String TIDAK_MUNGKIN = "!@&^%$#^(&^!*&^";

        public static String FORMAT_TANGGAL_BULAN_TAHUN_TRANSAKSI = "MMyy";
        public static String FORMAT_TANGGAL_TAHUN = "yyyy";

        // Jangan lupa ubah di tutup periode
        public static String PROSES_TUTUP_PERIODE = "Tutup Periode";

        public static String TIDAK_ADA = "[Tidak Ada]";

        public static String STATUS_SELESAI = "Selesai";
        public static String STATUS_SELESAI_BELUM = "Belum Selesai";

        public static String STATUS_YA = "Ya";
        public static String STATUS_TIDAK = "Tidak";

        public static String STATUS_1 = "1";
        public static String STATUS_0 = "0";

        public static String STATUS_AKTIF = "Aktif";
        public static String STATUS_AKTIF_TIDAK = "Tidak Aktif";

        public static String STATUS_SUDAH = "Sudah";
        public static String STATUS_SUDAH_BELUM = "Belum";

        public static String STATUS_PESANAN_PEMBELIAN_DALAM_PROSES = "Dalam Proses";
        public static String STATUS_PESANAN_PEMBELIAN_SELESAI = "Selesai";

        public static String STATUS_FAKTUR_PEMBELIAN_BELUM_LUNAS = "Belum Lunas";
        public static String STATUS_FAKTUR_PEMBELIAN_LUNAS = "Lunas";

        public static String STATUS_PESANAN_PENJUALAN_DALAM_PROSES = "Dalam Proses";
        public static String STATUS_PESANAN_PENJUALAN_SELESAI = "Selesai";

        public static String STATUS_BACK_ORDER_DALAM_PROSES = "Dalam Proses";
        public static String STATUS_BACK_ORDER_SELESAI = "Selesai";

        public static String JENIS_PESANAN_PENJUALAN_PESANAN_PENJUALAN = "Pesanan Penjualan";
        public static String JENIS_PESANAN_PENJUALAN_BACK_ORDER = "Back Order";

        public static String STATUS_FAKTUR_PENJUALAN_BELUM_LUNAS = "Belum Lunas";
        public static String STATUS_FAKTUR_PENJUALAN_LUNAS = "Lunas";

        public static String JENIS_PENERIMAAN_PENJUALAN_TUNAI = "Tunai/Bank";
        public static String JENIS_PENERIMAAN_PENJUALAN_CEK = "Cheque";

        public static String JENIS_PEMBAYARAN_PEMBELIAN_TUNAI = "Tunai/Bank";
        public static String JENIS_PEMBAYARAN_PEMBELIAN_CEK = "Cheque";

        public static String JENIS_PPN_NON_PPN = "Non PPN";
        public static String JENIS_PPN_INCLUDE_PPN = "Include PPN";
        public static String JENIS_PPN_EXCLUDE_PPN = "Exclude PPN";

        public static String STATUS_CEK_TIDAK_ADA = "Tidak Ada Cheque";
        public static String STATUS_CEK_MENUNGGU = "Menunggu";
        public static String STATUS_CEK_DIBATALKAN = "Dibatalkan";
        public static String STATUS_CEK_SELESAI = "Selesai";

        public static String STATUS_FAKTUR_PAJAK_SUDAH = "Sudah";
        public static String STATUS_FAKTUR_PAJAK_BELUM = "Belum";

        

        public static String JENIS_SETTLEMENT_CHEQUE_TERIMA = "Terima";
        public static String JENIS_SETTLEMENT_CHEQUE_TOLAK = "Tolak";

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

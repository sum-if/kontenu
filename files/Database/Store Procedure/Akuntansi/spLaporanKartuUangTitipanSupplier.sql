/*
	Author		: Santos Sabanari
    Date		: 28-03-2019
    Description	: Untuk Laporan Kartu Uang Titipan Supplier
	Example		: CALL spLaporanKartuUangTitipanSupplier('123','02/07/2019','30/10/2020','%');

DROP TABLE IF EXISTS `templaporankartuuangtitipansupplier`;
CREATE TABLE IF NOT EXISTS `templaporankartuuangtitipansupplier` (
  `guid` VARCHAR(300),
  `supplier` VARCHAR(45),
  `jenis` VARCHAR(45),
  `tanggal` VARCHAR(45),
  `referensi` VARCHAR(45),
  `keterangan` TEXT,
  `mutasi` DOUBLE,
  `waktu` TIMESTAMP)
ENGINE = InnoDB;
*/

DROP PROCEDURE IF EXISTS spLaporanKartuUangTitipanSupplier;

DELIMITER //

CREATE PROCEDURE spLaporanKartuUangTitipanSupplier(
	IN varGuid VARCHAR(1000),
    IN varTanggalAwal VARCHAR(1000),
    IN varTanggalAkhir VARCHAR(1000),
    IN varSupplier VARCHAR(1000)
)
BEGIN
	DELETE FROM templaporankartuuangtitipansupplier WHERE guid = varGuid;

    -- Masukan di kartu uangtitipansupplier untuk saldo awalnya
    INSERT INTO templaporankartuuangtitipansupplier (guid,supplier,jenis,tanggal,referensi,keterangan,mutasi, waktu)
    SELECT varGuid,A.supplier, 'A', toTanggalKurang(varTanggalAwal,1), '', 'Saldo Awal', COALESCE(SUM(A.jumlah),0), CURRENT_TIMESTAMP()
    FROM mutasiuangtitipansupplier A
    WHERE toDate(A.tanggal) < toDate(varTanggalAwal) AND A.supplier LIKE varSupplier
    GROUP BY A.supplier;
    
	-- Cari Mutasi Tambah Periode
    INSERT INTO templaporankartuuangtitipansupplier (guid,supplier,jenis,tanggal,referensi,keterangan,mutasi, waktu)
    SELECT varGuid,A.supplier,'B',A.tanggal, A.noreferensi, B.nama, A.jumlah, A.create_at
    FROM mutasiuangtitipansupplier A
    LEFT JOIN oswjenisdokumen B ON A.oswjenisdokumen = B.kode
    WHERE toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir) AND A.supplier LIKE varSupplier;
    
	-- Kembalikan Hasilnya
    SELECT A.supplier AS Supplier, CONCAT(B.kode,' - ',B.nama,' [',C.nama,']') AS Nama, A.tanggal AS Tanggal, A.keterangan AS 'Transaksi', A.referensi AS Referensi, A.mutasi AS Mutasi
    FROM templaporankartuuangtitipansupplier A
    INNER JOIN supplier B ON A.supplier = B.kode
    INNER JOIN kota C ON B.kota = C.kode
    WHERE A.guid = varGuid
    ORDER BY A.supplier, A.jenis, toTahunBulanTanggal(A.tanggal), A.waktu, A.referensi;
    
    DELETE FROM templaporankartuuangtitipansupplier WHERE guid = varGuid;
	
END //
 DELIMITER ;
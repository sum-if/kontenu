/*
	Author		: Santos Sabanari
    Date		: 28-03-2019
    Description	: Untuk Laporan Kartu Hutang
	Example		: CALL spLaporanKartuHutang('123','01/10/2016','30/10/2020','%');

DROP TABLE IF EXISTS `templaporankartuhutang`;
CREATE TABLE IF NOT EXISTS `templaporankartuhutang` (
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

DROP PROCEDURE IF EXISTS spLaporanKartuHutang;

DELIMITER //

CREATE PROCEDURE spLaporanKartuHutang(
	IN varGuid VARCHAR(1000),
    IN varTanggalAwal VARCHAR(1000),
    IN varTanggalAkhir VARCHAR(1000),
    IN varSupplier VARCHAR(1000)
)
BEGIN
    -- Masukan di kartu hutang untuk saldo awalnya
    INSERT INTO templaporankartuhutang (guid,supplier,jenis,tanggal,referensi,keterangan,mutasi, waktu)
    SELECT varGuid, A.supplier, 'A', toTanggalKurang(varTanggalAwal,1), '', 'Saldo Awal', ROUND(COALESCE(SUM(A.jumlah),0),2), CURRENT_TIMESTAMP()
    FROM mutasihutang A
    WHERE toDate(A.tanggal) < toDate(varTanggalAwal) AND A.supplier LIKE varSupplier
    GROUP BY A.supplier;
    
	-- Cari Mutasi Tambah Periode
    INSERT INTO templaporankartuhutang (guid,supplier,jenis,tanggal,referensi,keterangan,mutasi, waktu)
    SELECT varGuid, A.supplier,'B',A.tanggal, A.noreferensi, B.nama, A.jumlah, A.create_at
    FROM mutasihutang A
    LEFT JOIN oswjenisdokumen B ON A.oswjenisdokumen = B.kode
    WHERE toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir) AND A.supplier LIKE varSupplier;
    
	-- Kembalikan Hasilnya
    SELECT A.supplier AS Supplier, CONCAT(B.kode,' - ',B.nama) AS Nama,A.tanggal AS Tanggal, A.keterangan AS 'Transaksi', A.referensi AS Referensi, A.mutasi AS Mutasi
    FROM templaporankartuhutang A
    INNER JOIN supplier B ON A.supplier = B.kode
    WHERE A.guid = varGuid
    ORDER BY A.supplier, A.jenis, toTahunBulanTanggal(A.tanggal), A.waktu, A.referensi;
    
    DELETE FROM templaporankartuhutang WHERE guid = varGuid;
	
END //
 DELIMITER ;
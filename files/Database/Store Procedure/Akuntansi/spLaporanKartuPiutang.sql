/*
	Author		: Santos Sabanari
    Date		: 28-03-2019
    Description	: Untuk Laporan Kartu Piutang
	Example		: CALL spLaporanKartuPiutang('123','01/10/2016','30/10/2019','%');

DROP TABLE IF EXISTS `templaporankartupiutang`;
CREATE TABLE IF NOT EXISTS `templaporankartupiutang` (
  `guid` VARCHAR(300),
  `customer` VARCHAR(45),
  `jenis` VARCHAR(45),
  `tanggal` VARCHAR(45),
  `referensi` VARCHAR(45),
  `keterangan` TEXT,
  `mutasi` DOUBLE,
  `waktu` TIMESTAMP)
ENGINE = InnoDB;
*/

DROP PROCEDURE IF EXISTS spLaporanKartuPiutang;

DELIMITER //

CREATE PROCEDURE spLaporanKartuPiutang(
	IN varGuid VARCHAR(1000),
    IN varTanggalAwal VARCHAR(1000),
    IN varTanggalAkhir VARCHAR(1000),
    IN varCustomer VARCHAR(1000)
)
BEGIN
    -- Masukan di kartu piutang untuk saldo awalnya
    INSERT INTO templaporankartupiutang (guid,customer,jenis,tanggal,referensi,keterangan,mutasi, waktu)
    SELECT varGuid, A.customer, 'A', toTanggalKurang(varTanggalAwal,1), '', 'Saldo Awal', ROUND(COALESCE(SUM(A.jumlah),0),2), CURRENT_TIMESTAMP()
    FROM mutasipiutang A
    WHERE toDate(A.tanggal) < toDate(varTanggalAwal) AND A.customer LIKE varCustomer
    GROUP BY A.customer;
    
	-- Cari Mutasi Tambah Periode
    INSERT INTO templaporankartupiutang (guid,customer,jenis,tanggal,referensi,keterangan,mutasi, waktu)
    SELECT varGuid,A.customer,'B',A.tanggal, A.noreferensi, B.nama, A.jumlah, A.create_at
    FROM mutasipiutang A
    LEFT JOIN oswjenisdokumen B ON A.oswjenisdokumen = B.kode
    WHERE toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir) AND A.customer LIKE varCustomer;
    
	-- Kembalikan Hasilnya
    SELECT A.customer AS Customer, CONCAT(B.kode,' - ',B.nama) AS Nama, A.tanggal AS Tanggal, A.keterangan AS 'Transaksi', A.referensi AS Referensi, A.mutasi AS Mutasi
    FROM templaporankartupiutang A
	INNER JOIN customer B ON A.customer = B.kode
    WHERE A.guid = varGuid
    ORDER BY A.customer, A.jenis, toTahunBulanTanggal(A.tanggal), A.waktu, A.referensi;
    
    DELETE FROM templaporankartupiutang WHERE guid = varGuid;
	
END //
 DELIMITER ;
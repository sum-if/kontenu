/*
	Author		: Santos Sabanari
    Date		: 18-09-2020
               
   Example		: CALL spLaporanMutasiPersediaan('123','01/08/2020','31/08/2020');
         
DROP TABLE IF EXISTS `templaporanmutasipersediaan`;
CREATE TABLE IF NOT EXISTS `templaporanmutasipersediaan` (
  `guid` VARCHAR(300),
  `oswjenisdokumen` VARCHAR(45),
  `noreferensi` VARCHAR(45),
  `no` INT,
  `barang` VARCHAR(45),
  `jumlahawal` DECIMAL(19,2) DEFAULT 0,
  `nilaiawal` DECIMAL(19,2) DEFAULT 0,
  `selisihawal` DECIMAL(19,2) DEFAULT 0,
  `jumlahmasuk` DECIMAL(19,2) DEFAULT 0,
  `nilaimasuk` DECIMAL(19,2) DEFAULT 0,
  `selisihmasuk` DECIMAL(19,2) DEFAULT 0,
  `jumlahkeluar` DECIMAL(19,2) DEFAULT 0,
  `nilaikeluar` DECIMAL(19,2) DEFAULT 0,
  `jumlahakhir` DECIMAL(19,2) DEFAULT 0,
  `nilaiakhir` DECIMAL(19,2) DEFAULT 0)
ENGINE = InnoDB;

DROP TABLE IF EXISTS `templaporanmutasipersediaan2`;
CREATE TABLE IF NOT EXISTS `templaporanmutasipersediaan2` (
  `guid` VARCHAR(300),
  `barang` VARCHAR(45),
  `jumlahawal` DECIMAL(19,2) DEFAULT 0,
  `nilaiawal` DECIMAL(19,2) DEFAULT 0,
  `jumlahmasuk` DECIMAL(19,2) DEFAULT 0,
  `nilaimasuk` DECIMAL(19,2) DEFAULT 0,
  `jumlahkeluar` DECIMAL(19,2) DEFAULT 0,
  `nilaikeluar` DECIMAL(19,2) DEFAULT 0,
  `jumlahakhir` DECIMAL(19,2) DEFAULT 0,
  `nilaiakhir` DECIMAL(19,2) DEFAULT 0)
ENGINE = InnoDB;
*/
DROP PROCEDURE IF EXISTS spLaporanMutasiPersediaan;

DELIMITER //

CREATE PROCEDURE spLaporanMutasiPersediaan(
	IN varGuid VARCHAR(100),
    IN varTanggalAwal VARCHAR(100),
    IN varTanggalAkhir VARCHAR(100)
)
BEGIN
    
    DELETE FROM templaporanmutasipersediaan WHERE guid = varGuid;
    DELETE FROM templaporanmutasipersediaan2 WHERE guid = varGuid;
        
    -- Ambil saldo sebelumnya
    INSERT INTO templaporanmutasipersediaan(guid,oswjenisdokumen,noreferensi,no,barang,jumlahawal)
    SELECT varGuid, A.oswjenisdokumen, A.noreferensi, A.no,A.barang, A.jumlah
    FROM mutasipersediaan A
    WHERE toDate(A.tanggal) < toDate(varTanggalAwal);
    
    --  Cari mutasi masuk dari tanggal awal sampai tanggal akhir
    INSERT INTO templaporanmutasipersediaan(guid,oswjenisdokumen,noreferensi,no,barang,jumlahmasuk)
    SELECT varGuid, A.oswjenisdokumen, A.noreferensi, A.no,A.barang, A.jumlah
    FROM mutasipersediaan A
    WHERE toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir) AND A.jumlah > 0;
    
    --  Cari mutasi keluar dari tanggal awal sampai tanggal akhir
    INSERT INTO templaporanmutasipersediaan(guid,oswjenisdokumen,noreferensi,no,barang,jumlahkeluar)
    SELECT varGuid, A.oswjenisdokumen, A.noreferensi, A.no,A.barang, -A.jumlah
    FROM mutasipersediaan A
    WHERE toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir) AND A.jumlah < 0;
    
    -- FAKTURPEMBELIAN
    UPDATE templaporanmutasipersediaan A
    INNER JOIN fakturpembeliandetail B ON A.noreferensi = B.fakturpembelian AND A.no = B.no
    SET A.nilaimasuk = B.dpp,
		A.nilaiawal = B.dpp
    WHERE A.guid = varGuid AND A.oswjenisdokumen = 'FAKTURPEMBELIAN';
    
    -- Selisih dpp dari pembelian
    UPDATE templaporanmutasipersediaan A
    INNER JOIN (
		SELECT A.fakturpembelian, A.no, B.selisih
		FROM fakturpembeliandetail A
		INNER JOIN (
			SELECT A.kode AS fakturpembelian, ROUND(A.totaldpp - B.totaldpp,2) AS selisih
			FROM fakturpembelian A
			INNER JOIN (
				SELECT B.fakturpembelian, ROUND(SUM(B.jumlah * B.dpp),2) AS totaldpp
				FROM fakturpembelian A
				INNER JOIN fakturpembeliandetail B ON A.kode = B.fakturpembelian
				GROUP BY B.fakturpembelian
			) B ON A.kode = B.fakturpembelian
			WHERE A.totaldpp != B.totaldpp
		) B ON A.fakturpembelian = B.fakturpembelian AND A.no = 1
    ) B ON A.noreferensi = B.fakturpembelian AND A.no = B.no
    SET A.selisihmasuk = IF(A.jumlahmasuk = 0, 0, B.selisih),
		A.selisihawal  = IF(A.jumlahawal = 0, 0, B.selisih)
    WHERE A.guid = varGuid AND A.oswjenisdokumen = 'FAKTURPEMBELIAN';
    
    -- FAKTURPENJUALAN
    UPDATE templaporanmutasipersediaan A
    INNER JOIN fakturpenjualandetail B ON A.noreferensi = B.fakturpenjualan AND A.no = B.no
    SET A.nilaikeluar = B.hpp,
		A.nilaiawal = B.hpp
    WHERE A.guid = varGuid AND A.oswjenisdokumen = 'FAKTURPENJUALAN';
    
	-- RETURPEMBELIAN
    UPDATE templaporanmutasipersediaan A
    INNER JOIN returpembeliandetail B ON A.noreferensi = B.returpembelian AND A.no = B.no
    SET A.nilaikeluar = B.dpp,
		A.nilaiawal = B.dpp
    WHERE A.guid = varGuid AND A.oswjenisdokumen = 'RETURPEMBELIAN';
    
    -- RETURPENJUALAN
    UPDATE templaporanmutasipersediaan A
    INNER JOIN returpenjualandetail B ON A.noreferensi = B.returpenjualan AND A.no = B.no
    SET A.nilaimasuk = B.hpp,
		A.nilaiawal = B.hpp
    WHERE A.guid = varGuid AND A.oswjenisdokumen = 'RETURPENJUALAN';
    
    -- JADIKAN 1
    INSERT INTO templaporanmutasipersediaan2(guid,barang,jumlahawal,nilaiawal,jumlahmasuk,nilaimasuk,jumlahkeluar,nilaikeluar)
    SELECT A.guid, A.barang, SUM(A.jumlahawal),ROUND(SUM((A.jumlahawal*nilaiawal)+ A.selisihawal),2), SUM(A.jumlahmasuk),ROUND(SUM((A.jumlahmasuk*nilaimasuk) + A.selisihmasuk),2), SUM(A.jumlahkeluar),ROUND(SUM(A.jumlahkeluar*nilaikeluar),2)
    FROM templaporanmutasipersediaan A
    WHERE A.guid = varGuid
    GROUP BY A.guid, A.barang;
    
    -- UPDATE akhir
    UPDATE templaporanmutasipersediaan2 A
    SET A.jumlahakhir = ROUND((A.jumlahawal + A.jumlahmasuk - A.jumlahkeluar),2),
		A.nilaiakhir = ROUND((A.nilaiawal + A.nilaimasuk - A.nilaikeluar),2)
    WHERE A.guid = varGuid;
    
	-- HASIL
	SELECT A.barang AS Barang, CONCAT(C.kode,' - ', C.nama,' , No Part : ', IF(C.nopart='','-',C.nopart),' , Rak : ', D.nama) AS Nama, A.jumlahawal AS 'Jumlah Awal', A.nilaiawal AS 'Nilai Awal', 
		   A.jumlahmasuk AS 'Jumlah Masuk', A.nilaimasuk AS 'Nilai Masuk', A.jumlahkeluar AS 'Jumlah Keluar', A.nilaikeluar AS 'Nilai Keluar', A.jumlahakhir AS 'Jumlah Akhir', A.nilaiakhir AS 'Nilai Akhir'
    FROM templaporanmutasipersediaan2 A
    INNER JOIN barang C ON A.barang = C.kode
    INNER JOIN barangrak D ON C.rak = D.kode
    WHERE A.guid = varGuid
    ORDER BY A.barang;
    
	DELETE FROM templaporanmutasipersediaan WHERE guid = varGuid;
    DELETE FROM templaporanmutasipersediaan2 WHERE guid = varGuid;
END //
 DELIMITER ;
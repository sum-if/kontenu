/*            
   Example		: CALL spLaporanAnalisaHPP('123','01/07/2019','31/07/2019');
   
DROP TABLE IF EXISTS `templaporananalisahpp`;
CREATE TABLE IF NOT EXISTS `templaporananalisahpp` (
  `guid` VARCHAR(300),
  `barang` VARCHAR(45),
  `jumlah` DOUBLE,
  `harga` DOUBLE,
  `hpp` DOUBLE)
ENGINE = InnoDB;

DROP TABLE IF EXISTS `templaporananalisahpphasil`;
CREATE TABLE IF NOT EXISTS `templaporananalisahpphasil` (
  `guid` VARCHAR(300),
  `urutan` INT,
  `barang` VARCHAR(45),
  `harga` DOUBLE,
  `hpp` DOUBLE,
  `selisih` DOUBLE,
  `margin` DOUBLE)
ENGINE = InnoDB;
*/
DROP PROCEDURE IF EXISTS spLaporanAnalisaHPP;

DELIMITER //

CREATE PROCEDURE spLaporanAnalisaHPP(
	IN varGuid VARCHAR(100),
    IN varTanggalAwal VARCHAR(100),
    IN varTanggalAkhir VARCHAR(100)
)
BEGIN
	DELETE FROM templaporananalisahpp WHERE guid = varGuid;
    DELETE FROM templaporananalisahpphasil WHERE guid = varGuid;
    
	-- faktur penjualan
    INSERT INTO templaporananalisahpp(guid, barang,jumlah,harga,hpp)
    SELECT varGuid, A.barang, A.jumlah * -1, ROUND(B.dpp,2), ROUND(B.hpp,2)
    FROM mutasipersediaan A
    INNER JOIN fakturpenjualandetail B ON A.noreferensi = B.fakturpenjualan AND A.no = B.no
    WHERE A.oswjenisdokumen = 'FAKTURPENJUALAN' AND toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir);

    -- Groupkan
    INSERT INTO templaporananalisahpphasil(guid, urutan, barang,harga,hpp,selisih,margin)
    SELECT varGuid, 1, A.barang, ROUND(SUM(A.jumlah * A.harga),2), ROUND(SUM(A.jumlah * A.hpp),2),0,0
    FROM templaporananalisahpp A
    WHERE A.guid = varGuid
    GROUP BY A.barang;
    
    -- TOTAL 
    INSERT INTO templaporananalisahpphasil(guid, urutan, barang,harga,hpp,selisih,margin)
    SELECT varGuid, 100, '', ROUND(SUM(A.harga),2), ROUND(SUM(A.hpp),2),0,0
    FROM templaporananalisahpphasil A
    WHERE A.guid = varGuid;
    
    -- selisih
    UPDATE templaporananalisahpphasil
    SET selisih = ROUND(harga-hpp,2)
    WHERE guid = varGuid;
    
    -- margin
    UPDATE templaporananalisahpphasil
    SET margin = ROUND((selisih/hpp) * 100,2)
    WHERE guid = varGuid;
    
    -- HASIL
    SELECT A.barang AS 'Kode Barang', COALESCE(B.nama,'TOTAL') AS 'Nama Barang', A.harga AS Nilai, A.hpp AS HPP, A.margin AS Margin
    FROM templaporananalisahpphasil A
    LEFT JOIN barang B ON A.barang = B.kode
    WHERE A.guid = varGuid
    ORDER BY A.urutan, B.nama;
    
    DELETE FROM templaporananalisahpp WHERE guid = varGuid;
    DELETE FROM templaporananalisahpphasil WHERE guid = varGuid;
END //
 DELIMITER ;
 
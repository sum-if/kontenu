/*            
   Example		: CALL spLaporanRekapHutang('123','%','01/01/2019','01/09/2019');
   
DROP TABLE IF EXISTS `templaporanrekaphutang`;
CREATE TABLE IF NOT EXISTS `templaporanrekaphutang` (
  `guid` VARCHAR(300),
  `supplier` VARCHAR(45),
  `saldoawal` DOUBLE,
  `hutang` DOUBLE,
  `pembayaran` DOUBLE,
  `sisa` DOUBLE)
ENGINE = InnoDB;
*/
DROP PROCEDURE IF EXISTS spLaporanRekapHutang;

DELIMITER //

CREATE PROCEDURE spLaporanRekapHutang(
	IN varGuid VARCHAR(100),
    IN varKodeSupplier VARCHAR(100),
    IN varTanggalAwal VARCHAR(100),
    IN varTanggalAkhir VARCHAR(100)
)
BEGIN
	DELETE FROM templaporanrekaphutang WHERE guid = varGuid;
    
	-- Saldo Awal
	INSERT INTO templaporanrekaphutang(guid, supplier,saldoawal,hutang,pembayaran,sisa)
    SELECT varGuid,A.supplier, SUM(A.jumlah),0,0,0
    FROM mutasihutang A
    WHERE A.supplier LIKE varKodeSupplier AND toDate(A.tanggal) < toDate(toTanggalKurang(varTanggalAwal,1))
    GROUP BY A.supplier;
    
    -- Hutang
    INSERT INTO templaporanrekaphutang(guid, supplier,saldoawal,hutang,pembayaran,sisa)
    SELECT varGuid,A.supplier, 0, SUM(A.jumlah),0,0
    FROM mutasihutang A
    WHERE A.supplier LIKE varKodeSupplier AND toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir) AND A.jumlah > 0
    GROUP BY A.supplier;
    
    -- Pembayaran
    INSERT INTO templaporanrekaphutang(guid, supplier,saldoawal,hutang,pembayaran,sisa)
    SELECT varGuid,A.supplier, 0,0, SUM(A.jumlah * -1),0
    FROM mutasihutang A
    WHERE A.supplier LIKE varKodeSupplier AND toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir) AND A.jumlah < 0
    GROUP BY A.supplier;
    
    -- Sisa
    UPDATE templaporanrekaphutang
    SET sisa = saldoawal+hutang-pembayaran
    WHERE guid = varGuid;
    
    -- Hasil
    SELECT A.supplier AS 'Kode Supplier', B.nama AS 'Nama Supplier', SUM(A.saldoawal) AS 'Saldo Awal', SUM(A.hutang) AS Hutang, SUM(A.pembayaran) AS Pembayaran, SUM(A.sisa) AS Sisa
    FROM templaporanrekaphutang A
    INNER JOIN supplier B ON A.supplier = B.kode
    GROUP BY A.supplier;
    
    DELETE FROM templaporanrekaphutang WHERE guid = varGuid;
END //
 DELIMITER ;
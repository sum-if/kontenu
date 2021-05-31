/*            
   Example		: CALL spLaporanRekapPiutang('123','%','01/01/2019','01/09/2019');
   
DROP TABLE IF EXISTS `templaporanrekappiutang`;
CREATE TABLE IF NOT EXISTS `templaporanrekappiutang` (
  `guid` VARCHAR(300),
  `customer` VARCHAR(45),
  `saldoawal` DOUBLE,
  `piutang` DOUBLE,
  `pembayaran` DOUBLE,
  `sisa` DOUBLE)
ENGINE = InnoDB;
*/
DROP PROCEDURE IF EXISTS spLaporanRekapPiutang;

DELIMITER //

CREATE PROCEDURE spLaporanRekapPiutang(
	IN varGuid VARCHAR(100),
    IN varKodeCustomer VARCHAR(100),
    IN varTanggalAwal VARCHAR(100),
    IN varTanggalAkhir VARCHAR(100)
)
BEGIN
	DELETE FROM templaporanrekappiutang WHERE guid = varGuid;
    
	-- Saldo Awal
	INSERT INTO templaporanrekappiutang(guid, customer,saldoawal,piutang,pembayaran,sisa)
    SELECT varGuid,A.customer, SUM(A.jumlah),0,0,0
    FROM mutasipiutang A
    WHERE A.customer LIKE varKodeCustomer AND toDate(A.tanggal) < toDate(toTanggalKurang(varTanggalAwal,1))
    GROUP BY A.customer;
    
    -- Piutang
    INSERT INTO templaporanrekappiutang(guid, customer,saldoawal,piutang,pembayaran,sisa)
    SELECT varGuid,A.customer, 0, SUM(A.jumlah),0,0
    FROM mutasipiutang A
    WHERE A.customer LIKE varKodeCustomer AND toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir) AND A.jumlah > 0
    GROUP BY A.customer;
    
    -- Pembayaran
    INSERT INTO templaporanrekappiutang(guid, customer,saldoawal,piutang,pembayaran,sisa)
    SELECT varGuid,A.customer, 0,0, SUM(A.jumlah * -1),0
    FROM mutasipiutang A
    WHERE A.customer LIKE varKodeCustomer AND toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir) AND A.jumlah < 0
    GROUP BY A.customer;
    
    -- Sisa
    UPDATE templaporanrekappiutang
    SET sisa = saldoawal+piutang-pembayaran
    WHERE guid = varGuid;
    
    -- Hasil
    SELECT A.customer AS 'Kode Customer', B.nama AS 'Nama Customer', SUM(A.saldoawal) AS 'Saldo Awal', SUM(A.piutang) AS Piutang, SUM(A.pembayaran) AS Pembayaran, SUM(A.sisa) AS Sisa
    FROM templaporanrekappiutang A
    INNER JOIN customer B ON A.customer = B.kode
    GROUP BY A.customer;
    
    DELETE FROM templaporanrekappiutang WHERE guid = varGuid;
END //
 DELIMITER ;
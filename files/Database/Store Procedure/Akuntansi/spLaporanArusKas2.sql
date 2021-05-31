/*            
   Example		: CALL spLaporanArusKas2('123123','202001');
   
DROP TABLE IF EXISTS `templaporanaruskas2`;
CREATE TABLE IF NOT EXISTS `templaporanaruskas2` (
  `guid` VARCHAR(300),
  `urutankategori` INT,
  `kategori` VARCHAR(300),
  `subkategori` VARCHAR(300),
  `nilai` DOUBLE)
ENGINE = InnoDB;

*/
DROP PROCEDURE IF EXISTS spLaporanArusKas2;

DELIMITER //

CREATE PROCEDURE spLaporanArusKas2(
	IN varGuid VARCHAR(100),
    IN varPeriode VARCHAR(100)
)
BEGIN
	DECLARE varNilai DECIMAL(19,2);
    DECLARE varNilai2 DECIMAL(19,2);
    DECLARE varNilai3 DECIMAL(19,2);
    DECLARE varPeriodeSebelum TEXT;
    
    SET varPeriodeSebelum = toPeriodeSebelum(varPeriode);
    
	DELETE FROM templaporanaruskas2 WHERE guid = varGuid;
    
	-- ARUS KAS DARI AKTIVITAS OPERASI
    -- LABA ( RUGI ) BERSIH
    SET varNilai = (
		SELECT COALESCE(SUM(IF(A.penutupkredit = 0,-A.penutupdebit, A.penutupkredit)),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('3212.01')
    );
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','LABA ( RUGI ) BERSIH',varNilai;
    
    -- Penyesuaian untuk :	
	-- Kenaikan / (Penurunan) Akumulasi Penyusutan    
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('1212.04')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('1212.04')
    );
    
    SET varNilai = varNilai2 - varNilai3;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Akumulasi Penyusutan',varNilai;
    
	-- (Kenaikan) / Penurunan Persediaan
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('1141.01')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('1141.01')
    );
    
    SET varNilai = (varNilai2 - varNilai3) * -1;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Persediaan',varNilai;
    
	-- (Kenaikan) / Penurunan Piutang Usaha
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('1131.01')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('1131.01')
    );
    
    SET varNilai = (varNilai2 - varNilai3) * -1;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Piutang Usaha',varNilai;
    
	-- (Kenaikan) / Penurunan Piutang Lain - Lain
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('1131.02')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('1131.02')
    );
    
    SET varNilai = (varNilai2 - varNilai3) * -1;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Piutang Lain - Lain',varNilai;
    
	-- (Kenaikan) / Penurunan Piutang  Giro Belum Jatuh Tempo
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('1133.01')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('1133.01')
    );
    
    SET varNilai = (varNilai2 - varNilai3) * -1;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Piutang  Giro Belum Jatuh Tempo',varNilai;
    
	-- (Kenaikan) / Penurunan Uang Muka Pembelian
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Uang Muka Pembelian',0;
    
	-- (Kenaikan) / Penurunan Biaya Dibayar Dimuka
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('1152.01')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('1152.01')
    );
    
    SET varNilai = (varNilai2 - varNilai3) * -1;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Biaya Dibayar Dimuka',varNilai;
    
	-- (Kenaikan) / Penurunan Pajak PPN
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('1162.01')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('1162.01')
    );
    
    SET varNilai = (varNilai2 - varNilai3) * -1;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Pajak PPN',varNilai;
    
	-- (Kenaikan) / Penurunan Uang Muka Pajak
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('1161.01')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('1161.01')
    );
    
    SET varNilai = (varNilai2 - varNilai3) * -1;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Uang Muka Pajak',varNilai;
    
    -- (Kenaikan) / Penurunan Uang Muka Pajak PPh 23
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('1161.02')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('1161.02')
    );
    
    SET varNilai = (varNilai2 - varNilai3) * -1;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Uang Muka Pajak PPh 23',varNilai;
    
	-- (Kenaikan) / Penurunan Ayat Silang
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Ayat Silang',0;
    
	-- Kenaikan / (Penurunan) Hutang Usaha
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('2111.01')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('2111.01')
    );
    
    SET varNilai = varNilai2 - varNilai3;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Hutang Usaha',varNilai;
    
	-- Kenaikan / (Penurunan) Uang Muka Penjualan
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('2121.02')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('2121.02')
    );
    
    SET varNilai = varNilai2 - varNilai3;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Uang Muka Penjualan',varNilai;
    
	-- Kenaikan / (Penurunan) Hutang Ongkos Angkut Pembelian
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('2141.01')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('2141.01')
    );
    
    SET varNilai = varNilai2 - varNilai3;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Hutang Ongkos Angkut Pembelian',varNilai;
    
	-- Kenaikan / (Penurunan) Hutang Gaji / THR
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('2151.01','2151.02')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('2151.01','2151.02')
    );
    
    SET varNilai = varNilai2 - varNilai3;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Hutang Gaji / THR',varNilai;
    
	-- Kenaikan / (Penurunan) Hutang Salah Transfer
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('2191.01')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('2191.01')
    );
    
    SET varNilai = varNilai2 - varNilai3;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Hutang Salah Transfer',varNilai;
    
	-- Kenaikan / (Penurunan) Biaya YMHD - Listrik, Air, Telpon
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('2141.02')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('2141.02')
    );
    
    SET varNilai = varNilai2 - varNilai3;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Biaya YMHD - Listrik, Air, Telpon',varNilai;
    
	-- Kenaikan / (Penurunan) Hutang PPN
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('2132.01')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('2132.01')
    );
    
    SET varNilai = varNilai2 - varNilai3;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Hutang PPN',varNilai;
    
	-- Kenaikan / (Penurunan) Hutang Pajak
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('2131.01')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('2131.01')
    );
    
    SET varNilai = varNilai2 - varNilai3;
    
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Hutang Pajak',varNilai;
    
	-- Kenaikan / (Penurunan) Hutang Lain-lain
    INSERT INTO templaporanaruskas2(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Hutang Lain-lain',0;    
END //
 DELIMITER ;
 
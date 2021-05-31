/*            
   Example		: CALL spLaporanArusKas('123123','202001');
   
DROP TABLE IF EXISTS `templaporanaruskas`;
CREATE TABLE IF NOT EXISTS `templaporanaruskas` (
  `guid` VARCHAR(300),
  `urutankategori` INT,
  `kategori` VARCHAR(300),
  `subkategori` VARCHAR(300),
  `nilai` DOUBLE)
ENGINE = InnoDB;

*/
DROP PROCEDURE IF EXISTS spLaporanArusKas;

DELIMITER //

CREATE PROCEDURE spLaporanArusKas(
	IN varGuid VARCHAR(100),
    IN varPeriode VARCHAR(100)
)
BEGIN
	DECLARE varNilai DECIMAL(19,2);
    DECLARE varNilai2 DECIMAL(19,2);
    DECLARE varNilai3 DECIMAL(19,2);
    DECLARE varPeriodeSebelum TEXT;
    
    SET varPeriodeSebelum = toPeriodeSebelum(varPeriode);
    
	DELETE FROM templaporanaruskas WHERE guid = varGuid;
    
	-- ARUS KAS DARI AKTIVITAS OPERASI
    -- LABA ( RUGI ) BERSIH
    SET varNilai = (
		SELECT COALESCE(SUM(IF(A.penutupkredit = 0,-A.penutupdebit, A.penutupkredit)),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('3212.01')
    );
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','LABA ( RUGI ) BERSIH',varNilai;
    
    -- Penyesuaian untuk :
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Piutang  Giro Belum Jatuh Tempo',varNilai;
    
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Persediaan',varNilai;
    
	-- (Kenaikan) / Penurunan Uang Muka Pembelian
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Biaya Dibayar Dimuka',varNilai;
    
    -- (Kenaikan) / Penurunan Uang Muka Pajak PPh 25
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Uang Muka Pajak PPh 25',varNilai;
    
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Uang Muka Pajak PPh 23',varNilai;
    
    -- (Kenaikan) / Penurunan Uang Muka Pajak PPh 22
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan Uang Muka Pajak PPh 22',0;
    
	-- (Kenaikan) / Penurunan PPN Masukan
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','(Kenaikan) / Penurunan PPN Masukan',varNilai;
    
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Uang Muka Penjualan',varNilai;
    
    -- Kenaikan / (Penurunan) Hutang PPh
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Hutang PPh',varNilai;
    
    -- Kenaikan / (Penurunan) PPN Keluaran
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) PPN Keluaran',varNilai;
    
    -- Kenaikan / (Penurunan) Hutang Biaya Ongkos Angkut Pembelian
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Hutang Biaya Ongkos Angkut Pembelian',varNilai;
    
    -- Kenaikan / (Penurunan) Hutang Biaya Sewa
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Hutang Biaya Sewa',varNilai;
    
    -- Kenaikan / (Penurunan) Hutang Gaji & THR
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Hutang Gaji & THR',varNilai;
    
    -- Kenaikan / (Penurunan) Hutang Lancar Lainnya
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Hutang Lancar Lainnya',varNilai;
    
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
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','Kenaikan / (Penurunan) Akumulasi Penyusutan',varNilai;
    
	-- TOTAL ARUS KAS DARI AKTIVITAS OPERASI
     SET varNilai = (
		SELECT COALESCE(SUM(A.nilai),0)
		FROM templaporanaruskas A
		WHERE A.guid = varGuid AND A.urutankategori = 10
    );
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,10,'ARUS KAS DARI AKTIVITAS OPERASI','TOTAL',varNilai;
    
	-- ----------------------------------------------------------------------------------------------------------------------------------------------------

	-- ARUS KAS DARI AKTIVITAS INVESTASI
    -- Pembelian Aktiva Tidak Lancar, (Kenaikan) / Penurunan 
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('1211.04')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('1211.04')
    );
    
    SET varNilai = (varNilai2 - varNilai3) * -1;
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,20,'ARUS KAS DARI AKTIVITAS INVESTASI','Pembelian Aktiva Tidak Lancar, (Kenaikan) / Penurunan',varNilai;
    
	-- TOTAL ARUS KAS DARI AKTIVITAS INVESTASI
    SET varNilai = (
		SELECT COALESCE(SUM(A.nilai),0)
		FROM templaporanaruskas A
		WHERE A.guid = varGuid AND A.urutankategori = 20
    );
    
	INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,20,'ARUS KAS DARI AKTIVITAS INVESTASI','TOTAL',varNilai;
    
	-- ----------------------------------------------------------------------------------------------------------------------------------------------------
    
	-- ARUS KAS DARI AKTIVITAS PENDANAAN
	-- Kenaikan / (Penurunan) Modal
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('3111.01')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('3111.01')
    );
    
    SET varNilai = varNilai3 - varNilai2;
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,30,'ARUS KAS DARI AKTIVITAS PENDANAAN','Kenaikan / (Penurunan) Modal',varNilai;
    
	-- Kenaikan / (Penurunan) Hutang Afiliasi
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('2212.01')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('2212.01')
    );
    
    SET varNilai = varNilai2 - varNilai3;
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,30,'ARUS KAS DARI AKTIVITAS PENDANAAN','Kenaikan / (Penurunan) Hutang Afiliasi',varNilai;
    
	-- TOTAL ARUS KAS DARI AKTIVITAS PENDANAAN
    SET varNilai = (
		SELECT COALESCE(SUM(A.nilai),0)
		FROM templaporanaruskas A
		WHERE A.guid = varGuid AND A.urutankategori = 30
    );
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,30,'ARUS KAS DARI AKTIVITAS PENDANAAN','TOTAL',varNilai;

	-- ----------------------------------------------------------------------------------------------------------------------------------------------------
    
	-- Kenaikan Bersih Kas dan Setara Kas
    SET varNilai = (
		SELECT COALESCE(SUM(A.nilai),0)
		FROM templaporanaruskas A
		WHERE A.guid = varGuid AND A.subkategori = 'TOTAL' AND A.urutankategori IN (10,20,30)
    );
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,40,'Kenaikan Bersih Kas dan Setara Kas','Kenaikan Bersih Kas dan Setara Kas',varNilai;
    
    -- ----------------------------------------------------------------------------------------------------------------------------------------------------
    
	-- KAS DAN SETARA KAS PADA AWAL PERIODE
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('1111.02')
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriodeSebelum AND A.akun IN ('1121.01')
    );
    
    SET varNilai = varNilai2 + varNilai3;
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,50,'KAS DAN SETARA KAS PADA AWAL PERIODE','KAS DAN SETARA KAS PADA AWAL PERIODE',varNilai;
    
	-- KAS DAN SETARA KAS PADA AKHIR PERIODE
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.nilai),0)
		FROM templaporanaruskas A
		WHERE A.guid = varGuid AND A.subkategori = 'KAS DAN SETARA KAS PADA AWAL PERIODE' AND A.urutankategori = 50
    );
    
    SET varNilai3 = (
		SELECT COALESCE(SUM(A.nilai),0)
		FROM templaporanaruskas A
		WHERE A.guid = varGuid AND A.subkategori = 'Kenaikan Bersih Kas dan Setara Kas' AND A.urutankategori = 40
    );
    
    SET varNilai = varNilai2 + varNilai3;
    
    INSERT INTO templaporanaruskas(guid,urutankategori,kategori,subkategori,nilai)
    SELECT varGuid,50,'KAS DAN SETARA KAS PADA AKHIR PERIODE','KAS DAN SETARA KAS PADA AKHIR PERIODE',varNilai;
    
    -- ----------------------------------------------------------------------------------------------------------------------------------------------------
    
    -- HASIL
    SELECT *
    FROM templaporanaruskas A
    WHERE A.guid = varGuid
    ORDER BY A.urutankategori;
    
    DELETE FROM templaporanaruskas WHERE guid = varGuid;
END //
 DELIMITER ;
 
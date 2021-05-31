/*            
   Example		: CALL spLaporanRasioKeuangan('123','202001');
   
DROP TABLE IF EXISTS `templaporanrasiokeuangan`;
CREATE TABLE IF NOT EXISTS `templaporanrasiokeuangan` (
  `guid` VARCHAR(300),
  `urutankategori` INT,
  `kategori` VARCHAR(300),
  `subkategori` VARCHAR(300),
  `satuan` VARCHAR(100),
  `nilai` DOUBLE,
  `keterangan` VARCHAR(300))
ENGINE = InnoDB;

*/
DROP PROCEDURE IF EXISTS spLaporanRasioKeuangan;

DELIMITER //

CREATE PROCEDURE spLaporanRasioKeuangan(
	IN varGuid VARCHAR(100),
    IN varPeriode VARCHAR(100)
)
BEGIN
	DECLARE varNilai DOUBLE;
    DECLARE varNilai2 DOUBLE;
    DECLARE varNilai3 DOUBLE;
    DECLARE varNilai4 DOUBLE;
    DECLARE varNilai5 DOUBLE;
    
    DECLARE varPeriodeSebelum TEXT;
    
    SET varPeriodeSebelum = toPeriodeSebelum(varPeriode);

	DELETE FROM templaporanrasiokeuangan WHERE guid = varGuid;
    
	-- RASIO LIKUIDITAS						
	-- CURRENT RATIO		%				Aktiva lancar/Kewajiban lancar
    SET varNilai = (SELECT COALESCE(SUM(A.akhirdebit),0)
					FROM saldoakunbulanan A
					INNER JOIN akun B ON A.akun = B.kode
					WHERE A.periode = varPeriode AND B.akunsubkategori IN ('1100.00'));
                    
	SET varNilai2 = (SELECT COALESCE(SUM(A.akhirkredit),0)
					FROM saldoakunbulanan A
					INNER JOIN akun B ON A.akun = B.kode
					WHERE A.periode = varPeriode AND B.akunsubkategori IN ('2100.00'));
                    
	SET varNilai3 = (varNilai / varNilai2) * 100;
    
	INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,10,'RASIO LIKUIDITAS','CURRENT RATIO','%',varNilai3,'Aktiva lancar / Kewajiban lancar');
	
	-- QUICK RATIO		%					(Aktiva lancar - Persediaan)/Kewajiban lancar
	SET varNilai = (SELECT COALESCE(SUM(A.akhirdebit),0)
					FROM saldoakunbulanan A
					INNER JOIN akun B ON A.akun = B.kode
					WHERE A.periode = varPeriode AND B.akunsubkategori IN ('1100.00'));
                    
	SET varNilai2 = (SELECT COALESCE(SUM(A.akhirkredit),0)
					FROM saldoakunbulanan A
					INNER JOIN akun B ON A.akun = B.kode
					WHERE A.periode = varPeriode AND B.akunsubkategori IN ('2100.00'));
                    
	SET varNilai3 = (SELECT COALESCE(SUM(A.akhirdebit),0)
					FROM saldoakunbulanan A
					WHERE A.periode = varPeriode AND A.akun IN ('1141.01'));
                    
	SET varNilai4 = ((varNilai - varNilai3) / varNilai2) * 100;
    
    INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,10,'RASIO LIKUIDITAS','QUICK RATIO','%',varNilai4,'(Aktiva lancar - Persediaan) / Kewajiban lancar');
    
	-- RASIO AKTIVITAS						
	-- RECEIVABLE TURNOVER			Kali		Penjualan/Piutang usaha
    SET varNilai = (SELECT COALESCE(SUM(A.penutupdebit),0)
					FROM saldoakunbulanan A
					WHERE A.periode = varPeriode AND A.akun = '4111.01');
                    
	SET varNilai2 = (SELECT COALESCE(SUM(A.akhirdebit),0)
					FROM saldoakunbulanan A
					WHERE A.periode = varPeriode AND A.akun IN ('1131.01'));
                    
	SET varNilai3 = varNilai / varNilai2;
    
	INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,20,'RASIO AKTIVITAS','RECEIVABLE TURNOVER','Kali',varNilai3,'Penjualan / Piutang usaha');
    
	-- DAYS SALES OUTSTANDING		Hari		30/Receivable turnover
    SET varNilai4 = 30 / varNilai3;
    
    INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,20,'RASIO AKTIVITAS','DAYS SALES OUTSTANDING','Hari',varNilai4,'30 / Receivable turnover');
    
	-- PAYABLE TURNOVER				Kali		HPP/Hutang Usaha
    SET varNilai = (SELECT COALESCE(SUM(A.penutupkredit),0)
					FROM saldoakunbulanan A
					WHERE A.periode = varPeriode AND A.akun = '5111.01');
                    
	SET varNilai2 = (SELECT COALESCE(SUM(A.akhirkredit),0)
					FROM saldoakunbulanan A
					WHERE A.periode = varPeriode AND A.akun IN ('2111.01'));
                    
	SET varNilai3 = varNilai / varNilai2;
    
	INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,20,'RASIO AKTIVITAS','PAYABLE TURNOVER','Kali',varNilai3,'HPP / Hutang Usaha');
    
	-- DAYS PAYABLE OUTSTANDING		Hari		30/Payable turnover
    SET varNilai4 = 30 / varNilai3;
    
    INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,20,'RASIO AKTIVITAS','DAYS PAYABLE OUTSTANDING','Hari',varNilai4,'30 / Payable turnover');
    
	-- ASSET TURNOVER				Kali		Penjualan/Total Asset
    SET varNilai = (SELECT COALESCE(SUM(A.penutupdebit),0)
					FROM saldoakunbulanan A
					WHERE A.periode = varPeriode AND A.akun = '4111.01');
                    
	SET varNilai2 = (SELECT COALESCE(SUM(A.akhirdebit-A.akhirkredit),0)
					FROM saldoakunbulanan A
					INNER JOIN akun B ON A.akun = B.kode
					WHERE A.periode = varPeriode AND B.akunkategori IN ('1000.00'));
	
    SET varNilai3 = varNilai / varNilai2;
    
    INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,20,'RASIO AKTIVITAS','ASSET TURNOVER','Kali',varNilai3,'Penjualan / Total Asset');
    
	-- STOCK/PENJUALAN				Kali		Stock/Penjualan
    SET varNilai = (
		SELECT COALESCE(SUM(A.akhirdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('1141.01')
    );
    
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.penutupdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun = '4111.01'
    );
    
    SET varNilai3 = varNilai / varNilai2;
    
    INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,20,'RASIO AKTIVITAS','STOCK/PENJUALAN','Kali',varNilai3,'Stock / Penjualan');
    						
	-- RASIO SOLVABILITAS						
	-- DEBT RATIO					%			Total hutang/Total aktiva
    SET varNilai = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('2111.01')
    );
    
    SET varNilai2 = (SELECT COALESCE(SUM(A.akhirdebit),0)
					FROM saldoakunbulanan A
					INNER JOIN akun B ON A.akun = B.kode
					WHERE A.periode = varPeriode AND B.akunsubkategori IN ('1100.00'));
	
    SET varNilai3 = (varNilai / varNilai2) * 100;
    
    INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,30,'RASIO SOLVABILITAS','DEBT RATIO','%',varNilai3,'Total hutang / Total aktiva');
    
	-- DEBT EQUITY RATIO			%			Total hutang/Total modal
    SET varNilai = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('2111.01')
    );
    
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('3111.01')
    );
    
    SET varNilai3 = (varNilai / varNilai2) * 100;
    
    INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,30,'RASIO SOLVABILITAS','DEBT EQUITY RATIO','%',varNilai3,'Total hutang / Total modal');
    
	-- TIMES INTEREST EARNED		Kali		Biaya bunga/Laba operasional
    INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,30,'RASIO SOLVABILITAS','TIMES INTEREST EARNED','Kali',0,'Biaya bunga / Laba operasional');
							
	-- RASIO PROFITABILITAS						
	-- NET PROFIT MARGIN			%			Laba bersih/Penjualan
    SET varNilai = (SELECT COALESCE(SUM(IF(A.penutupdebit > 0 && A.penutupkredit > 0, A.penutupkredit, A.penutupkredit-A.penutupdebit)),0)
					FROM saldoakunbulanan A
					WHERE A.periode = varPeriode AND A.akun = '3212.01');
    
    SET varNilai2 = (
		SELECT COALESCE(SUM(A.penutupdebit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun = '4111.01'
    );
    
    SET varNilai3 = (varNilai / varNilai2) * 100;
    
    INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,40,'RASIO PROFITABILITAS','NET PROFIT MARGIN','%',varNilai3,'Laba bersih / Penjualan');
    
	-- RETURN ON ASSET				%			Laba bersih/Total aktiva
    SET varNilai = (SELECT COALESCE(SUM(IF(A.penutupdebit > 0 && A.penutupkredit > 0, A.penutupkredit, A.penutupkredit-A.penutupdebit)),0)
					FROM saldoakunbulanan A
					WHERE A.periode = varPeriode AND A.akun = '3212.01');
                    
	SET varNilai2 = (SELECT COALESCE(SUM(A.akhirdebit-A.akhirkredit),0)
					FROM saldoakunbulanan A
					INNER JOIN akun B ON A.akun = B.kode
					WHERE A.periode = varPeriode AND B.akunkategori IN ('1000.00'));

	SET varNilai3 = (varNilai / varNilai2) * 100;
    
    INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,40,'RASIO PROFITABILITAS','RETURN ON ASSET','%',varNilai3,'Laba bersih / Total aktiva');
    
	-- RETURN ON EQUITY				%			Laba bersih/Total modal
    SET varNilai = (SELECT COALESCE(SUM(IF(A.penutupdebit > 0 && A.penutupkredit > 0, A.penutupkredit, A.penutupkredit-A.penutupdebit)),0)
					FROM saldoakunbulanan A
					WHERE A.periode = varPeriode AND A.akun = '3212.01');
                    
	 SET varNilai2 = (
		SELECT COALESCE(SUM(A.akhirkredit),0)
		FROM saldoakunbulanan A
		WHERE A.periode = varPeriode AND A.akun IN ('3111.01')
    );
    
    SET varNilai3 = (varNilai / varNilai2) * 100;
    
    INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,40,'RASIO PROFITABILITAS','RETURN ON EQUITY','%',varNilai3,'Laba bersih / Total modal');
    						
	-- RASIO ARUS KAS 						
	-- OPERATING CASH FLOW TO CURRENT LIABILITIES	%		Arus kas operasi/Kewajiban lancar
    CALL spLaporanArusKas2(varGuid,varPeriode);
    SET varNilai = (SELECT COALESCE(SUM(A.nilai),0)
					FROM templaporanaruskas2 A
					WHERE A.guid = varGuid AND A.urutankategori = 10);
    
    
    SET varNilai2 = (SELECT COALESCE(SUM(A.akhirkredit),0)
					FROM saldoakunbulanan A
					INNER JOIN akun B ON A.akun = B.kode
					WHERE A.periode = varPeriode AND B.akunsubkategori IN ('2100.00'));
                    
    SET varNilai3 = (varNilai / varNilai2) * 100;
    
    INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,50,'RASIO ARUS KAS','OPERATING CASH FLOW TO CURRENT LIABILITIES','%',varNilai3,'Arus kas operasi / Kewajiban lancar');
    
	-- OPERATING CASH FLOW TO TOTAL LIABILITIES		%		Arus kas operasi/Total kewajiban
    SET varNilai2 = (SELECT COALESCE(SUM(A.akhirkredit-A.akhirdebit),0)
					FROM saldoakunbulanan A
					INNER JOIN akun B ON A.akun = B.kode
					WHERE A.periode = varPeriode AND B.akunkategori IN ('2000.00','3000.00'));
                    
    SET varNilai3 = (varNilai / varNilai2) * 100;
    
    INSERT INTO templaporanrasiokeuangan(guid,urutankategori,kategori,subkategori,satuan,nilai,keterangan)
    VALUES(varGuid,50,'RASIO ARUS KAS','OPERATING CASH FLOW TO TOTAL LIABILITIES','%',varNilai3,'Arus kas operasi / Total kewajiban');

    -- HASIL
    SELECT *
    FROM templaporanrasiokeuangan A
    WHERE A.guid = varGuid
    ORDER BY A.urutankategori;
    
    DELETE FROM templaporanrasiokeuangan WHERE guid = varGuid;
END //
 DELIMITER ;
 
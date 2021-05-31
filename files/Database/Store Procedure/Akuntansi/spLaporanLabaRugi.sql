/*
	Author		: Santos Sabanari
    Date		: 29-08-2017
    Description	: Untuk laporan laba rugi
    Step		: 
			- Isi akun laba rugi
			- Update kategori dan kodekategori
			- Isi nilai dari akun untuk periode tersebut
			- update untuk total subkategori
			- buat tabel daftar total berdasarkan subkategori
            - update untuk total kategori
            
	Example		: CALL spLaporanLabaRugi('123', '06', '2019');

DROP TABLE IF EXISTS `templaporanlabarugi`;
CREATE TABLE IF NOT EXISTS `templaporanlabarugi` (
  `guid` VARCHAR(300),
  `kodeurutan` VARCHAR(45),
  `kodesubkategori` VARCHAR(45) NOT NULL,
  `namasubkategori` VARCHAR(300) NOT NULL,
  `kodeakun` VARCHAR(45) NOT NULL,
  `namaakun` VARCHAR(300) NOT NULL,
  `totalakun` DOUBLE NULL,
  `totalytdakun` DOUBLE NULL)
ENGINE = InnoDB;

DROP TABLE IF EXISTS `templaporanlabarugisubkategori`;
CREATE TABLE IF NOT EXISTS `templaporanlabarugisubkategori` (
  `guid` VARCHAR(300),
  `kodesubkategori` VARCHAR(45) NOT NULL,
  `namasubkategori` VARCHAR(300) NOT NULL,
  `totalsubkategori` DOUBLE NULL,
  `totalytdsubkategori` DOUBLE NULL)
ENGINE = InnoDB;

*/

DROP PROCEDURE IF EXISTS spLaporanLabaRugi;

DELIMITER //

CREATE PROCEDURE spLaporanLabaRugi(
	IN varGuid VARCHAR(1000),
    IN varBulan VARCHAR(1000),
    IN varTahun VARCHAR(1000)
)
BEGIN
    DECLARE varPeriode VARCHAR(6);
    DECLARE varPeriodeAwal VARCHAR(6);

    DECLARE varTidak TEXT;
    DECLARE varYa TEXT;
    DECLARE varStatusAktif TEXT;
	
    -- Untuk perhitungan total kategori
    DECLARE varPenjualan DOUBLE;
    DECLARE varHPP DOUBLE;
	DECLARE varBiayaPenjualan DOUBLE;
	DECLARE varBiayaUmum DOUBLE;
	DECLARE varBiayaLain DOUBLE;
	DECLARE varPendapatanLain DOUBLE;
    
    DECLARE varBiayaLain2 DOUBLE;
	DECLARE varPendapatanLain2 DOUBLE;
    
	DECLARE varLabaKotor DOUBLE;
	DECLARE varBiayaOperasional DOUBLE;
	DECLARE varLabaOperasional DOUBLE;
    
	DECLARE varLabaBersih DOUBLE;

    SET varPeriode = CONCAT(varTahun, varBulan);
    SET varPeriodeAwal = CONCAT(varTahun, '01');
    
    SET varTidak = 'Tidak';
    SET varYa = 'Ya';
    SET varStatusAktif = 'Aktif';
    
	-- Delete isi temp table 
    DELETE FROM templaporanlabarugi WHERE guid = varGuid;
    DELETE FROM templaporanlabarugisubkategori WHERE guid = varGuid;
    
    -- Isi akun laba rugi
    INSERT INTO templaporanlabarugi(guid,kodeurutan,kodesubkategori,namasubkategori,kodeakun,namaakun,totalakun,totalytdakun)
    SELECT varGuid, A.kode, B.kode, B.nama, A.kode, A.nama, 0, 0
	FROM akun A
	INNER JOIN akunsubkategori B ON A.akunsubkategori = B.kode
	WHERE A.akunkategori IN ('4000.00', '5000.00', '6000.00', '7000.00');

    -- Isi nilai dari akun untuk periode tersebut
    UPDATE templaporanlabarugi X
    INNER JOIN (
        SELECT A.kode AS akun, CASE WHEN A.saldonormallabarugi = 'Debit' THEN COALESCE(B.debit,0) - COALESCE(B.kredit,0) ELSE COALESCE(B.kredit,0) - COALESCE(B.debit,0) END AS total
        FROM akun A
        LEFT JOIN (
            SELECT A.akun, SUM(A.debit) AS debit, SUM(A.kredit) AS kredit 
            FROM jurnal A
            INNER JOIN akun B ON A.akun = B.kode
            WHERE toPeriode(A.tanggal) = varPeriode AND A.status = varStatusAktif AND A.jurnalpenutup = varTidak AND B.akunkategori IN ('4000.00', '5000.00', '6000.00', '7000.00')
            GROUP BY A.akun
        ) B ON A.kode = B.akun
        WHERE A.akunkategori IN ('4000.00', '5000.00', '6000.00', '7000.00')
    ) A ON X.kodeakun = A.akun
    SET X.totalakun = A.total
    WHERE X.guid = varGuid;

    -- Isi nilai dari akun untuk year to date
    UPDATE templaporanlabarugi X
    INNER JOIN (
        SELECT A.kode AS akun, CASE WHEN A.saldonormallabarugi = 'Debit' THEN COALESCE(B.debit,0) - COALESCE(B.kredit,0) ELSE COALESCE(B.kredit,0) - COALESCE(B.debit,0) END AS total
        FROM akun A
        LEFT JOIN (
            SELECT A.akun, SUM(A.debit) AS debit, SUM(A.kredit) AS kredit 
            FROM jurnal A
            INNER JOIN akun B ON A.akun = B.kode
            WHERE toPeriode(A.tanggal) BETWEEN varPeriodeAwal AND varPeriode AND A.status = varStatusAktif AND A.jurnalpenutup = varTidak AND B.akunkategori IN ('4000.00', '5000.00', '6000.00', '7000.00')
            GROUP BY A.akun
        ) B ON A.kode = B.akun
        WHERE A.akunkategori IN ('4000.00', '5000.00', '6000.00', '7000.00')
    ) A ON X.kodeakun = A.akun
    SET X.totalytdakun = A.total
    WHERE X.guid = varGuid;

    INSERT INTO templaporanlabarugisubkategori(guid, kodesubkategori, namasubkategori, totalsubkategori, totalytdsubkategori)
    SELECT varGuid, A.kodesubkategori, A.namasubkategori, SUM(totalakun), SUM(totalytdakun)
    FROM templaporanlabarugi A
    WHERE A.guid = varGuid
    GROUP BY A.kodesubkategori;

    -- Hitung Total dan Subtotal
    -- Periode tersebut
    SET varPenjualan = (SELECT COALESCE(SUM(totalsubkategori), 0) FROM templaporanlabarugisubkategori WHERE guid = varGuid AND kodesubkategori = '4100.00'); -- 4100.00 | 4999.99z
    SET varHPP = (SELECT COALESCE(SUM(totalsubkategori), 0) FROM templaporanlabarugisubkategori WHERE guid = varGuid AND kodesubkategori = '5100.00'); -- 5100.00 | 5999.99z
    SET varBiayaPenjualan = (SELECT COALESCE(SUM(totalsubkategori), 0) FROM templaporanlabarugisubkategori WHERE guid = varGuid AND kodesubkategori = '6100.00'); -- 6100.00 | 6199.99z
    SET varBiayaUmum = (SELECT COALESCE(SUM(totalsubkategori), 0) FROM templaporanlabarugisubkategori WHERE guid = varGuid AND kodesubkategori = '6200.00'); -- 6200.00 | 6299.99z
    
    SET varPendapatanLain = (SELECT COALESCE(SUM(totalakun), 0) FROM templaporanlabarugi WHERE guid = varGuid AND kodeakun IN ('7111.01','7111.02',',7111.03','7111.04','7111.05','7119.01')); -- 7100.00 | 7199.99z
    SET varBiayaLain = (SELECT COALESCE(SUM(totalakun), 0) FROM templaporanlabarugi WHERE guid = varGuid AND kodeakun IN ('7121.01')); -- 7100.00 | 7199.99z
    
    SET varPendapatanLain2 = (SELECT COALESCE(SUM(totalakun), 0) FROM templaporanlabarugi WHERE guid = varGuid AND kodeakun IN ('7211.01')); -- 7200.00 | 7299.99z
    SET varBiayaLain2 = (SELECT COALESCE(SUM(totalakun), 0) FROM templaporanlabarugi WHERE guid = varGuid AND kodeakun IN ('7211.02')); -- 7200.00 | 7299.99z
    
    SET varLabaKotor = varPenjualan - varHPP; -- 4100.00 - 5100.00 | 5999.99zz
    SET varBiayaOperasional = varBiayaPenjualan + varBiayaUmum; -- 6100.00 + 6200.00 | 6299.99zz
    SET varLabaOperasional = varLabaKotor - varBiayaOperasional; -- 5999.99zz - 6299.99zz | 6299.99zzz
    
    SET varLabaBersih = varLabaOperasional + varPendapatanLain - varBiayaLain + varPendapatanLain2 - varBiayaLain2; -- 7299.99zz - 7999.99z | 7999.99zz

    -- Subtotal
    INSERT INTO templaporanlabarugi(guid,kodeurutan,kodesubkategori,namasubkategori,kodeakun,namaakun,totalakun,totalytdakun)
    SELECT varGuid, '4999.99z', kodesubkategori, namasubkategori, 'TOTAL', CONCAT('TOTAL ', namasubkategori), varPenjualan, 0
    FROM templaporanlabarugisubkategori
    WHERE guid = varGuid AND kodesubkategori = '4100.00';
    
    INSERT INTO templaporanlabarugi(guid,kodeurutan,kodesubkategori,namasubkategori,kodeakun,namaakun,totalakun,totalytdakun)
    SELECT varGuid, '5999.99z', kodesubkategori, namasubkategori, 'TOTAL', CONCAT('TOTAL ', namasubkategori), varHPP, 0
    FROM templaporanlabarugisubkategori
    WHERE guid = varGuid AND kodesubkategori = '5100.00';
    
    INSERT INTO templaporanlabarugi(guid,kodeurutan,kodesubkategori,namasubkategori,kodeakun,namaakun,totalakun,totalytdakun)
    SELECT varGuid, '6199.99z', kodesubkategori, namasubkategori, 'TOTAL', CONCAT('TOTAL ', namasubkategori), varBiayaPenjualan, 0
    FROM templaporanlabarugisubkategori
    WHERE guid = varGuid AND kodesubkategori = '6100.00';
    
    INSERT INTO templaporanlabarugi(guid,kodeurutan,kodesubkategori,namasubkategori,kodeakun,namaakun,totalakun,totalytdakun)
    SELECT varGuid, '6299.99z', kodesubkategori, namasubkategori, 'TOTAL', CONCAT('TOTAL ', namasubkategori), varBiayaUmum, 0
    FROM templaporanlabarugisubkategori
    WHERE guid = varGuid AND kodesubkategori = '6200.00';
    
    INSERT INTO templaporanlabarugi(guid,kodeurutan,kodesubkategori,namasubkategori,kodeakun,namaakun,totalakun,totalytdakun)
    SELECT varGuid, '7199.99z', kodesubkategori, namasubkategori, 'TOTAL', CONCAT('TOTAL ', namasubkategori), varPendapatanLain - varBiayaLain, 0
    FROM templaporanlabarugisubkategori
    WHERE guid = varGuid AND kodesubkategori = '7100.00';
    
    INSERT INTO templaporanlabarugi(guid,kodeurutan,kodesubkategori,namasubkategori,kodeakun,namaakun,totalakun,totalytdakun)
    SELECT varGuid, '7299.99z', kodesubkategori, namasubkategori, 'TOTAL', CONCAT('TOTAL ', namasubkategori), varPendapatanLain2 - varBiayaLain2, 0
    FROM templaporanlabarugisubkategori
    WHERE guid = varGuid AND kodesubkategori = '7200.00';

    -- Total
    INSERT INTO templaporanlabarugi(guid,kodeurutan,kodesubkategori,namasubkategori,kodeakun,namaakun,totalakun,totalytdakun)
    SELECT varGuid, '5999.99zz', kodesubkategori, namasubkategori, 'TOTAL SUB', 'LABA KOTOR', varLabaKotor, 0
    FROM templaporanlabarugisubkategori
    WHERE guid = varGuid AND kodesubkategori = '5100.00';
    
    INSERT INTO templaporanlabarugi(guid,kodeurutan,kodesubkategori,namasubkategori,kodeakun,namaakun,totalakun,totalytdakun)
    SELECT varGuid, '6299.99zz', kodesubkategori, namasubkategori, 'TOTAL SUB', 'TOTAL BIAYA', varBiayaOperasional, 0
    FROM templaporanlabarugisubkategori
    WHERE guid = varGuid AND kodesubkategori = '6200.00';
    
    INSERT INTO templaporanlabarugi(guid,kodeurutan,kodesubkategori,namasubkategori,kodeakun,namaakun,totalakun,totalytdakun)
    SELECT varGuid, '6299.99zzz', kodesubkategori, namasubkategori, 'TOTAL SUB', 'LABA OPERASIONAL', varLabaOperasional, 0
    FROM templaporanlabarugisubkategori
    WHERE guid = varGuid AND kodesubkategori = '6200.00';
    
    INSERT INTO templaporanlabarugi(guid,kodeurutan,kodesubkategori,namasubkategori,kodeakun,namaakun,totalakun,totalytdakun)
    SELECT varGuid, '7299.99zz', kodesubkategori, namasubkategori, 'TOTAL SUB', 'LABA BERSIH', varLabaBersih, 0
    FROM templaporanlabarugisubkategori
    WHERE guid = varGuid AND kodesubkategori = '7200.00';

    -- Year To Date
    SET varPenjualan = (SELECT COALESCE(SUM(totalytdsubkategori), 0) FROM templaporanlabarugisubkategori WHERE guid = varGuid AND kodesubkategori = '4100.00'); -- 4100.00 | 4999.99z
    SET varHPP = (SELECT COALESCE(SUM(totalytdsubkategori), 0) FROM templaporanlabarugisubkategori WHERE guid = varGuid AND kodesubkategori = '5100.00'); -- 5100.00 | 5999.99z
    SET varBiayaPenjualan = (SELECT COALESCE(SUM(totalytdsubkategori), 0) FROM templaporanlabarugisubkategori WHERE guid = varGuid AND kodesubkategori = '6100.00'); -- 6100.00 | 6199.99z
    SET varBiayaUmum = (SELECT COALESCE(SUM(totalytdsubkategori), 0) FROM templaporanlabarugisubkategori WHERE guid = varGuid AND kodesubkategori = '6200.00'); -- 6200.00 | 6299.99z
    
    SET varPendapatanLain = (SELECT COALESCE(SUM(totalytdakun), 0) FROM templaporanlabarugi WHERE guid = varGuid AND kodeakun IN ('7111.01','7111.02',',7111.03','7111.04','7111.05','7119.01')); -- 7100.00 | 7199.99z
    SET varBiayaLain = (SELECT COALESCE(SUM(totalytdakun), 0) FROM templaporanlabarugi WHERE guid = varGuid AND kodeakun IN ('7121.01')); -- 7100.00 | 7199.99z
    
    SET varPendapatanLain2 = (SELECT COALESCE(SUM(totalytdakun), 0) FROM templaporanlabarugi WHERE guid = varGuid AND kodeakun IN ('7211.01')); -- 7200.00 | 7299.99z
    SET varBiayaLain2 = (SELECT COALESCE(SUM(totalytdakun), 0) FROM templaporanlabarugi WHERE guid = varGuid AND kodeakun IN ('7211.02')); -- 7200.00 | 7299.99z
    
    SET varLabaKotor = varPenjualan - varHPP; -- 4100.00 - 5100.00 | 5999.99zz
    SET varBiayaOperasional = varBiayaPenjualan + varBiayaUmum; -- 6100.00 + 6200.00 | 6299.99zz
    SET varLabaOperasional = varLabaKotor - varBiayaOperasional; -- 5999.99zz - 6299.99zz | 6299.99zzz
    
    SET varLabaBersih = varLabaOperasional + varPendapatanLain - varBiayaLain + varPendapatanLain2 - varBiayaLain2; -- 7299.99zz - 7999.99z | 7999.99zz
    
    -- Subtotal
    UPDATE templaporanlabarugi SET totalytdakun = varPenjualan WHERE guid = varGuid AND kodeurutan = '4999.99z';
    UPDATE templaporanlabarugi SET totalytdakun = varHPP WHERE guid = varGuid AND kodeurutan = '5999.99z';
    UPDATE templaporanlabarugi SET totalytdakun = varBiayaPenjualan WHERE guid = varGuid AND kodeurutan = '6199.99z';
    UPDATE templaporanlabarugi SET totalytdakun = varBiayaUmum WHERE guid = varGuid AND kodeurutan = '6299.99z';
    UPDATE templaporanlabarugi SET totalytdakun = varPendapatanLain - varBiayaLain WHERE guid = varGuid AND kodeurutan = '7199.99z';
    UPDATE templaporanlabarugi SET totalytdakun = varPendapatanLain2 - varBiayaLain2 WHERE guid = varGuid AND kodeurutan = '7299.99z';
    
    -- Total
    UPDATE templaporanlabarugi SET totalytdakun = varLabaKotor WHERE guid = varGuid AND kodeurutan = '5999.99zz';
    UPDATE templaporanlabarugi SET totalytdakun = varBiayaOperasional WHERE guid = varGuid AND kodeurutan = '6299.99zz';
    UPDATE templaporanlabarugi SET totalytdakun = varLabaOperasional WHERE guid = varGuid AND kodeurutan = '6299.99zzz';
    UPDATE templaporanlabarugi SET totalytdakun = varLabaBersih WHERE guid = varGuid AND kodeurutan = '7299.99zz';
    
    SET @row_num := 0;

    SELECT (@row_num := @row_num + 1) AS 'Urutan', kodesubkategori AS 'Kode Sub', namasubkategori AS 'Subkategori', kodeakun AS 'Kode', namaakun AS 'Keterangan', totalakun AS 'Current Month', totalytdakun AS 'Year to Date'
    FROM templaporanlabarugi 
    WHERE guid = varGuid
    ORDER BY kodeurutan;
  
    -- 5. Delete isi temp table 
    DELETE FROM templaporanlabarugi WHERE guid = varGuid;
    DELETE FROM templaporanlabarugisubkategori WHERE guid = varGuid;
    
END //
 DELIMITER ;
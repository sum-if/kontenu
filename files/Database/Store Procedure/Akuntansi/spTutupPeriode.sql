/*
	Author		: Santos Sabanari
    Date		: 31-05-2017
    Description	: Untuk menutup periode yang diinginkan
    Step		: 
			1. Validasi 
			2. Akun
            3. Hutang
            4. Piutang
            5. Persediaan
            6. Titipan Supplier
            7. Titipan Customer
            8. Cari saldo bulanan yang minus
            9. Jurnal laba rugi ditahan
           
    Example		: CALL spTutupPeriode('123','201903','Maret','2019','%d/%m/%Y','SANTOS');
				  Tidak bisa di lakukan manual, karena untuk validasi saldo bulanan minus ada di form, bukan di database
    
DROP TABLE IF EXISTS `tempsaldobulananerror`;
CREATE TABLE IF NOT EXISTS `tempsaldobulananerror` (
  `periode` VARCHAR(45),
  `jenis` VARCHAR(100),
  `kode` VARCHAR(100),
  `nama` VARCHAR(200),
  `awal` DOUBLE,
  `mutasitambah` DOUBLE,
  `mutasikurang` DOUBLE,
  `akhir` DOUBLE)
ENGINE = InnoDB;

DROP TABLE IF EXISTS `tempsaldoakunbulanan`;
CREATE TABLE IF NOT EXISTS `tempsaldoakunbulanan` (
  `guid` VARCHAR(200),
  `periode` VARCHAR(45) NOT NULL,
  `akun` VARCHAR(45) NOT NULL,
  `awaldebit` DOUBLE NULL,
  `awalkredit` DOUBLE NULL,
  `mutasidebit` DOUBLE NULL,
  `mutasikredit` DOUBLE NULL,
  `laporankeuangandebit` DOUBLE NULL,
  `laporankeuangankredit` DOUBLE NULL,
  `penutupdebit` DOUBLE NULL,
  `penutupkredit` DOUBLE NULL,
  `akhirdebit` DOUBLE NULL,
  `akhirkredit` DOUBLE NULL,
  PRIMARY KEY (`guid`, `periode`, `akun`))
ENGINE = InnoDB;

DROP TABLE IF EXISTS `tempsaldohutangbulanan`;
CREATE TABLE IF NOT EXISTS `tempsaldohutangbulanan` (
  `guid` VARCHAR(300),
  `periode` VARCHAR(45) NOT NULL,
  `supplier` VARCHAR(45) NOT NULL,
  `awal` DOUBLE NULL,
  `mutasitambah` DOUBLE NULL,
  `mutasikurang` DOUBLE NULL,
  `akhir` DOUBLE NULL)
ENGINE = InnoDB;

DROP TABLE IF EXISTS `tempsaldopiutangbulanan`;
CREATE TABLE IF NOT EXISTS `tempsaldopiutangbulanan` (
  `guid` VARCHAR(300),
  `periode` VARCHAR(45) NOT NULL,
  `customer` VARCHAR(45) NOT NULL,
  `awal` DOUBLE NULL,
  `mutasitambah` DOUBLE NULL,
  `mutasikurang` DOUBLE NULL,
  `akhir` DOUBLE NULL)
ENGINE = InnoDB;

DROP TABLE IF EXISTS `tempsaldopersediaanbulanan`;
CREATE TABLE IF NOT EXISTS `tempsaldopersediaanbulanan` (
  `guid` VARCHAR(300),
  `periode` VARCHAR(45) NOT NULL,
  `barang` VARCHAR(45) NOT NULL,
  `awal` DOUBLE NULL,
  `mutasitambah` DOUBLE NULL,
  `mutasikurang` DOUBLE NULL,
  `akhir` DOUBLE NULL)
ENGINE = InnoDB;
    
*/
DROP PROCEDURE IF EXISTS spTutupPeriode;

DELIMITER //

CREATE PROCEDURE spTutupPeriode(
	IN varGuid VARCHAR(1000),
    IN varPeriode VARCHAR(1000),
    IN varNamaBulan VARCHAR(1000),
    IN varTahun VARCHAR(1000),
    IN varFormatDate VARCHAR(1000),
    IN varUserLogin VARCHAR(1000)
)
BEGIN
	DECLARE varTipeProses TEXT;
    DECLARE varPesan TEXT;
    
    SET varTipeProses = 'Tutup Periode'; 
    
	-- 1. Validasi 
    CALL spTutupPeriodeValidasi(varGuid,
								varPeriode,
								varNamaBulan,
								varTahun,
								varTipeProses);

    -- 2. Akun 
    CALL spTutupPeriodeAkunBulanan(varGuid,
								varPeriode,
								varNamaBulan,
								varTahun,
								varFormatDate,
								varUserLogin);
    
    -- 3. Hutang 
    CALL spTutupPeriodeHutangBulanan(varGuid,
								varPeriode,
								varNamaBulan,
								varTahun,
								varFormatDate,
								varUserLogin);

    -- 4. Piutang
	CALL spTutupPeriodePiutangBulanan(varGuid,
								varPeriode,
								varNamaBulan,
								varTahun,
								varFormatDate,
								varUserLogin);
        
	-- 5. Persediaan
    CALL spTutupPeriodePersediaanBulanan(varGuid,
								varPeriode,
								varNamaBulan,
								varTahun,
								varFormatDate,
								varUserLogin);
	
    -- 5. HPP
    CALL spTutupPeriodeHPPBulanan(varGuid,
								varPeriode,
								varNamaBulan,
								varTahun,
								varFormatDate,
								varUserLogin);
                                
    -- 8. Cari saldo bulanan yang minus
    CALL spTutupPeriodeSaldoMinus(varGuid,
								varPeriode,
								varNamaBulan,
								varTahun,
								varFormatDate,
								varUserLogin);

    -- Tambah Admin
    INSERT INTO admin(periode,proses,create_at,create_user) VALUES(varPeriode,varTipeProses,CURRENT_TIMESTAMP,varUserLogin);
    
END //
 DELIMITER ;
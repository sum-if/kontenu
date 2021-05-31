/*
	Author		: Santos Sabanari
    Date		: 28-04-2017
    Description	: Untuk laporan buku besar
    Step		: 
               - Ambil saldo bulan sebelumnya
               - Cari mutasi dari awal bulan sampai tanggal awal
               - Cari mutasi dari tanggal awal sampai tanggal akhir
               
   Example		: CALL spLaporanBukuBesar('123','15/01/2019','01/04/2019','%');
         
DROP TABLE IF EXISTS `templaporanbukubesarawal`;
CREATE TABLE IF NOT EXISTS `templaporanbukubesarawal` (
  `guid` VARCHAR(300),
  `akun` VARCHAR(45),
  `debit` DOUBLE,
  `kredit` DOUBLE)
ENGINE = InnoDB;

DROP TABLE IF EXISTS `templaporanbukubesarmutasi`;
CREATE TABLE IF NOT EXISTS `templaporanbukubesarmutasi` (
  `guid` VARCHAR(300),
  `akun` VARCHAR(45),
  `no` DOUBLE,
  `tanggal` VARCHAR(45),
  `referensi` VARCHAR(100),
  `keterangan` VARCHAR(500),
  `debit` DOUBLE,
  `kredit` DOUBLE,
  `waktu` TIMESTAMP NULL)
ENGINE = InnoDB;

DROP TABLE IF EXISTS `templaporanbukubesar`;
CREATE TABLE IF NOT EXISTS `templaporanbukubesar` (
  `guid` VARCHAR(300),
  `akun` VARCHAR(45),
  `id` DOUBLE,
  `tanggal` VARCHAR(45),
  `referensi` VARCHAR(100),
  `keterangan` VARCHAR(500),
  `normal` VARCHAR(45),
  `debit` DOUBLE,
  `kredit` DOUBLE,
  `saldo` DOUBLE,
  `waktu` TIMESTAMP NULL)
ENGINE = InnoDB;

*/
DROP PROCEDURE IF EXISTS spLaporanBukuBesar;

DELIMITER //

CREATE PROCEDURE spLaporanBukuBesar(
	IN varGuid VARCHAR(100),
    IN varTanggalAwal VARCHAR(100),
    IN varTanggalAkhir VARCHAR(100),
    IN varAkun VARCHAR(100)
)
BEGIN
	DECLARE varPeriodeSebelum TEXT;
    DECLARE varTanggalAntaraAwal DATE;
    DECLARE varTanggalAntaraAkhir DATE;
    DECLARE varTanggalAntaraAkhirString TEXT;
    DECLARE varPesan TEXT;
	DECLARE varTanggal TEXT;
	DECLARE varReferensi TEXT;
    DECLARE varKeterangan TEXT;
	DECLARE varDebit DOUBLE;
	DECLARE varKredit DOUBLE;
    DECLARE varSaldoAkhir DOUBLE;
    DECLARE varJumlahSaldoAwal DOUBLE;
    DECLARE varTipeProses TEXT;
    DECLARE varNo DOUBLE;
    DECLARE varId DOUBLE;
    
    SET varTanggalAntaraAkhir = toDate(toTanggalKurang(varTanggalAwal,1));
    SET varTanggalAntaraAkhirString = toTanggalKurang(varTanggalAwal,1);
    
    DELETE FROM templaporanbukubesarawal WHERE guid = varGuid;
    DELETE FROM templaporanbukubesarmutasi WHERE guid = varGuid;
    DELETE FROM templaporanbukubesar WHERE guid = varGuid;
    
    --  Cari mutasi dari awal bulan sampai tanggal awal
    INSERT INTO templaporanbukubesarawal(guid,akun,debit,kredit)
    SELECT varGuid,akun,debit,kredit
    FROM jurnal A 
    WHERE toDate(A.tanggal) <= varTanggalAntaraAkhir AND A.akun LIKE varAkun AND A.status = 'Aktif';
    
    --  Cari mutasi dari tanggalawal sampai tanggalakhir
    INSERT INTO templaporanbukubesarmutasi(guid,akun,no,tanggal,referensi,keterangan,debit,kredit,waktu)
    SELECT varGuid, A.akun, A.no, A.tanggal, A.noreferensi, A.keterangan, A.debit, A.kredit, A.create_at
    FROM jurnal A 
    WHERE toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir) AND A.akun LIKE varAkun AND A.status = 'Aktif'
    ORDER BY toDate(A.tanggal), A.akun, A.create_at, A.noreferensi, A.no;
    
    --  masukan saldo awal                  
    INSERT INTO templaporanbukubesar(guid,akun,id,tanggal,referensi,keterangan,debit,kredit,saldo,waktu)
	SELECT A.guid,A.akun, 0, varTanggalAntaraAkhirString, 'SALDO AWAL','',ROUND(SUM(A.debit),2),ROUND(SUM(A.kredit),2),0, CURRENT_TIMESTAMP()
	FROM templaporanbukubesarawal A
	WHERE A.guid = varGuid
	GROUP BY A.akun;
    
	SET @row_number = 0;

    -- masukan mutasi
    INSERT INTO templaporanbukubesar(guid,akun,id,tanggal,referensi,keterangan,debit,kredit,saldo,waktu)
    SELECT A.guid, A.akun, (@row_number:=@row_number + 1), A.tanggal, A.referensi, A.keterangan, A.debit, A.kredit, 0, A.waktu
    FROM templaporanbukubesarmutasi A
    WHERE A.guid = varGuid
    ORDER BY toDate(A.tanggal), A.waktu, A.referensi, A.no;
    
    -- saldo normal akun
	UPDATE templaporanbukubesar A
    INNER JOIN akun B ON A.akun = B.kode
    SET A.normal = B.saldonormal
    WHERE A.guid = varGuid;
    
    -- update kolom saldo
    UPDATE templaporanbukubesar A
    SET A.saldo = IF(A.normal = 'Debit', A.debit-A.kredit, A.kredit-A.debit)
    WHERE A.guid = varGuid;
    
	-- HASIL
	SELECT A.akun,CONCAT(B.kode,' - ',B.nama) AS nama,A.tanggal, A.referensi, A.keterangan, A.debit, A.kredit, A.saldo
	FROM templaporanbukubesar A
    INNER JOIN akun B ON A.akun = B.kode
	WHERE A.guid = varGuid
	ORDER BY A.akun,A.id;
    
    DELETE FROM templaporanbukubesarawal WHERE guid = varGuid;
    DELETE FROM templaporanbukubesarmutasi WHERE guid = varGuid;
    DELETE FROM templaporanbukubesar WHERE guid = varGuid;
END //
 DELIMITER ;
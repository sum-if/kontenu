/*
	Author		: Santos Sabanari
    Date		: 18-06-2019
    Description	: Untuk laporan kas bank
   Example		: CALL spLaporanKasBank('123','01/01/2020','31/01/2020','1111.02',1,1,'%');
         
DROP TABLE IF EXISTS `templaporankasbank`;
CREATE TABLE IF NOT EXISTS `templaporankasbank` (
  `guid` VARCHAR(300),
  `oswjenisdokumen` VARCHAR(45),
  `noreferensi` VARCHAR(100),
  `tanggal` VARCHAR(500),
  `nourut` VARCHAR(45),
  `kodeakunlawan` VARCHAR(45),
  `namaakunlawan` VARCHAR(300),
  `keterangan` VARCHAR(500),
  `debit` DOUBLE,
  `kredit` DOUBLE)
ENGINE = InnoDB;
*/
DROP PROCEDURE IF EXISTS spLaporanKasBank;

DELIMITER //

CREATE PROCEDURE spLaporanKasBank(
	IN varGuid VARCHAR(100),
    IN varTanggalAwal VARCHAR(100),
    IN varTanggalAkhir VARCHAR(100),
    IN varAkunKasBank VARCHAR(100),
    IN varJenisDebit VARCHAR(45),
    IN varJenisKredit VARCHAR(45),
    IN varAkunLawan VARCHAR(100)
)
BEGIN
	DELETE FROM templaporankasbank WHERE guid = varGuid;
    
    -- SALDO AWAL
    INSERT INTO templaporankasbank(guid,oswjenisdokumen,noreferensi,tanggal,nourut,kodeakunlawan,namaakunlawan,keterangan,debit,kredit)
    SELECT varGuid,'','',toTanggalKurang(varTanggalAwal,1),'','','','SALDO AWAL',IF(A.saldo > 0,A.saldo,0), IF(A.saldo < 0,A.saldo,0)
    FROM (
		SELECT A.akun,SUM(A.debit - A.kredit) AS saldo
		FROM jurnal A
		WHERE A.status = 'Aktif' AND A.akun = varAkunKasBank AND toDate(A.tanggal) < toDate(varTanggalAwal)
    ) A;
    
    -- KAS MASUK
    INSERT INTO templaporankasbank(guid,oswjenisdokumen,noreferensi,tanggal,nourut,kodeakunlawan,namaakunlawan,keterangan,debit,kredit)
    SELECT varGuid,A.oswjenisdokumen,A.noreferensi,A.tanggal,B.nourut,A.akun,C.nama,A.keterangan,A.debit,A.kredit
    FROM jurnal A
    INNER JOIN kasmasuk B ON A.noreferensi = B.kode
    INNER JOIN akun C ON A.akun = C.kode
    WHERE A.status = 'Aktif' AND A.akun = varAkunKasBank AND A.oswjenisdokumen = 'KASMASUK' AND toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir);
    
    -- TRANSFER KAS (MASUK)
    INSERT INTO templaporankasbank(guid,oswjenisdokumen,noreferensi,tanggal,nourut,kodeakunlawan,namaakunlawan,keterangan,debit,kredit)
    SELECT varGuid,A.oswjenisdokumen,A.noreferensi,A.tanggal,B.kode,A.akun,C.nama,A.keterangan,A.debit,A.kredit
    FROM jurnal A
    INNER JOIN transferkas B ON A.noreferensi = B.kode
    INNER JOIN akun C ON A.akun = C.kode
    WHERE A.status = 'Aktif' AND A.akun = varAkunKasBank AND A.oswjenisdokumen = 'TRANSFERKAS' AND toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir);
    
    -- KAS KELUAR
    INSERT INTO templaporankasbank(guid,oswjenisdokumen,noreferensi,tanggal,nourut,kodeakunlawan,namaakunlawan,keterangan,debit,kredit)
    SELECT varGuid,A.oswjenisdokumen,A.noreferensi,A.tanggal,B.nourut,A.akun,C.nama,A.keterangan,A.debit,A.kredit
    FROM jurnal A
    INNER JOIN kaskeluar B ON A.noreferensi = B.kode
    INNER JOIN akun C ON A.akun = C.kode
    WHERE A.status = 'Aktif' AND A.akun = varAkunKasBank AND A.oswjenisdokumen = 'KASKELUAR' AND toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir);
    
    -- PEMBAYARAN PEMBELIAN
    INSERT INTO templaporankasbank(guid,oswjenisdokumen,noreferensi,tanggal,nourut,kodeakunlawan,namaakunlawan,keterangan,debit,kredit)
    SELECT varGuid,A.oswjenisdokumen,A.noreferensi,A.tanggal,B.nourut,A.akun,C.nama,A.keterangan,A.debit,A.kredit
    FROM jurnal A
    INNER JOIN pembayaranpembelian B ON A.noreferensi = B.kode
    INNER JOIN akun C ON A.akun = C.kode
    WHERE A.status = 'Aktif' AND A.akun = varAkunKasBank AND A.oswjenisdokumen = 'PEMBAYARANPEMBELIAN' AND toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir);
    
    -- PEMBAYARAN HUTANG EKSPEDISI
    INSERT INTO templaporankasbank(guid,oswjenisdokumen,noreferensi,tanggal,nourut,kodeakunlawan,namaakunlawan,keterangan,debit,kredit)
    SELECT varGuid,A.oswjenisdokumen,A.noreferensi,A.tanggal,B.nourut,A.akun,C.nama,A.keterangan,A.debit,A.kredit
    FROM jurnal A
    INNER JOIN pembayaranhutangekspedisi B ON A.noreferensi = B.kode
    INNER JOIN akun C ON A.akun = C.kode
    WHERE A.status = 'Aktif' AND A.akun = varAkunKasBank AND A.oswjenisdokumen = 'PEMBAYARANHUTANGEKSPEDISI' AND toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir);
    
    -- PENERIMAAN PENJUALAN
    INSERT INTO templaporankasbank(guid,oswjenisdokumen,noreferensi,tanggal,nourut,kodeakunlawan,namaakunlawan,keterangan,debit,kredit)
    SELECT varGuid,A.oswjenisdokumen,A.noreferensi,A.tanggal,B.nourut,A.akun,C.nama,A.keterangan,A.debit,A.kredit
    FROM jurnal A
    INNER JOIN penerimaanpenjualan B ON A.noreferensi = B.kode
    INNER JOIN akun C ON A.akun = C.kode
    WHERE A.status = 'Aktif' AND A.akun = varAkunKasBank AND A.oswjenisdokumen = 'PENERIMAANPENJUALAN' AND toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir);
    
    -- SETTLEMENT CHEQUE
    INSERT INTO templaporankasbank(guid,oswjenisdokumen,noreferensi,tanggal,nourut,kodeakunlawan,namaakunlawan,keterangan,debit,kredit)
    SELECT varGuid,A.oswjenisdokumen,A.noreferensi,A.tanggal,B.nourut,A.akun,C.nama,A.keterangan,A.debit,A.kredit
    FROM jurnal A
    INNER JOIN settlementchecque B ON A.noreferensi = B.kode
    INNER JOIN akun C ON A.akun = C.kode
    WHERE A.status = 'Aktif' AND A.akun = varAkunKasBank AND A.oswjenisdokumen = 'SETTLEMENTCHEQUE' AND toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir);
    
    /*
    -- TRANSFER KAS (KELUAR)
    INSERT INTO templaporankasbank(guid,oswjenisdokumen,noreferensi,tanggal,nourut,kodeakunlawan,namaakunlawan,keterangan,debit,kredit)
    SELECT varGuid,A.oswjenisdokumen,A.noreferensi,A.tanggal,B.kode,A.akun,C.nama,A.keterangan,A.debit,A.kredit
    FROM jurnal A
    INNER JOIN transferkas B ON A.noreferensi = B.kode
    INNER JOIN akun C ON A.akun = C.kode
    WHERE A.status = 'Aktif' AND A.akun = varAkunKasBank AND A.oswjenisdokumen = 'TRANSFERKAS' AND toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir);
    */
    
	-- HASIL
	SELECT A.noreferensi,A.tanggal,A.nourut,A.kodeakunlawan,A.namaakunlawan,A.keterangan,A.debit,A.kredit,IF(A.keterangan = 'SALDO AWAL',0,A.debit) AS debit2, IF(A.keterangan = 'SALDO AWAL',0,A.kredit) AS kredit2
	FROM templaporankasbank A
	WHERE A.guid = varGuid AND (IF(varJenisDebit > 0, A.debit > 0, FALSE) OR IF(varJenisKredit > 0, A.kredit > 0, FALSE))
	ORDER BY toDate(A.tanggal),A.nourut;
    
    DELETE FROM templaporankasbank WHERE guid = varGuid;
END //
 DELIMITER ;
/*
	Author		: Santos Sabanari
    Date		: 11-01-2020
    Description	: Untuk Laporan Neraca Lajur
            
	Example		: CALL spLaporanNeracaLajur('123','201904');

	
CREATE TABLE `tempjurnal` (
  `oswjenisdokumen` varchar(45) DEFAULT NULL,
  `noreferensi` varchar(45) DEFAULT NULL,
  `tanggal` varchar(45) DEFAULT NULL,
  `keterangan` text,
  `no` int(11) DEFAULT NULL,
  `akun` varchar(45) DEFAULT NULL,
  `debit` double DEFAULT NULL,
  `kredit` double DEFAULT NULL,
  `status` varchar(45) DEFAULT NULL,
  `jurnalpenutup` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
*/

DROP PROCEDURE IF EXISTS spLaporanNeracaLajur;

DELIMITER //

CREATE PROCEDURE spLaporanNeracaLajur(
	IN varGuid VARCHAR(1000),
    IN varPeriode VARCHAR(1000)
)
BEGIN
    DECLARE varJumlahJurnalPenutupDebit DOUBLE;
    DECLARE varJumlahJurnalPenutupKredit DOUBLE;
    DECLARE varJumlah DOUBLE;
    DECLARE saldoLabaRugiBerjalan DOUBLE;
    DECLARE varNominal DOUBLE;
    DECLARE varDebit DOUBLE;
    DECLARE varKredit DOUBLE;
    
    DECLARE saldoNormalLabaRugiBerjalan TEXT;
    DECLARE varTransaksiJurnalPenutup TEXT;
	DECLARE varTransaksiJurnalPenutupKeterangan TEXT;
    
    DECLARE varLastDate TEXT;
    DECLARE varLastDateTimestamp TIMESTAMP;
    DECLARE varPeriodeBerikut TEXT;
    DECLARE varPeriodeSebelum TEXT;
    DECLARE varKodeAkunLabaRugiBerjalan TEXT;
    DECLARE varKodeAkunLabaRugiDitahan TEXT;
    DECLARE varTidak TEXT;
    DECLARE varYa TEXT;
    DECLARE varStatusAktif TEXT;
    
    DECLARE varFormatDate TEXT;
    
    SET varFormatDate = '%d/%m/%Y';
    
    SET varTransaksiJurnalPenutup = 'JURNALPENUTUP';
	SET varTransaksiJurnalPenutupKeterangan = 'Jurnal Penutup';
	
    SET varLastDate = toTanggalAkhirPeriode(varPeriode);
    SET varLastDateTimestamp = toTanggalAkhirPeriodeDateTime(varPeriode);
    SET varPeriodeBerikut = toPeriodeBerikut(varPeriode);
    SET varPeriodeSebelum = toPeriodeSebelum(varPeriode);
    
    SET varTidak = 'Tidak';
    SET varYa = 'Ya';
    SET varStatusAktif = 'Aktif';
    
	-- Delete isi temp table 
    DELETE FROM tempsaldoakunbulanan WHERE guid = varGuid;
    
    -- 1. Pindah data periode sebelum ke tabel tamp
    INSERT INTO tempsaldoakunbulanan(guid,periode,akun,awaldebit,awalkredit,mutasidebit,mutasikredit,laporankeuangandebit,laporankeuangankredit,penutupdebit,penutupkredit,akhirdebit,akhirkredit)
	SELECT varGuid,varPeriode,akun,akhirdebit,akhirkredit,0,0,0,0,0,0,0,0
    FROM saldoakunbulanan
    WHERE periode = varPeriodeSebelum;
    
    -- 2. Isi kolom mutasi dari tabel jurnal yang statusnya 'Ya' dan berada dalam periode tersebut
    -- update akun yang sudah ada
    UPDATE tempsaldoakunbulanan A
    INNER JOIN (
		SELECT  akun,ROUND(SUM(debit),2) AS debit, ROUND(SUM(kredit),2) AS kredit
		FROM jurnal
		WHERE DATE_FORMAT(STR_TO_DATE(tanggal, varFormatDate),'%Y%m') = varPeriode AND status = varStatusAktif AND jurnalpenutup = varTidak
		GROUP BY akun
    ) B ON A.guid = varGuid AND A.periode = varPeriode AND A.akun = B.akun
    SET A.mutasidebit = B.debit,
		A.mutasikredit = B.kredit;
    
    -- insert akun yang belum ada
    INSERT INTO tempsaldoakunbulanan(guid,periode,akun,awaldebit,awalkredit,mutasidebit,mutasikredit,laporankeuangandebit,laporankeuangankredit,penutupdebit,penutupkredit,akhirdebit,akhirkredit)
    SELECT varGuid,varPeriode,A.akun,0,0,A.debit,A.kredit,0,0,0,0,0,0
    FROM (
		SELECT akun,ROUND(SUM(debit),2) AS debit, ROUND(SUM(kredit),2) AS kredit
		FROM jurnal
		WHERE DATE_FORMAT(STR_TO_DATE(tanggal, varFormatDate),'%Y%m') = varPeriode AND status = varStatusAktif AND jurnalpenutup = varTidak
		GROUP BY akun
    ) A 
    LEFT JOIN tempsaldoakunbulanan B ON B.guid = varGuid AND B.periode = varPeriode AND A.akun = B.akun
    WHERE B.akun IS NULL;
    
    -- 3. Isi Kolom laporankeuangan dari kolom awal + kolom mutasi
    UPDATE tempsaldoakunbulanan A
    SET A.laporankeuangandebit = A.awaldebit + A.mutasidebit,
		A.laporankeuangankredit = A.awalkredit + A.mutasikredit
	WHERE A.guid = varGuid AND A.periode = varPeriode;
    
    -- 4. Buat jurnal penutup
    -- Buat jurnal dengan kolom jurnalpenutup = 'Ya' (Nilai kolom debit dan kredit di balik saat jurnal)
    INSERT INTO tempjurnal(oswjenisdokumen, noreferensi, tanggal, keterangan, akun, debit, kredit, status, jurnalpenutup)
    SELECT varTransaksiJurnalPenutup,'-',varLastDate,varTransaksiJurnalPenutupKeterangan, A.akun, A.laporankeuangankredit,A.laporankeuangandebit, varStatusAktif, varYa
    FROM tempsaldoakunbulanan A
    INNER JOIN akun B ON A.akun = B.kode
    INNER JOIN kelompokakunsetting C ON B.kode LIKE C.akun AND B.akunkategori LIKE C.kategori AND B.akunsubkategori LIKE C.subkategori AND B.akungroup LIKE C.`group` AND B.akunsubgroup LIKE C.subgroup
    WHERE A.guid = varGuid AND A.periode = varPeriode AND C.kelompokakun = 'akun_yang_ditutup' AND NOT (A.laporankeuangankredit = 0 AND A.laporankeuangandebit = 0);
    
    -- Ambil selisih debit dan kredit
    SET varJumlahJurnalPenutupDebit = (	SELECT ROUND(SUM(debit),2)
										FROM tempjurnal
										WHERE DATE_FORMAT(STR_TO_DATE(tanggal, varFormatDate),'%Y%m') = varPeriode AND status = varStatusAktif AND jurnalpenutup = varYa);
    
    SET varJumlahJurnalPenutupKredit = (SELECT ROUND(SUM(kredit),2)
										FROM tempjurnal
										WHERE DATE_FORMAT(STR_TO_DATE(tanggal, varFormatDate),'%Y%m') = varPeriode AND status = varStatusAktif AND jurnalpenutup = varYa);
    
    -- 5. Jurnalkan dengan akun Laba rugi Berjalan
    IF varJumlahJurnalPenutupDebit != 0 || varJumlahJurnalPenutupKredit != 0 THEN
        SET varKodeAkunLabaRugiBerjalan = ( SELECT isi
											FROM oswsetting 
											WHERE kode ='akun_laba_rugi_tahun_berjalan');
		
        -- ambil saldo laba rugi tahun berjalan
        SET varJumlah = (	SELECT COUNT(*) 
							FROM saldoakunbulanan
							WHERE akun = varKodeAkunLabaRugiBerjalan AND periode = varPeriodeSebelum);
		
		IF varJumlah = 0 THEN
			SET saldoLabaRugiBerjalan = 0;
		ELSE 
			SET saldoLabaRugiBerjalan = (	SELECT akhirdebit-akhirkredit 
											FROM saldoakunbulanan
											WHERE akun = varKodeAkunLabaRugiBerjalan AND periode = varPeriodeSebelum);
		END IF;
        
        -- ambil jenis saldo normal laba rugi berjalan
        SET saldoNormalLabaRugiBerjalan = (	SELECT saldonormal 
											FROM akun
											WHERE kode = varKodeAkunLabaRugiBerjalan);
    
		IF(varJumlahJurnalPenutupDebit > varJumlahJurnalPenutupKredit) THEN
			INSERT INTO tempjurnal(oswjenisdokumen, noreferensi, tanggal, keterangan, akun, debit, kredit, status, jurnalpenutup)
					VALUES(varTransaksiJurnalPenutup,'-',varLastDate,varTransaksiJurnalPenutupKeterangan, varKodeAkunLabaRugiBerjalan, 0,COALESCE(varJumlahJurnalPenutupDebit,0) - COALESCE(varJumlahJurnalPenutupKredit,0), varStatusAktif, varYa);
			
            IF saldoNormalLabaRugiBerjalan = 'Debit' THEN
				SET saldoLabaRugiBerjalan = saldoLabaRugiBerjalan + (0-((COALESCE(varJumlahJurnalPenutupDebit,0) - COALESCE(varJumlahJurnalPenutupKredit,0))));
			ELSE
				SET saldoLabaRugiBerjalan = saldoLabaRugiBerjalan + ((COALESCE(varJumlahJurnalPenutupDebit,0) - COALESCE(varJumlahJurnalPenutupKredit,0)));
            END IF;
		ELSE
			INSERT INTO tempjurnal(oswjenisdokumen, noreferensi, tanggal, keterangan, akun, debit, kredit, status, jurnalpenutup)
					VALUES(varTransaksiJurnalPenutup,'-',varLastDate,varTransaksiJurnalPenutupKeterangan, varKodeAkunLabaRugiBerjalan, COALESCE(varJumlahJurnalPenutupKredit,0) - COALESCE(varJumlahJurnalPenutupDebit,0), 0, varStatusAktif, varYa);
			
            IF saldoNormalLabaRugiBerjalan = 'Debit' THEN
				SET saldoLabaRugiBerjalan = saldoLabaRugiBerjalan + (COALESCE(varJumlahJurnalPenutupKredit,0) - COALESCE(varJumlahJurnalPenutupDebit,0));
			ELSE
				SET saldoLabaRugiBerjalan = saldoLabaRugiBerjalan + (0-(COALESCE(varJumlahJurnalPenutupKredit,0) - COALESCE(varJumlahJurnalPenutupDebit,0)));
            END IF;
		END IF;
    END IF;
    
    -- Jika Bulan 01, Tutup Laba Rugi Ditahan
    IF LPAD(MONTH(STR_TO_DATE(varPeriode,'%Y%m')),2,'0') = '01' THEN 
		SET varKodeAkunLabaRugiBerjalan = (	SELECT isi 
						FROM oswsetting 
						WHERE kode ='akun_laba_rugi_tahun_berjalan');

		SET varKodeAkunLabaRugiDitahan = (	SELECT isi 
						FROM oswsetting 
						WHERE kode ='akun_laba_rugi_ditahan');
		
		SET varDebit = ( SELECT akhirdebit
						FROM saldoakunbulanan A
						WHERE A.periode = varPeriodeSebelum AND A.akun = varKodeAkunLabaRugiBerjalan);
    
		SET varKredit = ( SELECT akhirkredit
						FROM saldoakunbulanan A
						WHERE A.periode = varPeriodeSebelum AND A.akun = varKodeAkunLabaRugiBerjalan);
                        
		SET varNominal = varKredit - varDebit;
                        
		-- Buat jurnal dengan kolom jurnalpenutup = 'Ya'
        IF varNominal > 0 THEN
			INSERT INTO tempjurnal(oswjenisdokumen, noreferensi, tanggal, keterangan, akun, debit, kredit, status, jurnalpenutup)
			VALUES ('JURNALPENUTUP','-',varLastDate,'Jurnal Penutup Laba Rugi Ditahan', varKodeAkunLabaRugiBerjalan, varNominal, 0, 'Aktif', 'Ya');
			
			INSERT INTO tempjurnal(oswjenisdokumen, noreferensi, tanggal, keterangan, akun, debit, kredit, status, jurnalpenutup)
			VALUES ('JURNALPENUTUP','-',varLastDate,'Jurnal Penutup Laba Rugi Ditahan', varKodeAkunLabaRugiDitahan, 0, varNominal, 'Aktif', 'Ya');
        ELSE 
			SET varNominal = varNominal * -1;
			INSERT INTO tempjurnal(oswjenisdokumen, noreferensi, tanggal, keterangan, akun, debit, kredit, status, jurnalpenutup)
			VALUES ('JURNALPENUTUP','-',varLastDate,'Jurnal Penutup Laba Rugi Ditahan', varKodeAkunLabaRugiBerjalan, 0,varNominal, 'Aktif', 'Ya');
			
			INSERT INTO tempjurnal(oswjenisdokumen, noreferensi, tanggal, keterangan, akun, debit, kredit, status, jurnalpenutup)
			VALUES ('JURNALPENUTUP','-',varLastDate,'Jurnal Penutup Laba Rugi Ditahan', varKodeAkunLabaRugiDitahan, varNominal, 0, 'Aktif', 'Ya');
        END IF;
		
    END IF;
    
    -- 8. Isi kolom penutup dengan jurnal penutup yang barusan dibuat
    -- update akun yang sudah ada
    UPDATE tempsaldoakunbulanan A
    INNER JOIN (
		SELECT  akun,ROUND(SUM(debit),2) AS debit, ROUND(SUM(kredit),2) AS kredit
		FROM tempjurnal
		WHERE DATE_FORMAT(STR_TO_DATE(tanggal, varFormatDate),'%Y%m') = varPeriode AND status = varStatusAktif AND jurnalpenutup = varYa
		GROUP BY akun
    ) B ON A.guid = varGuid AND A.periode = varPeriode AND A.akun = B.akun
    SET A.penutupdebit = B.debit,
		A.penutupkredit = B.kredit;
    
    -- insert akun yang belum ada
    INSERT INTO tempsaldoakunbulanan(guid,periode,akun,awaldebit,awalkredit,mutasidebit,mutasikredit,laporankeuangandebit,laporankeuangankredit,penutupdebit,penutupkredit,akhirdebit,akhirkredit)
    SELECT varGuid,varPeriode,A.akun,0,0,0,0,0,0,A.debit,A.kredit,0,0
    FROM (
		SELECT akun,ROUND(SUM(debit),2) AS debit, ROUND(SUM(kredit),2) AS kredit
		FROM tempjurnal
		WHERE DATE_FORMAT(STR_TO_DATE(tanggal, varFormatDate),'%Y%m') = varPeriode AND status = varStatusAktif AND jurnalpenutup = varYa
		GROUP BY akun
    ) A 
    LEFT JOIN tempsaldoakunbulanan B ON B.guid = varGuid AND B.periode = varPeriode AND A.akun = B.akun
    WHERE B.akun IS NULL;
    
    -- 9. Isi kolom akhir dengan selisih kolom debit dan kredit, masuk ke kolom yang terbesar
    UPDATE tempsaldoakunbulanan A
    SET A.akhirdebit = CASE WHEN (laporankeuangandebit + penutupdebit) >= (laporankeuangankredit+penutupkredit) THEN (laporankeuangandebit + penutupdebit) - (laporankeuangankredit+penutupkredit)
							ELSE 0
							END,
		A.akhirkredit = CASE WHEN (laporankeuangandebit + penutupdebit) < (laporankeuangankredit+penutupkredit) THEN (laporankeuangankredit+penutupkredit) - (laporankeuangandebit + penutupdebit)
							ELSE 0
							END
    WHERE A.guid = varGuid AND A.periode = varPeriode;
    
	-- 10. HASIL
    SELECT A.akun, B.nama namaakun,A.awaldebit,A.awalkredit,A.mutasidebit,A.mutasikredit,A.laporankeuangandebit,A.laporankeuangankredit,A.penutupdebit,A.penutupkredit,A.akhirdebit,A.akhirkredit
    FROM tempsaldoakunbulanan A
	INNER JOIN akun B ON A.akun = B.kode
    WHERE A.guid = varGuid AND A.periode = varPeriode;
    
    -- 11. Delete isi temp table 
    DELETE FROM tempsaldoakunbulanan WHERE guid = varGuid;
    
END //
 DELIMITER ;
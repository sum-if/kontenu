/*
	Author		: Santos Sabanari
    Date		: 03-10-2016
    Description	: Untuk menutup akun bulanan periode yang diinginkan
    Step		: 
			1. Pindah data periode dari tabel asli ke tabel tamp
			2. Isi kolom mutasi dari tabel jurnal yang statusnya 'Ya' dan berada dalam periode tersebut
			3. Isi Kolom laporankeuangan dari kolom awal + kolom mutasi
			4. Buat jurnal penutup
			5. Jurnalkan dengan akun Laba rugi Berjalan
			6. jurnal laba rugi di tahan --> jika proses untuk bulan 12
            7. Update saldo aktual akun yang di tutup
			8. Isi kolom penutup dengan jurnal penutup yang barusan dibuat
			9. Isi kolom akhir dengan selisih kolom debit dan kredit, masuk ke kolom yang terbesar
			10. Pindah dari temp ke tabel asli
			11. Delete isi temp table 
            
	Example		: CALL spTutupPeriodeAkunBulanan('123','201704','April','2017','MLG','%d/%m/%Y','SANTOS');

*/

DROP PROCEDURE IF EXISTS spTutupPeriodeAkunBulanan;

DELIMITER //

CREATE PROCEDURE spTutupPeriodeAkunBulanan(
	IN varGuid VARCHAR(1000),
    IN varPeriode VARCHAR(1000),
    IN varNamaBulan VARCHAR(1000),
    IN varTahun VARCHAR(1000),
    IN varFormatDate VARCHAR(1000),
    IN varUserLogin VARCHAR(1000)
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
    INSERT INTO jurnal(oswjenisdokumen, noreferensi, tanggal, keterangan, akun, debit, kredit, status, jurnalpenutup, tanggalrekonsiliasi, version, create_at, create_user)
    SELECT varTransaksiJurnalPenutup,'-',varLastDate,varTransaksiJurnalPenutupKeterangan, A.akun, A.laporankeuangankredit,A.laporankeuangandebit, varStatusAktif, varYa, '', '1', varLastDateTimestamp, varUserLogin
    FROM tempsaldoakunbulanan A
    INNER JOIN akun B ON A.akun = B.kode
    INNER JOIN kelompokakunsetting C ON B.kode LIKE C.akun AND B.akunkategori LIKE C.kategori AND B.akunsubkategori LIKE C.subkategori AND B.akungroup LIKE C.`group` AND B.akunsubgroup LIKE C.subgroup
    WHERE A.guid = varGuid AND A.periode = varPeriode AND C.kelompokakun = 'akun_yang_ditutup' AND NOT (A.laporankeuangankredit = 0 AND A.laporankeuangandebit = 0);
    
    -- Ambil selisih debit dan kredit
    SET varJumlahJurnalPenutupDebit = (	SELECT ROUND(SUM(debit),2)
										FROM jurnal
										WHERE DATE_FORMAT(STR_TO_DATE(tanggal, varFormatDate),'%Y%m') = varPeriode AND status = varStatusAktif AND jurnalpenutup = varYa);
    
    SET varJumlahJurnalPenutupKredit = (SELECT ROUND(SUM(kredit),2)
										FROM jurnal
										WHERE DATE_FORMAT(STR_TO_DATE(tanggal, varFormatDate),'%Y%m') = varPeriode AND status = varStatusAktif AND jurnalpenutup = varYa);
    
    -- 5. Jurnalkan dengan akun Laba rugi Berjalan
    IF varJumlahJurnalPenutupDebit != 0 || varJumlahJurnalPenutupKredit != 0 THEN
        SET varKodeAkunLabaRugiBerjalan = ( SELECT isi
											FROM oswsetting 
											WHERE kode ='akun_laba_rugi_tahun_berjalan');
		
        -- ambil saldo laba rugi tahun berjalan
        SET varJumlah = (	SELECT COUNT(*) 
							FROM saldoakunaktual
							WHERE akun = varKodeAkunLabaRugiBerjalan);
		
		IF varJumlah = 0 THEN
			SET saldoLabaRugiBerjalan = 0;
		ELSE 
			SET saldoLabaRugiBerjalan = (	SELECT jumlah 
											FROM saldoakunaktual
											WHERE akun = varKodeAkunLabaRugiBerjalan);
		END IF;
        
        -- ambil jenis saldo normal laba rugi berjalan
        SET saldoNormalLabaRugiBerjalan = (	SELECT saldonormal 
											FROM akun
											WHERE kode = varKodeAkunLabaRugiBerjalan);
    
		IF(varJumlahJurnalPenutupDebit > varJumlahJurnalPenutupKredit) THEN
			INSERT INTO jurnal(oswjenisdokumen, noreferensi, tanggal, keterangan, akun, debit, kredit, status, jurnalpenutup, tanggalrekonsiliasi, version, create_at, create_user)
					VALUES(varTransaksiJurnalPenutup,'-',varLastDate,varTransaksiJurnalPenutupKeterangan, varKodeAkunLabaRugiBerjalan, 0,COALESCE(varJumlahJurnalPenutupDebit,0) - COALESCE(varJumlahJurnalPenutupKredit,0), varStatusAktif, varYa, '', '1', varLastDateTimestamp, varUserLogin);
			
            IF saldoNormalLabaRugiBerjalan = 'Debit' THEN
				SET saldoLabaRugiBerjalan = saldoLabaRugiBerjalan + (0-((COALESCE(varJumlahJurnalPenutupDebit,0) - COALESCE(varJumlahJurnalPenutupKredit,0))));
			ELSE
				SET saldoLabaRugiBerjalan = saldoLabaRugiBerjalan + ((COALESCE(varJumlahJurnalPenutupDebit,0) - COALESCE(varJumlahJurnalPenutupKredit,0)));
            END IF;
		ELSE
			INSERT INTO jurnal(oswjenisdokumen, noreferensi, tanggal, keterangan, akun, debit, kredit, status, jurnalpenutup, tanggalrekonsiliasi, version, create_at, create_user)
					VALUES(varTransaksiJurnalPenutup,'-',varLastDate,varTransaksiJurnalPenutupKeterangan, varKodeAkunLabaRugiBerjalan, COALESCE(varJumlahJurnalPenutupKredit,0) - COALESCE(varJumlahJurnalPenutupDebit,0), 0, varStatusAktif, varYa, '', '1', varLastDateTimestamp, varUserLogin);
			
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
			INSERT INTO jurnal(oswjenisdokumen, noreferensi, tanggal, keterangan, akun, debit, kredit, status, jurnalpenutup, tanggalrekonsiliasi, version, create_at, create_user)
			VALUES ('JURNALPENUTUP','-',varLastDate,'Jurnal Penutup Laba Rugi Ditahan', varKodeAkunLabaRugiBerjalan, varNominal, 0, 'Aktif', 'Ya', '', '1', varLastDateTimestamp, varUserLogin);
			
			INSERT INTO jurnal(oswjenisdokumen, noreferensi, tanggal, keterangan, akun, debit, kredit, status, jurnalpenutup, tanggalrekonsiliasi, version, create_at, create_user)
			VALUES ('JURNALPENUTUP','-',varLastDate,'Jurnal Penutup Laba Rugi Ditahan', varKodeAkunLabaRugiDitahan, 0, varNominal, 'Aktif', 'Ya', '', '1', varLastDateTimestamp, varUserLogin);
        ELSE 
			SET varNominal = varNominal * -1;
			INSERT INTO jurnal(oswjenisdokumen, noreferensi, tanggal, keterangan, akun, debit, kredit, status, jurnalpenutup, tanggalrekonsiliasi, version, create_at, create_user)
			VALUES ('JURNALPENUTUP','-',varLastDate,'Jurnal Penutup Laba Rugi Ditahan', varKodeAkunLabaRugiBerjalan, 0,varNominal, 'Aktif', 'Ya', '', '1', varLastDateTimestamp, varUserLogin);
			
			INSERT INTO jurnal(oswjenisdokumen, noreferensi, tanggal, keterangan, akun, debit, kredit, status, jurnalpenutup, tanggalrekonsiliasi, version, create_at, create_user)
			VALUES ('JURNALPENUTUP','-',varLastDate,'Jurnal Penutup Laba Rugi Ditahan', varKodeAkunLabaRugiDitahan, varNominal, 0, 'Aktif', 'Ya', '', '1', varLastDateTimestamp, varUserLogin);
        END IF;
		
    END IF;
    
    -- 7. Update saldo aktual akun yang di tutup
    UPDATE saldoakunaktual A
    INNER JOIN (
		SELECT  A.akun,ROUND(SUM(CASE WHEN B.saldonormal = 'Debit' THEN A.debit-A.kredit ELSE A.kredit-A.debit END),2) AS selisih
		FROM jurnal A
		INNER JOIN akun B ON A.akun = B.kode
		WHERE DATE_FORMAT(STR_TO_DATE(A.tanggal, varFormatDate),'%Y%m') = varPeriode AND A.status = varStatusAktif AND A.jurnalpenutup = varYa
		GROUP BY A.akun
    ) B ON A.akun = B.akun
    SET A.jumlah = A.jumlah + B.selisih;
    
    -- insert saldo aktual akun yang di tutup
    INSERT INTO saldoakunaktual(akun,jumlah,version,create_at,create_user)
    SELECT A.akun,A.selisih,1, varLastDateTimestamp, varUserLogin
    FROM (
		SELECT  A.akun,ROUND(SUM(CASE WHEN B.saldonormal = 'Debit' THEN A.debit-A.kredit ELSE A.kredit-A.debit END),2) AS selisih
		FROM jurnal A
		INNER JOIN akun B ON A.akun = B.kode
		WHERE DATE_FORMAT(STR_TO_DATE(A.tanggal, varFormatDate),'%Y%m') = varPeriode AND A.status = varStatusAktif AND A.jurnalpenutup = varYa
		GROUP BY A.akun
    ) A 
    LEFT JOIN saldoakunaktual B ON A.akun = B.akun
    WHERE B.akun IS NULL;
    
    -- hapus saldo aktual yang jumlahnya 0
    DELETE FROM saldoakunaktual WHERE jumlah = 0;
    
    -- 8. Isi kolom penutup dengan jurnal penutup yang barusan dibuat
    -- update akun yang sudah ada
    UPDATE tempsaldoakunbulanan A
    INNER JOIN (
		SELECT  akun,ROUND(SUM(debit),2) AS debit, ROUND(SUM(kredit),2) AS kredit
		FROM jurnal
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
		FROM jurnal
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
    
	-- 10. Pindah dari temp ke tabel asli
    INSERT INTO saldoakunbulanan(periode,akun,awaldebit,awalkredit,mutasidebit,mutasikredit,laporankeuangandebit,laporankeuangankredit,penutupdebit,penutupkredit,akhirdebit,akhirkredit)
    SELECT periode,akun,awaldebit,awalkredit,mutasidebit,mutasikredit,laporankeuangandebit,laporankeuangankredit,penutupdebit,penutupkredit,akhirdebit,akhirkredit
    FROM tempsaldoakunbulanan A
    WHERE A.guid = varGuid AND A.periode = varPeriode;
    
    -- 11. Delete isi temp table 
    DELETE FROM tempsaldoakunbulanan WHERE guid = varGuid;
    
END //
 DELIMITER ;
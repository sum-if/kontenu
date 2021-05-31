/*
	Author		: Santos Sabanari
    Date		: 10-07-2017
    Description	: Validasi Untuk menutup periode yang diinginkan
    Step		: 
			1. Validasi 
				- Sudah tutup periode atau belum
                - Sudah ada tutup periode pada periode berikutnya
                - cari periode sebelumnya sudah ada atau belum, jika blm cek lagi apakah sebelum2nya sudah ada
                - Cek Akun Laba Rugi Tahun Berjalan ada atau tidak
                - Cek Akun Laba Rugi ditahan ada atau tidak
*/
DROP PROCEDURE IF EXISTS spTutupPeriodeValidasi;

DELIMITER //

CREATE PROCEDURE spTutupPeriodeValidasi(
	IN varGuid VARCHAR(1000),
    IN varPeriode VARCHAR(1000),
    IN varNamaBulan VARCHAR(1000),
    IN varTahun VARCHAR(1000),
    IN varTipeProses VARCHAR(1000)
)
BEGIN
	DECLARE varPesan TEXT;
    DECLARE varText TEXT;
	DECLARE varJumlahTutup NUMERIC;
    DECLARE varJumlahAkunLabaRugi NUMERIC;
    DECLARE varJumlahAkun NUMERIC;
    
    DECLARE varJumlahSebelum NUMERIC;
    DECLARE varJumlahSebelum2 NUMERIC;
    DECLARE varTutupHarian NUMERIC;
        
    DECLARE varPeriodeSebelum TEXT;
    DECLARE varTanggalTerakhir TEXT;
    
    DECLARE varTipeProses TEXT;
    DECLARE varStatusTidak TEXT;
    DECLARE varStatusYa TEXT;
    
    DECLARE varKodePusat TEXT;
    
    SET varPeriodeSebelum = toPeriodeSebelum(varPeriode);
    
    SET varTanggalTerakhir = toTanggalAkhirPeriode(varPeriode);
    
    SET varTipeProses = 'Tutup Periode'; 
    SET varStatusTidak = 'Tidak';
    SET varStatusYa = 'Ya';
							
	-- 1. Validasi 
	-- Sudah tutup periode atau belum
	SET varJumlahTutup = ( 	SELECT COUNT(*) 
							FROM admin
							WHERE periode = varPeriode AND proses = varTipeProses);
    
    IF varJumlahTutup > 0 THEN
		SET varPesan = CONCAT('Periode \'',varNamaBulan, ' ', varTahun,'\' telah di proses');
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = varPesan;
    END IF;
    
	-- Sudah ada tutup periode pada periode berikutnya
    SET varJumlahTutup = (	SELECT COUNT(*) 
							FROM admin
							WHERE periode > varPeriode AND proses = varTipeProses);
    
    IF varJumlahTutup > 0 THEN
		SET varPesan = CONCAT('Periode setelah \'',varNamaBulan, ' ', varTahun,'\' telah di proses');
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = varPesan;
    END IF;
    
    -- cari periode sebelumnya sudah ada atau belum, jika blm cek lagi apakah sebelum2nya sudah ada
    SET varJumlahSebelum = (SELECT COUNT(*) 
							FROM admin A
							WHERE A.periode = varPeriodeSebelum);
    
    SET varJumlahSebelum2 = (	SELECT COUNT(*) 
								FROM admin A
								WHERE A.periode < varPeriodeSebelum);
     
    IF varJumlahSebelum = 0 AND varJumlahSebelum2 > 0 THEN
		SET varPesan = CONCAT('Periode sebelum \'',varNamaBulan,' ', varTahun,'\' harus telah di proses');
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = varPesan;
    END IF;
    
    -- Cek Akun Laba Rugi Tahun Berjalan ada atau tidak
    SET varJumlahAkun = (	SELECT COUNT(*) 
							FROM oswsetting 
							WHERE kode = 'akun_laba_rugi_tahun_berjalan');
    
    IF varJumlahAkun = 0 THEN
		SET varPesan = CONCAT('Akun Laba Rugi Tahun Berjalan belum disetting');
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = varPesan;
    END IF;
    
    SET varText = (	SELECT isi 
					FROM oswsetting 
					WHERE kode ='akun_laba_rugi_tahun_berjalan');
    
    SET varJumlahAkun = (	SELECT COUNT(*) 
							FROM akun 
							WHERE kode = varText);
    
    IF varJumlahAkun = 0 THEN
		SET varPesan = CONCAT('Akun Laba Rugi Tahun Berjalan \'',varText,'\' tidak ditemukan');
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = varPesan;
    END IF;
    
    -- Cek Akun Laba Rugi ditahan ada atau tidak
    SET varJumlahAkun = (	SELECT COUNT(*) 
							FROM oswsetting 
							WHERE kode ='akun_laba_rugi_ditahan');
    
     IF varJumlahAkun = 0 THEN
		SET varPesan = CONCAT('Akun Laba Rugi Ditahan belum disetting');
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = varPesan;
    END IF;
    
    SET varText = (	SELECT isi 
					FROM oswsetting 
					WHERE kode ='akun_laba_rugi_ditahan');
    
    SET varJumlahAkun = (	SELECT COUNT(*) 
							FROM akun 
							WHERE kode = varText);
    
    IF varJumlahAkun = 0 THEN
		SET varPesan = CONCAT('Akun Laba Rugi Ditahan \'',varText,'\' tidak ditemukan');
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = varPesan;
    END IF;               
END //
 DELIMITER ;
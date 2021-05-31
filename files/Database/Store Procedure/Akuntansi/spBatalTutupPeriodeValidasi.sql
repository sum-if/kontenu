/*
	Author		: Santos Sabanari
    Date		: 21-10-2016
    Modified	: 19-07-2017
    Description	: Validasi Untuk batal menutup periode yang diinginkan
    Step		: 
			1. Validasi 
				- Cek apakah sudah tutup periode
                - Cek tutup periode yang lebih baru dari yang akan di tutup
*/
DROP PROCEDURE IF EXISTS spBatalTutupPeriodeValidasi;

DELIMITER //

CREATE PROCEDURE spBatalTutupPeriodeValidasi(
	IN varGuid VARCHAR(1000),
    IN varPeriode VARCHAR(1000)
)
BEGIN
	DECLARE varPesan TEXT;
    DECLARE varText TEXT;
	DECLARE varJumlah NUMERIC;
    DECLARE varTipeProses TEXT;
    DECLARE varStatusTidak TEXT;
    DECLARE varStatusYa TEXT;
    DECLARE varKodePusat TEXT;
    
    SET varTipeProses = 'Tutup Periode'; 
    SET varStatusTidak = 'Tidak';
    SET varStatusYa = 'Ya';
							
	-- 1. Validasi 
    -- Cek apakah sudah tutup periode
    SET varJumlah = (	SELECT COUNT(*)
						FROM admin A
						WHERE A.periode = varPeriode AND A.proses = varTipeProses);
		
	IF varJumlah = 0 THEN
		SET varPesan = CONCAT('Tutup periode belum dilakukan');
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = varPesan;
	END IF;
    
    -- Cek tutup periode yang lebih baru dari yang akan di tutup
    SET varJumlah = (	SELECT COUNT(*) 
						FROM admin A
						WHERE A.periode > varPeriode AND A.proses = varTipeProses);
    
    IF varJumlah > 0 THEN
		SET varPesan = CONCAT('Terdapat tutup periode untuk periode yang lebih baru');
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = varPesan;
    END IF;
    
END //
 DELIMITER ;
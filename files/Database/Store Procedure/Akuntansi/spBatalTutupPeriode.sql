/*
	Author		: Santos Sabanari
    Date		: 19-10-2016
    Modified	: 19-07-2017
    Description	: Untuk membatalkan tutup periode yang diinginkan
    Step		: 
			1. Validasi 
			2. Proses Batal
           
    Example		: CALL spBatalTutupPeriode('123','201903');
    
*/
DROP PROCEDURE IF EXISTS spBatalTutupPeriode;

DELIMITER //

CREATE PROCEDURE spBatalTutupPeriode(
	IN varGuid VARCHAR(1000),
    IN varPeriode VARCHAR(1000)
)
BEGIN
	DECLARE varTipeProses TEXT;
    DECLARE varPeriodeSebelum TEXT;
    DECLARE varStatusYa TEXT;
    
    SET varTipeProses = 'Tutup Periode'; 
    SET varPeriodeSebelum = toPeriodeSebelum(varPeriode);
    SET varStatusYa = 'Ya';
    
	-- 1. Validasi 
    CALL spBatalTutupPeriodeValidasi(varGuid,
								varPeriode);

    -- 2. Proses Batal 
    -- Delete saldo akun bulanan
    DELETE FROM saldoakunbulanan WHERE periode = varPeriode;
    
    -- Delete jurnal penutup
    DELETE FROM jurnal WHERE toPeriode(tanggal) = varPeriode AND jurnalpenutup = varStatusYa;

    -- Delete saldo hutang bulanan
    DELETE FROM saldohutangbulanan WHERE periode = varPeriode;
    
    -- Delete saldo persediaan bulanan
    DELETE FROM saldopersediaanbulanan WHERE periode = varPeriode;
    
    -- Delete saldo persediaan hpp bulanan
    DELETE FROM saldopersediaanhppbulanan WHERE periode = varPeriode;
    
    -- Delete saldo piutang bulanan
    DELETE FROM saldopiutangbulanan WHERE periode = varPeriode;
    
    -- Hapus temp saldo bulanan error
    DELETE FROM tempsaldobulananerror WHERE periode = varPeriode;
    
    -- Hapus Admin
    DELETE FROM admin WHERE periode = varPeriode AND proses = varTipeProses;
    
END //
 DELIMITER ;
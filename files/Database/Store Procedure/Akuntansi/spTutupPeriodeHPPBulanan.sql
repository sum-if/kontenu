/*
	Author		: Santos Sabanari
    Date		: 03-10-2016
    Description	: Untuk menutup hpp periode yang diinginkan
    Step		: 
			   - Ambil saldo bulan sebelumnya
               - Cari mutasi bulan tersebut
               - Hitung akhir saldo bulan tersebut
           
	Example		: CALL spTutupPeriodeHPPBulanan('123','201903','Maret','2019','%d/%m/%Y','SANTOS');

*/
DROP PROCEDURE IF EXISTS spTutupPeriodeHPPBulanan;

DELIMITER //

CREATE PROCEDURE spTutupPeriodeHPPBulanan(
	IN varGuid VARCHAR(1000),
    IN varPeriode VARCHAR(1000),
    IN varNamaBulan VARCHAR(1000),
    IN varTahun VARCHAR(1000),
    IN varFormatDate VARCHAR(1000),
    IN varUserLogin VARCHAR(1000)
)
BEGIN
    -- Masukkan di tabel asli hpp bulanan
    INSERT INTO saldopersediaanhppbulanan(periode,barang,nilai)
    SELECT varPeriode, A.barang, (SELECT B.hppakhir FROM mutasihpp B WHERE toPeriode(B.tanggal) = varPeriode AND B.barang = A.barang ORDER BY B.tanggal DESC LIMIT 1)
	FROM mutasihpp A
	WHERE toPeriode(A.tanggal) = varPeriode
    GROUP BY A.barang;
	
END //
 DELIMITER ;
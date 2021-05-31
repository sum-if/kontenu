/*
	Author		: Santos Sabanari
    Date		: 03-10-2016
    Description	: Untuk menutup persediaan periode yang diinginkan
    Step		: 
               - Ambil saldo bulan sebelumnya
               - Cari mutasi bulan tersebut
               - Hitung akhir saldo bulan tersebut
			   - Validasi tidak boleh minus
               
   Example		: CALL spTutupPeriodePersediaanBulanan('123','201903','Maret','2019','%d/%m/%Y','SANTOS');

*/
DROP PROCEDURE IF EXISTS spTutupPeriodePersediaanBulanan;

DELIMITER //

CREATE PROCEDURE spTutupPeriodePersediaanBulanan(
	IN varGuid VARCHAR(1000),
    IN varPeriode VARCHAR(1000),
    IN varNamaBulan VARCHAR(1000),
    IN varTahun VARCHAR(1000),
    IN varFormatDate VARCHAR(1000),
    IN varUserLogin VARCHAR(1000)
)
BEGIN
	DECLARE varPeriodeSebelum TEXT;
    SET varPeriodeSebelum = toPeriodeSebelum(varPeriode);
        
    DELETE FROM tempsaldopersediaanbulanan WHERE guid = varGuid;
    
    -- Ambil saldo bulan sebelumnya
    INSERT INTO tempsaldopersediaanbulanan(guid,periode,barang,awal,mutasitambah,mutasikurang,akhir)
    SELECT varGuid,varPeriode,barang,akhir,0,0,0
    FROM saldopersediaanbulanan A 
    WHERE A.periode = varPeriodeSebelum;
    
    -- Cari mutasi bulan tersebut
    INSERT INTO tempsaldopersediaanbulanan(guid,periode,barang,awal,mutasitambah,mutasikurang,akhir)
    SELECT varGuid,varPeriode, A.barang, 0, SUM(A.jumlah), 0, 0
    FROM mutasipersediaan A
    WHERE toPeriode(A.tanggal) = varPeriode AND A.jumlah > 0
    GROUP BY A.barang;
    
    INSERT INTO tempsaldopersediaanbulanan(guid,periode,barang,awal,mutasitambah,mutasikurang,akhir)
    SELECT varGuid,varPeriode, A.barang, 0, 0, -SUM(A.jumlah), 0
    FROM mutasipersediaan A
    WHERE toPeriode(A.tanggal) = varPeriode AND A.jumlah < 0
    GROUP BY A.barang;
    
    -- Hitung akhir saldo bulan tersebut
    UPDATE tempsaldopersediaanbulanan
    SET akhir = awal + mutasitambah - mutasikurang
    WHERE periode = varPeriode AND guid = varGuid;
    
     -- Masukkan di tabel asli persediaan bulanan
    INSERT INTO saldopersediaanbulanan(periode,barang,awal,mutasitambah,mutasikurang,akhir)
    SELECT periode,barang,ROUND(SUM(awal),2),ROUND(SUM(mutasitambah),2),ROUND(SUM(mutasikurang),2),ROUND(SUM(akhir),2)
    FROM tempsaldopersediaanbulanan
    WHERE periode = varPeriode AND guid = varGuid
    GROUP BY periode,barang;
    
    DELETE FROM tempsaldopersediaanbulanan WHERE guid = varGuid;
END //
 DELIMITER ;
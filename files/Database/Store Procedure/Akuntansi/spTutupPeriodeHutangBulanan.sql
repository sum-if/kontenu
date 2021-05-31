/*
	Author		: Santos Sabanari
    Date		: 03-10-2016
    Description	: Untuk menutup hutang periode yang diinginkan
    Step		: 
			   - Ambil saldo bulan sebelumnya
               - Cari mutasi bulan tersebut
               - Hitung akhir saldo bulan tersebut
           
	Example		: CALL spTutupPeriodeHutangBulanan('123','201903','Maret','2019','%d/%m/%Y','SANTOS');

*/
DROP PROCEDURE IF EXISTS spTutupPeriodeHutangBulanan;

DELIMITER //

CREATE PROCEDURE spTutupPeriodeHutangBulanan(
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
	
    DELETE FROM tempsaldohutangbulanan WHERE guid = varGuid;
    
	-- Ambil saldo bulan sebelumnya
    INSERT INTO tempsaldohutangbulanan(guid,periode,supplier,awal,mutasitambah,mutasikurang,akhir)
    SELECT varGuid,varPeriode,supplier,akhir,0,0,0
    FROM saldohutangbulanan A 
    WHERE A.periode = varPeriodeSebelum;
    
    -- Cari mutasi bulan tersebut
    -- Mutasi Tambah
    INSERT INTO tempsaldohutangbulanan(guid,periode,supplier,awal,mutasitambah,mutasikurang,akhir)
    SELECT varGuid,varPeriode,A.supplier,0,SUM(A.jumlah),0,0
    FROM mutasihutang A
    WHERE toPeriode(A.tanggal) = varPeriode AND A.jumlah > 0
    GROUP BY A.supplier;
    
    -- Mutasi Kurang
	INSERT INTO tempsaldohutangbulanan(guid,periode,supplier,awal,mutasitambah,mutasikurang,akhir)
    SELECT varGuid,varPeriode,A.supplier,0,0,-SUM(A.jumlah),0
    FROM mutasihutang A
    WHERE toPeriode(A.tanggal) = varPeriode AND A.jumlah < 0
    GROUP BY A.supplier;
    
	-- Hitung akhir saldo bulan tersebut
    UPDATE tempsaldohutangbulanan
    SET akhir = awal + mutasitambah - mutasikurang
    WHERE periode = varPeriode AND guid = varGuid;
    
    -- Masukkan di tabel asli hutang bulanan
    INSERT INTO saldohutangbulanan(periode,supplier,awal,mutasitambah,mutasikurang,akhir)
    SELECT periode,supplier,ROUND(SUM(awal),2),ROUND(SUM(mutasitambah),2),ROUND(SUM(mutasikurang),2),ROUND(SUM(akhir),2)
    FROM tempsaldohutangbulanan
    WHERE periode = varPeriode AND guid = varGuid
    GROUP BY periode,supplier;
        
    DELETE FROM tempsaldohutangbulanan WHERE guid = varGuid;
	
END //
 DELIMITER ;
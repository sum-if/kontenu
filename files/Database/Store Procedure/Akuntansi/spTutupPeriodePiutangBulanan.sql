/*
	Author		: Santos Sabanari
    Date		: 03-10-2016
    Description	: Untuk menutup piutang periode yang diinginkan
    Step		: 
               - Ambil saldo bulan sebelumnya
               - Cari mutasi bulan tersebut
               - Hitung akhir saldo bulan tersebut
               
	Example		: CALL spTutupPeriodePiutangBulanan('123','201903','Maret','2019','%d/%m/%Y','SANTOS');

*/
DROP PROCEDURE IF EXISTS spTutupPeriodePiutangBulanan;

DELIMITER //

CREATE PROCEDURE spTutupPeriodePiutangBulanan(
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
    
    DELETE FROM tempsaldopiutangbulanan WHERE guid = varGuid;
    
	-- Ambil saldo bulan sebelumnya
    INSERT INTO tempsaldopiutangbulanan(guid,periode,customer,awal,mutasitambah,mutasikurang,akhir)
    SELECT varGuid,varPeriode,customer,akhir,0,0,0
    FROM saldopiutangbulanan A 
    WHERE A.periode = varPeriodeSebelum;
    
    -- Cari mutasi bulan tersebut
    -- Mutasi Tambah
    INSERT INTO tempsaldopiutangbulanan(guid,periode,customer,awal,mutasitambah,mutasikurang,akhir)
    SELECT varGuid,varPeriode,A.customer,0,SUM(A.jumlah),0,0
    FROM mutasipiutang A
    WHERE toPeriode(A.tanggal) = varPeriode AND A.jumlah > 0
    GROUP BY A.customer;
    
    -- Mutasi Kurang
    INSERT INTO tempsaldopiutangbulanan(guid,periode,customer,awal,mutasitambah,mutasikurang,akhir)
    SELECT varGuid,varPeriode,A.customer,0,0,-SUM(A.jumlah),0
    FROM mutasipiutang A
    WHERE toPeriode(A.tanggal) = varPeriode AND A.jumlah < 0
    GROUP BY A.customer;
    
    -- Hitung akhir saldo bulan tersebut
    UPDATE tempsaldopiutangbulanan
    SET akhir = awal + mutasitambah - mutasikurang
    WHERE periode = varPeriode AND guid = varGuid;
    
    -- Masukkan di tabel asli piutang bulanan
    INSERT INTO saldopiutangbulanan(periode,customer,awal,mutasitambah,mutasikurang,akhir)
    SELECT periode,customer,ROUND(SUM(awal),2),ROUND(SUM(mutasitambah),2),ROUND(SUM(mutasikurang),2),ROUND(SUM(akhir),2)
    FROM tempsaldopiutangbulanan
    WHERE periode = varPeriode AND guid = varGuid
    GROUP BY periode,customer;
        
    DELETE FROM tempsaldopiutangbulanan WHERE guid = varGuid;
    
END //
 DELIMITER ;
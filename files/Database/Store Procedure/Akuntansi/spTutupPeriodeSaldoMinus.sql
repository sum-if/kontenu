/*
	Author		: Santos Sabanari
    Date		: 12-12-2018
    Description	: Untuk validasi saldo minus bulanan
    Step		: 
           
	Example		: CALL spTutupPeriodeSaldoMinus('123','201903','Maret','2019','%d/%m/%Y','SANTOS');

*/
DROP PROCEDURE IF EXISTS spTutupPeriodeSaldoMinus;

DELIMITER //

CREATE PROCEDURE spTutupPeriodeSaldoMinus(
	IN varGuid VARCHAR(1000),
    IN varPeriode VARCHAR(1000),
    IN varNamaBulan VARCHAR(1000),
    IN varTahun VARCHAR(1000),
    IN varFormatDate VARCHAR(1000),
    IN varUserLogin VARCHAR(1000)
)
BEGIN
	DELETE FROM tempsaldobulananerror WHERE periode = varPeriode;
    
    -- hutang
	INSERT INTO tempsaldobulananerror(periode,jenis,kode,nama,awal,mutasitambah,mutasikurang,akhir)
    SELECT A.periode, 'Hutang Supplier', A.supplier, B.nama,A.awal,A.mutasitambah,A.mutasikurang,A.akhir
    FROM saldohutangbulanan A
    LEFT JOIN supplier B ON A.supplier = B.kode
    WHERE A.periode = varPeriode AND A.akhir < 0;
    
    -- piutang
    INSERT INTO tempsaldobulananerror(periode,jenis,kode,nama,awal,mutasitambah,mutasikurang,akhir)
    SELECT A.periode, 'Piutang Customer', A.customer, B.nama,A.awal,A.mutasitambah,A.mutasikurang,A.akhir
    FROM saldopiutangbulanan A
    LEFT JOIN customer B ON A.customer = B.kode
    WHERE A.periode = varPeriode AND A.akhir < 0;
    
    -- persediaan
    INSERT INTO tempsaldobulananerror(periode,jenis,kode,nama,awal,mutasitambah,mutasikurang,akhir)
    SELECT A.periode, 'Persediaan', A.barang, B.nama,A.awal,A.mutasitambah,A.mutasikurang,A.akhir
    FROM saldopersediaanbulanan A    
    LEFT JOIN barang B ON A.barang = B.kode
    WHERE A.periode = varPeriode AND A.akhir < 0;
END //
 DELIMITER ;
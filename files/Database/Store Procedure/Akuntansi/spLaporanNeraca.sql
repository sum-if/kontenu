/*
    Example		: CALL spLaporanNeraca('123','ASET','201907');
                  CALL spLaporanNeraca('123','NONASET','201907');
*/
DROP PROCEDURE IF EXISTS spLaporanNeraca;

DELIMITER //

CREATE PROCEDURE spLaporanNeraca(
	IN varGuid VARCHAR(100),
    IN varJenisAkun VARCHAR(100),
    IN varPeriode VARCHAR(100)
)
BEGIN
    
    IF varJenisAkun = 'ASET' THEN
        SELECT A.akunkategori AS 'kodekategori', B.nama AS 'namakategori', A.akunsubkategori AS 'kodesubkategori', C.nama AS 'namasubkategori', A.akungroup AS 'kodetampil', D.nama AS 'namatampil', A.saldo
        FROM (
            SELECT A.akunkategori, A.akunsubkategori, A.akungroup, SUM(COALESCE(B.akhirdebit - B.akhirkredit, 0)) AS saldo
            FROM akun A
            LEFT JOIN saldoakunbulanan B ON A.kode = B.akun AND B.periode = varPeriode
            WHERE A.akunkategori = '1000.00'
            GROUP BY A.akunkategori, A.akunsubkategori, A.akungroup
        ) A
        INNER JOIN akunkategori B ON A.akunkategori = B.kode
        INNER JOIN akunsubkategori C ON A.akunsubkategori = C.kode
        INNER JOIN akungroup D ON A.akungroup = D.kode
        ORDER BY A.akunkategori, A.akunsubkategori, A.akungroup;
    ELSE
        SELECT A.akunkategori AS 'kodekategori', B.nama AS 'namakategori', A.akunsubkategori AS 'kodesubkategori', C.nama AS 'namasubkategori', A.akungroup AS 'kodetampil', COALESCE(D.nama, E.nama) AS 'namatampil', A.saldo
        FROM (
            SELECT A.akunkategori, A.akunsubkategori, A.akungroup, SUM(CASE A.saldonormalneraca WHEN 'Debit' THEN COALESCE(B.akhirdebit - B.akhirkredit, 0) ELSE COALESCE(B.akhirkredit - B.akhirdebit, 0) END) AS saldo
            FROM akun A
            LEFT JOIN saldoakunbulanan B ON A.kode = B.akun AND B.periode = varPeriode
            WHERE A.akunkategori IN ('2000.00', '3000.00') AND A.akunsubkategori <> '3200.00'
            GROUP BY A.akunkategori, A.akunsubkategori, A.akungroup
            UNION ALL
            SELECT A.akunkategori, A.akunsubkategori, A.akunsubgroup AS 'akungroup', SUM(CASE A.saldonormalneraca WHEN 'Debit' THEN COALESCE(B.akhirdebit - B.akhirkredit, 0) ELSE COALESCE(B.akhirkredit - B.akhirdebit, 0) END) AS saldo
            FROM akun A
            LEFT JOIN saldoakunbulanan B ON A.kode = B.akun AND B.periode = varPeriode
            WHERE A.akunkategori IN ('2000.00', '3000.00') AND A.akunsubkategori = '3200.00'
            GROUP BY A.akunkategori, A.akunsubkategori, A.akunsubgroup
        ) A
        INNER JOIN akunkategori B ON A.akunkategori = B.kode
        INNER JOIN akunsubkategori C ON A.akunsubkategori = C.kode
        LEFT JOIN akungroup D ON A.akungroup = D.kode
        LEFT JOIN akunsubgroup E ON A.akungroup = E.kode
        ORDER BY A.akunkategori, A.akunsubkategori, A.akungroup;
    END IF;

END //
DELIMITER ;
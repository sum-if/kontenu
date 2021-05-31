/*            
   Example		: CALL spLaporanAgingPiutang('123','31/10/2019');
*/
DROP PROCEDURE IF EXISTS spLaporanAgingPiutang;

DELIMITER //

CREATE PROCEDURE spLaporanAgingPiutang(
	IN varGuid VARCHAR(100),
    IN varTanggal VARCHAR(100)
)
BEGIN
    SELECT A.customer AS Customer, SUM(A.`Blm Jth Tempo`) AS 'Blm Jth Tempo', SUM(A.`1-30`) AS `1-30`, SUM(A.`31-60`) AS `31-60`, SUM(A.`61-90`) AS `61-90`, SUM(A.`91-120`) AS `91-120`, SUM(A.`>120`) AS `>120`
    FROM (
		SELECT X.customer AS 'Kode Customer', X.namacustomer AS 'Customer', X.kode AS 'No Faktur', X.tanggal AS 'Tanggal', X.jatuhtempo AS 'Jatuh Tempo', X.sisabayar AS 'Total', 
				CASE WHEN DATEDIFF(toDate(varTanggal), toDate(X.jatuhtempo)) <= 0 THEN X.sisabayar ELSE 0 END AS 'Blm Jth Tempo', 
				CASE WHEN DATEDIFF(toDate(varTanggal), toDate(X.jatuhtempo)) BETWEEN 1 AND 30 THEN X.sisabayar ELSE 0 END AS '1-30', 
				CASE WHEN DATEDIFF(toDate(varTanggal), toDate(X.jatuhtempo)) BETWEEN 31 AND 60 THEN X.sisabayar ELSE 0 END AS '31-60', 
				CASE WHEN DATEDIFF(toDate(varTanggal), toDate(X.jatuhtempo)) BETWEEN 61 AND 90 THEN X.sisabayar ELSE 0 END AS '61-90', 
				CASE WHEN DATEDIFF(toDate(varTanggal), toDate(X.jatuhtempo)) BETWEEN 91 AND 120 THEN X.sisabayar ELSE 0 END AS '91-120', 
				CASE WHEN DATEDIFF(toDate(varTanggal), toDate(X.jatuhtempo)) > 120 THEN X.sisabayar ELSE 0 END AS '>120'
		FROM (
			SELECT A.kode, A.tanggal AS 'tanggal', A.tanggaljatuhtempo AS jatuhtempo, A.customer, C.nama AS namacustomer, A.grandtotal - COALESCE(D.retur, 0) AS total, A.grandtotal - COALESCE(B.bayar, 0) - COALESCE(D.retur, 0) AS sisabayar
			FROM fakturpenjualan A
			INNER JOIN customer C ON A.customer = C.kode
			LEFT JOIN (
				SELECT Y.fakturpenjualan, SUM(Y.jumlah + Y.selisih) AS bayar
				FROM penerimaanpenjualan X
				INNER JOIN penerimaanpenjualandetail Y ON X.kode = Y.penerimaanpenjualan
				WHERE toDate(X.tanggal) <= toDate(varTanggal)
				GROUP BY Y.fakturpenjualan
			) B ON A.kode = B.fakturpenjualan
			LEFT JOIN (
				SELECT fakturpenjualan, SUM(grandtotal) AS retur
				FROM returpenjualan
				WHERE toDate(tanggal) <= toDate(varTanggal)
				GROUP BY fakturpenjualan
			) D ON A.kode = D.fakturpenjualan
			WHERE toDate(A.tanggal) <= toDate(varTanggal) AND (A.grandtotal - COALESCE(B.bayar, 0) - COALESCE(D.retur, 0)) > 0
		) X
    ) A
    GROUP BY A.customer
    ORDER BY A.customer;
END //
 DELIMITER ;
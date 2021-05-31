/*            
   Example		: CALL spLaporanAgingHutang('123','31/10/2019');
*/
DROP PROCEDURE IF EXISTS spLaporanAgingHutang;

DELIMITER //

CREATE PROCEDURE spLaporanAgingHutang(
	IN varGuid VARCHAR(100),
    IN varTanggal VARCHAR(100)
)
BEGIN
    SELECT A.supplier AS Supplier, SUM(A.`Blm Jth Tempo`) AS 'Blm Jth Tempo', SUM(A.`1-30`) AS `1-30`, SUM(A.`31-60`) AS `31-60`, SUM(A.`61-90`) AS `61-90`, SUM(A.`91-120`) AS `91-120`, SUM(A.`>120`) AS `>120`
    FROM (
		SELECT X.supplier AS 'Kode Supplier', X.namasupplier AS 'Supplier', X.kode AS 'No Faktur', X.tanggal AS 'Tanggal', X.jatuhtempo AS 'Jatuh Tempo', X.sisabayar AS 'Total', 
				CASE WHEN DATEDIFF(toDate(varTanggal), toDate(X.jatuhtempo)) <= 0 THEN X.sisabayar ELSE 0 END AS 'Blm Jth Tempo', 
				CASE WHEN DATEDIFF(toDate(varTanggal), toDate(X.jatuhtempo)) BETWEEN 1 AND 30 THEN X.sisabayar ELSE 0 END AS '1-30', 
				CASE WHEN DATEDIFF(toDate(varTanggal), toDate(X.jatuhtempo)) BETWEEN 31 AND 60 THEN X.sisabayar ELSE 0 END AS '31-60', 
				CASE WHEN DATEDIFF(toDate(varTanggal), toDate(X.jatuhtempo)) BETWEEN 61 AND 90 THEN X.sisabayar ELSE 0 END AS '61-90', 
				CASE WHEN DATEDIFF(toDate(varTanggal), toDate(X.jatuhtempo)) BETWEEN 91 AND 120 THEN X.sisabayar ELSE 0 END AS '91-120', 
				CASE WHEN DATEDIFF(toDate(varTanggal), toDate(X.jatuhtempo)) > 120 THEN X.sisabayar ELSE 0 END AS '>120'
		FROM (
			SELECT A.kode, A.tanggal AS 'tanggal', A.jatuhtempo, A.supplier, C.nama AS namasupplier, A.grandtotal - COALESCE(D.retur, 0) AS total, A.grandtotal - COALESCE(B.bayar, 0) - COALESCE(D.retur, 0) AS sisabayar
			FROM fakturpembelian A
			INNER JOIN supplier C ON A.supplier = C.kode
			LEFT JOIN (
				SELECT Y.fakturpembelian, SUM(Y.jumlah + Y.selisih) AS bayar
				FROM pembayaranpembelian X
				INNER JOIN pembayaranpembeliandetail Y ON X.kode = Y.pembayaranpembelian
				WHERE toDate(X.tanggal) <= toDate(varTanggal)
				GROUP BY Y.fakturpembelian
			) B ON A.kode = B.fakturpembelian
			LEFT JOIN (
				SELECT fakturpembelian, SUM(grandtotal) AS retur
				FROM returpembelian
				WHERE toDate(tanggal) <= toDate(varTanggal)
				GROUP BY fakturpembelian
			) D ON A.kode = D.fakturpembelian
			WHERE toDate(A.tanggal) <= toDate(varTanggal) AND (A.grandtotal - COALESCE(B.bayar, 0) - COALESCE(D.retur, 0)) > 0
		) X
    ) A
    GROUP BY A.supplier
    ORDER BY A.supplier;
END //
 DELIMITER ;
/*            
   Example		: CALL spLaporanPenjualan('123','10/01/2019','01/07/2019','%');
*/
DROP PROCEDURE IF EXISTS spLaporanPenjualan;

DELIMITER //

CREATE PROCEDURE spLaporanPenjualan(
	IN varGuid VARCHAR(100),
    IN varTanggalAwal VARCHAR(100),
    IN varTanggalAkhir VARCHAR(100),
    IN varCustomer VARCHAR(100)
)
BEGIN
    
    SELECT X.kode AS 'Kode Faktur', X.tanggal AS 'Tanggal', X.namacustomer AS 'Customer', X.kotacustomer AS 'Kota', X.dpp AS 'DPP', X.ppn AS 'PPN', X.grandtotal AS 'Grand Total'
    FROM (
        (SELECT A.kode, A.tanggal, A.customer, C.nama AS namacustomer, D.nama AS kotacustomer, A.totaldpp AS dpp, A.totalppn AS ppn, A.grandtotal
        FROM fakturpenjualan A
        INNER JOIN customer C ON A.customer = C.kode
		INNER JOIN kota D ON C.kota = D.kode
        WHERE toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir) AND A.customer LIKE varCustomer)
    UNION ALL
		(SELECT A.kode, A.tanggal, A.customer, C.nama AS namacustomer, D.nama AS kotacustomer, -A.totaldpp AS dpp, -A.totalppn AS ppn, -A.grandtotal
        FROM returpenjualan A
        INNER JOIN customer C ON A.customer = C.kode
		INNER JOIN kota D ON C.kota = D.kode
        WHERE toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir) AND A.customer LIKE varCustomer)
    ) X 
    ORDER BY toDate(X.tanggal), X.kode;

END //
 DELIMITER ;
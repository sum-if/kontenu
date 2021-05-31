/*            
   Example		: CALL spLaporanPembelian('123','10/01/2018','01/07/2018','%');
*/
DROP PROCEDURE IF EXISTS spLaporanPembelian;

DELIMITER //

CREATE PROCEDURE spLaporanPembelian(
	IN varGuid VARCHAR(100),
    IN varTanggalAwal VARCHAR(100),
    IN varTanggalAkhir VARCHAR(100),
    IN varSupplier VARCHAR(100)
)
BEGIN
    
    SELECT X.kode AS 'Kode Faktur', X.faktursupplier AS 'Faktur Supplier', X.tanggal AS 'Tanggal', X.supplier AS 'Kode Supplier', X.namasupplier AS 'Supplier', X.dpp AS 'DPP', X.ppn AS 'PPN', X.grandtotal AS 'TOTAL'
    FROM (
        (SELECT A.kode, A.faktursupplier, A.tanggal, A.supplier, C.nama AS namasupplier, A.totaldpp AS dpp, ROUND(A.totalppn) AS ppn, A.grandtotal
        FROM fakturpembelian A
        INNER JOIN supplier C ON A.supplier = C.kode
        WHERE toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir) AND A.supplier LIKE varSupplier)
    UNION ALL
        (SELECT A.kode, A.fakturpembelian AS faktursupplier, A.tanggal , A.supplier, C.nama AS namasupplier, -ROUND(A.totaldpp) AS dpp, -ROUND(A.totalppn) AS ppn, -A.grandtotal
        FROM returpembelian A
        INNER JOIN supplier C ON A.supplier = C.kode
        WHERE toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir) AND A.supplier LIKE varSupplier)
    ) X 
    ORDER BY toDate(X.tanggal), X.kode;

END //
 DELIMITER ;
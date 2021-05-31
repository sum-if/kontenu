/*
	Author		: Santos Sabanari
    Date		: 04-05-2016
    Description	: Untuk mendapatkan nilai hpp barang berdasarkan satuan
    
           
    Example		: SELECT getTanggalTerakhirBarangCustomer('SUP1','BRG1');
*/
DROP FUNCTION IF EXISTS getTanggalTerakhirBarangCustomer;

DELIMITER //
 
CREATE FUNCTION getTanggalTerakhirBarangCustomer(varCustomer VARCHAR(45),varBarang VARCHAR(45)) RETURNS VARCHAR(45)
    DETERMINISTIC
BEGIN
    DECLARE varTanggal VARCHAR(45);
    
    SET varTanggal = (SELECT A.tanggal
					FROM fakturpenjualan A
					INNER JOIN fakturpenjualandetail B ON A.kode = B.fakturpenjualan
					WHERE A.customer = varCustomer AND B.barang = varBarang
					ORDER BY toDate(A.tanggal) DESC
					LIMIT 1);
    
    IF varTanggal IS NULL THEN
		RETURN '';
    END IF;
    
    RETURN varTanggal;
END //
 DELIMITER ;
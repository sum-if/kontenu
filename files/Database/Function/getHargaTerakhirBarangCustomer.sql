/*
	Author		: Santos Sabanari
    Date		: 04-05-2016
    Description	: Untuk mendapatkan nilai hpp barang berdasarkan satuan
    
           
    Example		: SELECT getHargaTerakhirBarangCustomer('SUP1','BRG1');
*/
DROP FUNCTION IF EXISTS getHargaTerakhirBarangCustomer;

DELIMITER //
 
CREATE FUNCTION getHargaTerakhirBarangCustomer(varCustomer VARCHAR(45),varBarang VARCHAR(45)) RETURNS DOUBLE
    DETERMINISTIC
BEGIN
    DECLARE varHarga DOUBLE;
    
    SET varHarga = (SELECT dpp
					FROM fakturpenjualan A
					INNER JOIN fakturpenjualandetail B ON A.kode = B.fakturpenjualan
					WHERE A.customer = varCustomer AND B.barang = varBarang
					ORDER BY toDate(A.tanggal) DESC
					LIMIT 1);
    
    IF varHarga IS NULL THEN
		RETURN 0;
    END IF;
    
    RETURN varHarga;
END //
 DELIMITER ;
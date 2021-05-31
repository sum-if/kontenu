/*
	Author		: Santos Sabanari
    Date		: 04-05-2016
    Description	: Untuk mendapatkan nilai hpp barang berdasarkan satuan
    
           
    Example		: SELECT getHargaTerakhirBarangSupplier('SUP1','BRG1');
*/
DROP FUNCTION IF EXISTS getHargaTerakhirBarangSupplier;

DELIMITER //
 
CREATE FUNCTION getHargaTerakhirBarangSupplier(varSupplier VARCHAR(45),varBarang VARCHAR(45)) RETURNS DOUBLE
    DETERMINISTIC
BEGIN
    DECLARE varHarga DOUBLE;
    
    SET varHarga = (SELECT dpp
					FROM fakturpembelian A
					INNER JOIN fakturpembeliandetail B ON A.kode = B.fakturpembelian
					WHERE A.supplier = varSupplier AND B.barang = varBarang
					ORDER BY toDate(A.tanggal) DESC
					LIMIT 1);
    
    IF varHarga IS NULL THEN
		RETURN 0;
    END IF;
    
    RETURN varHarga;
END //
 DELIMITER ;
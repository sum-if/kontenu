/*
	Author		: Santos Sabanari
    Date		: 01-11-2016
    Description	: Untuk menambah tanggal
    
           
    Example		: SELECT toTanggalTambah('15/10/2016',3);
*/
DROP FUNCTION IF EXISTS toTanggalTambah;

DELIMITER //
 
CREATE FUNCTION toTanggalTambah(tanggal VARCHAR(10), jumlah INT) RETURNS VARCHAR(10)
    DETERMINISTIC
BEGIN
    RETURN DATE_FORMAT(DATE_ADD(toDate(tanggal), INTERVAL jumlah DAY),'%d/%m/%Y');
END //
 DELIMITER ;
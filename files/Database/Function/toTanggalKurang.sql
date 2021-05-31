/*
	Author		: Santos Sabanari
    Date		: 01-11-2016
    Description	: Untuk mengurangi tanggal
    
           
    Example		: SELECT toTanggalKurang('15/10/2016',1);
*/
DROP FUNCTION IF EXISTS toTanggalKurang;

DELIMITER //
 
CREATE FUNCTION toTanggalKurang(tanggal VARCHAR(10), jumlah INT) RETURNS VARCHAR(10)
    DETERMINISTIC
BEGIN
    RETURN DATE_FORMAT(DATE_SUB(toDate(tanggal), INTERVAL jumlah DAY),'%d/%m/%Y');
END //
 DELIMITER ;
/*
	Author		: Santos Sabanari
    Date		: 23-03-2016
    Description	: Untuk mencari tanggal awal dari periode
    
           
    Example		: SELECT toTanggalAwalPeriode('201601');
*/
DROP FUNCTION IF EXISTS toTanggalAwalPeriode;

DELIMITER //
 
CREATE FUNCTION toTanggalAwalPeriode(periode VARCHAR(6)) RETURNS VARCHAR(10)
    DETERMINISTIC
BEGIN
    RETURN DATE_FORMAT(STR_TO_DATE(CONCAT(periode,'01'), '%Y%m%d'),'%d/%m/%Y');
END //
 DELIMITER ;
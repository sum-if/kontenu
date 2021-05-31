/*
	Author		: Santos Sabanari
    Date		: 05-04-2016
    Description	: Untuk mencari tanggal akhir dari periode
    
           
    Example		: SELECT toTanggalAkhirPeriode('201601');
*/
DROP FUNCTION IF EXISTS toTanggalAkhirPeriode;

DELIMITER //
 
CREATE FUNCTION toTanggalAkhirPeriode(periode VARCHAR(6)) RETURNS VARCHAR(10)
    DETERMINISTIC
BEGIN
    RETURN DATE_FORMAT(LAST_DAY(STR_TO_DATE(CONCAT(periode,'01'), '%Y%m%d')),'%d/%m/%Y');
END //
 DELIMITER ;
/*
	Author		: Santos Sabanari
    Date		: 05-04-2016
    Description	: Untuk mencari tanggal akhir dari periode
    
           
    Example		: SELECT toTanggalAkhirPeriodeDateTime('201601');
*/
DROP FUNCTION IF EXISTS toTanggalAkhirPeriodeDateTime;

DELIMITER //
 
CREATE FUNCTION toTanggalAkhirPeriodeDateTime(periode VARCHAR(6)) RETURNS TIMESTAMP
    DETERMINISTIC
BEGIN
    RETURN TIMESTAMP(DATE_SUB(DATE_ADD(LAST_DAY(STR_TO_DATE(CONCAT(periode,'01'), '%Y%m%d')), INTERVAL 1 DAY),INTERVAL 1 SECOND));
END //
 DELIMITER ;
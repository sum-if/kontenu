/*
	Author		: Santos Sabanari
    Date		: 23-03-2016
    Description	: Untuk mencari periode sebelumnya dari periode parameter
    
           
    Example		: SELECT toPeriodeSebelum('201601');
*/
DROP FUNCTION IF EXISTS toPeriodeSebelum;

DELIMITER //
 
CREATE FUNCTION toPeriodeSebelum(periode VARCHAR(6)) RETURNS VARCHAR(6)
    DETERMINISTIC
BEGIN
    RETURN DATE_FORMAT(DATE_SUB(STR_TO_DATE(CONCAT(periode,'01'), '%Y%m%d'), INTERVAL 1 MONTH),'%Y%m');
END //
 DELIMITER ;
/*
	Author		: Santos Sabanari
    Date		: 23-03-2016
    Description	: Untuk mencari periode berikutnya dari periode parameter
    
           
    Example		: SELECT toPeriodeBerikut('201601');
*/
DROP FUNCTION IF EXISTS toPeriodeBerikut;

DELIMITER //
 
CREATE FUNCTION toPeriodeBerikut(periode VARCHAR(6)) RETURNS VARCHAR(6)
    DETERMINISTIC
BEGIN
    RETURN DATE_FORMAT(DATE_ADD(LAST_DAY(STR_TO_DATE(CONCAT(periode,'01'), '%Y%m%d')), INTERVAL 1 DAY),'%Y%m');
END //
 DELIMITER ;
/*
	Author		: Santos Sabanari
    Date		: 14-03-2016
    Description	: Untuk menjadikan periode dari tanggal
    
           
    Example		: SELECT toPeriode('14/03/2016');
*/
DROP FUNCTION IF EXISTS toPeriode;

DELIMITER //
 
CREATE FUNCTION toPeriode(tanggal VARCHAR(10)) RETURNS VARCHAR(6)
    DETERMINISTIC
BEGIN
    RETURN (CONCAT(SUBSTR(tanggal,7,4),SUBSTR(tanggal,4,2)));
END //
 DELIMITER ;
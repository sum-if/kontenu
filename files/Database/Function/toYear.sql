/*
	Author		: Santos Sabanari
    Date		: 17-02-2018
    Description	: Untuk mengubah string jadi date
    
           
    Example		: SELECT toYear('01/01/2018');
*/
DROP FUNCTION IF EXISTS toYear;

DELIMITER //
 
CREATE FUNCTION toYear(tanggal VARCHAR(10)) RETURNS VARCHAR(4)
    DETERMINISTIC
BEGIN
    RETURN YEAR(STR_TO_DATE(tanggal,'%d/%m/%Y'));
END //
 DELIMITER ;
/*
	Author		: Santos Sabanari
    Date		: 04-05-2016
    Description	: Untuk mengubah string jadi date
    
           
    Example		: SELECT toDate('01/01/2016');
*/
DROP FUNCTION IF EXISTS toDate;

DELIMITER //
 
CREATE FUNCTION toDate(tanggal VARCHAR(10)) RETURNS DATE
    DETERMINISTIC
BEGIN
    RETURN STR_TO_DATE(tanggal,'%d/%m/%Y');
END //
 DELIMITER ;
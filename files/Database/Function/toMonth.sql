/*
	Author		: Santos Sabanari
    Date		: 17-02-2018
    Description	: Untuk mengubah string jadi date
    
           
    Example		: SELECT toMonth('01/01/2018');
*/
DROP FUNCTION IF EXISTS toMonth;

DELIMITER //
 
CREATE FUNCTION toMonth(tanggal VARCHAR(10)) RETURNS VARCHAR(2)
    DETERMINISTIC
BEGIN
    RETURN LPAD(MONTH(STR_TO_DATE(tanggal,'%d/%m/%Y')),2,'0');
END //
 DELIMITER ;
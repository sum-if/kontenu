/*
	Author		: Santos Sabanari
    Date		: 01-11-2016
    Description	: Untuk mengubah tanggal menjadi Tahun Bulan Tanggal
    
           
    Example		: SELECT toTahunBulanTanggal('15/10/2016');
*/
DROP FUNCTION IF EXISTS toTahunBulanTanggal;

DELIMITER //
 
CREATE FUNCTION toTahunBulanTanggal(tanggal VARCHAR(10)) RETURNS VARCHAR(10)
    DETERMINISTIC
BEGIN
    RETURN DATE_FORMAT(toDate(tanggal),'%Y%m%d');
END //
 DELIMITER ;
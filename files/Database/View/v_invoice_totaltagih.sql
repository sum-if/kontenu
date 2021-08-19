/*
	Author		: Santos Sabanari
    Date		: 19-08-2021
           
    Example		: SELECT * FROM v_invoice_totaltagih;
*/
DROP VIEW IF EXISTS v_invoice_totaltagih;
 
CREATE VIEW v_invoice_totaltagih AS
SELECT A.kode AS invoice, COALESCE(SUM(B.grandtotal),0) AS totaltagih
FROM invoice A
LEFT JOIN penagihan B ON A.kode = B.invoice
GROUP BY A.kode;
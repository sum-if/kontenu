/*
	Author		: Santos Sabanari
    Date		: 19-08-2021
           
    Example		: SELECT * FROM v_invoice_totalterima;
*/
DROP VIEW IF EXISTS v_invoice_totalterima;
 
CREATE VIEW v_invoice_totalterima AS
SELECT A.kode AS invoice, COALESCE(SUM(C.diterima),0) AS totalterima
FROM invoice A
LEFT JOIN penagihan B ON A.kode = B.invoice
LEFT JOIN penerimaan C ON B.kode = C.penagihan
GROUP BY A.kode;
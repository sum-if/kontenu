/*
	Author		: Santos Sabanari
    Date		: 19-08-2021
           
    Example		: SELECT * FROM v_purchase_totalbayar;
*/
DROP VIEW IF EXISTS v_purchase_totalbayar;
 
CREATE VIEW v_purchase_totalbayar AS
SELECT A.kode AS purchase, COALESCE(SUM(B.nominalbayar),0) AS totalbayar
FROM purchase A
LEFT JOIN purchasepaymentdetail B ON A.kode = B.purchase
GROUP BY A.kode;
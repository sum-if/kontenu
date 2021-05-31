/*
	Author		: Santos Sabanari
    Date		: 06-03-2017
    Description	: Untuk laporan persediaan kartu stok
    Step		: 
               - Ambil saldo bulan sebelumnya
               - Cari mutasi dari awal bulan sampai tanggal awal
               - Cari mutasi dari tanggal awal sampai tanggal akhir
               
   Example		: CALL spLaporanKartuStok('123','01/12/2018','31/12/2020','0179-0001');
				  CALL spLaporanKartuStok('123','01/12/2018','31/12/2020','%');
         
DROP TABLE IF EXISTS `templaporankartustok`;
CREATE TABLE IF NOT EXISTS `templaporankartustok` (
  `guid` VARCHAR(300),
  `barang` VARCHAR(45),
  `tanggal` VARCHAR(45),
  `jenis` VARCHAR(100),
  `keterangan` VARCHAR(300),
  `referensi` VARCHAR(100),
  `jumlah` DOUBLE,
  `create_at` TIMESTAMP)
ENGINE = InnoDB;
*/
DROP PROCEDURE IF EXISTS spLaporanKartuStok;

DELIMITER //

CREATE PROCEDURE spLaporanKartuStok(
	IN varGuid VARCHAR(100),
    IN varTanggalAwal VARCHAR(100),
    IN varTanggalAkhir VARCHAR(100),
    IN varBarang VARCHAR(100)
)
BEGIN
    
    DELETE FROM templaporankartustok WHERE guid = varGuid;
        
    -- Ambil saldo sebelumnya
    INSERT INTO templaporankartustok(guid,tanggal,barang,jenis,keterangan,referensi,jumlah,create_at)
    SELECT varGuid, toTanggalKurang(varTanggalAwal,1), A.barang, '','', 'Saldo Awal', COALESCE(SUM(A.jumlah),0),CURRENT_TIMESTAMP()
    FROM mutasipersediaan A
    WHERE A.barang LIKE varBarang AND toDate(A.tanggal) < toDate(varTanggalAwal)
    GROUP BY A.barang;
    
    --  Cari mutasi dari tanggal awal sampai tanggal akhir
    INSERT INTO templaporankartustok(guid,tanggal,barang,jenis,keterangan,referensi,jumlah,create_at)
    SELECT varGuid, A.tanggal, A.barang, A.oswjenisdokumen,'', A.noreferensi, A.jumlah, A.create_at
    FROM mutasipersediaan A
    WHERE A.barang LIKE varBarang AND toDate(A.tanggal) BETWEEN toDate(varTanggalAwal) AND toDate(varTanggalAkhir)
    ORDER BY toDate(A.tanggal), A.create_at, A.jumlah DESC;
    
    -- FAKTURPENJUALAN
    UPDATE templaporankartustok A
    INNER JOIN fakturpenjualan B ON A.referensi = B.kode
    INNER JOIN customer C ON B.customer = C.kode
    INNER JOIN kota D ON C.kota = D.kode
    SET A.keterangan = CONCAT(C.nama,' [',D.nama,']')
    WHERE A.guid = varGuid AND A.jenis = 'FAKTURPENJUALAN';
    
	-- FAKTURPEMBELIAN
    UPDATE templaporankartustok A
    INNER JOIN fakturpembelian B ON A.referensi = B.kode
    INNER JOIN supplier C ON B.supplier = C.kode
    INNER JOIN kota D ON C.kota = D.kode
    SET A.keterangan = CONCAT(C.nama,' [',D.nama,']')
    WHERE A.guid = varGuid AND A.jenis = 'FAKTURPEMBELIAN';
    
	-- RETURPEMBELIAN
    UPDATE templaporankartustok A
    INNER JOIN returpembelian B ON A.referensi = B.kode
    INNER JOIN supplier C ON B.supplier = C.kode
    INNER JOIN kota D ON C.kota = D.kode
    SET A.keterangan = CONCAT(C.nama,' [',D.nama,']')
    WHERE A.guid = varGuid AND A.jenis = 'RETURPEMBELIAN';
    
    -- RETURPENJUALAN
    UPDATE templaporankartustok A
    INNER JOIN returpenjualan B ON A.referensi = B.kode
    INNER JOIN customer C ON B.customer = C.kode
    INNER JOIN kota D ON C.kota = D.kode
    SET A.keterangan = CONCAT(C.nama,' [',D.nama,']')
    WHERE A.guid = varGuid AND A.jenis = 'RETURPENJUALAN';
    
	-- HASIL
	SELECT A.barang AS Barang, CONCAT(C.kode,' - ', C.nama,' , No Part : ', IF(C.nopart='','-',C.nopart),' , Rak : ', D.nama) AS Nama, A.tanggal AS Tanggal, COALESCE(B.nama,'') AS Jenis, A.keterangan AS Keterangan, A.referensi AS Referensi, A.jumlah AS Jumlah
    FROM templaporankartustok A
    INNER JOIN barang C ON A.barang = C.kode
    INNER JOIN barangrak D ON C.rak = D.kode
	LEFT JOIN oswjenisdokumen B ON A.jenis = B.kode
    WHERE A.guid = varGuid
    ORDER BY A.barang, toDate(A.tanggal), A.create_at, A.jumlah DESC;
    
	DELETE FROM templaporankartustok WHERE guid = varGuid;
END //
 DELIMITER ;
----------- Vorbereitung der Übung -------------------------------------------------------------------------------
-- Setze isolation mode auf read committed (ohne Snapshot) 
USE master
ALTER DATABASE FlughafenDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE 
ALTER DATABASE FlughafenDB SET READ_COMMITTED_SNAPSHOT OFF
ALTER DATABASE FlughafenDB SET MULTI_USER
SELECT CASE is_read_committed_snapshot_on WHEN 1 THEN 'Snapshot ON' ELSE 'Snapshot OFF' END
   FROM sys.databases WHERE name= 'FlughafenDB'
USE FlughafenDB


/* Deadlock simulieren:
1. Statement #1 markieren und ausführen in Deadlock1.sql
2. Deadlock2.sql öffnen und Statement #2 ausführen
3. Statement #3 ausführen in Deadlock2.sql
4. Statement #4 ausführen in Deadlock1.sql
5. Deadlock!
6. Rollback in Deadlock2.sql ausführen
*/

-- Statement #1
BEGIN TRANSACTION;
UPDATE Flugzeug SET Kapazitaet = 100 WHERE Flugzeug_ID = 2

-- Statement #4
UPDATE Flug SET Flugzeug_ID = 4 WHERE Flug_ID = 2 -- Deadlock

/* 
Rollback kann nicht mehr durchgeführt werden, da Transaktion aufgrund
deadlock abgebrochen wird!
*/
ROLLBACK;






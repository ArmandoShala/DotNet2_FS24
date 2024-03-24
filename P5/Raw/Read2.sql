-- Statement #2
USE FlughafenDB
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
BEGIN TRANSACTION
SELECT count(*) FROM test;    

-- Statement #4 (Warten Sie nicht auf das Ende des Befehls - gehen Sie zu Statement #5)
INSERT INTO test (Flugzeug_ID, Kapazitaet, Typ_ID, Fluglinie_ID )
   SELECT TOP 500 * FROM Flugzeug WHERE flugzeug_id NOT IN  (SELECT flugzeug_id FROM Test)
SELECT count(*) FROM test;

-- Statement #6
COMMIT TRANSACTION;
SELECT count(*) FROM test; 
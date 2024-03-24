/*

Parallele Ausführung simulieren:

1. Statement #1 ausführen in Concurrent1.sql
2. Statement #2 ausführen in Concurrent2.sql
3. Statement #3 ausführen in Concurrent1.sql
4. Statement #4 ausführen in Concurrent2.sql
4. Statement #5 ausführen in Concurrent1.sql

*/

-- Statement #1
USE FlughafenDB
BEGIN TRANSACTION
UPDATE Flugzeug SET Kapazitaet = 160 WHERE Flugzeug_ID = 1
SELECT TOP (1) * FROM Flugzeug WHERE Flugzeug_ID = 1

-- Statement #3
COMMIT TRANSACTION

-- Statement #5 - undo changes
BEGIN TRANSACTION
SELECT TOP (1) * FROM Flugzeug WHERE Flugzeug_ID = 1
UPDATE Flugzeug SET Kapazitaet = 150 WHERE Flugzeug_ID = 1
COMMIT TRANSACTION
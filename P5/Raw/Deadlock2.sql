-- Statement #2
USE FlughafenDB
BEGIN TRANSACTION;
UPDATE Flug SET Flugzeug_ID = 4 WHERE Flug_ID = 2 

-- Statement #3
UPDATE Flugzeug SET Kapazitaet = 100 WHERE Flugzeug_ID = 2 -- Blockierung
/* Zu Deadlock1.sql zurück wechseln */

/* Rollback durchführen */
ROLLBACK;
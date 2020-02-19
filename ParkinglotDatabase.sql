USE[ParkingLotDatabase]

CREATE TABLE Vehicle(
ID INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
Vehicle_Type varchar(4) NOT NULL CHECK(LEN(Vehicle_Type) > 2),
Registration DATETIME NOT NULL,
)

CREATE TABLE Parking_Slot(
ID INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
Slot_Number INT NOT NULL,
Occupied_By INT NOT NULL,
)

CREATE TABLE Parking_Slot_Reservation(
ID INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
Parking_Slot_ID INT FOREIGN KEY REFERENCES Parking_Slot(ID) NOT NULL,
Vehicle_ID  INT FOREIGN KEY REFERENCES Vehicle(ID) NOT NULL,
Regnumber varchar(10) UNIQUE NOT NULL CHECK(LEN(Regnumber) > 2),
Booking_DATE_TIME DATETIME NOT NULL,
Cost money NOT NULL,
)

CREATE TABLE Parking_Slip_History_Table(
ID INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
Regnumber varchar(10) NOT NULL,
Vehicle_Type varchar(4) NOT NULL,
Entry_DateTime DATETIME NOT NULL,
Time_Parked time NOT NULL,
Exit_DateTime DATETIME NOT NULL,
Cost money NOT NULL,
)

--create 100 free slots
DECLARE @hid INT;
SET @hid=1;
WHILE @hid < 101
BEGIN 
    INSERT INTO Parking_Slot(Slot_Number, Occupied_By) 
    VALUES(@hid,0); 
    SET @hid = @hid + 1;
END



--ADD 10 vehicles to parking_slot_Reservation
INSERT INTO Vehicle(Registration,Vehicle_Type)
VALUES(GETDATE(),'Car');
UPDATE Parking_Slot SET Occupied_By = 2 WHERE Slot_Number = 1
INSERT INTO Parking_Slot_Reservation(Regnumber,Vehicle_ID,Parking_Slot_ID,Booking_DATE_TIME,Cost)
VALUES('Fordon1',(SELECT Vehicle.ID FROM Vehicle WHERE Vehicle.ID = (SELECT MAX(ID)FROM Vehicle)),
(SELECT Parking_Slot.ID FROM Parking_Slot WHERE Slot_Number = 1),GETDATE(),0);

INSERT INTO Vehicle(Registration,Vehicle_Type)
VALUES(GETDATE(),'Car');
UPDATE Parking_Slot SET Occupied_By = 2 WHERE Slot_Number = 5
INSERT INTO Parking_Slot_Reservation(Regnumber,Vehicle_ID,Parking_Slot_ID,Booking_DATE_TIME,Cost)
VALUES('Fordon2',(SELECT Vehicle.ID FROM Vehicle WHERE Vehicle.ID = (SELECT MAX(ID)FROM Vehicle)),
(SELECT Parking_Slot.ID FROM Parking_Slot WHERE Slot_Number = 5),GETDATE(),0);

INSERT INTO Vehicle(Registration,Vehicle_Type)
VALUES(GETDATE(),'Bike');
UPDATE Parking_Slot SET Occupied_By = 1 WHERE Slot_Number = 15
INSERT INTO Parking_Slot_Reservation(Regnumber,Vehicle_ID,Parking_Slot_ID,Booking_DATE_TIME,Cost)
VALUES('Fordon3',(SELECT Vehicle.ID FROM Vehicle WHERE Vehicle.ID = (SELECT MAX(ID)FROM Vehicle)),
(SELECT Parking_Slot.ID FROM Parking_Slot WHERE Slot_Number = 15),GETDATE(),0);

INSERT INTO Vehicle(Registration,Vehicle_Type)
VALUES(GETDATE(),'Bike');
UPDATE Parking_Slot SET Occupied_By = 2 WHERE Slot_Number = 15
INSERT INTO Parking_Slot_Reservation(Regnumber,Vehicle_ID,Parking_Slot_ID,Booking_DATE_TIME,Cost)
VALUES('Fordon4',(SELECT Vehicle.ID FROM Vehicle WHERE Vehicle.ID = (SELECT MAX(ID)FROM Vehicle)),
(SELECT Parking_Slot.ID FROM Parking_Slot WHERE Slot_Number = 15),GETDATE(),0);

INSERT INTO Vehicle(Registration,Vehicle_Type)
VALUES(GETDATE(),'Bike');
UPDATE Parking_Slot SET Occupied_By = 1 WHERE Slot_Number = 32
INSERT INTO Parking_Slot_Reservation(Regnumber,Vehicle_ID,Parking_Slot_ID,Booking_DATE_TIME,Cost)
VALUES('Fordon5',(SELECT Vehicle.ID FROM Vehicle WHERE Vehicle.ID = (SELECT MAX(ID)FROM Vehicle)),
(SELECT Parking_Slot.ID FROM Parking_Slot WHERE Slot_Number = 32),GETDATE(),0);


INSERT INTO Vehicle(Registration,Vehicle_Type)
VALUES(GETDATE(),'Bike');
UPDATE Parking_Slot SET Occupied_By = 1 WHERE Slot_Number = 54
INSERT INTO Parking_Slot_Reservation(Regnumber,Vehicle_ID,Parking_Slot_ID,Booking_DATE_TIME,Cost)
VALUES('Fordon6',(SELECT Vehicle.ID FROM Vehicle WHERE Vehicle.ID = (SELECT MAX(ID)FROM Vehicle)),
(SELECT Parking_Slot.ID FROM Parking_Slot WHERE Slot_Number = 54),GETDATE(),0);


INSERT INTO Vehicle(Registration,Vehicle_Type)
VALUES(GETDATE(),'Car');
UPDATE Parking_Slot SET Occupied_By = 2 WHERE Slot_Number = 99
INSERT INTO Parking_Slot_Reservation(Regnumber,Vehicle_ID,Parking_Slot_ID,Booking_DATE_TIME,Cost)
VALUES('Fordon7',(SELECT Vehicle.ID FROM Vehicle WHERE Vehicle.ID = (SELECT MAX(ID)FROM Vehicle)),
(SELECT Parking_Slot.ID FROM Parking_Slot WHERE Slot_Number = 99),GETDATE(),0);

INSERT INTO Vehicle(Registration,Vehicle_Type)
VALUES(GETDATE(),'Car');
UPDATE Parking_Slot SET Occupied_By = 2 WHERE Slot_Number = 82
INSERT INTO Parking_Slot_Reservation(Regnumber,Vehicle_ID,Parking_Slot_ID,Booking_DATE_TIME,Cost)
VALUES('Fordon8',(SELECT Vehicle.ID FROM Vehicle WHERE Vehicle.ID = (SELECT MAX(ID)FROM Vehicle)),
(SELECT Parking_Slot.ID FROM Parking_Slot WHERE Slot_Number = 82),GETDATE(),0);

INSERT INTO Vehicle(Registration,Vehicle_Type)
VALUES(GETDATE(),'Bike')
UPDATE Parking_Slot SET Occupied_By = 1 WHERE Slot_Number = 38
INSERT INTO Parking_Slot_Reservation(Regnumber,Vehicle_ID,Parking_Slot_ID,Booking_DATE_TIME,Cost)
VALUES('Fordon9',(SELECT Vehicle.ID FROM Vehicle WHERE Vehicle.ID = (SELECT MAX(ID)FROM Vehicle)),
(SELECT Parking_Slot.ID FROM Parking_Slot WHERE Slot_Number = 38),GETDATE(),0);

INSERT INTO Vehicle(Registration,Vehicle_Type)
VALUES(GETDATE(),'Car');
UPDATE Parking_Slot SET Occupied_By = 2 WHERE Slot_Number = 10
INSERT INTO Parking_Slot_Reservation(Regnumber,Vehicle_ID,Parking_Slot_ID,Booking_DATE_TIME,Cost)
VALUES('Fordon10',(SELECT Vehicle.ID FROM Vehicle WHERE Vehicle.ID = (SELECT MAX(ID)FROM Vehicle)),
(SELECT Parking_Slot.ID FROM Parking_Slot WHERE Slot_Number = 10),GETDATE(),0);


---ADD 10 vehicles to history
DECLARE @timeparked time,@arrived DATETIME,@timeleft DATETIME
SET @arrived = '2020-01-18 12:12:12'
SET @timeleft = GETDATE();
SET @timeparked = @timeleft - @arrived;

INSERT INTO Parking_Slip_History_Table(Regnumber,Vehicle_Type,Entry_DateTime,Time_Parked,Exit_DateTime,Cost)
VALUES('Fordon1','Bike',@arrived,@timeparked,@timeleft,1000)
INSERT INTO Parking_Slip_History_Table(Regnumber,Vehicle_Type,Entry_DateTime,Time_Parked,Exit_DateTime,Cost)
VALUES('Fordon2','Car',@arrived,@timeparked,@timeleft,1000)
INSERT INTO Parking_Slip_History_Table(Regnumber,Vehicle_Type,Entry_DateTime,Time_Parked,Exit_DateTime,Cost)
VALUES('Fordon3','Car',@arrived,@timeparked,@timeleft,1000)
INSERT INTO Parking_Slip_History_Table(Regnumber,Vehicle_Type,Entry_DateTime,Time_Parked,Exit_DateTime,Cost)
VALUES('Fordon4','Bike',@arrived,@timeparked,@timeleft,1000)
INSERT INTO Parking_Slip_History_Table(Regnumber,Vehicle_Type,Entry_DateTime,Time_Parked,Exit_DateTime,Cost)
VALUES('Fordon5','Car',@arrived,@timeparked,@timeleft,1000)
INSERT INTO Parking_Slip_History_Table(Regnumber,Vehicle_Type,Entry_DateTime,Time_Parked,Exit_DateTime,Cost)
VALUES('Fordon6','Bike',@arrived,@timeparked,@timeleft,1000)
INSERT INTO Parking_Slip_History_Table(Regnumber,Vehicle_Type,Entry_DateTime,Time_Parked,Exit_DateTime,Cost)
VALUES('Fordon7','Car',@arrived,@timeparked,@timeleft,1000)
INSERT INTO Parking_Slip_History_Table(Regnumber,Vehicle_Type,Entry_DateTime,Time_Parked,Exit_DateTime,Cost)
VALUES('Fordon8','Car',@arrived,@timeparked,@timeleft,1000)
INSERT INTO Parking_Slip_History_Table(Regnumber,Vehicle_Type,Entry_DateTime,Time_Parked,Exit_DateTime,Cost)
VALUES('Fordon9','Bike',@arrived,@timeparked,@timeleft,1000)
INSERT INTO Parking_Slip_History_Table(Regnumber,Vehicle_Type,Entry_DateTime,Time_Parked,Exit_DateTime,Cost)
VALUES('Fordon10','Car',@arrived,@timeparked,@timeleft,1000)

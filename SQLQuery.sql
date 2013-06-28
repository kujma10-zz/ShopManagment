use shop;

CREATE TABLE Admins(
   Username NVARCHAR(40) NOT NULL PRIMARY KEY,
   Password NVARCHAR(12) NOT NULL,
   AdminType INT NOT NULL,
   check(AdminType > 0 and AdminType < 4)
)

insert into Admins values('kote', '123', 1);
insert into Admins values('kikola', '123', 2);
insert into Admins values('jilberta', '123', 3);
insert into Admins values('shota', '123', 2);

CREATE TABLE Categories(
   ID INT IDENTITY NOT NULL PRIMARY KEY, 
   Name NVARCHAR(150) NOT NULL,
   Descr NVARCHAR(300)
)

CREATE TABLE Products(
   ID INT IDENTITY NOT NULL PRIMARY KEY,
   CatID INT NOT NULL, 
   Name NVARCHAR(150) NOT NULL,
   Descr NVARCHAR(300),
   Price float
   FOREIGN KEY (CatID) REFERENCES Categories(ID)
)

CREATE TABLE Storages(
   ID INT IDENTITY NOT NULL PRIMARY KEY,
   Name NVARCHAR(150) NOT NULL,
   Descr NVARCHAR(300),
   Addr NVARCHAR(150) NOT NULL,
   Opened Date NOT NULL,
   Closed Date
)

insert into Categories values('ჰიგიენური საშუალებები', 'საპონი, შამპუნი და ა.შ');
insert into Categories values('საყოფაცხოვრებო ნივთები', 'დასდასდას');
insert into Categories values('საკვები', 'ადასდასდასდასდასდ');

insert into Products values(1, 'Soap Protex', 'magaree saponee', 0.5);
insert into Products values(1, 'Head&Shoulders', 'magaree shampunee', 2.5);
insert into Products values(2, 'Samsung TV', 'dasd', 388);
insert into Products values(2, 'X-Box', 'mds', 500);
insert into Products values(2, 'Sony Playstation', 'dasdsa', 200);
insert into Products values(3, 'Shaurma', 'xorcis gareshe', 5);


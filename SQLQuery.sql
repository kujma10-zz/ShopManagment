use shop;

CREATE TABLE Admins(
   ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
   Username NVARCHAR(40) NOT NULL,
   Password NVARCHAR(12) NOT NULL,
   AdminType INT NOT NULL,
   check(AdminType > 0 and AdminType < 4)
)

CREATE TABLE Categories(
   ID INT IDENTITY NOT NULL PRIMARY KEY, 
   Name NVARCHAR(150) NOT NULL,
   Descr NVARCHAR(300),
   Disabled BIT NOT NULL DEFAULT 0
)

CREATE TABLE Products(
   ID INT IDENTITY NOT NULL PRIMARY KEY,
   CatID INT NOT NULL, 
   Name NVARCHAR(150) NOT NULL,
   Descr NVARCHAR(300),
   Price float NOT NULL,
   Disabled BIT NOT NULL DEFAULT 0
   FOREIGN KEY (CatID) REFERENCES Categories(ID)
)

CREATE TABLE Storages(
   ID INT IDENTITY NOT NULL PRIMARY KEY,
   Name NVARCHAR(150) NOT NULL,
   Descr NVARCHAR(300),
   Addr NVARCHAR(150),
   Opened Datetime NOT NULL DEFAULT GETDATE(),
   Closed Datetime
)

CREATE TABLE Balances(
   ID INT IDENTITY NOT NULL PRIMARY KEY,
   StorageID INT NOT NULL,
   CatID INT NOT NULL,
   ProductID INT NOT NULL,
   Quantity INT NOT NULL
   FOREIGN KEY (StorageID) REFERENCES Storages(ID),
   FOREIGN KEY (CatID) REFERENCES Categories(ID),
   FOREIGN KEY (ProductID) REFERENCES Products(ID)
)

CREATE TABLE Sales(
   ID INT IDENTITY NOT NULL PRIMARY KEY,
   AdminID INT NOT NULL,
   StorageID INT NOT NULL,
   CatID INT NOT NULL,
   ProductID INT NOT NULL,
   Date Datetime NOT NULL DEFAULT GETDATE(),
   Price float NOT NULL,
   Quantity INT NOT NULL
   Returned BIT DEFAULT 0,
   FOREIGN KEY (AdminID) REFERENCES Admins(ID),
   FOREIGN KEY (StorageID) REFERENCES Storages(ID),
   FOREIGN KEY (CatID) REFERENCES Categories(ID),
   FOREIGN KEY (ProductID) REFERENCES Products(ID)
)

insert into Admins values('kote', '123', 1);
insert into Admins values('kikola', '123', 2);
insert into Admins values('jilberta', '123', 3);
insert into Admins values('shota', '123', 2);

insert into Categories(Name, Descr) values('ჰიგიენური საშუალებები', 'საპონი, შამპუნი და ა.შ');
insert into Categories(Name, Descr) values('საყოფაცხოვრებო ნივთები', 'დასდასდას');
insert into Categories(Name, Descr) values('საკვები', 'ადასდასდასდასდასდ');

insert into Products(CatID, Name, Descr, Price) values(1, 'Soap Protex', 'magaree saponee', 0.5);
insert into Products(CatID, Name, Descr, Price) values(1, 'Head&Shoulders', 'magaree shampunee', 2.5);
insert into Products(CatID, Name, Descr, Price) values(2, 'Samsung TV', 'dasd', 388);
insert into Products(CatID, Name, Descr, Price) values(2, 'X-Box', 'mds', 500);
insert into Products(CatID, Name, Descr, Price) values(2, 'Sony Playstation', 'dasdsa', 200);
insert into Products(CatID, Name, Descr, Price) values(3, 'Shaurma', 'xorcis gareshe', 5);

insert into Storages(Name, Descr, Addr) values('sawyobi1', 'pirveli sawyobi', 'misamarti1');
insert into Storages(Name, Descr, Addr) values('sawyobi2', 'meore sawyobi', 'misamarti2');
insert into Storages(Name, Descr, Addr) values('sawyobi3', 'mesame sawyobi', 'misamarti3');
insert into Storages(Name, Descr, Addr) values('sawyobi4', 'meotxe sawyobi', 'misamarti4');

insert into Balances values(1, 1, 1, 40);
insert into Balances values(1, 1, 2, 100);
insert into Balances values(1, 2, 3, 40);
insert into Balances values(1, 2, 4, 46);
insert into Balances values(2, 2, 5, 20);
insert into Balances values(2, 1, 1, 150);
insert into Balances values(3, 1, 1, 200);
insert into Balances values(3, 1, 2, 150);
insert into Balances values(3, 3, 6, 1150);

select * from Categories;
select * from Products;
select * from Balances;
select * from Storages;

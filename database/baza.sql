DROP Database IF EXISTS sklepexPOL;

Create Database IF NOT EXISTS sklepexPOL;
Use sklepexPOL;

SET SESSION sql_mode = 'NO_AUTO_VALUE_ON_ZERO';

DROP TABLE IF EXISTS poziomy;

CREATE TABLE poziomy (
  ID_poziom int NOT NULL,
  Nazwa text NOT NULL,
  pracownicy_min int unsigned NOT NULL,
  pracownicy_opt int unsigned NOT NULL,
  produkty_min int unsigned NOT NULL,
  kategorie_min int unsigned NOT NULL,
  pojemnosc_magazynu int unsigned NOT NULL,
  oplaty_miesieczne double NOT NULL,
  wynagrodzenie double NOT NULL,
  klienci_max int unsigned NOT NULL,
  marza_max double NOT NULL,
  PRIMARY KEY (ID_poziom)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


LOCK TABLES poziomy WRITE;
INSERT INTO poziomy VALUES (0,'Pustostan',0,0,0,0,50000,825,23,100,0.8),(1,'Sklepik',0,0,1,1,50000,825,46,100,0.8),(2,'Sklep',1,10,5,2,75000,3300,69,500,0.7),(3,'Dyskont',10,18,10,5,100000,19250,92,1500,0.6),(4,'Supermarket',18,30,15,10,500000,55000,115,10000,0.5),(5,'Hipermarket',30,50,20,15,2000000,330000,138,100000,0.4);
UNLOCK TABLES;




DROP TABLE IF EXISTS rodzaje;

CREATE TABLE rodzaje (
  ID_rodzaj int NOT NULL AUTO_INCREMENT,
  Nazwa text NOT NULL,
  PRIMARY KEY (ID_rodzaj)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES rodzaje WRITE;
INSERT INTO rodzaje VALUES (1,'Spożywczy'),(2,'Warzywniak'),(3,'RTV AGD'),(4,'Mięsny'),(5,'Drogeria'),(6,'Butik'),(7,'Papierniczy'),(8,'Piekarnia'),(9,'Ogrodniczy'),(10,'Market'),(11,'Monopolowy');

UNLOCK TABLES;



DROP TABLE IF EXISTS info;

CREATE TABLE info (
  Nazwa text NOT NULL,
  Data_zalozenia date NOT NULL,
  Saldo double NOT NULL,
  Dzisiaj date DEFAULT NULL,
  Marza double NOT NULL,
  Dochod_dzienny double DEFAULT NULL,
  Dochod_calkowity double DEFAULT NULL,
  Wydatki_dzienne double DEFAULT NULL,
  Wydatki_calkowite double DEFAULT NULL,
  Liczba_zamowien bigint unsigned DEFAULT NULL,
  Koszt_zamowien double DEFAULT NULL,
  Liczba_pracownikow int unsigned DEFAULT NULL,
  Ruch int unsigned DEFAULT NULL,
  Rodzaj int  REFERENCES Rodzaje(ID_rodzaj),
  Poziom int  REFERENCES poziomy(ID_poziom),
  Najwyzszy_poziom int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


DROP TABLE IF EXISTS kategorie;
CREATE TABLE kategorie (
  ID_kat int NOT NULL AUTO_INCREMENT,
  Nazwa text NOT NULL,
  PRIMARY KEY (ID_kat)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES kategorie WRITE;

INSERT INTO kategorie VALUES (1,'Warzywa'),(2,'Owoce'),(3,'Mięso i ryby'),(4,'Nabiał'),(5,'Pieczywo'),(6,'Kosmetyki'),(7,'RTV AGD'),(8,'Artykuły biurowe'),(9,'Ubrania'),(10,'Artykuły gospodarstwa domowego'),(11,'Rośliny'),(12,'Słodycze'),(13,'Przekąski'),(14,'Napoje'),(15,'Alkohol');

UNLOCK TABLES;




DROP TABLE IF EXISTS podatki;

CREATE TABLE podatki (
  ID_pod int NOT NULL AUTO_INCREMENT,
  Nazwa text NOT NULL,
  Wysokosc double NOT NULL,
  PRIMARY KEY (ID_pod)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES podatki WRITE;

INSERT INTO podatki VALUES (1,'VAT Warzywa i owoce',0.05),(2,'VAT Owoce egzotyczne',0.05),(3,'VAT Przyprawy',0.08),(4,'VAT Artykuły spożywcze',0.23),(5,'VAT Produkty przemysłu młynarskiego',0.05),(6,'VAT Produkty pochodzenia zwierzęcego',0.05),(7,'VAT Mięso i podroby jadalne',0.05),(8,'VAT Ryby',0.05),(9,'VAT Artykuły dziecięce',0.05),(10,'VAT Artykuły higieniczne',0.05),(11,'VAT Wyroby medyczne',0.08),(12,'VAT Oprogramowanie i gry komputerowe',0.23),(13,'VAT Artykuły RTV AGD',0.23),(14,'VAT Artykuły biurowe i piśmiennicze',0.23),(15,'VAT Książki',0.05),(16,'VAT Gazety i czasopisma',0.08),(17,'VAT Odzież',0.23),(18,'VAT Rośliny żywe i cięte',0.08),(19,'Akcyza Alkohole mocne',0.75),(20,'Akcyza Piwo ',0.35),(21,'Akcyza Wina',0.26),(22,'Brak cła',0),(23,'Kraje Uni Celnej',0),(24,'Cło Azja',0.8),(25,'Cło Afryka',0.5),(26,'Cło Europa',0.23),(27,'Cło Ameryka',0.8),(28,'Cło Australia i Oceania',0.3);

UNLOCK TABLES;




DROP TABLE IF EXISTS kraje;

CREATE TABLE kraje (
  ID_kra int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  Nazwa text NOT NULL,
  Skrot varchar(3) NOT NULL,
  Clo int REFERENCES podatki(ID_pod)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


LOCK TABLES kraje WRITE;

INSERT INTO kraje VALUES (1,'Rzeczpospolita Polska','POL',22),(2,'Republika Włoska','ITA',23),(3,'Stany Zjednoczone Ameryki','USA',27),(4,'Chińska Republika Ludowa','CPR',24),(5,'Zjednoczone Królestwo Wielkiej Brytanii i Irlandii Północnej','GBR',26),(6,'Republika Francuska','FRA',23);

UNLOCK TABLES;


DROP TABLE IF EXISTS magazyny;

CREATE TABLE magazyny (
  ID_mag int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  Nazwa text NOT NULL,
  Marza double NOT NULL,
  Czas_dostawy int NOT NULL,
  Kraj int REFERENCES kraje(ID_kra)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


LOCK TABLES magazyny WRITE;
INSERT INTO magazyny VALUES (0,'Program Wsparcia Dla Nowych Przedsiębiorców',0,0,1),(1,'Bella Fantastico',0.35,3,2),(2,'Fromages Hadiuk',0.5,4,6),(3,'ITAmore',0.3,3,2),(4,'Spożywex SA',0.7,3,1),(5,'Marian-Hurt Sp. c.',0.6,5,1),(6,'MięsoPOL Sp. z o. o.',0.4,2,1),(7,'BiggerBetter',0.65,4,3),(8,'Yiaotobie',0.2,7,4),(9,'Deli of Queen',0.4,3,5),(10,'Ilaspeed',0.1,12,4);

UNLOCK TABLES;


DROP TABLE IF EXISTS zamowienia;

CREATE TABLE zamowienia (
  ID_zam int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  Data_zamowienia date NOT NULL,
  Data_dostarczenia date DEFAULT NULL,
  Magazyn int REFERENCES magazyny(ID_mag),
  Koszt double DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



DROP TABLE IF EXISTS produkty;
CREATE TABLE produkty (
  ID_prod int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  Nazwa text NOT NULL,
  Cena double NOT NULL,
  Termin_przydatnosci int NOT NULL,
  Kategoria int REFERENCES kategorie(ID_kat),
  Podatek int REFERENCES podatki(ID_pod)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


LOCK TABLES produkty WRITE;
INSERT INTO produkty VALUES (1,'Marchew (1 kg)',1.4,20,1,1),(2,'Ziemniak (1 kg)',1.1,20,1,1),(3,'Cebula (1 kg)',1.8,20,1,1),(4,'Por (szt)',2.2,15,1,1),(5,'Pietruszka (1 kg)',2.5,15,1,1),(6,'Natka pietruszki (pęczek)',2,4,1,1),(7,'Pomidor (1 kg)',2.99,10,1,1),(8,'Cytryna (1 kg)',5,20,2,2),(9,'Pomarańcza (1 kg)',4,20,2,2),(10,'Jabłko (1 kg)',2.5,20,2,1),(11,'Gruszka (1 kg)',3,20,2,1),(12,'Truskawka (500 g)',5,10,2,1),(13,'Malina (500 g)',4.5,10,2,1),(14,'Jeżyna (500 g)',5.1,10,2,1),(15,'Pierś z kurczaka (1 kg)',8.99,10,3,7),(16,'Dorsz (250 g)',6.75,10,3,8),(17,'Łosoś (250 g)',15,10,3,8),(18,'Okoń (250 g)',6.5,10,3,8),(19,'Stek wołowy (500 g)',15.5,10,3,7),(20,'Kindziuk (100 g)',4,40,3,7),(21,'Salami (100 g)',5,40,3,7),(22,'Grana Padano (100 g)',10,60,4,6),(23,'Masło (250 g)',3,50,4,6),(24,'Śmietana (500 g)',1.8,30,4,6),(25,'Gouda (100 g)',2.9,40,4,6),(26,'Mleko (1 L)',1.9,30,4,6),(27,'Brie de Meaux (200 g)',2,30,4,6),(28,'Parmigiano Reggiano (100 g)',11,60,4,6),(29,'Chleb biały (szt)',4.75,5,5,5),(30,'Chleb cebulowy (szt)',4.5,5,5,5),(31,'Chleb orkiszowy (szt)',5,5,5,5),(32,'Bułka pszenna (szt)',1,4,5,5),(33,'Bagietka (szt)',1.5,4,5,5),(34,'Chleb żytni (szt)',4.9,5,5,5),(35,'Kajzerka (szt)',0.5,4,5,5),(36,'Czekolada gorzka (szt)',2.5,50,12,4),(37,'Czekolada mleczna (szt)',2.5,50,12,4),(38,'Czekolada biała (szt)',2.5,50,12,4),(39,'Cukierki owocowe (szt)',1,60,12,4),(40,'Nutella (500 g)',7.5,50,12,4),(41,'Ferrero Rocher (250 g)',15,50,12,4),(42,'Guma kulka (10 szt)',0.3,70,12,4),(43,'Biały Bocian (0.5 L)',14,100,15,19),(44,'Soplica (0.5 L)',10,100,15,19),(45,'Pan Tadeusz (0.5 L)',12,100,15,19),(46,'Chianti (0.75 L)',15,100,15,21),(47,'Bordeux (0.75 L)',13,100,15,21),(48,'Prosecco (0.75 L)',14,100,15,21),(49,'Pinot noir (0.75 L)',14,100,15,21),(50,'Woda niegazowana (2 L)',1.4,60,14,4),(51,'Woda gazowana (2 L)',1.4,60,14,4),(52,'Sok pomarańczowy (1 L)',2.2,60,14,4),(53,'Sok jabłkowy (1 L)',2.2,60,14,4),(54,'Coca Cola (2 L)',3.5,60,14,4),(55,'Sprite (2 L)',3.5,60,14,4),(56,'Fanta (2 l)',3.5,60,14,4);

UNLOCK TABLES;



DROP TABLE IF EXISTS stan;

CREATE TABLE stan (
  ID_stan int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  Ilosc int NOT NULL,
  Produkt int REFERENCES produkty(ID_prod),
  Zamowienie int REFERENCES zamowienia(ID_zam)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


DROP TABLE IF EXISTS pro_mag;

CREATE TABLE pro_mag (
  Produkt int REFERENCES produkty(ID_prod),
  Magazyn int REFERENCES magazyny(ID_mag)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


LOCK TABLES pro_mag WRITE;

INSERT INTO pro_mag VALUES (21,1),(22,1),(28,1),(40,1),(41,1),(46,1),(48,1),(27,2),(33,2),(47,2),(49,2),(7,3),(9,3),(10,3),(21,3),(23,3),(26,3),(28,3),(36,3),(37,3),(38,3),(40,3),(41,3),(46,3),(48,3),(52,3),(53,3),(1,4),(2,4),(3,4),(4,4),(5,4),(6,4),(7,4),(10,4),(11,4),(12,4),(13,4),(14,4),(23,4),(24,4),(26,4),(25,5),(29,5),(30,5),(31,5),(32,5),(33,5),(34,5),(35,5),(36,5),(37,5),(38,5),(39,5),(42,5),(43,5),(44,5),(45,5),(50,5),(51,5),(52,5),(53,5),(54,5),(55,5),(56,5),(15,6),(16,6),(17,6),(18,6),(19,6),(20,6),(21,6),(23,6),(24,6),(25,6),(26,6),(19,7),(25,7),(36,7),(37,7),(38,7),(39,7),(40,7),(41,7),(42,7),(50,7),(51,7),(52,7),(53,7),(54,7),(55,7),(56,7),(1,9),(2,9),(3,9),(4,9),(5,9),(6,9),(7,9),(10,9),(11,9),(12,9),(13,9),(14,9),(15,9),(16,9),(17,9),(18,9),(23,9),(24,9),(25,9),(26,9),(29,9),(30,9),(31,9),(32,9),(33,9),(34,9),(35,9),(36,9),(37,9),(38,9),(39,9),(50,9),(51,9),(52,9),(53,9),(8,1),(8,5);

UNLOCK TABLES;


DROP TABLE IF EXISTS pro_zam;

CREATE TABLE pro_zam (
  Produkt int REFERENCES produkty(ID_prod),
  Zamowienie int REFERENCES zamowienia(ID_zam),
  Ilosc int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


DROP TABLE IF EXISTS appopen;
DROP VIEW IF EXISTS appopen;

DROP TABLE IF EXISTS dodostarczenia;
DROP VIEW IF EXISTS dodostarczenia;

DROP TABLE IF EXISTS dostawcy;
DROP VIEW IF EXISTS dostawcy;

DROP TABLE IF EXISTS nastanie;
DROP VIEW IF EXISTS nastanie;

DROP TABLE IF EXISTS osklepie;
DROP VIEW IF EXISTS osklepie;


Create view doDostarczenia AS
SELECT * FROM zamowienia 
Where Data_dostarczenia >= CURDATE()
Order By Data_dostarczenia asc, Data_zamowienia asc;


Create view naStanie AS
Select p.Nazwa, s.Ilosc, p.Cena, pod.Wysokosc, m.Marza, (z.Data_dostarczenia + INTERVAL p.Termin_przydatnosci DAY) as Termin_przydatnosci, z.Magazyn
FROM
stan s JOIN zamowienia z ON s.Zamowienie = z.ID_zam
JOIN
magazyny m ON z.Magazyn = m.ID_mag
JOIN
pro_zam ON pro_zam.Zamowienie = z.ID_zam
JOIN
produkty p ON p.ID_prod = pro_zam.Produkt
JOIN 
podatki pod ON p.Podatek = pod.ID_pod;


Create view dostawcy AS 
SELECT m.ID_mag ID, m.Nazwa Dostawca, m.Czas_dostawy, m.Marza, k.Nazwa Kraj, k.Skrot, p.Nazwa Podatek, p.Wysokosc
FROM magazyny m JOIN kraje k on m.Kraj = k.ID_kra 
JOIN podatki p on k.Clo=p.ID_pod
WHERE m.ID_mag>0
ORDER BY m.ID_mag ASC;


Create view oSklepie AS
Select i.Nazwa, i.Data_zalozenia, i.Marza, i.Liczba_pracownikow, i.Ruch, r.Nazwa Rodzaj, p.Nazwa Poziom, p.pojemnosc_magazynu, p.wynagrodzenie, p.oplaty_miesieczne
FROM info i JOIN rodzaje r ON i.Rodzaj = r.ID_rodzaj
JOIN poziomy p ON i.Poziom = p.ID_poziom;


Create view appOpen AS
Select i.Saldo, i.Dochod_dzienny, i.Wydatki_dzienne, i.Ruch, i.Liczba_pracownikow, i.Rodzaj, i.Poziom, i.Marza, p.oplaty_miesieczne, p.wynagrodzenie, p.pracownicy_min, p.pracownicy_opt, p.pojemnosc_magazynu, p.klienci_max
FROM info i JOIN poziomy p ON i.Poziom = p.ID_poziom;



DELIMITER ;;
CREATE PROCEDURE NextDay()
BEGIN
Update info SET Dzisiaj = Dzisiaj + INTERVAL 1 DAY, Dochod_calkowity = Dochod_calkowity + Dochod_dzienny, Wydatki_calkowite = Wydatki_calkowite + Wydatki_dzienne;
Update info SET Saldo = (Saldo + Dochod_dzienny - Wydatki_dzienne);
Update info SET Dochod_dzienny = 0, Wydatki_dzienne = 0;
END ;;
DELIMITER ;

DELIMITER ;;
CREATE PROCEDURE oferta(IN id int)
Begin
SELECT p.ID_prod, p.Nazwa, p.Cena, pod.Nazwa Podatek, pod.Wysokosc 
FROM pro_mag JOIN produkty p on pro_mag.Produkt = p.ID_prod JOIN podatki pod ON p.Podatek=pod.ID_pod 
WHERE pro_mag.Magazyn = id
ORDER BY p.Nazwa ASC;
End ;;
DELIMITER ;

DELIMITER ;;
CREATE PROCEDURE podglad(IN id int)
SELECT p.Nazwa, z.Ilosc 
FROM pro_zam z JOIN produkty p on z.Produkt = p.ID_prod 
WHERE z.Zamowienie = id
ORDER BY p.Nazwa ASC ;;
DELIMITER ;

DELIMITER ;;
CREATE PROCEDURE TheBlip()
BEGIN
DELETE FROM info;
DELETE FROM stan;
DELETE FROM zamowienia;
DELETE FROM pro_zam;
ALTER TABLE stan AUTO_INCREMENT=0;
ALTER TABLE zamowienia AUTO_INCREMENT=0;
END ;;

DELIMITER ;

CREATE TRIGGER LicznikZamowien 
AFTER INSERT ON zamowienia
FOR each row
UPDATE info SET Liczba_zamowien = Liczba_zamowien + 1;


Create trigger PaniWiesia AFTER UPDATE On stan
for each row
DELETE FROM stan WHERE Ilosc=0;



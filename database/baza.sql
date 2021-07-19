-- MySQL dump 10.13  Distrib 8.0.23, for Win64 (x86_64)
--
-- Host: localhost    Database: shopmanager
-- ------------------------------------------------------
-- Server version	8.0.23

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

Create Database IF NOT EXISTS sklepexpol;
Use sklepexpol;

--
-- Temporary view structure for view `appopen`
--

DROP TABLE IF EXISTS `appopen`;
/*!50001 DROP VIEW IF EXISTS `appopen`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `appopen` AS SELECT 
 1 AS `Saldo`,
 1 AS `Dochod_dzienny`,
 1 AS `Wydatki_dzienne`,
 1 AS `Ruch`,
 1 AS `Liczba_pracownikow`,
 1 AS `Rodzaj`,
 1 AS `Poziom`,
 1 AS `Marza`,
 1 AS `oplaty_miesieczne`,
 1 AS `wynagrodzenie`,
 1 AS `pracownicy_min`,
 1 AS `pracownicy_opt`,
 1 AS `pojemnosc_magazynu`,
 1 AS `klienci_max`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `dodostarczenia`
--

DROP TABLE IF EXISTS `dodostarczenia`;
/*!50001 DROP VIEW IF EXISTS `dodostarczenia`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `dodostarczenia` AS SELECT 
 1 AS `ID_zam`,
 1 AS `Data_zamowienia`,
 1 AS `Data_dostarczenia`,
 1 AS `Magazyn`,
 1 AS `Koszt`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `dostawcy`
--

DROP TABLE IF EXISTS `dostawcy`;
/*!50001 DROP VIEW IF EXISTS `dostawcy`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `dostawcy` AS SELECT 
 1 AS `Dostawca`,
 1 AS `Czas_dostawy`,
 1 AS `Marza`,
 1 AS `ID_mag`,
 1 AS `Nazwa`,
 1 AS `Skrot`,
 1 AS `Produkt`,
 1 AS `Wysokosc`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `info`
--

DROP TABLE IF EXISTS `info`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `info` (
  `Nazwa` text NOT NULL,
  `Data_zalozenia` date NOT NULL,
  `Saldo` double NOT NULL,
  `Dzisiaj` date DEFAULT NULL,
  `Marza` double NOT NULL,
  `Dochod_dzienny` double DEFAULT NULL,
  `Dochod_calkowity` double DEFAULT NULL,
  `Wydatki_dzienne` double DEFAULT NULL,
  `Wydatki_calkowite` double DEFAULT NULL,
  `Liczba_zamowien` bigint unsigned DEFAULT NULL,
  `Koszt_zamowien` double DEFAULT NULL,
  `Liczba_pracownikow` int unsigned DEFAULT NULL,
  `Ruch` int unsigned DEFAULT NULL,
  `Rodzaj` int DEFAULT NULL,
  `Poziom` int DEFAULT NULL,
  `Najwyzszy_poziom` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `info`
--

LOCK TABLES `info` WRITE;
/*!40000 ALTER TABLE `info` DISABLE KEYS */;
/*!40000 ALTER TABLE `info` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kategorie`
--

DROP TABLE IF EXISTS `kategorie`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kategorie` (
  `ID_kat` int NOT NULL AUTO_INCREMENT,
  `Nazwa` text NOT NULL,
  PRIMARY KEY (`ID_kat`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kategorie`
--

LOCK TABLES `kategorie` WRITE;
/*!40000 ALTER TABLE `kategorie` DISABLE KEYS */;
INSERT INTO `kategorie` VALUES (1,'Warzywa'),(2,'Owoce'),(3,'Mięso i ryby'),(4,'Nabiał'),(5,'Pieczywo'),(6,'Kosmetyki'),(7,'RTV AGD'),(8,'Artykuły biurowe'),(9,'Ubrania'),(10,'Artykuły gospodarstwa domowego'),(11,'Rośliny'),(12,'Słodycze'),(13,'Przekąski'),(14,'Napoje'),(15,'Alkohol');
/*!40000 ALTER TABLE `kategorie` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kraje`
--

DROP TABLE IF EXISTS `kraje`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kraje` (
  `ID_kra` int NOT NULL AUTO_INCREMENT,
  `Nazwa` text NOT NULL,
  `Skrot` varchar(3) NOT NULL,
  `Clo` int NOT NULL,
  PRIMARY KEY (`ID_kra`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kraje`
--

LOCK TABLES `kraje` WRITE;
/*!40000 ALTER TABLE `kraje` DISABLE KEYS */;
INSERT INTO `kraje` VALUES (1,'Rzeczpospolita Polska','POL',22),(2,'Republika Włoska','ITA',23),(3,'Stany Zjednoczone Ameryki','USA',27),(4,'Chińska Republika Ludowa','CPR',24),(5,'Zjednoczone Królestwo Wielkiej Brytanii i Irlandii Północnej','GBR',26),(6,'Republika Francuska','FRA',23);
/*!40000 ALTER TABLE `kraje` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `magazyny`
--

DROP TABLE IF EXISTS `magazyny`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `magazyny` (
  `ID_mag` int NOT NULL AUTO_INCREMENT,
  `Nazwa` text NOT NULL,
  `Marza` double NOT NULL,
  `Czas_dostawy` int NOT NULL,
  `Kraj` int DEFAULT NULL,
  PRIMARY KEY (`ID_mag`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `magazyny`
--

LOCK TABLES `magazyny` WRITE;
/*!40000 ALTER TABLE `magazyny` DISABLE KEYS */;
INSERT INTO `magazyny` VALUES (0,'Program Wsparcia Dla Nowych Przedsiębiorców',0,0,1),(1,'Bella Fantastico',0.35,3,2),(2,'Fromages Hadiuk',0.5,4,6),(3,'ITAmore',0.3,3,2),(4,'Spożywex SA',0.7,3,1),(5,'Marian-Hurt Sp. c.',0.6,5,1),(6,'MięsoPOL Sp. z o. o.',0.4,2,1),(7,'BiggerBetter',0.65,4,3),(8,'Yiaotobie',0.2,7,4),(9,'Deli of Queen',0.4,3,5),(10,'Ilaspeed',0.1,12,4);
/*!40000 ALTER TABLE `magazyny` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `nastanie`
--

DROP TABLE IF EXISTS `nastanie`;
/*!50001 DROP VIEW IF EXISTS `nastanie`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `nastanie` AS SELECT 
 1 AS `Nazwa`,
 1 AS `Ilosc`,
 1 AS `Cena`,
 1 AS `Wysokosc`,
 1 AS `Marza`,
 1 AS `Termin_przydatnosci`,
 1 AS `Magazyn`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `osklepie`
--

DROP TABLE IF EXISTS `osklepie`;
/*!50001 DROP VIEW IF EXISTS `osklepie`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `osklepie` AS SELECT 
 1 AS `Nazwa`,
 1 AS `Data_zalozenia`,
 1 AS `Marza`,
 1 AS `Liczba_pracownikow`,
 1 AS `Ruch`,
 1 AS `Rodzaj`,
 1 AS `Poziom`,
 1 AS `pojemnosc_magazynu`,
 1 AS `wynagrodzenie`,
 1 AS `oplaty_miesieczne`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `podatki`
--

DROP TABLE IF EXISTS `podatki`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `podatki` (
  `ID_pod` int NOT NULL AUTO_INCREMENT,
  `Nazwa` text NOT NULL,
  `Wysokosc` double NOT NULL,
  PRIMARY KEY (`ID_pod`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `podatki`
--

LOCK TABLES `podatki` WRITE;
/*!40000 ALTER TABLE `podatki` DISABLE KEYS */;
INSERT INTO `podatki` VALUES (1,'VAT Warzywa i owoce',0.05),(2,'VAT Owoce egzotyczne',0.05),(3,'VAT Przyprawy',0.08),(4,'VAT Artykuły spożywcze',0.23),(5,'VAT Produkty przemysłu młynarskiego',0.05),(6,'VAT Produkty pochodzenia zwierzęcego',0.05),(7,'VAT Mięso i podroby jadalne',0.05),(8,'VAT Ryby',0.05),(9,'VAT Artykuły dziecięce',0.05),(10,'VAT Artykuły higieniczne',0.05),(11,'VAT Wyroby medyczne',0.08),(12,'VAT Oprogramowanie i gry komputerowe',0.23),(13,'VAT Artykuły RTV AGD',0.23),(14,'VAT Artykuły biurowe i piśmiennicze',0.23),(15,'VAT Książki',0.05),(16,'VAT Gazety i czasopisma',0.08),(17,'VAT Odzież',0.23),(18,'VAT Rośliny żywe i cięte',0.08),(19,'Akcyza Alkohole mocne',0.75),(20,'Akcyza Piwo ',0.35),(21,'Akcyza Wina',0.26),(22,'Brak cła',0),(23,'Kraje Uni Celnej',0),(24,'Cło Azja',0.8),(25,'Cło Afryka',0.5),(26,'Cło Europa',0.23),(27,'Cło Ameryka',0.8),(28,'Cło Australia i Oceania',0.3);
/*!40000 ALTER TABLE `podatki` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `poziomy`
--

DROP TABLE IF EXISTS `poziomy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `poziomy` (
  `ID_poziom` int NOT NULL,
  `Nazwa` text NOT NULL,
  `pracownicy_min` int unsigned NOT NULL,
  `pracownicy_opt` int unsigned NOT NULL,
  `produkty_min` int unsigned NOT NULL,
  `kategorie_min` int unsigned NOT NULL,
  `pojemnosc_magazynu` int unsigned NOT NULL,
  `oplaty_miesieczne` double NOT NULL,
  `wynagrodzenie` double NOT NULL,
  `klienci_max` int unsigned NOT NULL,
  `marza_max` double NOT NULL,
  PRIMARY KEY (`ID_poziom`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `poziomy`
--

LOCK TABLES `poziomy` WRITE;
/*!40000 ALTER TABLE `poziomy` DISABLE KEYS */;
INSERT INTO `poziomy` VALUES (0,'Pustostan',0,0,0,0,50000,825,23,100,0.8),(1,'Sklepik',0,0,1,1,50000,825,46,100,0.8),(2,'Sklep',1,10,5,2,55000,3300,69,500,0.7),(3,'Dyskont',10,18,10,5,100000,19250,92,1500,0.6),(4,'Supermarket',18,30,15,10,500000,55000,115,10000,0.5),(5,'Hipermarket',30,50,20,15,2000000,330000,138,100000,0.4);
/*!40000 ALTER TABLE `poziomy` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pro_mag`
--

DROP TABLE IF EXISTS `pro_mag`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pro_mag` (
  `Produkt` int DEFAULT NULL,
  `Magazyn` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pro_mag`
--

LOCK TABLES `pro_mag` WRITE;
/*!40000 ALTER TABLE `pro_mag` DISABLE KEYS */;
/*!40000 ALTER TABLE `pro_mag` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pro_zam`
--

DROP TABLE IF EXISTS `pro_zam`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pro_zam` (
  `Produkt` int DEFAULT NULL,
  `Zamowienie` int DEFAULT NULL,
  `Ilosc` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pro_zam`
--

LOCK TABLES `pro_zam` WRITE;
/*!40000 ALTER TABLE `pro_zam` DISABLE KEYS */;
/*!40000 ALTER TABLE `pro_zam` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `produkty`
--

DROP TABLE IF EXISTS `produkty`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `produkty` (
  `ID_prod` int NOT NULL AUTO_INCREMENT,
  `Nazwa` text NOT NULL,
  `Cena` double NOT NULL,
  `Termin_przydatnosci` int NOT NULL,
  `Kategoria` int DEFAULT NULL,
  `Podatek` int DEFAULT NULL,
  PRIMARY KEY (`ID_prod`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `produkty`
--

LOCK TABLES `produkty` WRITE;
/*!40000 ALTER TABLE `produkty` DISABLE KEYS */;
/*!40000 ALTER TABLE `produkty` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rodzaje`
--

DROP TABLE IF EXISTS `rodzaje`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rodzaje` (
  `ID_rodzaj` int NOT NULL AUTO_INCREMENT,
  `Nazwa` text NOT NULL,
  PRIMARY KEY (`ID_rodzaj`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rodzaje`
--

LOCK TABLES `rodzaje` WRITE;
/*!40000 ALTER TABLE `rodzaje` DISABLE KEYS */;
INSERT INTO `rodzaje` VALUES (1,'Spożywczy'),(2,'Warzywniak'),(3,'RTV AGD'),(4,'Mięsny'),(5,'Drogeria'),(6,'Butik'),(7,'Papierniczy'),(8,'Piekarnia'),(9,'Ogrodniczy'),(10,'Market'),(11,'Monopolowy');
/*!40000 ALTER TABLE `rodzaje` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stan`
--

DROP TABLE IF EXISTS `stan`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stan` (
  `ID_stan` int NOT NULL AUTO_INCREMENT,
  `Ilosc` int NOT NULL,
  `Produkt` int DEFAULT NULL,
  `Zamowienie` int DEFAULT NULL,
  PRIMARY KEY (`ID_stan`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stan`
--

LOCK TABLES `stan` WRITE;
/*!40000 ALTER TABLE `stan` DISABLE KEYS */;
/*!40000 ALTER TABLE `stan` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `PaniWiesia` AFTER UPDATE ON `stan` FOR EACH ROW DELETE FROM stan WHERE Ilosc=0 */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `zamowienia`
--

DROP TABLE IF EXISTS `zamowienia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `zamowienia` (
  `ID_zam` int NOT NULL AUTO_INCREMENT,
  `Data_zamowienia` date NOT NULL,
  `Data_dostarczenia` date DEFAULT NULL,
  `Magazyn` int DEFAULT NULL,
  `Koszt` double DEFAULT NULL,
  PRIMARY KEY (`ID_zam`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `zamowienia`
--

LOCK TABLES `zamowienia` WRITE;
/*!40000 ALTER TABLE `zamowienia` DISABLE KEYS */;
/*!40000 ALTER TABLE `zamowienia` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `LicznikZamowien` AFTER INSERT ON `zamowienia` FOR EACH ROW UPDATE info SET Liczba_zamowien = Liczba_zamowien + 1 */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Dumping events for database 'shopmanager'
--

--
-- Dumping routines for database 'shopmanager'
--
/*!50003 DROP PROCEDURE IF EXISTS `NextDay` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `NextDay`()
BEGIN
Update info SET Dzisiaj = Dzisiaj + INTERVAL 1 DAY, Dochod_calkowity = Dochod_calkowity + Dochod_dzienny, Wydatki_calkowite = Wydatki_calkowite + Wydatki_dzienne;
Update info SET Saldo = (Saldo + Dochod_dzienny - Wydatki_dzienne);
Update info SET Dochod_dzienny = 0, Wydatki_dzienne = 0;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `oferta` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `oferta`(id int)
SELECT p.Nazwa, p.Cena, pod.Nazwa, pod.Wysokosc 
FROM pro_mag JOIN produkty p on pro_mag.Produkt = p.ID_prod JOIN podatki pod ON p.Podatek=pod.ID_pod 
WHERE pro_mag.Magazyn = @id ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `podglad` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `podglad`(id int)
SELECT p.Nazwa, z.Ilosc 
FROM pro_zam z JOIN produkty p on z.Produkt = p.ID_prod 
WHERE z.Zamowienie = @id ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `TheBlip` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `TheBlip`()
BEGIN
DELETE FROM info;
DELETE FROM stan;
DELETE FROM zamowienia;
DELETE FROM pro_zam;
ALTER TABLE stan AUTO_INCREMENT=1;
ALTER TABLE zamowienia AUTO_INCREMENT=1;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Final view structure for view `appopen`
--

/*!50001 DROP VIEW IF EXISTS `appopen`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `appopen` AS select `i`.`Saldo` AS `Saldo`,`i`.`Dochod_dzienny` AS `Dochod_dzienny`,`i`.`Wydatki_dzienne` AS `Wydatki_dzienne`,`i`.`Ruch` AS `Ruch`,`i`.`Liczba_pracownikow` AS `Liczba_pracownikow`,`i`.`Rodzaj` AS `Rodzaj`,`i`.`Poziom` AS `Poziom`,`i`.`Marza` AS `Marza`,`p`.`oplaty_miesieczne` AS `oplaty_miesieczne`,`p`.`wynagrodzenie` AS `wynagrodzenie`,`p`.`pracownicy_min` AS `pracownicy_min`,`p`.`pracownicy_opt` AS `pracownicy_opt`,`p`.`pojemnosc_magazynu` AS `pojemnosc_magazynu`,`p`.`klienci_max` AS `klienci_max` from (`info` `i` join `poziomy` `p` on((`i`.`Poziom` = `p`.`ID_poziom`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `dodostarczenia`
--

/*!50001 DROP VIEW IF EXISTS `dodostarczenia`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `dodostarczenia` AS select `zamowienia`.`ID_zam` AS `ID_zam`,`zamowienia`.`Data_zamowienia` AS `Data_zamowienia`,`zamowienia`.`Data_dostarczenia` AS `Data_dostarczenia`,`zamowienia`.`Magazyn` AS `Magazyn`,`zamowienia`.`Koszt` AS `Koszt` from `zamowienia` where (`zamowienia`.`Data_dostarczenia` >= curdate()) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `dostawcy`
--

/*!50001 DROP VIEW IF EXISTS `dostawcy`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `dostawcy` AS select `m`.`Nazwa` AS `Dostawca`,`m`.`Czas_dostawy` AS `Czas_dostawy`,`m`.`Marza` AS `Marza`,`m`.`ID_mag` AS `ID_mag`,`k`.`Nazwa` AS `Nazwa`,`k`.`Skrot` AS `Skrot`,`p`.`Nazwa` AS `Produkt`,`p`.`Wysokosc` AS `Wysokosc` from ((`magazyny` `m` join `kraje` `k` on((`m`.`Kraj` = `k`.`ID_kra`))) join `podatki` `p` on((`k`.`Clo` = `p`.`ID_pod`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `nastanie`
--

/*!50001 DROP VIEW IF EXISTS `nastanie`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `nastanie` AS select `p`.`Nazwa` AS `Nazwa`,`s`.`Ilosc` AS `Ilosc`,`p`.`Cena` AS `Cena`,`pod`.`Wysokosc` AS `Wysokosc`,`m`.`Marza` AS `Marza`,(`z`.`Data_dostarczenia` + interval `p`.`Termin_przydatnosci` day) AS `Termin_przydatnosci`,`z`.`Magazyn` AS `Magazyn` from (((((`stan` `s` join `zamowienia` `z` on((`s`.`Zamowienie` = `z`.`ID_zam`))) join `magazyny` `m` on((`z`.`Magazyn` = `m`.`ID_mag`))) join `pro_zam` on((`pro_zam`.`Zamowienie` = `z`.`ID_zam`))) join `produkty` `p` on((`p`.`ID_prod` = `pro_zam`.`Produkt`))) join `podatki` `pod` on((`p`.`Podatek` = `pod`.`ID_pod`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `osklepie`
--

/*!50001 DROP VIEW IF EXISTS `osklepie`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `osklepie` AS select `i`.`Nazwa` AS `Nazwa`,`i`.`Data_zalozenia` AS `Data_zalozenia`,`i`.`Marza` AS `Marza`,`i`.`Liczba_pracownikow` AS `Liczba_pracownikow`,`i`.`Ruch` AS `Ruch`,`r`.`Nazwa` AS `Rodzaj`,`p`.`Nazwa` AS `Poziom`,`p`.`pojemnosc_magazynu` AS `pojemnosc_magazynu`,`p`.`wynagrodzenie` AS `wynagrodzenie`,`p`.`oplaty_miesieczne` AS `oplaty_miesieczne` from ((`info` `i` join `rodzaje` `r` on((`i`.`Rodzaj` = `r`.`ID_rodzaj`))) join `poziomy` `p` on((`i`.`Poziom` = `p`.`ID_poziom`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-07-16 22:28:09

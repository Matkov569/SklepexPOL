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
INSERT INTO `magazyny` VALUES (1,'Bella Fantastico',0.35,3,2),(2,'Fromages Hadiuk',0.5,4,6),(3,'ITAmore',0.3,3,2),(4,'Spożywex SA',0.7,3,1),(5,'Marian-Hurt Sp. c.',0.6,5,1),(6,'MięsoPOL Sp. z o. o.',0.4,2,1),(7,'BiggerBetter',0.65,4,3),(8,'Yiaotobie',0.2,7,4),(9,'Deli of Queen',0.4,3,5),(10,'Ilaspeed',0.1,12,4);
/*!40000 ALTER TABLE `magazyny` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Table structure for table `rod_kat`
--

DROP TABLE IF EXISTS `rod_kat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rod_kat` (
  `Rodzaj` int DEFAULT NULL,
  `Kategoria` int DEFAULT NULL,
  `Udzial` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rod_kat`
--

LOCK TABLES `rod_kat` WRITE;
/*!40000 ALTER TABLE `rod_kat` DISABLE KEYS */;
INSERT INTO `rod_kat` VALUES (1,1,2),(1,2,2),(1,3,2),(1,4,2),(1,5,2),(1,12,2),(1,13,2),(1,14,2),(1,15,2),(2,1,2),(2,2,2),(3,7,0.9),(4,3,1),(5,6,0.95),(6,9,1),(7,8,1),(8,5,1),(9,11,0.8),(10,1,2),(10,2,2),(10,3,2),(10,4,2),(10,5,2),(10,6,2),(10,7,2),(10,8,2),(10,9,2),(10,10,2),(10,11,2),(10,12,2),(10,13,2),(10,14,2),(10,15,2),(11,15,0.85);
/*!40000 ALTER TABLE `rod_kat` ENABLE KEYS */;
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
  `Promocja` int DEFAULT NULL,
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
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-07-12 15:43:41

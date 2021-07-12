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
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-07-12 15:43:36

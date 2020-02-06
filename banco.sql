USE [master]
GO

/****** Object:  Database [BDCHURRASCO]    Script Date: 03/02/2020 10:05:33 ******/
CREATE DATABASE [BDCHURRASCO]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BDCHURRASCO', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\BDCHURRASCO.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BDCHURRASCO_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\BDCHURRASCO_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BDCHURRASCO].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [BDCHURRASCO] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET ARITHABORT OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [BDCHURRASCO] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [BDCHURRASCO] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET  DISABLE_BROKER 
GO

ALTER DATABASE [BDCHURRASCO] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [BDCHURRASCO] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [BDCHURRASCO] SET  MULTI_USER 
GO

ALTER DATABASE [BDCHURRASCO] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [BDCHURRASCO] SET DB_CHAINING OFF 
GO

ALTER DATABASE [BDCHURRASCO] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [BDCHURRASCO] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [BDCHURRASCO] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [BDCHURRASCO] SET QUERY_STORE = OFF
GO

ALTER DATABASE [BDCHURRASCO] SET  READ_WRITE 
GO

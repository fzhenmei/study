IF  Not EXISTS (SELECT name FROM sys.databases WHERE name = N'DevSamples')
CREATE DATABASE [DevSamples] 
GO
use [DevSamples]
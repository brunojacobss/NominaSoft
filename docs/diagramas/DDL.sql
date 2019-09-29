/* ---------------------------------------------------- */
/*  Generated by Enterprise Architect Version 13.5 		*/
/*  Created On : 25-Set.-2019 22:36:36 				*/
/*  DBMS       : MySql 						*/
/* ---------------------------------------------------- */

SET FOREIGN_KEY_CHECKS=0 
;

/* Drop Tables */

DROP TABLE IF EXISTS `Afp` CASCADE
;

DROP TABLE IF EXISTS `BoletaPago` CASCADE
;

DROP TABLE IF EXISTS `ConceptosDeDescuentos` CASCADE
;

DROP TABLE IF EXISTS `ConceptosDeIngresos` CASCADE
;

DROP TABLE IF EXISTS `Contrato` CASCADE
;

DROP TABLE IF EXISTS `Empleado` CASCADE
;

DROP TABLE IF EXISTS `PeriodoPago` CASCADE
;

/* Create Tables */

CREATE TABLE `Afp`
(
	`Nombre` NVARCHAR(40) NULL,
	`PorcentajeDescuento` DECIMAL(6,4) NULL,
	`AfpID` INT NOT NULL,
	CONSTRAINT `PK_Afp` PRIMARY KEY (`AfpID` ASC)
)

;

CREATE TABLE `BoletaPago`
(
	`FechaPago` DATE NULL,
	`BoletaPagoID` INT NOT NULL,
	`ContratoID` INT NOT NULL,
	`ConceptosDeIngresosID` INT NOT NULL,
	`PeriodoPagoID` INT NOT NULL,
	`ConceptosDeDescuentosID` INT NOT NULL,
	CONSTRAINT `PK_BoletaPago` PRIMARY KEY (`BoletaPagoID` ASC)
)

;

CREATE TABLE `ConceptosDeDescuentos`
(
	`MontoDeOtrosDescuentos` DECIMAL(5,2) NULL,
	`MontoPorAdelantos` DECIMAL(5,2) NULL,
	`MontoPorHorasAusentes` DECIMAL(5,2) NULL,
	`ConceptosDeDescuentosID` INT NOT NULL,
	CONSTRAINT `PK_ConceptosDeDescuentos` PRIMARY KEY (`ConceptosDeDescuentosID` ASC)
)

;

CREATE TABLE `ConceptosDeIngresos`
(
	`MontoDeOtrosIngresos` DECIMAL(5,2) NULL,
	`MontoPorHorasExtra` DECIMAL(5,2) NULL,
	`MontoPorReintegros` DECIMAL(5,2) NULL,
	`ConceptosDeIngresosID` INT NOT NULL AUTO_INCREMENT,
	CONSTRAINT `PK_ConceptosDeIngresos` PRIMARY KEY (`ConceptosDeIngresosID` ASC)
)

;

CREATE TABLE `Contrato`
(
	`FechaFin` DATE NULL,
	`FechaInicio` DATE NULL,
	`TieneAsignacionFamiliar` BOOL NULL,
	`TotalHoras` INT NULL,
	`ContratoID` INT NOT NULL,
	`AfpID` INT NOT NULL,
	`Cargo` VARCHAR(50) NULL,
	`EmpleadoID` INT NULL,
	CONSTRAINT `PK_Contrato` PRIMARY KEY (`ContratoID` ASC)
)

;

CREATE TABLE `Empleado`
(
	`Direccion` NVARCHAR(40) NULL,
	`Dni` VARCHAR(8) NULL,
	`EstadoCivil` VARCHAR(40) NULL,
	`FechaNacimiento` DATE NULL,
	`GradoAcademico` VARCHAR(40) NULL,
	`Nombre` NVARCHAR(40) NULL,
	`EmpleadoID` INT NOT NULL,
	CONSTRAINT `PK_Empleado` PRIMARY KEY (`EmpleadoID` ASC)
)

;

CREATE TABLE `PeriodoPago`
(
	`Esactivo` BOOL NULL,
	`FechaFin` DATE NULL,
	`FechaInicio` DATE NULL,
	`PeriodoPagoID` INT NOT NULL,
	CONSTRAINT `PK_PeriodoPago` PRIMARY KEY (`PeriodoPagoID` ASC)
)

;

/* Create Foreign Key Constraints */

ALTER TABLE `BoletaPago` 
 ADD CONSTRAINT `FK_BoletaPago_ConceptosDeDescuentos`
	FOREIGN KEY (`ConceptosDeDescuentosID`) REFERENCES `ConceptosDeDescuentos` (`ConceptosDeDescuentosID`) ON DELETE Restrict ON UPDATE Restrict
;

ALTER TABLE `BoletaPago` 
 ADD CONSTRAINT `FK_BoletaPago_ConceptosDeIngresos`
	FOREIGN KEY (`ConceptosDeIngresosID`) REFERENCES `ConceptosDeIngresos` (`ConceptosDeIngresosID`) ON DELETE Restrict ON UPDATE Restrict
;

ALTER TABLE `BoletaPago` 
 ADD CONSTRAINT `FK_BoletaPago_Contrato`
	FOREIGN KEY (`ContratoID`) REFERENCES `Contrato` (`ContratoID`) ON DELETE No Action ON UPDATE No Action
;

ALTER TABLE `BoletaPago` 
 ADD CONSTRAINT `FK_BoletaPago_PeriodoPago`
	FOREIGN KEY (`PeriodoPagoID`) REFERENCES `PeriodoPago` (`PeriodoPagoID`) ON DELETE No Action ON UPDATE No Action
;

ALTER TABLE `Contrato` 
 ADD CONSTRAINT `FK_Contrato_Afp`
	FOREIGN KEY (`AfpID`) REFERENCES `Afp` (`AfpID`) ON DELETE No Action ON UPDATE No Action
;

ALTER TABLE `Contrato` 
 ADD CONSTRAINT `FK_Contrato_Empleado`
	FOREIGN KEY (`EmpleadoID`) REFERENCES `Empleado` (`EmpleadoID`) ON DELETE No Action ON UPDATE No Action
;

SET FOREIGN_KEY_CHECKS=1 
;

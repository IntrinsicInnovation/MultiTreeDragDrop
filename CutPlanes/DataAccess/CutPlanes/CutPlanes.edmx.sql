
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/18/2019 19:52:15
-- Generated from EDMX file: D:\EGBC\repos\CutPlanes\CutPlanes\CutPlanes\DataAccess\CutPlanes\CutPlanes.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CutPlanes];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[LinkSubstationPools]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LinkSubstationPools];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[Constraints]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[Constraints];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[ConstraintSets]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[ConstraintSets];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[CutPlanes]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[CutPlanes];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[CutPlaneTypes]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[CutPlaneTypes];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[DimCutPlaneSubstations]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[DimCutPlaneSubstations];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[DimCutPlaneTimePeriods]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[DimCutPlaneTimePeriods];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[DimLrbSubstations]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[DimLrbSubstations];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[ElectricalGroups]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[ElectricalGroups];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[LinkSubstationElectricalGroups]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[LinkSubstationElectricalGroups];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[Maps]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[Maps];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[MapTypes]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[MapTypes];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[Pools]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[Pools];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[PoolTypes]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[PoolTypes];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[SubstationAttributes]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[SubstationAttributes];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[Substations]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[Substations];
GO
IF OBJECT_ID(N'[CutPlanesModelStoreContainer].[SubstationTypes]', 'U') IS NOT NULL
    DROP TABLE [CutPlanesModelStoreContainer].[SubstationTypes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Constraints'
CREATE TABLE [dbo].[Constraints] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ConstraintSetId] int  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [UserCreatedId] int  NOT NULL,
    [DateUpdated] datetime  NULL,
    [UserUpdatedId] int  NULL,
    [FiscalYear] int  NULL,
    [SummerN0] decimal(20,10)  NULL,
    [SummerN1] decimal(20,10)  NULL,
    [WinterN0] decimal(20,10)  NULL,
    [WinterN1] decimal(20,10)  NULL,
    [AreaLoss] decimal(20,10)  NULL,
    [AreaLossInPercentage] decimal(10,2)  NULL,
    [TotalLoss] decimal(20,10)  NULL,
    [TotalLossInPercentage] decimal(10,2)  NULL,
    [IntermittentRmr] decimal(20,10)  NULL
);
GO

-- Creating table 'ConstraintSets'
CREATE TABLE [dbo].[ConstraintSets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [PoolId] int  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [UserCreatedId] int  NOT NULL,
    [DateUpdated] datetime  NULL,
    [UserUpdatedId] int  NULL
);
GO

-- Creating table 'CutPlanes'
CREATE TABLE [dbo].[CutPlanes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(256)  NULL,
    [Description] nvarchar(max)  NULL,
    [TypeId] int  NOT NULL,
    [UserCreatedId] int  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [DateUpdated] datetime  NULL,
    [UserUpdatedId] int  NULL,
    [MapId] int  NOT NULL
);
GO

-- Creating table 'CutPlaneTypes'
CREATE TABLE [dbo].[CutPlaneTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(256)  NOT NULL,
    [Code] nvarchar(256)  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [UserCreatedId] int  NOT NULL,
    [DateUpdated] datetime  NULL,
    [UserUpdatedId] int  NULL
);
GO

-- Creating table 'DimCutPlaneSubstations'
CREATE TABLE [dbo].[DimCutPlaneSubstations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [Code] nvarchar(50)  NULL,
    [Description] nvarchar(255)  NULL,
    [UserCreated] nvarchar(255)  NULL,
    [DateCreated] datetime  NOT NULL,
    [BeginDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [SubtstaionType] nvarchar(255)  NULL,
    [FuelType] nvarchar(255)  NULL,
    [PrimaryPoi] nvarchar(255)  NULL,
    [SecondaryPoi] nvarchar(255)  NULL,
    [PoiVoltage] decimal(20,10)  NULL,
    [PoiCircuitDesignation] nvarchar(255)  NULL,
    [MapName] nvarchar(255)  NULL,
    [MapDivisiion] nvarchar(255)  NULL,
    [MapRegion] nvarchar(255)  NULL,
    [MapSubRegion] nvarchar(255)  NULL,
    [MapArea] nvarchar(255)  NULL,
    [MapZone] nvarchar(255)  NULL
);
GO

-- Creating table 'DimCutPlaneTimePeriods'
CREATE TABLE [dbo].[DimCutPlaneTimePeriods] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [Year] int  NULL,
    [Description] nvarchar(255)  NULL,
    [UserCreated] nvarchar(255)  NOT NULL,
    [DateCreated] datetime  NOT NULL
);
GO

-- Creating table 'DimLrbSubstations'
CREATE TABLE [dbo].[DimLrbSubstations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(255)  NULL,
    [Code] nvarchar(50)  NULL,
    [Description] nvarchar(max)  NULL,
    [UserCreated] nvarchar(255)  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [BeginDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [SubstationType] nvarchar(255)  NULL,
    [FuelType] nvarchar(255)  NULL,
    [PrimaryPoi] nvarchar(255)  NULL,
    [SecondaryPoi] nvarchar(255)  NULL,
    [PoiVoltage] decimal(20,10)  NULL,
    [PoiCircuitDesignation] nvarchar(255)  NULL
);
GO

-- Creating table 'LinkSubstationElectricalGroups'
CREATE TABLE [dbo].[LinkSubstationElectricalGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SubstationId] int  NOT NULL,
    [ElectricalGroupId] int  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [UserCreatedId] int  NOT NULL
);
GO

-- Creating table 'MapTypes'
CREATE TABLE [dbo].[MapTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(256)  NOT NULL,
    [Code] nvarchar(256)  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [UserCreatedId] int  NOT NULL,
    [DateUpdated] datetime  NULL,
    [UserUpdatedId] int  NULL
);
GO

-- Creating table 'Pools'
CREATE TABLE [dbo].[Pools] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(256)  NULL,
    [DateCreated] datetime  NOT NULL,
    [UserCreatedId] int  NOT NULL,
    [CutPlaneId] int  NOT NULL,
    [PoolTypeId] int  NOT NULL,
    [DateUpdated] datetime  NULL,
    [UserUpdatedId] int  NULL
);
GO

-- Creating table 'PoolTypes'
CREATE TABLE [dbo].[PoolTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(256)  NOT NULL,
    [Value] nvarchar(256)  NOT NULL,
    [UserCreatedId] int  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [UserUpdatedId] int  NULL,
    [DateUpdated] datetime  NULL
);
GO

-- Creating table 'SubstationAttributes'
CREATE TABLE [dbo].[SubstationAttributes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OriginalId] int  NULL,
    [UserCreatedId] int  NULL,
    [UserUpdatedId] int  NULL,
    [DateCreated] datetime  NOT NULL,
    [DateEnd] datetime  NULL,
    [DateReplaced] datetime  NULL,
    [DateEffective] datetime  NULL,
    [ProjectPhaseId] int  NULL,
    [SubstationId] int  NOT NULL,
    [ResourcePlanId] int  NOT NULL,
    [RatedMVA] decimal(20,10)  NULL,
    [RatedOverExcitedPowerFactor] decimal(20,10)  NULL,
    [RatedUnderExcitedPowerFactor] decimal(20,10)  NULL,
    [NamePlateCapacityInMW] decimal(20,10)  NULL,
    [RatedOverExcitedMvar] decimal(20,10)  NULL,
    [RatedUnderExcitedMvar] decimal(20,10)  NULL,
    [RatedPowerFactor] decimal(20,10)  NULL,
    [DependableGeneratingCapacity] decimal(20,10)  NULL,
    [EffectiveLoadCarryingCapacity] decimal(20,10)  NULL,
    [LoadGrossSummerPeak] decimal(20,10)  NULL,
    [LoadGrossSummerMin] decimal(20,10)  NULL,
    [LoadGrossWinterPeak] decimal(20,10)  NULL,
    [LoadGrossWinterMin] decimal(20,10)  NULL,
    [MaximumPowerOutput] decimal(20,10)  NULL,
    [Nominated] bit  NULL,
    [Probability] int  NULL,
    [InServiceStartDate] datetime  NULL,
    [InServiceEndDate] datetime  NULL,
    [NitsDesignatedCapacity] decimal(20,10)  NULL,
    [EPACapacity] decimal(20,10)  NULL,
    [MaxTakeOrPayCapacity] decimal(20,10)  NULL,
    [PointToPointCapacity] decimal(20,10)  NULL,
    [ReliabilityMustRunCapacity] decimal(20,10)  NULL,
    [SystemCapacity] decimal(20,10)  NULL,
    [UsePdgcMasteredValue] bit  NOT NULL,
    [UsePmpoMasteredValue] bit  NOT NULL,
    [UsePptpMasteredValue] bit  NOT NULL,
    [UsePrmrMasteredValue] bit  NOT NULL,
    [UsePndcMasteredValue] bit  NOT NULL,
    [UsePnpMasteredValue] bit  NOT NULL,
    [UsePdmx0MasteredValue] bit  NOT NULL,
    [UsePdmx1MasteredValue] bit  NOT NULL,
    [MaxOutputForFirmTransmission] decimal(20,10)  NULL,
    [MaxOutputForNonFirmTransmission] decimal(20,10)  NULL,
    [DateUpdated] datetime  NULL
);
GO

-- Creating table 'Substations'
CREATE TABLE [dbo].[Substations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [DateUpdated] datetime  NULL,
    [UserCreatedId] int  NOT NULL,
    [UserUpdatedId] int  NULL,
    [GeoLocationId] int  NULL,
    [Description] nvarchar(max)  NOT NULL,
    [SingleLineDiagramUri] varchar(255)  NULL,
    [LocalOperationOrderLink] varchar(255)  NULL,
    [MeterId] int  NULL,
    [ElectricalGroupId] int  NULL,
    [Latitude] decimal(20,10)  NULL,
    [Longitude] decimal(20,10)  NULL,
    [PowerCallId] int  NULL,
    [IsSynchronousCondenserCapable] bit  NULL,
    [CIMLoadTS] datetime  NULL,
    [LRBLoadTS] datetime  NULL,
    [FacilityLoadTS] datetime  NULL,
    [NITSPlanLoadTS] datetime  NULL,
    [SubStationTypeId] int  NULL,
    [FuelTypeId] int  NULL,
    [OwnershipTypeId] int  NULL,
    [LTAPSubRegionId] int  NULL,
    [ResourceStatusId] int  NULL,
    [PrimaryPoi] nvarchar(100)  NULL,
    [SecondaryPoi] nvarchar(100)  NULL,
    [PoiCircuitDesignation] nvarchar(100)  NULL,
    [PoiVoltage] decimal(20,10)  NULL,
    [Name] nvarchar(255)  NULL,
    [TLA] nvarchar(255)  NULL
);
GO

-- Creating table 'SubstationTypes'
CREATE TABLE [dbo].[SubstationTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] nvarchar(100)  NOT NULL,
    [UserUpdatedId] int  NULL,
    [UserCreatedId] int  NULL,
    [DateCreated] datetime  NOT NULL,
    [DateUpdated] datetime  NULL
);
GO

-- Creating table 'Maps'
CREATE TABLE [dbo].[Maps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(256)  NULL,
    [Description] nvarchar(max)  NULL,
    [DateCreated] datetime  NOT NULL,
    [UserCreatedId] int  NOT NULL,
    [UserUpatedId] int  NULL,
    [DateUpdated] datetime  NULL,
    [IsActive] bit  NULL,
    [MapDefinitionRootId] int  NOT NULL,
    [TypeId] int  NOT NULL
);
GO

-- Creating table 'ElectricalGroups'
CREATE TABLE [dbo].[ElectricalGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [DateUpdated] datetime  NULL,
    [UserCreatedId] int  NOT NULL,
    [UserUpdatedId] int  NULL,
    [ParentPk] int  NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Abbreviation] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'LinkSubstationPools'
CREATE TABLE [dbo].[LinkSubstationPools] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SubstationId] int  NOT NULL,
    [PoolId] int  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [UserCreatedId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id], [ConstraintSetId], [DateCreated], [UserCreatedId] in table 'Constraints'
ALTER TABLE [dbo].[Constraints]
ADD CONSTRAINT [PK_Constraints]
    PRIMARY KEY CLUSTERED ([Id], [ConstraintSetId], [DateCreated], [UserCreatedId] ASC);
GO

-- Creating primary key on [Id], [PoolId], [DateCreated], [UserCreatedId] in table 'ConstraintSets'
ALTER TABLE [dbo].[ConstraintSets]
ADD CONSTRAINT [PK_ConstraintSets]
    PRIMARY KEY CLUSTERED ([Id], [PoolId], [DateCreated], [UserCreatedId] ASC);
GO

-- Creating primary key on [Id], [TypeId], [UserCreatedId], [DateCreated], [MapId] in table 'CutPlanes'
ALTER TABLE [dbo].[CutPlanes]
ADD CONSTRAINT [PK_CutPlanes]
    PRIMARY KEY CLUSTERED ([Id], [TypeId], [UserCreatedId], [DateCreated], [MapId] ASC);
GO

-- Creating primary key on [Id], [Name], [Code], [DateCreated], [UserCreatedId] in table 'CutPlaneTypes'
ALTER TABLE [dbo].[CutPlaneTypes]
ADD CONSTRAINT [PK_CutPlaneTypes]
    PRIMARY KEY CLUSTERED ([Id], [Name], [Code], [DateCreated], [UserCreatedId] ASC);
GO

-- Creating primary key on [Id], [Name], [DateCreated] in table 'DimCutPlaneSubstations'
ALTER TABLE [dbo].[DimCutPlaneSubstations]
ADD CONSTRAINT [PK_DimCutPlaneSubstations]
    PRIMARY KEY CLUSTERED ([Id], [Name], [DateCreated] ASC);
GO

-- Creating primary key on [Id], [Name], [UserCreated], [DateCreated] in table 'DimCutPlaneTimePeriods'
ALTER TABLE [dbo].[DimCutPlaneTimePeriods]
ADD CONSTRAINT [PK_DimCutPlaneTimePeriods]
    PRIMARY KEY CLUSTERED ([Id], [Name], [UserCreated], [DateCreated] ASC);
GO

-- Creating primary key on [Id], [UserCreated], [DateCreated] in table 'DimLrbSubstations'
ALTER TABLE [dbo].[DimLrbSubstations]
ADD CONSTRAINT [PK_DimLrbSubstations]
    PRIMARY KEY CLUSTERED ([Id], [UserCreated], [DateCreated] ASC);
GO

-- Creating primary key on [Id], [SubstationId], [ElectricalGroupId], [DateCreated], [UserCreatedId] in table 'LinkSubstationElectricalGroups'
ALTER TABLE [dbo].[LinkSubstationElectricalGroups]
ADD CONSTRAINT [PK_LinkSubstationElectricalGroups]
    PRIMARY KEY CLUSTERED ([Id], [SubstationId], [ElectricalGroupId], [DateCreated], [UserCreatedId] ASC);
GO

-- Creating primary key on [Id], [Name], [Code], [DateCreated], [UserCreatedId] in table 'MapTypes'
ALTER TABLE [dbo].[MapTypes]
ADD CONSTRAINT [PK_MapTypes]
    PRIMARY KEY CLUSTERED ([Id], [Name], [Code], [DateCreated], [UserCreatedId] ASC);
GO

-- Creating primary key on [Id], [DateCreated], [UserCreatedId], [CutPlaneId], [PoolTypeId] in table 'Pools'
ALTER TABLE [dbo].[Pools]
ADD CONSTRAINT [PK_Pools]
    PRIMARY KEY CLUSTERED ([Id], [DateCreated], [UserCreatedId], [CutPlaneId], [PoolTypeId] ASC);
GO

-- Creating primary key on [Id], [Name], [Value], [UserCreatedId], [DateCreated] in table 'PoolTypes'
ALTER TABLE [dbo].[PoolTypes]
ADD CONSTRAINT [PK_PoolTypes]
    PRIMARY KEY CLUSTERED ([Id], [Name], [Value], [UserCreatedId], [DateCreated] ASC);
GO

-- Creating primary key on [Id], [DateCreated], [SubstationId], [ResourcePlanId], [UsePdgcMasteredValue], [UsePmpoMasteredValue], [UsePptpMasteredValue], [UsePrmrMasteredValue], [UsePndcMasteredValue], [UsePnpMasteredValue], [UsePdmx0MasteredValue], [UsePdmx1MasteredValue] in table 'SubstationAttributes'
ALTER TABLE [dbo].[SubstationAttributes]
ADD CONSTRAINT [PK_SubstationAttributes]
    PRIMARY KEY CLUSTERED ([Id], [DateCreated], [SubstationId], [ResourcePlanId], [UsePdgcMasteredValue], [UsePmpoMasteredValue], [UsePptpMasteredValue], [UsePrmrMasteredValue], [UsePndcMasteredValue], [UsePnpMasteredValue], [UsePdmx0MasteredValue], [UsePdmx1MasteredValue] ASC);
GO

-- Creating primary key on [Id], [DateCreated], [UserCreatedId], [Description] in table 'Substations'
ALTER TABLE [dbo].[Substations]
ADD CONSTRAINT [PK_Substations]
    PRIMARY KEY CLUSTERED ([Id], [DateCreated], [UserCreatedId], [Description] ASC);
GO

-- Creating primary key on [Id], [Value], [DateCreated] in table 'SubstationTypes'
ALTER TABLE [dbo].[SubstationTypes]
ADD CONSTRAINT [PK_SubstationTypes]
    PRIMARY KEY CLUSTERED ([Id], [Value], [DateCreated] ASC);
GO

-- Creating primary key on [Id], [DateCreated], [UserCreatedId], [MapDefinitionRootId], [TypeId] in table 'Maps'
ALTER TABLE [dbo].[Maps]
ADD CONSTRAINT [PK_Maps]
    PRIMARY KEY CLUSTERED ([Id], [DateCreated], [UserCreatedId], [MapDefinitionRootId], [TypeId] ASC);
GO

-- Creating primary key on [Id], [DateCreated], [UserCreatedId], [Name], [Abbreviation] in table 'ElectricalGroups'
ALTER TABLE [dbo].[ElectricalGroups]
ADD CONSTRAINT [PK_ElectricalGroups]
    PRIMARY KEY CLUSTERED ([Id], [DateCreated], [UserCreatedId], [Name], [Abbreviation] ASC);
GO

-- Creating primary key on [Id] in table 'LinkSubstationPools'
ALTER TABLE [dbo].[LinkSubstationPools]
ADD CONSTRAINT [PK_LinkSubstationPools]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
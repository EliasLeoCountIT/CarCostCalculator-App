PRINT N'Deleting existing data from CostEntries table...';
DELETE FROM [CarCostCalculator].[dbo].[CostEntries]

PRINT N'Deleting existing data from KilometerEntries table...';
DELETE FROM [CarCostCalculator].[dbo].[KilometerEntries]

PRINT N'Deleting existing data from MonthlyDatas table...';
DELETE FROM [CarCostCalculator].[dbo].[MonthlyDatas]

PRINT N'Deleting existing data from AnnualDatas table...';
DELETE FROM [CarCostCalculator].[dbo].[AnnualDatas]

PRINT N'Deleting existing data from Categories table...';
DELETE FROM [CarCostCalculator].[dbo].[Categories]


PRINT N'Inserting new data into Categories table...';
SET IDENTITY_INSERT Categories ON;

INSERT INTO [CarCostCalculator].[dbo].[Categories]
([Id], [Name]			  )
  VALUES
(1   , N'Kfz-Versicherung'),
(2   , N'Zulassung'       ),
(3   , N'Pickerl'         ),
(4   , N'Service'         ),
(5   , N'ÖAMTC'           ),
(6   , N'Vignette'        ),
(7   , N'Tanken'          ),
(8   , N'Sonstiges'       );

SET IDENTITY_INSERT Categories OFF;
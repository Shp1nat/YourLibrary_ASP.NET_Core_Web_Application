CREATE TABLE [dbo].[Example]
(
	[idExample] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[UidExample] UNIQUEIDENTIFIER NOT NULL UNIQUE,
	[idBook] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Book](idBook),
	[idPublisher] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Publisher](idPublisher),
	[YearOfCreation] INT NOT NULL,
	[IsTaken] BIT NULL DEFAULT 0
)

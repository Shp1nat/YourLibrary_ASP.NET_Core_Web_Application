CREATE TABLE [dbo].[TypeBkOfBook]
(
	[idBook] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Book](idBook),
	[idTypeBk] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[TypeBk](idTypeBk)
)

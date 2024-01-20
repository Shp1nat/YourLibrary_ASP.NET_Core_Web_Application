CREATE TABLE [dbo].[GenreOfBook]
(
	[idBook] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Book](idBook),
	[idGenre] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Genre](idGenre)
)

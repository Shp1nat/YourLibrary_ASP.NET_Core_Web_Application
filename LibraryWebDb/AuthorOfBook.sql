CREATE TABLE [dbo].[AuthorOfBook]
(
	[idBook] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Book](idBook),
	[idAuthor] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Author](idAuthor)
)

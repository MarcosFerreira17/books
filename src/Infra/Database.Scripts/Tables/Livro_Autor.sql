CREATE TABLE [dbo].[Livro_Autor]
(
	Livro_Codl INT FOREIGN KEY REFERENCES Livro(Codl),
    Autor_CodAu INT FOREIGN KEY REFERENCES Autor(CodAu),
    PRIMARY KEY (Livro_Codl, Autor_CodAu)
);

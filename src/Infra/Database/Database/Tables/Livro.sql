CREATE TABLE [dbo].Livro (
    Codl INT PRIMARY KEY IDENTITY,
    Titulo VARCHAR(40) NOT NULL,
    Editora VARCHAR(40) NOT NULL,
    Edicao INT,
    AnoPublicacao VARCHAR(4)
);
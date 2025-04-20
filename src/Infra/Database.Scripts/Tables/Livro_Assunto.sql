CREATE TABLE [dbo].Livro_Assunto 
(
    Livro_Codl INT FOREIGN KEY REFERENCES Livro(Codl),
    Assunto_codAs INT FOREIGN KEY REFERENCES Assunto(codAs),
    PRIMARY KEY (Livro_Codl, Assunto_codAs)
);
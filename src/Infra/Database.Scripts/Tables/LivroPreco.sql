CREATE TABLE LivroPreco (
    Codl INT PRIMARY KEY IDENTITY,
    Livro_Codl INT FOREIGN KEY REFERENCES Livro(Codl),
    TipoCompra VARCHAR(20) NOT NULL, -- Ex: Balcão, Internet
    Valor DECIMAL(10, 2) NOT NULL
);

CREATE VIEW RelatorioAutoresLivros AS
SELECT 
    A.Nome AS Autor,
    L.Titulo AS Livro,
    STRING_AGG(ASU.Descricao, ', ') AS Assuntos,
    COUNT(L.Codl) AS TotalLivros
FROM Livro_Autor LA
JOIN Autor A ON LA.Autor_CodAu = A.CodAu
JOIN Livro L ON LA.Livro_Codl = L.Codl
JOIN Livro_Assunto LAS ON L.Codl = LAS.Livro_Codl
JOIN Assunto ASU ON LAS.Assunto_codAs = ASU.codAs
GROUP BY A.Nome, L.Titulo;
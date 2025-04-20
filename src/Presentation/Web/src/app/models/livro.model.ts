import { Assunto } from './assunto.model';
import { Autor } from './autor.model';
import { LivroPreco } from './livropreco.model';

export interface Livro {
  codl: number;
  titulo: string;
  editora: string;
  edicao: number;
  anoPublicacao: string;
  autores: Autor[];
  assuntos: Assunto[];
  precos: LivroPreco[];
}

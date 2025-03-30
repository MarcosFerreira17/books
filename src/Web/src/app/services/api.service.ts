import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, of, throwError } from 'rxjs';

const API_URL = 'https://localhost:7155/api/v1';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  constructor(private http: HttpClient) {}

  // Operações para Livros
  getLivros(): Observable<any> {
    return this.http.get(`${API_URL}/Livro`);
  }

  getLivro(id: number): Observable<any> {
    return this.http.get(`${API_URL}/Livro/${id}`);
  }

  createLivro(livro: any): Observable<any> {
    return this.http.post(`${API_URL}/Livro`, livro);
  }

  updateLivro(id: number, livro: any): Observable<any> {
    return this.http.put(`${API_URL}/Livro/${id}`, livro);
  }

  deleteLivro(id: number): Observable<any> {
    return this.http.delete(`${API_URL}/Livro/${id}`);
  }

  getAutores(): Observable<any> {
    return this.http.get(`${API_URL}/Autores`);
  }

  createAutor(autor: any): Observable<any> {
    return this.http.post(`${API_URL}/Autores`, autor);
  }

  updateAutor(id: number, autor: any): Observable<any> {
    return this.http.put(`${API_URL}/Autores/${id}`, autor);
  }

  deleteAutor(id: number): Observable<any> {
    return this.http.delete(`${API_URL}/Autores/${id}`);
  }

  // Operações para Assuntos
  getAssuntos(): Observable<any> {
    return this.http.get(`${API_URL}/Assuntos`);
  }

  createAssunto(assunto: any): Observable<any> {
    return this.http.post(`${API_URL}/Assuntos`, assunto);
  }

  updateAssunto(id: number, assunto: any): Observable<any> {
    return this.http.put(`${API_URL}/Assuntos/${id}`, assunto);
  }

  deleteAssunto(id: number): Observable<any> {
    return this.http.delete(`${API_URL}/Assuntos/${id}`);
  }
  getPrecosByLivroCodl(codl: number): Observable<any> {
    return this.http.get<any>(`${API_URL}/LivroPrecos/${codl}`).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 404) {
          return of(); // Retorna array vazio para 404
        }
        return throwError(() => error);
      })
    );
  }
  // Operações para Preços
  getPrecos(): Observable<any> {
    return this.http.get(`${API_URL}/LivroPrecos`);
  }

  createPrecos(assunto: any): Observable<any> {
    return this.http.post(`${API_URL}/LivroPrecos`, assunto);
  }

  updatePrecos(id: number, assunto: any): Observable<any> {
    return this.http.put(`${API_URL}/LivroPrecos/${id}`, assunto);
  }

  deletePrecos(id: number): Observable<any> {
    return this.http.delete(`${API_URL}/LivroPrecos/${id}`);
  }

  //Relatorio
  getRelatorio(): Observable<Blob> {
    return this.http.get(`${API_URL}/Relatorio`, {
      responseType: 'blob',
      headers: {
        Accept: 'application/pdf',
      },
    });
  }
}

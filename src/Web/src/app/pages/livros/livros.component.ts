import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  FormArray,
} from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { DecimalPipe, NgFor, NgIf } from '@angular/common';
import { Autor } from '../../models/autor.model';
import { Assunto } from '../../models/assunto.model';
import { Livro } from '../../models/livro.model';
import { delay, forkJoin } from 'rxjs';
import { LivroPreco } from '../../models/livropreco.model';

@Component({
  selector: 'app-livros',
  imports: [NgIf, NgFor, ReactiveFormsModule, DecimalPipe],
  templateUrl: './livros.component.html',
  styleUrls: ['./livros.component.scss'],
})
export class LivrosComponent implements OnInit {
  autores: Autor[] = [];
  assuntos: Assunto[] = [];
  livros: Livro[] = [];
  livroForm: FormGroup;
  isEditing = false;
  codl!: number;

  constructor(private fb: FormBuilder, private api: ApiService) {
    this.livroForm = this.fb.group({
      titulo: ['', [Validators.required, Validators.maxLength(40)]],
      editora: ['', [Validators.required, Validators.maxLength(40)]],
      edicao: [null],
      anoPublicacao: ['', [Validators.required, Validators.pattern(/^\d{4}$/)]],
      autores: this.fb.array([], [Validators.required]), // Array inicial vazio
      assuntos: this.fb.array([], [Validators.required]), // Array inicial vazio
      precos: this.fb.array([]), // Array inicial vazio
    });
  }

  get autoresArray(): FormArray {
    return this.livroForm.get('autores') as FormArray;
  }

  get assuntosArray(): FormArray {
    return this.livroForm.get('assuntos') as FormArray;
  }

  addAutor(codAu?: number): void {
    this.autoresArray.push(this.fb.control(codAu || null));
  }

  addAssunto(codAs?: number): void {
    this.assuntosArray.push(this.fb.control(codAs || null));
  }

  removeAutor(index: number): void {
    this.autoresArray.removeAt(index);
  }

  removeAssunto(index: number): void {
    this.assuntosArray.removeAt(index);
  }

  ngOnInit(): void {
    this.loadLivros();
    this.loadDependencies();
  }

  get precosArray(): FormArray {
    return this.livroForm.get('precos') as FormArray;
  }

  addPreco(): void {
    const precoGroup = this.fb.group({
      tipoCompra: ['', Validators.required],
      valor: [0, [Validators.required, Validators.min(0)]],
    });
    this.precosArray.push(precoGroup);
  }

  downloadRelatorio(): any {
    this.api.getRelatorio().subscribe({
      next: (pdfBlob: Blob) => {
        const url = window.URL.createObjectURL(pdfBlob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `relatorio.pdf`;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        console.error('Erro ao gerar PDF:', err);
        alert('Erro ao gerar o PDF. Tente novamente mais tarde.');
      },
    });
  }

  removePreco(index: number): void {
    this.precosArray.removeAt(index);
  }

  loadLivros(): void {
    this.api.getLivros().subscribe({
      next: (data) => (this.livros = data),
      error: (err) => alert(`Erro ao carregar livros: ${err.error}`),
    });
  }

  loadDependencies(): void {
    this.api.getAutores().subscribe({
      next: (autores) => (this.autores = autores),
      error: (err) => alert(`Erro ao carregar autores: ${err.error}`),
    });

    this.api.getAssuntos().subscribe({
      next: (assuntos) => (this.assuntos = assuntos),
      error: (err) => alert(`Erro ao carregar assuntos: ${err.error}`),
    });
  }

  onSubmit(): void {
    if (this.livroForm.invalid) return;

    // Criar payload conforme especificação
    const formValue = this.livroForm.value;

    const livroPayload = {
      titulo: formValue.titulo,
      editora: formValue.editora,
      edicao: formValue.edicao,
      anoPublicacao: formValue.anoPublicacao,
      autores: formValue.autores.filter((a: number | null) => a !== null), // Remove valores nulos
      assuntos: formValue.assuntos.filter((a: number | null) => a !== null), // Remove valores nulos
    };
    const precosData = formValue.precos; // Separar os preços

    if (this.isEditing) {
      this.api.updateLivro(this.codl, livroPayload).subscribe({
        next: () => {
          this.handlePrecos(this.codl, precosData);
          this.loadLivros();
          this.resetForm();
        },
        error: (err) => {
          console.error('Erro ao atualizar livro:', err);
          alert(
            'Erro na atualização: ' +
              (err.error?.message || 'Erro desconhecido')
          );
        },
      });
    } else {
      this.api.createLivro(livroPayload).subscribe({
        next: (codl) => {
          this.handlePrecos(codl, precosData);
          this.loadLivros();
          this.resetForm();
        },
        error: (err) => {
          console.error('Erro ao criar livro:', err);
          alert(
            'Erro na criação: ' + (err.error?.message || 'Erro desconhecido')
          );
        },
      });
    }
  }

  private handlePrecos(livroCodl: number, precos: LivroPreco[]): void {
    if (this.isEditing) {
      this.api.getLivro(livroCodl).subscribe({
        next: (livro: Livro) => {
          this.processPrecos(livroCodl, precos, livro.precos);
        },
        error: (err) => {
          alert(`Erro ao carregar preços existentes ${err.error}`);
        },
      });
    } else {
      this.createPrecosForLivro(livroCodl, precos);
    }
  }

  private processPrecos(
    livroCodl: number,
    newPrecos: LivroPreco[],
    existingPrecos: LivroPreco[]
  ): void {
    // Função para comparar igualdade de preços
    const isSamePreco = (a: LivroPreco, b: LivroPreco) =>
      a.tipoCompra === b.tipoCompra && a.valor === b.valor;

    // 1. Encontrar preços para deletar (existem no servidor mas não nos novos)
    const toDelete = existingPrecos.filter(
      (ep) => !newPrecos.some((np) => isSamePreco(ep, np))
    );

    // 2. Encontrar preços para criar (existem nos novos mas não no servidor)
    const toCreate = newPrecos.filter(
      (np) => !existingPrecos.some((ep) => isSamePreco(ep, np))
    );

    // 3. Executar operações
    const deleteOperations = toDelete.map((preco) =>
      this.api.deletePrecos(preco.codp)
    );

    const createOperations = toCreate.map((preco) => {
      const newPreco = { ...preco, livroCodl };
      return this.api.createPrecos(newPreco);
    });

    // Executar todas as operações
    forkJoin([...deleteOperations, ...createOperations]).subscribe({
      next: () => console.log('Operações concluídas'),
      error: (err) => console.error('Erro nas operações:', err),
    });
  }

  private createPrecosForLivro(livroCodl: number, precos: LivroPreco[]): void {
    precos.forEach((preco) => {
      const precoData = {
        ...preco,
        livroCodl: livroCodl,
      };
      this.api.createPrecos(precoData).subscribe({
        next: () => console.log('Preço criado'),
        error: (err) => alert(`Erro ao criar preço: ${err.error}`),
      });
    });
  }

  onEdit(livro: Livro): void {
    this.isEditing = true;
    this.codl = livro.codl;

    window.scrollTo({
      top: 0,
      behavior: 'smooth', // Animação suave
    });

    // Limpar e popular arrays
    this.autoresArray.clear();
    this.assuntosArray.clear();

    // Popular autores
    livro.autores.forEach((autor) => {
      this.autoresArray.push(this.fb.control(autor.codAu));
    });

    // Popular assuntos
    livro.assuntos.forEach((assunto) => {
      this.assuntosArray.push(this.fb.control(assunto.codAs));
    });

    this.livroForm.patchValue({
      titulo: livro.titulo,
      editora: livro.editora,
      edicao: livro.edicao,
      anoPublicacao: livro.anoPublicacao,
    });

    this.loadPrecos(livro);
  }

  private loadPrecos(livro: Livro): void {
    this.precosArray.clear();
    livro.precos.forEach((preco) => {
      this.precosArray.push(
        this.fb.group({
          tipoCompra: [preco.tipoCompra, Validators.required],
          valor: [preco.valor, [Validators.required, Validators.min(0)]],
        })
      );
    });
  }

  onDelete(id: number): void {
    if (confirm('Tem certeza que deseja excluir este livro?')) {
      this.api.deleteLivro(id).subscribe({
        next: () => this.loadLivros(),
        error: (err) => alert(`Erro ao excluir livro: ${err.error}`),
      });
    }
  }

  resetForm(): void {
    this.livroForm.reset();
    this.isEditing = false;
    this.codl = 0;
    this.precosArray.clear();
  }
}

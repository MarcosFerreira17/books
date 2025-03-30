import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-autores',
  imports: [NgFor, NgIf, ReactiveFormsModule],
  templateUrl: './autores.component.html',
  styleUrls: ['./autores.component.scss'],
})
export class AutoresComponent implements OnInit {
  autores: any[] = [];
  autorForm: FormGroup;
  isEditing = false;
  currentId!: number;

  constructor(private fb: FormBuilder, private api: ApiService) {
    this.autorForm = this.fb.group({
      nome: ['', [Validators.required, Validators.maxLength(40)]],
    });
  }

  ngOnInit(): void {
    this.loadAutores();
  }

  loadAutores(): void {
    this.api.getAutores().subscribe({
      next: (data) => (this.autores = data),
      error: (err) => console.error('Erro ao carregar autores:', err),
    });
  }

  onSubmit(): void {
    if (this.autorForm.invalid) return;

    const autorData = this.autorForm.value;

    if (this.isEditing) {
      this.api.updateAutor(this.currentId, autorData).subscribe({
        next: () => {
          this.loadAutores();
          this.resetForm();
        },
        error: (err) => console.error('Erro ao atualizar autor:', err),
      });
    } else {
      this.api.createAutor(autorData).subscribe({
        next: () => {
          this.loadAutores();
          this.resetForm();
        },
        error: (err) => console.error('Erro ao criar autor:', err),
      });
    }
  }

  onEdit(autor: any): void {
    this.isEditing = true;
    this.currentId = autor.codAu;
    this.autorForm.patchValue(autor);
  }

  onDelete(id: number): void {
    if (confirm('Tem certeza que deseja excluir este autor?')) {
      this.api.deleteAutor(id).subscribe({
        next: () => this.loadAutores(),
        error: (err) => console.error('Erro ao excluir autor:', err),
      });
    }
  }

  resetForm(): void {
    this.autorForm.reset();
    this.isEditing = false;
    this.currentId = 0;
  }
}

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
  selector: 'app-assuntos',
  imports: [NgFor, NgIf, ReactiveFormsModule],
  templateUrl: './assuntos.component.html',
  styleUrls: ['./assuntos.component.scss'],
})
export class AssuntosComponent implements OnInit {
  assuntos: any[] = [];
  assuntoForm: FormGroup;
  isEditing = false;
  currentId!: number;

  constructor(private fb: FormBuilder, private api: ApiService) {
    this.assuntoForm = this.fb.group({
      descricao: ['', [Validators.required, Validators.maxLength(20)]],
    });
  }

  ngOnInit(): void {
    this.loadAssuntos();
  }

  loadAssuntos(): void {
    this.api.getAssuntos().subscribe({
      next: (data) => (this.assuntos = data),
      error: (err) => console.error('Erro ao carregar assuntos:', err),
    });
  }

  onSubmit(): void {
    if (this.assuntoForm.invalid) return;

    const formValue = this.assuntoForm.value;

    const operation = this.isEditing
      ? this.api.updateAssunto(this.currentId, formValue)
      : this.api.createAssunto(formValue);

    operation.subscribe({
      next: () => {
        this.loadAssuntos();
        this.resetForm();
      },
      error: (err) =>
        console.error(
          `Erro ao ${this.isEditing ? 'atualizar' : 'criar'} assunto:`,
          err
        ),
    });
  }

  onEdit(assunto: any): void {
    this.isEditing = true;
    this.currentId = assunto.codAs;
    this.assuntoForm.patchValue(assunto);
  }

  onDelete(id: number): void {
    if (confirm('Tem certeza que deseja excluir este assunto?')) {
      this.api.deleteAssunto(id).subscribe({
        next: () => this.loadAssuntos(),
        error: (err) => console.error('Erro ao excluir assunto:', err),
      });
    }
  }

  resetForm(): void {
    this.assuntoForm.reset();
    this.isEditing = false;
    this.currentId = 0;
  }
}

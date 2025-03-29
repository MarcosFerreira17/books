import { Component } from '@angular/core';
import { TableComponent } from '../../components/table/table.component';
import { FormComponent } from '../../components/form/form.component';

@Component({
  selector: 'app-books',
  imports: [TableComponent, FormComponent],
  templateUrl: './books.component.html',
  styleUrl: './books.component.scss',
})
export class BooksComponent {
  columns = [
    { key: 'id', label: 'ID' },
    { key: 'name', label: 'Nome' },
    { key: 'email', label: 'E-mail' },
  ];

  data = [
    { id: 1, name: 'Marcos', email: 'marcos@email.com' },
    { id: 2, name: 'Ferreira', email: 'ferreira@email.com' },
    { id: 3, name: 'João', email: 'joao@email.com' },
    { id: 4, name: 'Ana', email: 'ana@email.com' },
    { id: 5, name: 'Carlos', email: 'carlos@email.com' },
    { id: 6, name: 'Paula', email: 'paula@email.com' },
  ];
}

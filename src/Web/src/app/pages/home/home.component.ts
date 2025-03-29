import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  imports: [],
})
export class HomeComponent {
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

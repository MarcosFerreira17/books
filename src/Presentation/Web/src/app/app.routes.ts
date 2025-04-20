import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { AutoresComponent } from './pages/autores/autores.component';
import { LivrosComponent } from './pages/livros/livros.component';
import { AssuntosComponent } from './pages/assuntos/assuntos.component';

export const routes: Routes = [
  { path: 'authors', component: AutoresComponent },
  { path: 'books', component: LivrosComponent },
  { path: 'subjects', component: AssuntosComponent },
  { path: '', component: HomeComponent },
];

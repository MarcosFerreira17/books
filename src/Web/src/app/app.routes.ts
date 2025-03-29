import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { AuthorsComponent } from './pages/authors/authors.component';
import { BooksComponent } from './pages/books/books.component';

export const routes: Routes = [
  { path: 'authors', component: AuthorsComponent },
  { path: 'books', component: BooksComponent },
  { path: '', component: HomeComponent },
];

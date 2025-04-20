import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastService } from '../components/toast/toast.service';

export interface ProblemDetails {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  errors?: { [key: string]: string[] };
  instance?: string;
}

export type BackendError = string[] | ProblemDetails | any;

@Injectable({ providedIn: 'root' })
export class ErrorHandlerService {
  constructor(private toast: ToastService) {}

  handleError(error: HttpErrorResponse | any, context?: string): void {
    const messages = this.extractErrorMessages(error);
    this.showToast(messages, context);
  }

  extractErrorMessages(error: HttpErrorResponse | BackendError): string[] {
    let messages: string[] = [];

    // Caso 1: Erro HTTP padrão do Angular
    if (error instanceof HttpErrorResponse) {
      messages = this.handleHttpError(error);
    }
    // Caso 2: Erro direto do backend
    else {
      messages = this.handleBackendError(error);
    }

    return messages.length > 0
      ? messages
      : ['Erro desconhecido ao processar a requisição'];
  }

  private showToast(messages: string[], context?: string): void {
    const title = context ? `Erro em ${context}` : 'Erro!';
    messages.forEach((msg) => {
      this.toast.showError(msg, title);
    });
  }

  private handleHttpError(error: HttpErrorResponse): string[] {
    const serverError = error.error as BackendError;
    return this.handleBackendError(serverError);
  }

  private handleBackendError(error: BackendError): string[] {
    if (Array.isArray(error)) {
      return error;
    }

    if (this.isProblemDetails(error)) {
      return this.extractProblemDetailsMessages(error);
    }

    if (typeof error === 'string') {
      return [error];
    }

    return ['Ocorreu um erro inesperado'];
  }

  private isProblemDetails(error: any): error is ProblemDetails {
    return error && (error.detail || error.title || error.errors);
  }

  private extractProblemDetailsMessages(error: ProblemDetails): string[] {
    const messages: string[] = [];

    if (error.detail) {
      messages.push(error.detail);
    }

    if (error.errors) {
      Object.values(error.errors).forEach((errors) => {
        messages.push(...errors);
      });
    }

    if (messages.length === 0 && error.title) {
      messages.push(error.title);
    }

    return messages;
  }
}

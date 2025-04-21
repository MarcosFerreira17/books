import { Injectable } from '@angular/core';

export interface Toast {
  type: 'success' | 'danger' | 'warning' | 'info';
  message: string;
  title?: string;
  delay?: number;
}

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  toasts: Toast[] = [];

  show(toast: Toast): void {
    toast.delay = toast.delay || 5000;
    this.toasts.push(toast);
    setTimeout(() => this.remove(toast), toast.delay);
  }

  remove(toast: Toast): void {
    this.toasts = this.toasts.filter((t) => t !== toast);
  }

  // Métodos rápidos
  showSuccess(message: string, title?: string): void {
    this.show({ type: 'success', message, title });
  }

  showError(message: string, title?: string): void {
    this.show({ type: 'danger', message, title });
  }

  showWarning(message: string, title?: string): void {
    this.show({ type: 'warning', message, title });
  }

  showInfo(message: string, title?: string): void {
    this.show({ type: 'info', message, title });
  }
}

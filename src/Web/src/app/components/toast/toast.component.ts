import { Component, HostListener } from '@angular/core';
import { ToastService } from './toast.service';
import { NgClass, NgFor } from '@angular/common';

@Component({
  selector: 'app-toast',
  imports: [NgFor, NgClass],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.scss',
})
export class ToastComponent {
  constructor(public toastService: ToastService) {}

  @HostListener('window:resize')
  onWindowResize() {
    this.repositionToasts();
  }

  private repositionToasts() {
    const sidebarWidth = document.querySelector('.col-2')?.clientWidth || 0;
    const toastContainer = document.querySelector(
      '.toast-container'
    ) as HTMLElement;

    if (toastContainer) {
      toastContainer.style.right = `${sidebarWidth + 20}px`;
    }
  }
}

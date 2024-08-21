import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class SnackbarService {
  private snackbar = inject(MatSnackBar);

  error(mesage: string) {
    this.snackbar.open(mesage, 'Close', {
      duration: 5000,
      panelClass: ['snack-error'],
    });
  }
  success(mesage: string) {
    this.snackbar.open(mesage, 'Close', {
      duration: 5000,
      panelClass: ['snack-success'],
    });
  }
}

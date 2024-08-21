import { F } from '@angular/cdk/keycodes';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  loading = false;
  busyRewuestCount = 0;

  busy() {
    this.busyRewuestCount++;
    this.loading = true;
  }

  idle() {
    this.busyRewuestCount--;

    if (this.busyRewuestCount <= 0) {
      this.busyRewuestCount = 0;
      this.loading = false;
    }
  }
}

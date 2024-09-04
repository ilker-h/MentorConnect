import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, delay, finalize, identity } from 'rxjs';
import { BusyLoadingService } from '../_services/busy-loading.service';
import { environment } from '../../environments/environment';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyLoadingService: BusyLoadingService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // this.busyLoadingService.busy();

    return next.handle(request).pipe(
      // (environment.production ? identity : delay(1000)), // enable this is you'd like to see the loading icon
      finalize(() => {
        // this.busyLoadingService.idle();
      })
    );
  }
}

import { Injectable, OnDestroy } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Event } from '../model/event';
import { Observable } from 'rxjs';

@Injectable()
export class EventService {
  private readonly eventsUrl = '/api/events';
  private readonly globalEventsUrl = '/api/events/global';

  constructor(private http: HttpClient) {
  }

  save(event: any): Observable<Event> {
    return this.http.post<Event>(`${this.eventsUrl}`, event)
      .pipe(map(response => new Event(response)));
  }

  update(event: any, id: string): Observable<Event> {
    return this.http.put<Event>(`${this.eventsUrl}/${id}`, event);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.eventsUrl}/${id}`);
  }

  getOneById(id: string): Observable<Event> {
    return this.http.get(`${this.eventsUrl}/${id}`)
      .pipe(map(res => new Event(res)));
  }

  getOneGlobalById(id: string): Observable<Event> {
    return this.http.get(`${this.globalEventsUrl}/${id}`)
      .pipe(map(res => new Event(res)));
  }

  getAllGlobal(date: string): Observable<Event[]> {
    return this.http.get<Event[]>(this.globalEventsUrl, {params: new HttpParams().append('date', date)})
      .pipe(
        map((res: Event[]) => res.map(event => new Event(event)))
      );
  }

  getAllPersonal(date: string): Observable<Event[]> {
    return this.http.get<Event[]>(`${this.eventsUrl}`, {params: new HttpParams().append('date', date)})
      .pipe(
        map((res: Event[]) => res.map(event => new Event(event)))
      );
  }
}

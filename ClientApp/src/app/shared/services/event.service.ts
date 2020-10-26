import { Injectable, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
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

  getOneById(id: string): Observable<Event> {
    return this.http.get(`${this.eventsUrl}/${id}`)
      .pipe(map(res => new Event(res)));
  }

  getOneGlobalById(id: string): Observable<Event> {
    return this.http.get(`${this.globalEventsUrl}/${id}`)
      .pipe(map(res => new Event(res)));
  }

  getAllGlobal(): Observable<Event[]> {
    return this.http.get<Event[]>(this.globalEventsUrl)
      .pipe(
        map((res: Event[]) => res.map(event => new Event(event)))
      );
  }

  getAll(): Observable<Event[]> {
    return this.http.get<Event[]>(`${this.eventsUrl}`)
      .pipe(
        map((res: Event[]) => res.map(event => new Event(event)))
      );
  }
}

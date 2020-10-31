import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventNotificationsComponent } from './event-notifications.component';

describe('EventDetailsComponent', () => {
  let component: EventNotificationsComponent;
  let fixture: ComponentFixture<EventNotificationsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [EventNotificationsComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventNotificationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

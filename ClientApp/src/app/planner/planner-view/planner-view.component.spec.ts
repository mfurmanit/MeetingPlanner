import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlannerViewComponent } from './planner-view.component';

describe('CalendarViewComponent', () => {
  let component: PlannerViewComponent;
  let fixture: ComponentFixture<PlannerViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlannerViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlannerViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

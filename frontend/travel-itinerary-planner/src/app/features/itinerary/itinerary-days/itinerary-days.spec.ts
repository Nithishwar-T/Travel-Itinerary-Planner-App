import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItineraryDays } from './itinerary-days';

describe('ItineraryDays', () => {
  let component: ItineraryDays;
  let fixture: ComponentFixture<ItineraryDays>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ItineraryDays],
    }).compileComponents();

    fixture = TestBed.createComponent(ItineraryDays);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { ItineraryDayService } from './itinerary-day.service';

describe('ItineraryDayService', () => {

  let service: ItineraryDayService;

  beforeEach(() => {

    TestBed.configureTestingModule({});

    service = TestBed.inject(ItineraryDayService);

  });

  it('should be created', () => {

    expect(service).toBeTruthy();

  });

});
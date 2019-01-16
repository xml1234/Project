import { Component, EventEmitter, Injector, Input, OnInit, Output } from '@angular/core';
import { DefaultTimezoneScope, NameValueDto, TimingServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/component-base';

@Component({
    selector: 'timezone-combo',
    template:
    `<nz-select [(ngModel)]="selectedTimeZone" (ngModelChange)="selectedTimeZoneChange.emit($event)">
        <nz-option *ngFor="let timeZone of timeZones" [nzLabel]="timeZone.name" [nzValue]="timeZone.value"></nz-option>
    </nz-select>`
})
export class TimeZoneComboComponent extends AppComponentBase implements OnInit {

    @Output() selectedTimeZoneChange: EventEmitter<string> = new EventEmitter<string>();

    timeZones: NameValueDto[] = [];

    @Input() selectedTimeZone: string = undefined;
    @Input() defaultTimezoneScope: DefaultTimezoneScope;

    constructor(
        private _timingService: TimingServiceProxy,
        injector: Injector) {
        super(injector);
    }

    ngOnInit(): void {
        let self = this;
        self._timingService.getTimezones(self.defaultTimezoneScope).subscribe(result => {
            self.timeZones = result.items;
        });
    }
}

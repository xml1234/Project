import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from '@app/app-routing.module';

import { SharedModule } from '@shared/shared.module';
import { HttpClientModule } from '@angular/common/http';

import { AbpModule } from '@abp/abp.module';
import { LayoutModule } from '@layout/layout.module';
import { ImpersonationService } from '@shared/auth';


@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    AppRoutingModule,
    LayoutModule,
    SharedModule,
    AbpModule,
  ],
  declarations: [
    
  ],
  entryComponents: [

  ],
  providers: [
    ImpersonationService
  ],
})
export class AppModule {}

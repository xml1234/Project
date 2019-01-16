import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

import { HeaderComponent } from './default/header/header.component';
import { SidebarComponent } from './default/sidebar/sidebar.component';
import { HeaderFullScreenComponent } from './default/header/components/fullscreen.component';
import { HeaderI18nComponent } from './default/header/components/i18n.component';
import { HeaderStorageComponent } from './default/header/components/storage.component';
import { HeaderUserComponent } from './default/header/components/user.component';
import { ChangePasswordModalComponent } from './default/profile/change-password-modal.component';
import { LoginAttemptsModalComponent } from './default/profile/login-attempts-modal.component';
import { MySettingsModalComponent } from './default/profile/my-settings-modal.component';
import { LayoutDefaultComponent } from './default/layout-default.component';
import { YoYoSidebarNavComponent } from './default/sidebar/components/yoyo-sidebar-nav.component';

const COMPONENTS = [
  HeaderComponent,
  SidebarComponent,
  LayoutDefaultComponent
];

const HEADERCOMPONENTS = [
  HeaderFullScreenComponent,
  HeaderI18nComponent,
  HeaderStorageComponent,
  HeaderUserComponent,
];

// passport
const ABPCOMPONENTS = [
  ChangePasswordModalComponent,
  LoginAttemptsModalComponent,
  MySettingsModalComponent,
];
//
const SIDEBARCOMPONENTS = [
  YoYoSidebarNavComponent
]

@NgModule({
  imports: [SharedModule],
  declarations: [
    ...COMPONENTS,
    ...HEADERCOMPONENTS,
    ...ABPCOMPONENTS,
    ...SIDEBARCOMPONENTS,
  ],
  entryComponents: [
    ...ABPCOMPONENTS
  ],
  exports: [
    ...COMPONENTS
  ],
})
export class LayoutModule { }

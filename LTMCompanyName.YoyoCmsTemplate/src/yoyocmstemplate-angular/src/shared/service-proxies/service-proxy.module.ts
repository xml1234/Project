import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from '@abp/abpHttpInterceptor';
import { YoYoHttpInterceptor } from './YoYoHttpInterceptor';

import * as ApiServiceProxies from '@shared/service-proxies/service-proxies';

@NgModule({
  providers: [
    ApiServiceProxies.AccountServiceProxy,
    ApiServiceProxies.AuditLogServiceProxy,
    ApiServiceProxies.LanguageServiceProxy,
    ApiServiceProxies.NotificationServiceProxy,
    ApiServiceProxies.OrganizationUnitServiceProxy,
    ApiServiceProxies.PermissionServiceProxy,
    ApiServiceProxies.ProfileServiceProxy,
    ApiServiceProxies.RoleServiceProxy,
    ApiServiceProxies.SessionServiceProxy,
    ApiServiceProxies.TenantServiceProxy,
    ApiServiceProxies.TokenAuthServiceProxy,
    ApiServiceProxies.UserServiceProxy,
    ApiServiceProxies.UserLoginServiceProxy,
    ApiServiceProxies.HostSettingsServiceProxy,
    ApiServiceProxies.HostCachingServiceProxy,
    ApiServiceProxies.WebSiteLogServiceProxy,
    ApiServiceProxies.TenantSettingsServiceProxy,
    ApiServiceProxies.TenantRegistrationServiceProxy,
    ApiServiceProxies.EditionServiceProxy,
    ApiServiceProxies.TimingServiceProxy,
    ApiServiceProxies.CommonLookupServiceProxy,
    { provide: HTTP_INTERCEPTORS, useClass: YoYoHttpInterceptor, multi: true },
  ],
})
export class ServiceProxyModule { }

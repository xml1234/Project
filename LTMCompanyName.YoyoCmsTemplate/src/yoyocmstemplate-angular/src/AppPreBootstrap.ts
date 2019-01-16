import * as moment from 'moment';

import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { Type, CompilerOptions, NgModuleRef, Injector } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';

import { environment } from '@env/environment';
import { HttpClient } from '@angular/common/http';
import * as _ from 'lodash';
import { PermissionService } from '@shared/auth/permission.service';
import { ALAIN_I18N_TOKEN, MenuService } from '@delon/theme';
import { LocalizationService } from '@shared/i18n/localization.service';
import { AppMenus } from '@shared/AppMenus';
import { UtilsService } from 'abp-ng2-module/dist/src/utils/utils.service';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { AppSessionService } from '@shared/session/app-session.service';
import { TokenAuthServiceProxy, API_BASE_URL } from '@shared/service-proxies/service-proxies';
import { TokenService } from 'abp-ng2-module/dist/src/auth/token.service';

export class AppPreBootstrap {

  static run(injector: Injector, callback: () => void): void {
    console.log("由52ABP模板构建,详情请访问 https://www.52abp.com");

    let httpClient = injector.get(HttpClient);

    AppPreBootstrap.getApplicationConfig(injector, httpClient, () => {

      const queryStringObj = UrlHelper.getQueryParameters();

      if (queryStringObj.impersonationToken) {// 模拟登陆
        abp.multiTenancy.setTenantIdCookie(queryStringObj.tenantId);
        AppPreBootstrap.impersonatedAuthenticate(httpClient, queryStringObj.impersonationToken, () => {
          AppPreBootstrap.getUserConfiguration(injector, httpClient, callback);
        })

      } else if (queryStringObj.switchAccountToken) {// 切换关联账号
        abp.multiTenancy.setTenantIdCookie(queryStringObj.tenantId);
        AppPreBootstrap.linkedAccountAuthenticate(httpClient, queryStringObj.switchAccountToken, () => {
          AppPreBootstrap.getUserConfiguration(injector, httpClient, callback);
        })

      } else {// 普通登陆直接获取信息
        AppPreBootstrap.getUserConfiguration(injector, httpClient, callback);
      }
    });
  }






  /**
   * 初始化前端配置
   * @param httpClient 
   * @param callback 
   */
  private static getApplicationConfig(injector: Injector, httpClient: HttpClient, callback: () => void) {
    let envName = '';
    if (environment.production) {
      envName = 'prod';
    } else {
      envName = 'dev';
    }
    let url = '/assets/appconfig.' + envName + '.json';
    httpClient.get(url).subscribe((result: any) => {
      AppConsts.appBaseUrl = window.location.protocol + '//' + window.location.host;
      AppConsts.remoteServiceBaseUrl = result.remoteServiceBaseUrl;
      AppConsts.localeMappings = result.localeMappings;
      AppConsts.ngZorroLocaleMappings = result.ngZorroLocaleMappings;
      AppConsts.ngAlainLocaleMappings = result.ngAlainLocaleMappings;

      callback();
    }, (error) => {
      alert(`初始化配置信息出错,错误信息:\n\n${error.message}`);
    })
  }


  /**
   * 获取后端配置
   * @param injector ioc容器
   * @param httpClient  
   * @param callback 回调函数
   */
  private static getUserConfiguration(injector: Injector, httpClient: HttpClient, callback: () => void) {
    httpClient.get(`${AppConsts.remoteServiceBaseUrl}/api/services/app/Session/GetUserConfigurations`)
      .subscribe((response: any) => {
        let result = response.result;

        // 填充数据
        _.merge(abp, result);


        // 时区
        abp.clock.provider = this.getCurrentClockProvider(result.clock.provider);
        moment.locale(abp.localization.currentLanguage.name);

        (window as any).moment.locale(abp.localization.currentLanguage.name);
        if (abp.clock.provider.supportsMultipleTimezone) {
          moment.tz.setDefault(abp.timing.timeZoneInfo.iana.timeZoneId);
          (window as any).moment.tz.setDefault(abp.timing.timeZoneInfo.iana.timeZoneId);
        }


        // 权限
        const permissionService = injector.get(PermissionService);
        permissionService.extend(abp.auth);


        // 本地化
        const localization = injector.get<LocalizationService>(ALAIN_I18N_TOKEN);
        localization.extend(abp.localization);


        // 写入菜单
        const menuService = injector.get(MenuService);
        menuService.add(AppMenus.Menus);


        callback();
      }, (error) => {
        alert(`初始化用户信息出错,错误信息:\n\n${error.message}`);
      });
  }


  /**
 * 模拟登陆用户
 * @param httpClient 
 * @param impersonationToken 
 * @param callback 
 */
  private static impersonatedAuthenticate(httpClient: HttpClient, impersonationToken: string, callback: () => void) {
    httpClient.post(`${AppConsts.remoteServiceBaseUrl}/api/TokenAuth/ImpersonatedAuthenticate?impersonationToken=${impersonationToken}`, null)
      .subscribe((data: any) => {
        let result = data.result;

        abp.auth.setToken(result.accessToken);
        AppPreBootstrap.setEncryptedTokenCookie(result.encryptedAccessToken);
        location.search = '';
        callback();
      }, (error) => {
        alert(`模拟登陆出错,错误信息:\n\n${error.message}`);
      });
  }

  /**
   * 切换关联用户
   * @param httpClient 
   * @param switchAccountToken 
   * @param callback 
   */
  private static linkedAccountAuthenticate(httpClient: HttpClient, switchAccountToken: string, callback: () => void): void {
    httpClient.post(`${AppConsts.remoteServiceBaseUrl}/api/TokenAuth/LinkedAccountAuthenticate?switchAccountToken=${switchAccountToken}`, null).subscribe((data: any) => {
      let result = data.result;
      abp.auth.setToken(result.accessToken);
      AppPreBootstrap.setEncryptedTokenCookie(result.encryptedAccessToken);
      location.search = '';
      callback();
    }, (error) => {
      alert(`切换关联用户出错,错误信息:\n\n${error.message}`);
    });
  }



  private static getCurrentClockProvider(currentProviderName: string): abp.timing.IClockProvider {

    if (currentProviderName === 'unspecifiedClockProvider') {
      return abp.timing.unspecifiedClockProvider;
    }

    if (currentProviderName === 'utcClockProvider') {
      return abp.timing.utcClockProvider;
    }

    return abp.timing.localClockProvider;
  }

  private static setEncryptedTokenCookie(encryptedToken: string) {
    abp.utils.setCookieValue(AppConsts.authorization.encrptedAuthTokenName,
      encryptedToken,
      new Date(new Date().getTime() + 365 * 86400000), //1 year
      abp.appPath,
    )
  }

}

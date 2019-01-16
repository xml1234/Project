import { state } from '@angular/animations';
import {
  NzModalService,
  NzNotificationService,
  NzNotificationDataOptions,
  NzMessageService,
} from 'ng-zorro-antd';
import { parse } from 'date-fns';

export class AppConsts {
  static remoteServiceBaseUrl: string;

  static appBaseUrl: string;
  static uploadApiUrl = '/api/File/Upload';
  static maxProfilPictureMb = 1; // 个人头像上传最大MB
  static localeMappings: any;// 后端本地化和前端angular本地化映射
  static ngZorroLocaleMappings: any;// 后端本地化和ng-zorro本地化映射
  static ngAlainLocaleMappings: any;// 后端本地化和ng-alian本地化映射

  static readonly userManagement = {
    defaultAdminUserName: 'admin',
  };

  static readonly localization = {
    defaultLocalizationSourceName: 'YoyoCmsTemplate',
  };

  static readonly authorization = {
    encrptedAuthTokenName: 'enc_auth_token',
  }

  /**
   * 数据表格设置
   */
  // tslint:disable-next-line:member-ordering
  static readonly grid = {
    /**
     * 每页显示条目数
     */
    defaultPageSize: 10,
    /**
     * 每页显示条目数下拉框值
     */
    defaultPageSizes: [5, 10, 15, 20, 25, 30, 50, 80, 100],
  };
}

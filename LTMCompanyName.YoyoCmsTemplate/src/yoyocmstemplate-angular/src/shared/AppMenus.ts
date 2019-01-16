import { AppConsts } from '@shared/AppConsts';
import { Menu } from '@delon/theme';

/**
 * 全局的左侧边栏的菜单导航配置信息
 */
export class AppMenus {
  static Menus: Menu[] = [
    {// 工作台
      text: '',
      i18n: 'Dashboard',
      acl: undefined,
      icon: 'anticon anticon-dashboard',
      link: '/app/main/dashboard',
    },
    {// 租户
      text: '',
      i18n: 'Tenants',
      acl: 'Pages.Tenants',
      icon: 'anticon anticon-dashboard',
      link: '/app/admin/tenants',
    },
    {// 版本
      text: '',
      i18n: 'Editions',
      acl: 'Pages.Editions.Query',
      icon: 'anticon anticon-dashboard',
      link: '/app/admin/editions',
    },
    {// 管理
      text: '',
      i18n: 'Administration',
      acl: 'Pages',
      icon: 'anticon anticon-appstore',
      children: [
        {// 组织机构
          text: '',
          i18n: 'OrganizationUnits',
          acl: 'Pages.Administration.OrganizationUnits',
          icon: 'anticon anticon-team',
          link: '/app/admin/organization-units',
        },
        {// 角色
          text: '',
          i18n: 'Roles',
          acl: 'Pages.Administration.Roles',
          icon: 'anticon anticon-safety',
          link: '/app/admin/roles',
        },
        {// 用户
          text: '',
          i18n: 'Users',
          acl: 'Pages.Administration.Users',
          icon: 'anticon anticon-user',
          link: '/app/admin/users',
        },
        {// 语言
          text: '',
          i18n: 'Languages',
          acl: 'Pages.Administration.Languages',
          icon: 'anticon anticon-global',
          link: '/app/admin/languages',
        },
        {// 审计日志
          text: '',
          i18n: 'AuditLogs',
          acl: 'Pages.Administration.AuditLogs',
          icon: 'anticon anticon-book',
          link: '/app/admin/auditLogs',
        },
        {// 宿主机器设置/维护
          text: '',
          i18n: 'Maintenance',
          acl: 'Pages.Administration.Host.Maintenance',
          icon: 'anticon anticon-setting',
          link: '/app/admin/maintenance',
        },
        {// 租户设置
          text: '',
          i18n: 'Settings',
          acl: 'Pages.Administration.Tenant.Settings',
          icon: 'anticon anticon-setting',
          link: '/app/admin/tenant-settings',
        },

        {// 宿主设置
          text: '',
          i18n: 'Settings',
          acl: 'Pages.Administration.Host.Settings',
          icon: 'anticon anticon-setting',
          link: '/app/admin/host-settings',
        },
      ]
    },
    {// 关于我们
      text: '',
      i18n: 'About',
      icon: 'anticon anticon-info-circle',
      link: '/app/main/about',
    },
  ];
}

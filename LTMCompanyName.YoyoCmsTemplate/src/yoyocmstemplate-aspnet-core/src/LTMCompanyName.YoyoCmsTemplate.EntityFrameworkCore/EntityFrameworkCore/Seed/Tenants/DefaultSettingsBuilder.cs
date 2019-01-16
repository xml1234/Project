using System;
using System.Linq;
using Abp.Zero.Configuration;

namespace LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore.Seed.Tenants
{
    public class DefaultSettingsBuilder
    {
        private readonly YoyoCmsTemplateDbContext _context;
        private readonly int _tenantId;

        public DefaultSettingsBuilder(YoyoCmsTemplateDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {

            DefaultSetings();
            //CreateMemberRoles();
        }


        private void DefaultSetings()
        {
            var setting = _context.Settings.Where(o => o.Name == AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin && o.TenantId == this._tenantId).FirstOrDefault();
            if (setting == null)
            {
                _context.Settings.Add(new Abp.Configuration.Setting(_tenantId, null, AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin, "true"));
            }
            else if (!Convert.ToBoolean(setting.Value))
            {
                setting.Value = "true";
                _context.Settings.Update(setting);
            }
            _context.SaveChanges();

        }
    }
}

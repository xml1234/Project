using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.Users
{
    public class UserCfg : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", YoYoAbpefCoreConsts.SchemaNames.ABP);

            builder.Property(a => a.EmailAddress).IsRequired(false);
        }
    }
}
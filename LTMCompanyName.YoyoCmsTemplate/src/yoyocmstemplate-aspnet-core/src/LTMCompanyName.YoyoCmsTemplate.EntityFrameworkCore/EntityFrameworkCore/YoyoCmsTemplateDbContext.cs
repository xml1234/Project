using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.EntityMapper;
using LTMCompanyName.YoyoCmsTemplate.EntityMapper.Users;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.PhoneBooks.Books;
using LTMCompanyName.YoyoCmsTemplate.PhoneBooks.Persons;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.Editions;
using Abp.IdentityServer4;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Payments;
using LTMCompanyName.YoyoCmsTemplate.EntityMapper.SubscriptionPayments;

namespace LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore
{
    public class YoyoCmsTemplateDbContext : AbpZeroDbContext<Tenant, Role, User, YoyoCmsTemplateDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define a DbSet for each entity of the application */

        public DbSet<DataFileObject> DataFileObjects { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<Book> Books { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public YoyoCmsTemplateDbContext(DbContextOptions<YoyoCmsTemplateDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //修改ABP的默认表信息
            modelBuilder.ChangeAbpTablePrefix<Tenant, Role, User>(string.Empty, YoYoAbpefCoreConsts.SchemaNames.ABP);
            base.OnModelCreating(modelBuilder);

            // ============  其它对表信息的修改 ============= //

            // 用户表
            modelBuilder.ApplyConfiguration(new UserCfg());
            // 订阅支付
            modelBuilder.ApplyConfiguration(new SubscriptionPaymentCfg());


            modelBuilder.Entity<DataFileObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });


            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { e.PaymentId, e.Gateway });
            });

            // identityServer4
            modelBuilder.ConfigurePersistedGrantEntity(string.Empty, YoYoAbpefCoreConsts.SchemaNames.ABP);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Threading;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Timing;
using LTMCompanyName.YoyoCmsTemplate.Editions;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy
{
    public class SubscriptionExpirationCheckWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private const int CheckPeriodAsMilliseconds = 1 * 60 * 60 * 1000; //1 hour

        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<SubscribableEdition> _editionRepository;
        private readonly TenantManager _tenantManager;


        public SubscriptionExpirationCheckWorker(
            AbpTimer timer,
            IRepository<Tenant> tenantRepository,
            IRepository<SubscribableEdition> editionRepository,
            TenantManager tenantManager)
            : base(timer)
        {
            _tenantRepository = tenantRepository;
            _editionRepository = editionRepository;
            _tenantManager = tenantManager;


            Timer.Period = CheckPeriodAsMilliseconds;
            Timer.RunOnStart = true;

            LocalizationSourceName = YoyoCmsTemplateConsts.LocalizationSourceName;
        }

        protected override void DoWork()
        {
            var utcNow = Clock.Now.ToUniversalTime();
            var failedTenancyNames = new List<string>();

            var subscriptionExpiredTenants = _tenantRepository.GetAllList(
                tenant => tenant.SubscriptionEndUtc != null &&
                          tenant.SubscriptionEndUtc <= utcNow &&
                          tenant.IsActive &&
                          tenant.EditionId != null
            );

            foreach (var tenant in subscriptionExpiredTenants)
            {
                Debug.Assert(tenant.EditionId.HasValue);

                try
                {

                    var edition = _editionRepository.Get(tenant.EditionId.Value);

                    Debug.Assert(tenant.SubscriptionEndUtc != null, "tenant.SubscriptionEndDateUtc != null");

                    if (tenant.SubscriptionEndUtc.Value.AddDays(edition.WaitingDayAfterExpire ?? 0) >= utcNow)
                    {
                        //租户在过期后等待了几天:最好在从存储库查询时过滤这些实体!
                        continue;
                    }

                    var endSubscriptionResult = AsyncHelper.RunSync(() => _tenantManager.EndSubscriptionAsync(tenant, edition, utcNow));

                    // TODO: 发送邮件
                }
                catch (Exception exception)
                {
                    failedTenancyNames.Add(tenant.TenancyName);
                    Logger.Error($"Subscription of tenant {tenant.TenancyName} has been expired but tenant couldn't be made passive !");
                    Logger.Error(exception.Message, exception);
                }
            }

            if (!failedTenancyNames.Any())
            {
                return;
            }

        }
    }
}


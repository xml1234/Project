using System.Threading.Tasks;
using Abp.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore.Repositories;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Payments
{
    public class SubscriptionPaymentRepository : YoyoCmsTemplateRepositoryBase<SubscriptionPayment, long>, ISubscriptionPaymentRepository
    {
        public SubscriptionPaymentRepository(IDbContextProvider<YoyoCmsTemplateDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<SubscriptionPayment> UpdateByGatewayAndPaymentIdAsync(SubscriptionPaymentGatewayType gateway, string paymentId, int? tenantId, SubscriptionPaymentStatus status)
        {
            var payment = await SingleAsync(p => p.PaymentId == paymentId && p.Gateway == gateway);

            payment.Status = status;

            if (tenantId.HasValue)
            {
                payment.TenantId = tenantId.Value;
            }

            return payment;
        }

        public async Task<SubscriptionPayment> GetByGatewayAndPaymentIdAsync(SubscriptionPaymentGatewayType gateway, string paymentId)
        {
            return await SingleAsync(p => p.PaymentId == paymentId && p.Gateway == gateway);
        }
    }
}

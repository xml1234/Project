using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.EntityMapper.SubscriptionPayments
{
    public class SubscriptionPaymentCfg : IEntityTypeConfiguration<SubscriptionPayment>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPayment> builder)
        {
            builder.ToTable("SubscriptionPayments", YoYoAbpefCoreConsts.SchemaNames.ABP);
        }
    }
}

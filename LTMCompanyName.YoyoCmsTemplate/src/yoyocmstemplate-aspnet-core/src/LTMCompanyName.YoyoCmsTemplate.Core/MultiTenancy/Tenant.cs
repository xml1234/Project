using System;
using System.ComponentModel.DataAnnotations;
using Abp.MultiTenancy;
using Abp.Timing;
using LTMCompanyName.YoyoCmsTemplate.Editions;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Payments;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public const int MaxLogoMimeTypeLength = 64;


        /// <summary>
        /// 订阅结束时间
        /// </summary>
        public DateTime? SubscriptionEndUtc { get; set; }

        /// <summary>
        /// 是否试用
        /// </summary>
        public bool IsInTrialPeriod { get; set; }

        /// <summary>
        /// 自定义cssId
        /// </summary>
        public virtual Guid? CustomCssId { get; set; }

        /// <summary>
        /// 自定义LogoId
        /// </summary>
        public virtual Guid? LogoId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(MaxLogoMimeTypeLength)]
        public virtual string LogoFileType { get; set; }

        public Tenant()
        {
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {

        }


        public virtual bool HasLogo()
        {
            return LogoId != null && LogoFileType != null;
        }

        public void ClearLogo()
        {
            LogoId = null;
            LogoFileType = null;
        }

        /// <summary>
        /// 更新订阅付款日期
        /// </summary>
        /// <param name="paymentPeriodType"></param>
        /// <param name="editionPaymentType"></param>
        public void UpdateSubscriptionDateForPayment(PaymentPeriodType paymentPeriodType, EditionPaymentType editionPaymentType)
        {
            switch (editionPaymentType)
            {
                case EditionPaymentType.NewRegistration:
                case EditionPaymentType.BuyNow:
                    {
                        SubscriptionEndUtc = Clock.Now.ToUniversalTime().AddDays((int)paymentPeriodType);
                        break;
                    }
                case EditionPaymentType.Extend:
                    ExtendSubscriptionDate(paymentPeriodType);
                    break;
                case EditionPaymentType.Upgrade:
                    if (HasUnlimitedTimeSubscription())
                    {
                        SubscriptionEndUtc = Clock.Now.ToUniversalTime().AddDays((int)paymentPeriodType);
                    }
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// 延长订阅时间
        /// </summary>
        /// <param name="paymentPeriodType"></param>
        private void ExtendSubscriptionDate(PaymentPeriodType paymentPeriodType)
        {
            if (SubscriptionEndUtc == null)
            {
                throw new InvalidOperationException("Can not extend subscription date while it's null!");
            }

            if (IsSubscriptionEnded())
            {
                SubscriptionEndUtc = Clock.Now.ToUniversalTime();
            }

            SubscriptionEndUtc = SubscriptionEndUtc.Value.AddDays((int)paymentPeriodType);
        }

        /// <summary>
        /// 是否订阅结束
        /// </summary>
        /// <returns></returns>
        private bool IsSubscriptionEnded()
        {
            return SubscriptionEndUtc < Clock.Now.ToUniversalTime();
        }

        /// <summary>
        /// 剩余天数
        /// </summary>
        /// <returns></returns>
        public int CalculateRemainingDayCount()
        {
            return SubscriptionEndUtc != null ? (SubscriptionEndUtc.Value - Clock.Now.ToUniversalTime()).Days : 0;
        }

        /// <summary>
        /// 是否无限期订阅
        /// </summary>
        /// <returns></returns>
        public bool HasUnlimitedTimeSubscription()
        {
            return SubscriptionEndUtc == null;
        }
    }
}

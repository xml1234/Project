using Newtonsoft.Json;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Payments.Paypal
{
    public class Payer
    {
        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }
    }
}
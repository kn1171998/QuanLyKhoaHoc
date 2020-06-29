using DoAnTotNghiep.Models;

namespace DoAnTotNghiep
{
    public static class TypeUser
    {
        public static string User { get; set; } = "User";
        public static string Admin { get; set; } = "Admin";
        public static string Teacher { get; set; } = "Teacher";
    }
    public static class DefineCommon
    {
        public static int ExpireCookie { get; set; } = 3600;
    }
    public static class DefinePaymentMomo
    {
        public static string PartnerCode { get; set; } = "MOMOEWKJ20200614";
        public static string MomoTest { get; set; } = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
        public static string AccessKey { get; set; } = "U19Ag9RXqcgJ4omi";
        public static string SecretKey { get; set; } = "cvchKms1DLnNRIrFVILQ15VaPktksY1o";
        public static string NotifyUrl { get; set; } = "https://courselesson.somee.com/api/payment";
        public static string ReturnUrl { get; set; } = "https://courselesson.somee.com/api/values";
        public static string Signature { get; set; } = "";
        public static PayMomo PaymentMomo { get; set; }
    }
    public static class PaymentMethod
    {
        public static string MomoPay { get; set; } = "MomoPay";
        public static string ZaloPay { get; set; } = "ZaloPay";
    }
    public static class OrderStatus
    {
        public static string Unpaid { get; set; } = "Unpaid";
        public static string Paid { get; set; } = "Paid";
    }
}
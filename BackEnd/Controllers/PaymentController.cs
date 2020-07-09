using BackEnd.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebData.Implementation;
using WebData.Models;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<PaymentController> _log;

        public PaymentController(IOrderService orderService,
                                ILogger<PaymentController> log)
        {
            _orderService = orderService;
            _log = log;
        }

        public string SignatureMomoRequest(string requestId, string amount, string orderId, string orderInfo, string orderType, string transId,
                                      string message, string localMessage, string responseTime, string errorCode, string payType, string extraData)
        {
            string rawHash = string.Format(
                                                    "partnerCode={0}" +
                                                    "&accessKey={1}" +
                                                    "&requestId={2}" +
                                                    "&amount={3}" +
                                                    "&orderId={4}" +
                                                    "&orderInfo={5}" +
                                                    "&orderType={6}" +
                                                    "&transId={7}" +
                                                    "&message={8}" +
                                                    "&localMessage={9}" +
                                                    "&responseTime={10}" +
                                                    "&errorCode={11}" +
                                                    "&payType={12}" +
                                                    "&extraData={13}",
                                                    DefinePaymentMomo.PartnerCode,//0 partnerCode
                                                    DefinePaymentMomo.AccessKey,//1 accessKey
                                                    requestId,//2 requestId
                                                    amount,//3 amount
                                                    orderId,//4 orderId
                                                    orderInfo,//5 orderInfo
                                                    orderType,//6 returnUrl
                                                    transId,//7 notifyUrl
                                                    message,//8 extraData
                                                    localMessage,
                                                    responseTime,
                                                    errorCode,
                                                    payType,
                                                    extraData
                                                  );
            MoMoSecurity crypto = new MoMoSecurity();
            string signature = crypto.signSHA256(rawHash, DefinePaymentMomo.SecretKey);
            return signature;
        }

        public string SignatureMomoSendResponse(string requestId, string orderId, string errorCode,
                                      string message, string responseTime, string extraData)
        {
            string rawHash = string.Format(
                                                    "partnerCode={0}" +
                                                    "&accessKey={1}" +
                                                    "&requestId={2}" +
                                                    "&orderId={3}" +
                                                        "&errorCode={4}" +
                                                    "&message={5}" +
                                                    "&responseTime={6}" +
                                                    "&extraData={7}",
                                                    DefinePaymentMomo.PartnerCode,//0 partnerCode
                                                    DefinePaymentMomo.AccessKey,//1 accessKey
                                                    requestId,//2 requestId
                                                    orderId,//4 orderId
                                                    errorCode,
                                                    message,//8 extraData
                                                    responseTime,
                                                    extraData
                                                  );
            MoMoSecurity crypto = new MoMoSecurity();
            string signature = crypto.signSHA256(rawHash, DefinePaymentMomo.SecretKey);
            return signature;
        }

        // POST api/values
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Post([FromForm] RequestPayMomo pay)
        {
            try
            {
                PayMomo payMomoResponse = new PayMomo();
                payMomoResponse.partnerCode = pay.partnerCode;
                payMomoResponse.accessKey = pay.accessKey;
                payMomoResponse.requestId = pay.requestId;
                payMomoResponse.orderId = pay.orderId;
                payMomoResponse.errorCode = pay.errorCode;
                payMomoResponse.message = pay.message;
                payMomoResponse.responseTime = pay.responseTime;
                payMomoResponse.extraData = pay.extraData;
                Orders orders = _orderService.GetBySingle(pay.orderId);
                if (orders != null)
                {
                    string totalAmount = orders.TotalAmount.ToString();
                    string checkSignatureRequest = SignatureMomoRequest(orders.RequestId, totalAmount, orders.Id, "PaymentCourse", pay.orderType,
                                                                pay.transId, pay.message, pay.localMessage, pay.responseTime, pay.errorCode,
                                                                pay.payType, pay.extraData);
                    string sendSignature = SignatureMomoSendResponse(orders.RequestId, orders.Id, pay.errorCode, pay.message, pay.responseTime, pay.extraData);
                    payMomoResponse.signature = sendSignature;
                    if (pay.errorCode == "0")
                    {
                        if (pay.signature == checkSignatureRequest)
                        {
                            try
                            {
                                _log.LogInformation("vodung");
                                orders.Status = "Paid";
                                await _orderService.UpdateAsync(orders);
                                _log.LogInformation("luu xong");
                                return Ok(payMomoResponse);
                            }
                            catch (System.Exception ex)
                            {
                                _log.LogInformation("try catch vo " + ex.Message);
                                return BadRequest(payMomoResponse);
                            }
                        }
                        _log.LogInformation("khong vo pay.signature: " + pay.signature + "check: " + checkSignatureRequest);
                    }
                    else
                    {
                        _log.LogInformation(pay.errorCode);
                        return BadRequest(payMomoResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("try catch khong vo " + ex.Message);
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var list = _orderService.GetBySingle(id);
            return Ok(list);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id)
        {
            Orders orders = _orderService.GetBySingle(id);
            orders.Status = "Paid";
            await _orderService.UpdateAsync(orders);
            return Ok();
        }
    }

    public class RequestPayMomo
    {
        public string partnerCode { get; set; }
        public string accessKey { get; set; }
        public string requestId { get; set; }
        public string amount { get; set; }
        public string orderId { get; set; }
        public string orderInfo { get; set; }
        public string orderType { get; set; }
        public string transId { get; set; }
        public string errorCode { get; set; }
        public string message { get; set; }
        public string localMessage { get; set; }
        public string payType { get; set; }
        public string responseTime { get; set; }
        public string extraData { get; set; } = "";
        public string signature { get; set; }
    }

    public class PayMomo
    {
        public string partnerCode { get; set; }
        public string accessKey { get; set; }
        public string requestId { get; set; }
        public string orderId { get; set; }
        public string errorCode { get; set; }
        public string message { get; set; }
        public string responseTime { get; set; }
        public string extraData { get; set; } = "";
        public string signature { get; set; }
    }
}
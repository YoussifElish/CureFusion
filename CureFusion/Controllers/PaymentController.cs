using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Abstactions;
using System.Text.Json;

namespace CureFusion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IAppointmentService appointmentService,ILogger<PaymentController> logger) : ControllerBase
    {
        private readonly IAppointmentService _appointmentService = appointmentService;
        private readonly ILogger<PaymentController> _logger = logger;

        [HttpPost("processed-callback")]
        public async Task<IActionResult> PaymobProcessedCallback([FromBody] JsonElement data)
        {
            try
            {
                var obj = data.GetProperty("obj");
                var success = obj.GetProperty("success").GetBoolean();
                var merchantOrderId = obj.GetProperty("order").GetProperty("merchant_order_id").GetString();

                var appointmentId = int.Parse(merchantOrderId.Split('_')[0]);

                var result = await _appointmentService.ConfirmAppointmentPayment(appointmentId, success);
                return result.IsSuccess ? Ok() : result.ToProblem();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred in PaymobProcessedCallback");
                 return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("response-callback")]
        public async Task<IActionResult> PaymobResponseCallback([FromQuery] bool success,[FromQuery(Name = "merchant_order_id")] string merchantOrderId
)
        {
            try
            {
                var appointmentId = int.Parse(merchantOrderId.Split('_')[0]);

                _logger.LogInformation("Received Paymob response callback | AppointmentId: {AppointmentId} | Success: {Success}", appointmentId, success);

                return success
                    ? Ok("Payment was successful")
                    : BadRequest("Payment failed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred in PaymobResponseCallback");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

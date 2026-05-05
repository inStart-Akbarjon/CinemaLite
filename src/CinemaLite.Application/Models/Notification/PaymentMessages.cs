namespace CinemaLite.Application.Models.Notification;

public class PaymentMessages
{
    public readonly string PaymentSucceed =
        $"Your payment successfully received! Thank you for choosing our platform!";
    public readonly string PaymentFailed =
        $"Your payment failed! Please try again later!";
    public readonly string PaymentExpired =
        $"Your payment is expired! Please try to pay within 15 minutes to avoid this situation!";
}
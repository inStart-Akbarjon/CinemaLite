using CinemaLite.Application.Common.Stripe;
using CinemaLite.Application.CQRS.Payment.Commands.ConfirmPaymentTransaction;
using CinemaLite.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace CinemaLite.Api.Controllers;

[ApiController]
[Route("api/webhooks/stripe")]

public class StripeWebhook(
    IOptions<StripeSettings> stripeSettings, 
    IMediator mediator) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], stripeSettings.Value.WebhookSecret, throwOnApiVersionMismatch: false);
            
            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                
                if (paymentIntent == null)
                {
                    return BadRequest();
                }
                
                var command = new ConfirmPaymentTransactionCommand(paymentIntent.Id, PaymentStatus.Succeed);
                await mediator.Send(command);
            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                
                if (paymentIntent == null)
                {
                    return BadRequest();
                }
                
                var command = new ConfirmPaymentTransactionCommand(paymentIntent.Id, PaymentStatus.Failed);
                await mediator.Send(command);
            }
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }
            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest();
        }
    }
}
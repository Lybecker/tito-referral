# ti.to referral tracker
[ti.to](https://ti.to/) is an event ticketing system. We used it at [Copenhagen .NET Usergroup](https://cnug.dk/) when we arrange events and conferences like [MicroCPH.dk](https://microcph.dk/) or [Intelligent Cloud Conference](https://intelligentcloud.dk/).

An easy way to attract attention to an event is using a referral program, where both the attendee and the potential attendee gains a benefit, then there is a greater chance that the attendee will share the event with friends and collegues.

## Referral tracing

1. A ticket is assigned to an attendee
2. ti.to webhook calls the service
3. The service creates a ti.to Discount with code  `<referral attendee code>-<referral attendee first name>` e.g. YAZR-1-John 
4. The service creates a direct link to the tickets with the Discount applied
5. Sends an email to the attendee with the direct ticket link

To figure out how many referrals each attendee has, compare the attendee code with the discount code. ti.to keeps track of how many times a Discount has been used.

# Technology

The solution is build as a ASP.NET Core 2.2 Web API solution.

# Integration
We integrate with the [ti.to Admin API](https://ti.to/docs/api/).
 - Use the [Webhook](https://ti.to/docs/api/admin/#webhooks) ticket.completed event to trigger the workflow
 - Create a unique [discount](https://ti.to/docs/api/admin/#discount-codes) for each attendee, they can use as referral
 - Create a [direct ticket link with discount](https://ti.to/docs/sharing_urls)

# Security
The solution verifies the webhook payload via the HTTP header tito-signature by using HMAC digest with SHA2 in the `TitoPayloadVerifierMiddleware` for all Web API requests. The security token used as key can be found under Customice | Webhooks in the ti.to admin portal. 

To access the ti.to Admin API an API token is needed. To get your [API Token go here](https://id.tito.io/).

# Configuration
Be aware that slugs are case sensitive.
```
{
  "Tito": {
    "AccountName": "<ti.to account slug e.g. CNUG>",
    "ApiToken": "<API Token>",
    "Event": {
      "EventName": "<ti.to event slug e.g. my-conf-2019>",
      "WebHookSecurityToken": "<webhook security token>",
      "Discount": {
        "Pct": false,
        "Value": <discount value e.g. 100>,
        "TicketIds": [
          "<ticket identifier where the discount is valid e.g. 43asdfd-c34>"
        ]
      }
    }
  }
}
```
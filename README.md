# ti.to referral tracker
[ti.to](https://ti.to/) is an event ticketing system. We use it at [Copenhagen .NET Usergroup](https://cnug.dk/) when we arrange events and conferences like [MicroCPH.dk](https://microcph.dk/) or [Intelligent Cloud Conference](https://intelligentcloud.dk/).

An easy way to attract attention to an event is by using a referral program, where both the attendee and the potential attendee gain a benefit, providing an incentive for the attendee to share the event with friends and collegues.

## Referral tracing

1. A ticket is assigned to an attendee
2. ti.to webhook calls the service
3. The service creates a ti.to Discount with code e.g. YAZR-1
4. The service creates a direct link to the tickets with the Discount applied
5. The service sends an email to the attendee with the direct ticket link with discount, which is used to track referral
6. If the Discount is applied to one of the valid tickets ticket (e.g. not free tickets), then a thank you email is send to the referrer. If a ticket is re-assigned, then the new attendee will receive a email, but the referrer will not be notified.

To figure out how many referrals each attendee has, compare the attendee code with the discount code. ti.to keeps track of how many times a Discount has been used.
> When an user enters a discount code, tickets where the discount code applies are discounted, but other tickets are shown too. Only if the user purchase tickets where discount applies, will the referral system record it. ti.to will however still include the discount code in the `ticket.completed` webhook event.

# Technology

The solution is build as a ASP.NET Core 2.2 Web API solution running on Linux, Mac & Windows. 

[![Build Status](https://lybecker.visualstudio.com/Microsoft/_apis/build/status/tito-referral?branchName=master)](https://lybecker.visualstudio.com/Microsoft/_build/latest?definitionId=22?branchName=master)


## Hosting
All you need is a web server. No database or similar.


# Integration
We integrate with the [ti.to Admin API](https://ti.to/docs/api/).
 - Use the [Webhook](https://ti.to/docs/api/admin/#webhooks) ticket.completed event to trigger the workflow
 - Create a unique [discount](https://ti.to/docs/api/admin/#discount-codes) for each attendee, they can use as referral
 - Create a [direct ticket link with discount](https://ti.to/docs/sharing_urls)
 - Gmail is used for sending emails.

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
    "ReplyEmail": "ConfX <email@confx.com>",
    "EmailUsername": "email@confx.com",
    "EmailPassword": "P@ssword",
    "Event": {
      "EventName": "<ti.to event slug e.g. my-conf-2019>",
      "WebHookSecurityToken": "<webhook security token>",
      "DiscountPct": false,
      "DiscountValue": <discount value e.g. 10.0>,
      "TicketIds": [
        "<ticket identifier where the discount is valid e.g. 43asdfd-c34>"
      ]
    }
  }
}
```

To find the TicketIds (called releases at API level), go to Tickets, select a ticket. The URL will include the TicketId e.g. `https://ti.to/cnug/my-conf-2019/admin/releases/<TicketId>`

# ti.to setup
Go to the admin dashboard.
- Customize | Webhooks and add `https://<url>/api/titowebhook` and select the `ticket.completed` only.

ti.to has a webhook log: `https://api.tito.io/<account_slug>/<event_slug>/webhooks`

# Email templates
The system uses two HTML templates:
- `Referral Template` send to the attendee when assigned at ticket.
- `Referral Thanks` send to referrer when referral discount code is used.

The templates are stored in EmailTemplates folder and the system expects them to be stored EmailTemplates folder and the executable root.

# Running locally
Run in locally in .NET Core CLI `dotnet run` or in Visual Studio. It starts a web server on port 5000. Test it works by browsing to `http://localhost:5000/api/titowebhook`.

## Testing ti.to webhook locally
Use [ngrok](https://ngrok.com/) to expose the web server to the public internet, by installing ngrok and running the following command 

```ngrok http -host-header="localhost:5000" 5000```

Remeber to add the ngrok endpoint as a ti.to webhook.
using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Model
{
    public class TicketCompletedEvent
    {
        public string text { get; set; }
        public int id { get; set; }
        public TitoEvent @event { get; set; }
        public string name { get; set; }
        public string First_name { get; set; }
        public string last_name { get; set; }
        [Required]
        public string Email { get; set; }
        public object phone_number { get; set; }
        public string company_name { get; set; }
        public string reference { get; set; }
        public string price { get; set; }
        public string tax { get; set; }
        public string price_less_tax { get; set; }
        public string slug { get; set; }
        public string state_name { get; set; }
        public string gender { get; set; }
        public string discount_code_used { get; set; }
        public string total_paid { get; set; }
        public string total_paid_less_tax { get; set; }
        public DateTime updated_at { get; set; }
        public string url { get; set; }
        public string admin_url { get; set; }
        public string release_title { get; set; }
        public string release_slug { get; set; }
        public int release_id { get; set; }
        public TitoRelease release { get; set; }
        public string custom { get; set; }
        public string registration_id { get; set; }
        public string registration_slug { get; set; }
        public object metadata { get; set; }
    }
}
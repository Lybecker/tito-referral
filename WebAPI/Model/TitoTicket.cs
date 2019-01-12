using System;

namespace WebAPI.Model.Ticket
{
    public class RootTicket
    {
        public Ticket[] tickets { get; set; }
        public Meta meta { get; set; }
    }

    public class Meta
    {
        public int current_page { get; set; }
        public int? next_page { get; set; }
        public int? prev_page { get; set; }
        public int total_pages { get; set; }
        public int total_count { get; set; }
        public int per_page { get; set; }
        public int overall_total { get; set; }
        public int resources_hidden_by_default_count { get; set; }
        public string[] search_states_hidden_by_default { get; set; }
    }

    public class Ticket
    {
        public string _type { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
        public string company_name { get; set; }
        public string email { get; set; }
        public object metadata { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string name { get; set; }
        public int number { get; set; }
        public object phone_number { get; set; }
        public string price { get; set; }
        public string reference { get; set; }
        public string state { get; set; }
        public bool test_mode { get; set; }
        public int registration_id { get; set; }
        public int release_id { get; set; }
        public object consented_at { get; set; }
        public string discount_code_used { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public bool assigned { get; set; }
        public string price_less_tax { get; set; }
        public string total_paid { get; set; }
        public string total_tax_paid { get; set; }
        public string total_paid_less_tax { get; set; }
        public string tags { get; set; }
        public string registration_slug { get; set; }
        public string release_slug { get; set; }
        public string release_title { get; set; }
    }
}
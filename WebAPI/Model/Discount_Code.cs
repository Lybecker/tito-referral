using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace WebAPI.Model
{
    public class Discount_Code
    {
        //public Discount_Code()
        //{
        //    _type = "discount_code";
        //}
        //public string _type { get; set; }
        public int? id { get; set; }
        public string code { get; set; }
        public DateTime? end_at { get; set; }
        public int? max_quantity_per_release { get; set; }
        public int? min_quantity_per_release { get; set; }
        public bool only_show_attached { get; set; }
        public int? quantity { get; set; }
        public int quantity_used { get; set; }
        public string[] release_ids { get; set; }
        public bool reveal_secret { get; set; }
        public DateTime? start_at { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DiscountTypes type { get; set; }
        //public DiscountTypes discount_code_type { get; set; }
        public decimal value { get; set; }
    }

    public enum DiscountTypes
    {
        NotSet = 0,
        MoneyOffDiscountCode = 1,
        PercentOffDiscountCode = 2,
    }


}

//public class Discount_Code
//{
//    public string _type { get; set; }
//    public int id { get; set; }
//    public string code { get; set; }
//    public object end_at { get; set; }
//    public object max_quantity_per_release { get; set; }
//    public object min_quantity_per_release { get; set; }
//    public bool only_show_attached { get; set; }
//    public object quantity { get; set; }
//    public int quantity_used { get; set; }
//    public string[] release_ids { get; set; }
//    public bool reveal_secret { get; set; }
//    public object start_at { get; set; }
//    public string discount_code_type { get; set; }
//    public string value { get; set; }
//    public object source { get; set; }
//}

using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Model
{
    public class TitoEvent
    {
        public int id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        [Required]
        public string account_slug { get; set; }
        /// <summary>
        /// Event slug e.g. intelligent-cloud-2018
        /// </summary>
        [Required]
        public string slug { get; set; }
        public string currency { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
    }
}
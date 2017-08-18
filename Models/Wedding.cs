using System;
using System.Collections.Generic;

namespace wedding_planner.Models
{
    public class Wedding: BaseEntity
    {
        public int WeddingId {get; set;}
        public string WedderOne {get; set;}
        public string WedderTwo {get; set;}
        public DateTime Date {get; set;}
        public string Address {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UploadedAt {get; set;}
        public int UserId {get; set;}
        public User User {get; set;}
        public List<Guest> Guests {get; set;}

        public Wedding() {
            Guests = new List<Guest>();
        }
    }
}
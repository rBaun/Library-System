using DataAccess.Enums;
using System;

namespace DataAccess.Entities
{
    public class Notification
    {
        public int ID { get; set; }
        public DateTime SendDate { get; set; }
        public NotificationType NotificationType { get; set; }
        public Card Card { get; set; }
        public Loan Loan { get; set; }
    }
}
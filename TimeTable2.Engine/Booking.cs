using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TimeTable2.Engine
{
    public class Booking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int Week { get; set; }
        public int WeekDay { get; set; }
        public int StartBlock { get; set; }
        public int EndBlock { get; set; }
        public int Guests { get; set; }
        public string Classroom { get; set; }

        public BookingType Type { get; set; }


        [JsonIgnore]
        public string Owner { get; set; }
        [JsonIgnore]
        public Classroom Lokaal { get; set; }

        [JsonIgnore]
        public string TypeString
        {
            get => Type.ToString();
            private set => value.ParseEnum<BookingType>();
        }
    }

    public enum BookingType
    {
        Normal = 0,
        Maintenance = 1
    }
}

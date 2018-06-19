using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimeTable2.Engine
{
    public class User
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }

        public TimeTableRole Role { get; set; }
        public string RoleString    
        {
            get => Role.ToString();
            set => value.ParseEnum<TimeTableRole>();
        }

        public DateTime LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public enum TimeTableRole
    {
        Student = 0,
        Teacher = 1,
        Fit = 2,
        Management = 3,
        Admin = 4

    }
}

using System;

namespace WebH.Models
{
    public class LogActionModel : CountUsersModel
    {
        public string DisplayName { get; set; }
        public TimeSpan TimeElapsed { get; set; }

    }
}

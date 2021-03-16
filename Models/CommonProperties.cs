using System;

namespace SignupWithMailConfirmation.Models
{
    public class CommonProperties
    {
        public DateTime Created { get; set; } = new DateTime(1900, 1, 1);
        public DateTime Updated { get; set; } = new DateTime(1900, 1, 1);
        public string Message { get; set; } 
    }
}
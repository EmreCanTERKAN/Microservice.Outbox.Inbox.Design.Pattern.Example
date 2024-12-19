using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Order.API.Models.Entites
{
    public class OrderOutbox 
    {
        //idenpotent
        [Key]
        public Guid IdempotentToken { get; set; }
        public DateTime OccuredOn { get; set; }

        // Veriyi eklediğimizde hemen işlemeyeceğinden dolayı bu null olacaktır...
        public DateTime? ProcessedDate { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
    }
}

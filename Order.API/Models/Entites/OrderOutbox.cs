using Microsoft.EntityFrameworkCore;

namespace Order.API.Models.Entites
{
    public class OrderOutbox 
    {
        public int Id { get; set; }
        public DateTime OccuredOn { get; set; }

        // Veriyi eklediğimizde hemen işlemeyeceğinden dolayı bu null olacaktır...
        public DateTime? ProcessedDate { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
    }
}

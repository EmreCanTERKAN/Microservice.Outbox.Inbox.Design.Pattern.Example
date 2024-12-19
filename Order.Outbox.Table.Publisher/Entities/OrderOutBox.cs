using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Outbox.Table.Publisher.Entities
{
    public class OrderOutBox
    {
        public int Id { get; set; }
        public DateTime OccuredOn { get; set; }

        // Veriyi eklediğimizde hemen işlemeyeceğinden dolayı bu null olacaktır...
        public DateTime? ProcessedDate { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
    }
}

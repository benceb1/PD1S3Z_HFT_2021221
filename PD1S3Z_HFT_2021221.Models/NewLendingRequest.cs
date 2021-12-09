using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Models
{
    public class NewLendingRequest
    {
        public int borrowerId { get; set; }
        public int lendingWeeks { get; set; }
        public int bookId { get; set; }
    }
}

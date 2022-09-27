using DA_TOTNGHIEP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DA_TOTNGHIEP.ViewModels
{
        public class InstructorIndexData
        {
            public IEnumerable<Invoices> Invoices { get; set; }
            public IEnumerable<InvoiceDetails> InvoiceDetails { get; set; }
        }
}

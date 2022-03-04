using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNSLabWinApp.DTOs
{
    public class HostNamesAndCheckedDTO
    {
        public Guid Id { get; set; }
        public string HostName { get; set; }
        public bool IsChecked { get; set; }
    }
}

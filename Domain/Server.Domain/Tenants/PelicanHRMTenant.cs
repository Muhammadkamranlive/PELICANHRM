using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain
{
    public class PelicanHRMTenant
    {
        public Guid Id                  { get; set; }
        public string CompanyName       { get; set; }
        public int CompanyId            { get; set; }

    }
}

using System;
using System.Collections.Generic;

namespace DataMaintenanceWithEF.Models
{
    public partial class ProductionCompany
    {
        public ProductionCompany()
        {
            Movies = new HashSet<Movie>();
        }

        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}

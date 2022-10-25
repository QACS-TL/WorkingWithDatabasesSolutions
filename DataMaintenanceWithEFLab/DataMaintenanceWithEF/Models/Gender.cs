using System;
using System.Collections.Generic;

namespace DataMaintenanceWithEF.Models
{
    public partial class Gender
    {
        public Gender()
        {
            MovieCasts = new HashSet<MovieCast>();
        }

        public int GenderId { get; set; }
        public string? Gender1 { get; set; }

        public virtual ICollection<MovieCast> MovieCasts { get; set; }
    }
}

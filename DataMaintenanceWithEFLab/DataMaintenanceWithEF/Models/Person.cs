using System;
using System.Collections.Generic;

namespace DataMaintenanceWithEF.Models
{
    public partial class Person
    {
        public Person()
        {
            MovieCasts = new HashSet<MovieCast>();
        }

        public int PersonId { get; set; }
        public string? PersonName { get; set; }

        public virtual ICollection<MovieCast> MovieCasts { get; set; }
    }
}

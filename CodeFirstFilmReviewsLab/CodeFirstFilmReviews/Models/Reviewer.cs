using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeFirstFilmReviewsDB.Models
{
    internal class Reviewer
    {
        [Key]
        public int ReviewerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public List<Review> Reviews { get; set; }
    }
}

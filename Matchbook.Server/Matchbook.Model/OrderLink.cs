using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Matchbook.Model
{
    public class OrderLink
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string LinkName { get; set; }

        public ICollection<Order> LinkedOrders { get; set; }
    }
}

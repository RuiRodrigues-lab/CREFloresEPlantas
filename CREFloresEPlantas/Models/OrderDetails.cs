using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CREFloresEPlantas.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [Display(Name = "Ordem")]
        public int OrderId { get; set; }
        [Display(Name = "Produto")]
        public int PorductId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        [ForeignKey("PorductId")]
        public Produtos Produto { get; set; }

    }
}

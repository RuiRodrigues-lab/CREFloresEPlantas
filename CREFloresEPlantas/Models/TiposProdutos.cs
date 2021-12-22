using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CREFloresEPlantas.Models
{
    public class TiposProdutos
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="Tipo de Produto")]
        public string TipoProduto { get; set; }
    }
}

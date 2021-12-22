using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CREFloresEPlantas.Models
{
    public class Produtos
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        //Nunca mais vou usar decimal, porra pos bugs de "," e "." para casas decimais
        //[Required]
        //public decimal Preco { get; set; }
        [Required]
        public double Preco { get; set; }
        public string Imagem { get; set; }
        [Display(Name = "Cor do Produto")]
        public string CorProduto { get; set; }
        [Required]
        [Display(Name = "Disponivel")]
        public bool Disponivel { get; set; }
        [Display(Name = "Tipo de produto")]
        [Required]
        public int TiposProdutosId { get; set; }
        [ForeignKey("TiposProdutosId")]
        public virtual TiposProdutos TiposProdutos { get; set; }
        [Display(Name = "Special Tag")]
        [Required]
        public int SpecialTagId { get; set; }
        [ForeignKey("SpecialTagId")]
        public virtual SpecialTag SpecialTag { get; set; }

    }
}

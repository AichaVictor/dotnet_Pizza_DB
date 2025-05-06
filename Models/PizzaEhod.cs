using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Pizzéria.Models
{
    public class PizzaEhod
    {
        [Key]
        public int IdEhod { get; set; }
        public string? NomEhod { get; set; }
        public string? DescriptionEhod { get; set; }
    }
}

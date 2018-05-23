using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Helpers;

namespace Domain
{
    public class Promotion : EssentialEntityBase
    {
        public int PromotionId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        public DateTime ValidTo { get; set; }
        //foreign keys
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        //OneToMany
        public List<Menu> Menus { get; set; } = new List<Menu>();
        public List<Dish> Dishes { get; set; } = new List<Dish>();
        public List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
    }
}

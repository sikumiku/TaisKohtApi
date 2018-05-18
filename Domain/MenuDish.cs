using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Helpers;

namespace Domain
{
    public class MenuDish : EssentialEntityBase
    {
        public int MenuDishId { get; set; }
        //foreign keys
        [Required]
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        [Required]
        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}

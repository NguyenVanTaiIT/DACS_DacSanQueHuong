﻿namespace DACS.Models
{
    public class CartItem
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image{get; set;}
        public int Price{get; set;}

        public int Quantity { get; set; }

    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TruckLoader.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Customer
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please Enter Goods Type")]
        public string GoodsType { get; set; }

        [Required(ErrorMessage = "Please Enter Area")]
        public string Area { get; set; }

        [Required(ErrorMessage = "Please Enter City")]
        public string City { get; set; }
    }
}

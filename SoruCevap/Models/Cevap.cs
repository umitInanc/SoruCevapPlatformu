//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SoruCevap.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cevap
    {
        public int Id { get; set; }
        public string icerik { get; set; }
        public string uyeId { get; set; }
        public int soruId { get; set; }
    
        public virtual Soru Soru { get; set; }
    }
}
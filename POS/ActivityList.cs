//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace POS
{
    using System;
    using System.Collections.Generic;
    
    public partial class ActivityList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEN { get; set; }
        public string ModuleCode { get; set; }
        public string ActivityType { get; set; }
        public int TotalHours { get; set; }
        public string EvaluationType { get; set; }
        public string RoomType { get; set; }
        public string Staff { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        public string ModifiedBy { get; set; }
        public int GroupNumber { get; set; }
    }
}

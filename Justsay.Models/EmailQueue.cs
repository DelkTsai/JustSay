//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Justsay.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmailQueue
    {
        public int ID { get; set; }
        public int ConfessID { get; set; }
        public int RelationID { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
    
        public virtual Confess Confess { get; set; }
        public virtual Relation Relation { get; set; }
    }
}

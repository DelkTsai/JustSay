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
    
    public partial class Funny
    {
        public Funny()
        {
            this.Comments = new HashSet<Comment>();
        }
    
        public int ID { get; set; }
        public int MemberID { get; set; }
        public string ShowName { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public int Up { get; set; }
        public System.DateTime Time { get; set; }
        public int CommentCount { get; set; }
        public string ImgUrl { get; set; }
        public Nullable<int> ConfessID { get; set; }
        public int Status { get; set; }
    
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Member Member { get; set; }
        public virtual Confess Confess { get; set; }
    }
}
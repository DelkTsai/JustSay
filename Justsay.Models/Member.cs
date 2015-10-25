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
    
    public partial class Member
    {
        public Member()
        {
            this.ActionLogs = new HashSet<ActionLog>();
            this.Comments = new HashSet<Comment>();
            this.Confesses = new HashSet<Confess>();
            this.Funnies = new HashSet<Funny>();
            this.Messages = new HashSet<Message>();
            this.Messages1 = new HashSet<Message>();
            this.Relations = new HashSet<Relation>();
        }
    
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public System.DateTime JoinDate { get; set; }
        public string ShowName { get; set; }
        public Nullable<System.DateTime> LastVisit { get; set; }
        public string QQ { get; set; }
        public string Phone { get; set; }
        public string RealName { get; set; }
        public Nullable<System.DateTime> RoleDeadLine { get; set; }
        public int Score { get; set; }
        public int Money { get; set; }
        public System.DateTime LastPostTime { get; set; }
        public int MessageCount { get; set; }
        public int RoleID { get; set; }
    
        public virtual ICollection<ActionLog> ActionLogs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Confess> Confesses { get; set; }
        public virtual ICollection<Funny> Funnies { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Message> Messages1 { get; set; }
        public virtual ICollection<Relation> Relations { get; set; }
    }
}

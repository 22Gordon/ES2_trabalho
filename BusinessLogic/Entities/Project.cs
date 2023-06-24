using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Project
{
    public Guid Projectid { get; set; }

    public string Name { get; set; } = null!;

    public Guid? Projectleaderid { get; set; }

    public Guid? Clientid { get; set; }

    public double? Pricehour { get; set; }

    public virtual Client? Client { get; set; }

    public virtual ICollection<Invite> Invites { get; set; } = new List<Invite>();

    public virtual Freelancer? Projectleader { get; set; }

    public virtual ICollection<Taskproject> Taskprojects { get; set; } = new List<Taskproject>();
}

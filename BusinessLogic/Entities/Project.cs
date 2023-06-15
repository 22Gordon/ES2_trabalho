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

    public virtual Freelancer? Projectleader { get; set; }

    public virtual ICollection<Freelancer> Freelancers { get; set; } = new List<Freelancer>();

    public virtual ICollection<UserTask> Tasks { get; set; } = new List<UserTask>();
}

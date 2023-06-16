using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class UserTask
{
    public Guid Taskid { get; set; }

    public Guid? Freelancerid { get; set; }

    public Guid? Clientid { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Enddate { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public double? Pricehour { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Freelancer? Freelancer { get; set; }
    
    public TimeSpan Duration { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}

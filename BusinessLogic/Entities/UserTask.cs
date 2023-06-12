using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class UserTask
{
    public Guid Taskid { get; set; }

    public Guid? Projectid { get; set; }

    public Guid? Freelancerid { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Enddate { get; set; }

    public double? Pricehour { get; set; }

    public virtual Freelancer? Freelancer { get; set; }

    public virtual Project? Project { get; set; }
}

using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Taskproject
{
    public Guid Taskid { get; set; }

    public Guid Projectid { get; set; }

    public int? AuxColumn { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual UserTask Task { get; set; } = null!;
}

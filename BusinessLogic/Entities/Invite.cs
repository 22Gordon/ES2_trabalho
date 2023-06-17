using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Invite
{
    public Guid Projectid { get; set; }

    public Guid Freelancerid { get; set; }

    public bool Isaccepted { get; set; }

    public virtual Freelancer Freelancer { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}

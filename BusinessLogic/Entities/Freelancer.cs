﻿using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Freelancer
{
    public Guid Userid { get; set; }

    public double? Dailyavghours { get; set; }

    public virtual ICollection<Invite> Invites { get; set; } = new List<Invite>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
}

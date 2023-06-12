using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class User
{
    public Guid Userid { get; set; }

    public string? Displayname { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Freelancer? Freelancer { get; set; }
}

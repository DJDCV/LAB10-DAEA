using System;
using System.Collections.Generic;

namespace Lab10.Infrastructure.Entities;

public partial class user
{
    public Guid user_id { get; set; }

    public string username { get; set; } = null!;

    public string password_hash { get; set; } = null!;

    public string? email { get; set; }

    public DateTime? created_at { get; set; }

    public virtual ICollection<response> responses { get; set; } = new List<response>();

    public virtual ICollection<ticket> tickets { get; set; } = new List<ticket>();

    public virtual ICollection<user_role> user_roles { get; set; } = new List<user_role>();
}

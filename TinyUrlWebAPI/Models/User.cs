using System;
using System.Collections.Generic;

namespace TinyUrlWebAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UrlLink> UrlLinks { get; set; } = new List<UrlLink>();
}

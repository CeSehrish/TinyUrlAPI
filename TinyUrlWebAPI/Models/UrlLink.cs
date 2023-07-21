using System;
using System.Collections.Generic;

namespace TinyUrlWebAPI.Models;

public partial class UrlLink
{
    public int Id { get; set; }

    public string? LongUrl { get; set; }

    public string TinyUrl { get; set; } = null!;

    public string Alias { get; set; } = null!;

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}

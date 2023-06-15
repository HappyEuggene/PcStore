using System;
using System.Collections.Generic;

namespace PcStore.Models;

public partial class ItemSpec
{
    public int Id { get; set; }

    public string? Frequency { get; set; }

    public string? MemCapacity { get; set; }

    public string? MemType { get; set; }

    public string? Cores { get; set; }

    public string? Power { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}

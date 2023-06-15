using System;
using System.Collections.Generic;

namespace PcStore.Models;

public partial class ItemType
{
    public int Id { get; set; }

    public string? Model { get; set; }

    public string? Manufacture { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}

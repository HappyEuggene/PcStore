using System;
using System.Collections.Generic;

namespace PcStore.Models;

public partial class Order
{
    public int Id { get; set; }

    public double? TotalPrice { get; set; }

    public string? Delivery { get; set; }

    public string? UserId { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}

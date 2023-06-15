using System;
using System.Collections.Generic;

namespace PcStore.Models;

public partial class Item
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double? Price { get; set; }

    public int? ItemSpecId { get; set; }

    public int? ItemTypeId { get; set; }

    public string? UserId { get; set; }

    public virtual ItemSpec? ItemSpec { get; set; }

    public virtual ItemType? ItemType { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

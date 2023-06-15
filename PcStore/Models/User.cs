using Microsoft.AspNetCore.Identity;

namespace PcStore.Models;

public class User : IdentityUser
{
    public string Name { get; set; }
    public string LastName { get; set; }
}

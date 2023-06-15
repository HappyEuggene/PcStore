using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PcStore.Models;

namespace PcStore.Data;

public class Somkin1IdentityContext : IdentityDbContext<User>
{
    public Somkin1IdentityContext(DbContextOptions<Somkin1IdentityContext> options) : base(options)
    {
    }

}
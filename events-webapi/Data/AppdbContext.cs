using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using events_webapi.Models;

    public class AppdbContext : DbContext
    {
        public AppdbContext (DbContextOptions<AppdbContext> options)
            : base(options)
        {
        }

        public DbSet<events_webapi.Models.Event> Event { get; set; } = default!;

public DbSet<events_webapi.Models.EventType> EventType { get; set; } = default!;
    }

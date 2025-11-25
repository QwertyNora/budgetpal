using Microsoft.EntityFrameworkCore;
using BudgetTracker.Infrastructure.Data;

namespace BudgetTracker.API.Extensions;

public static class DatabaseExtensions
{
    public static void InitializeDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BudgetTrackerDbContext>();
        
        // This will create the database and apply any pending migrations
        context.Database.EnsureCreated();
    }
}

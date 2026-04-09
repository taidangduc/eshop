using EShop.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EShop.Persistence.Interceptors;

// ref: https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors
public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        OnBeforeSaving(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        OnBeforeSaving(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void OnBeforeSaving(DbContext? context)
    {
        try
        {
            if (context is null) return;

            foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
            {
                var isAuditable = entry.Entity.GetType().IsAssignableTo(typeof(IAuditableEntity));

                if (isAuditable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatedAt = DateTime.UtcNow;
                            break;
                        case EntityState.Modified:
                            entry.Entity.UpdatedAt = DateTime.UtcNow;
                            entry.Entity.Version++;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            entry.Entity.UpdatedAt = DateTime.UtcNow;
                            entry.Entity.Version++;
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error while setting auditable entity properties.", ex);
        }
    }
}

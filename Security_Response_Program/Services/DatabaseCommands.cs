using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Security_Response_Program.Models;

namespace Security_Response_Program.Services
{
    public interface IDatabaseCommands
    {
        /// <summary>
        /// Adds or updates an incident in the database.
        /// </summary>
        /// <param name="incident">The incident to add or update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<Incident> AddOrUpdateIncident(Incident incident);

        /// <summary>
        /// Adds or updates a system in the database.
        /// </summary>
        /// <param name="system">The system to add or update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<bool> AddOrUpdateSystem(Models.System system);

        Task<bool> RemoveIncident(Incident incident);
        Task<bool> RemoveSystem(Models.System system);

        /// <summary>
        /// Retrieves all systems from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<List<Models.System>> GetSystems();

        /// <summary>
        /// Retrieves all incidents from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<List<Incident>> GetIncidents();
    }

    /// <summary>
    /// Provides methods for executing database commands and utility methods for error handling.
    /// </summary>
    public class DatabaseCommands : IDatabaseCommands
    {
        private readonly IDbContextFactory<IncidentDbContext> _contextFactory;
        private readonly ISnackbarMessageService _snackbarMessageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseCommands"/> class.
        /// </summary>
        /// <param name="contextFactory">The factory used to create the database context.</param>
        /// <param name="snackbarMessageService">The service used to display error messages.</param>
        public DatabaseCommands(IDbContextFactory<IncidentDbContext> contextFactory, ISnackbarMessageService snackbarMessageService)
        {
            _contextFactory = contextFactory;
            _snackbarMessageService = snackbarMessageService;
        }

        #region Database Commands

        /// <summary>
        /// Adds or updates an incident in the database.
        /// </summary>
        /// <param name="incident">The incident to add or update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<Incident> AddOrUpdateIncident(Incident incident)
        {
            return await ExecuteDbOperation(async context =>
            {
                var existingIncident = await context.Incidents.FirstOrDefaultAsync(i => i.IncidentId == incident.IncidentId);

                if (existingIncident == null)
                {
                    var newIncident = context.Incidents.Add(incident);
                    await context.SaveChangesAsync();
                    return newIncident.Entity;
                }
                else
                {
                    context.Entry(existingIncident).CurrentValues.SetValues(incident);
                    await context.SaveChangesAsync();
                    return existingIncident;
                }


            }, "Failed to add or update incident.");
        }

        /// <summary>
        /// Adds or updates a system in the database.
        /// </summary>
        /// <param name="system">The system to add or update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<bool> AddOrUpdateSystem(Models.System system)
        {
            return await ExecuteDbOperation(async context =>
            {
                var existingSystem = await context.Systems.FirstOrDefaultAsync(i => i.SystemId == system.SystemId);

                if (existingSystem == null)
                {
                    context.Systems.Add(system);
                }
                else
                {
                    context.Entry(existingSystem).CurrentValues.SetValues(system);
                }

                await context.SaveChangesAsync();

                return true;
            }, "Failed to add or update system.");
        }

        // Remove incident
        public async Task<bool> RemoveIncident(Incident incident)
        {
            return await ExecuteDbOperation(async context =>
            {
                var existingIncident = await context.Incidents.FirstOrDefaultAsync(i => i.IncidentId == incident.IncidentId);

                if (existingIncident != null)
                {
                    context.Incidents.Remove(existingIncident);
                }

                await context.SaveChangesAsync();

                return true;
            }, "Failed to remove incident.");
        }

        // Remove system
        public async Task<bool> RemoveSystem(Models.System system)
        {
            return await ExecuteDbOperation(async context =>
            {
                var existingSystem = await context.Systems.FirstOrDefaultAsync(i => i.SystemId == system.SystemId);

                if (existingSystem != null)
                {
                    context.Systems.Remove(existingSystem);
                }

                await context.SaveChangesAsync();

                return true;
            }, "Failed to remove system.");
        }

        /// <summary>
        /// Retrieves all systems from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<List<Models.System>> GetSystems()
        {
            return await ExecuteDbOperation(async context =>
            {
                return await context.Systems.ToListAsync();
            }, "Failed to retrieve systems.");
        }

        /// <summary>
        /// Retrieves all incidents from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<List<Incident>> GetIncidents()
        {
            return await ExecuteDbOperation(async context =>
            {
                return await context.Incidents.ToListAsync();
            }, "Failed to retrieve incidents.");
        }
        #endregion

        #region Utility Methods

        private async Task<TResult> ExecuteDbOperation<TResult>(Func<IncidentDbContext, Task<TResult>> operation, string errorMessage)
        {
            try
            {
                using (var context = await _contextFactory.CreateDbContextAsync())
                {
                    // Executes a database operation within a database context.
                    return await operation(context);
                }
            }
            catch (Exception e)
            {
                LogAndDisplayErrorMessage(errorMessage, e);
                return default;
            }
        }

        private void LogAndDisplayErrorMessage(string userFriendlyErrorMessage, Exception exception)
        {
            // Log the exception (preferably in a centralized error log).
            Debug.WriteLine(exception);

            // Display a user-friendly message.
            _snackbarMessageService.ShowErrorSnackbar(userFriendlyErrorMessage);
        }
        #endregion
    }
}

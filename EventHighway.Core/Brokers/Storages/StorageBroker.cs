﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using EFxceptions;
using Microsoft.EntityFrameworkCore;

namespace EventHighway.Core.Brokers.Storages
{
    internal partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        private readonly string connectionString;

        public StorageBroker(string connectionString) =>
            this.connectionString = connectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
             optionsBuilder.UseSqlServer(this.connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureEvents(modelBuilder);
            ConfigureEventListeners(modelBuilder);
            ConfigureListenerEvent(modelBuilder);
        }

        private async ValueTask<T> InsertAsync<T>(T @object)
        {
            var broker = new StorageBroker(this.connectionString);
            broker.Entry(@object).State = EntityState.Added;
            await broker.SaveChangesAsync();

            return @object;
        }

        private async ValueTask<T> SelectAsync<T>(params object[] objectIds) where T : class
        {
            var broker = new StorageBroker(this.connectionString);

            return await broker.FindAsync<T>(objectIds);
        }

        private IQueryable<T> SelectAll<T>() where T : class
        {
            var broker = new StorageBroker(this.connectionString);

            return broker.Set<T>();
        }
    }
}

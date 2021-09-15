using System;
using HueCLI.Logic.Models;
using LiteDB;

namespace HueCLI.Logic
{
    public class ConfigurationStore : IDisposable
    {
        private readonly LiteDatabase _db;

        public ConfigurationStore()
        {
            _db = new LiteDatabase(@"huecli.db");
        }

        public Configuration GetConfiguration(string IPAddress)
        {
            var collection = _db.GetCollection<Configuration>("configurations");

            return collection.FindOne(c => c.IPAddress == IPAddress);
        }

        public void AddConfiguration(Configuration configuration)
        {
            var collection = _db.GetCollection<Configuration>("configurations");

            collection.Insert(configuration);
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _db.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~ConfigurationStore()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
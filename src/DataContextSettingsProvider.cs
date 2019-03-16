using System;

namespace Core.Identity.TableStorage
{
    /// <summary>
    /// Provider for Azure table storage connection settings.
    /// </summary>
    public class DataContextSettingsProvider : IDataContextSettingsProvider
    {
        /// <inheritdoc />
        public string AccountName { get; set; }
        /// <inheritdoc />
        public string KeyValue { get; set; }
        /// <inheritdoc />
        public Uri TableStorageUri { get; set; }
        /// <inheritdoc />
        public string PartitionKey { get; set; }
    }
}

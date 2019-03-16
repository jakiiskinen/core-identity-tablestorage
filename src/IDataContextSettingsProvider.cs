using System;

namespace Core.Identity.TableStorage
{
    /// <summary>
    /// Interface describing a provider for data context settings that are used to connect to the table storage.
    /// </summary>
    public interface IDataContextSettingsProvider
    {
        /// <summary>
        /// Name of the Azure storage account used to connect.
        /// </summary>
        string AccountName { get; set; }
        /// <summary>
        /// Access key of the storage account.
        /// </summary>
        string KeyValue { get; set; }               
        /// <summary>
        /// Partition key used in user data when storing it in the table storage.
        /// </summary>
        string PartitionKey { get; set; }
        /// <summary>
        /// Url of the table storage to be used.
        /// </summary>
        Uri TableStorageUri { get; set; }
    }
}
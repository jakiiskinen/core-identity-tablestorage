using System;

namespace Core.Identity.TableStorage
{
    public interface IDataContextSettingsProvider
    {
        string AccountName { get; set; }
        string KeyValue { get; set; }
        string PartitionKey { get; set; }
        Uri TableStorageUri { get; set; }
    }
}
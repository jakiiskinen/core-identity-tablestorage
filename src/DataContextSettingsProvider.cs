using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Identity.TableStorage
{
    public class DataContextSettingsProvider : IDataContextSettingsProvider
    {
        public string AccountName { get; set; }
        public string KeyValue { get; set; }
        public Uri TableStorageUri { get; set; }
        public string PartitionKey { get; set; }
    }
}

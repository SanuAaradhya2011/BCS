using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAB.License.DataStore
{
    /// <summary>
    /// Interface for dataStoremanager
    /// </summary>
    public interface IDataStore
    {
        void ReadDataFromIsolatedStorage();
        void WriteDataToIsolatedStorage();
    }
}

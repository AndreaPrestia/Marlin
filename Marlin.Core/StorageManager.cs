using Marlin.Core.Common;
using Marlin.Core.Interfaces;

namespace Marlin.Core
{
    internal class StorageManager
    {
        internal static IStorage Storage { get; private set; }

        static StorageManager()
        {
            Storage = Helper.CreateInstance<IStorage>(Settings.Current.StorageImplementationType);
        }
    }
}

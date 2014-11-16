using Antix.NuGet.Packages;

namespace Antix.NuGet.Events
{
    public static class EventBusExtensions
    {
        public const string PACKAGE_STORE_EVENT_TEMPLATE = "packageStoreEvent:{0}";

        public static void Raise(
            this IEventsBus bus,
            PackageStoreEvents e)
        {
            bus.Raise(new Event(
                string.Format(PACKAGE_STORE_EVENT_TEMPLATE, e))
                );
        }
    }
}
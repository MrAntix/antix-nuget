namespace Antix.NuGet.Events
{
    public interface IEventsBus 
    {
        void Raise(Event model);
    }
}
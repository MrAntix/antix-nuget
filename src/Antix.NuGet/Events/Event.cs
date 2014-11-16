namespace Antix.NuGet.Events
{
    public class Event
    {
        readonly string _name;
        readonly object _args;

        public Event(
            string name,
            object args = null)
        {
            _name = name;
            _args = args;
        }

        public string Name
        {
            get { return _name; }
        }

        public object Args
        {
            get { return _args; }
        }
    }
}
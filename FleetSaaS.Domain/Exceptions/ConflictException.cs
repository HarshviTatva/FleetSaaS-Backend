namespace FleetSaaS.Domain.Exceptions
{
    public class ConflictException : Exception
    {
        public string Field { get; }

        public ConflictException(string field,  string message)
            : base(message)
        {
            Field = field;
        }
    }
}

namespace FleetSaaS.Application.DTOs.Response
{
    public class DropdownResponse
    {
        public string Label { get; set; }
        public Guid? Value { get; set; }
        public bool? IsDisabled { get; set; }
    }
}

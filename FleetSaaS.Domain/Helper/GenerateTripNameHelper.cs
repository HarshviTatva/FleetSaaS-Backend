namespace FleetSaaS.Domain.Helper
{
    public static class GenerateTripNameHelper
    {
        public static string GenerateTripName(string origin, string destination, DateTime? plannedDate)
        {
            string from = string.IsNullOrWhiteSpace(origin)
            ? string.Empty
            : origin.Trim()[..Math.Min(3, origin.Trim().Length)].ToUpper();

            string to = string.IsNullOrWhiteSpace(destination)
                ? string.Empty
                : destination.Trim()[..Math.Min(3, destination.Trim().Length)].ToUpper();

            string datePart = plannedDate.HasValue
                ? plannedDate.Value.Day.ToString("D3")
                : "000";

            return $"{from}TO{to}{datePart}";
        }
    }
}

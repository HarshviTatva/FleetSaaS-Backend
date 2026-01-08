namespace FleetSaaS.Infrastructure.Common
{
    public class SystemConstant
    {
        public const string APPLICATION_JSON = "application/json";
        public const string PROJECT_NAME = "FleetSaaS";
        public const string REPORT_NAME = "Trip Details Report";
    }

    public class Fields
    {
        public const string Email = "email";
        public const string License_Number = "licenseNumber";
        public const string Vin = "vin";
        public const string License_Plate = "licensePlate";
        public const string RANDOM_STRING = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=[{]};:<>|./?";
        public const string NOT_ASSIGNED = "Not Assigned";
        public const string NO_DATA = "--";
        public const string PHONE_REGEX = @"^\+?\d{10,13}$";
        public const string VIN_REGEX = "^[A-HJ-NPR-Z0-9]{17}$";
        public const string LICENSE_PLATE_REGEX = "^[A-Z]{2}[0-9]{1,2}[A-Z]{1,3}[0-9]{4}$";
        public const string LICENSE_REGEX = "";
    }

    public class DateFormat
    {
        public static readonly string CURRENT_UTC_DATE =
            DateTime.UtcNow.ToString("dd MMM yyyy HH:mm");

        public static readonly string CURRENT_DATE =
            DateTime.Now.ToString("dd MMM yyyy HH:mm");

        public const string IST_DATE = "dd MMM yyyy, hh:mm tt";
    }
}

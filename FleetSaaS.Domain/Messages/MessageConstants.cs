namespace FleetSaaS.Domain.Common.Messages
{
    public class MessageConstants
    {
        public const string COMPANY_REGISTERED = "Company registered successfully. Please login to continue.";

        public const string USER_REGISTERED = "User registered successfully. Please login to continue.";

        public const string LOGIN_SUCCESS = "Logged in successfully!";

        public const string LOGOUT_SUCCESS = "Logged out successfully!";

        public const string UPDATED_MESSAGE = "{0} updated successfully.";

        public const string CREATED_MESSAGE = "{0} created successfully.";

        public const string SAVED_MESSAGE = "{0} saved successfully.";

        public const string INVALID_USER_DETAILS = "Invalid email or password.";

        public const string INVALID_CREDENTIALS = "Invalid credentials";

        public const string USER_EXISTS = "User already exists";

        public const string DATA_RETRIEVED = "Data retrieved successfully.";

        public const string DATA_RETRIEVAL_FAILED = "Data retrieval failed.";

        public const string NO_RECORD_FOUND = "No Record found.";

        public const string DELETED_MESSAGE = "{0} deleted successfully.";

        public const string CANCELLED_MESSAGE = "{0} cancelled successfully.";

        public const string ACCEPTED_MESSAGE = "{0} accepted successfully.";

        public const string STARTED_MESSAGE = "{0} started successfully.";

        public const string COMPLETED_MESSAGE = "{0} completed successfully.";

        public const string PLANNED_MESSAGE = "{0} planned successfully to driver.";

        public const string ASSIGNED_MESSAGE = "{0} assigned successfully to driver.";

        public const string REASSIGNED_MESSAGE = "{0} reassigned successfully to driver.";

        public const string UNASSIGNED_MESSAGE = "{0} unassigned successfully to driver.";

        public const string INVALID_TOKEN = "Invalid or expired refresh token!";

        public const string RESET_PASSWORD_MAIL = "Please check your mail to reset password.";
        
        public const string RESET_PASSWORD_SUCCESS = "Password updated successfully.";

        #region File
        public const string FILE_IS_NULL = "File is null or empty.";

        public const string FILE_NOT_FOUND = "File not found.";

        public const string FILE_TYPE_NOT_SUPPORTED = "File type not supported. Supported types are: {0}.";

        public const string FILE_SIZE_EXCEEDED = "File size exceeds the maximum limit of {0} MB.";

        public const string FILE_SAVE_FAILED = "File save failed.";

        public const string FILE_DELETE_FAILED = "File delete failed.";
        #endregion
    }

    public class Common
    {
        public const string CORS_POLICY_NAME = "AllowFleetSaaSApp";
        public const string SERILOG_MESSAGE = "HTTP {RequestMethod} {RequestPath} responded {StatusCode}";

    }

    public class TenantCommonMessages
    {
        public const string ALREADY_REGISTERED = "Company already registered";
        public const string INFO_NOT_FOUND = "Tenant information missing.";
        public const string INACTIVE_COMPANY = "Company is not active.";
    }

    public class DriverMessages
    {
        public const string LICENSE_NO_EXISTS = "License number already exists!";
    }

    public class VehicleMessages
    {
        public const string LICENSE_PLATE_EXISTS = "License plate already exists!";
        public const string VIN_EXISTS = "Duplicate Vehicle Identification number!";
    }

    public class ValidationMessages 
    {
        public const string FIELD_REQUIRED = "This field is required.";
        public const string EMAIL_REQUIRED = "Email is required.";
        public const string EMAIL_INVALID_FORMAT = "Invalid email format!";
        public const string PASSWORD_FORMAT = "Password must be at least 8 characters.";
        public const string PHONE_NUMBER_FORMAT = "Phone number must be 10 to 13 digits.";
        public const string VEHICLE_MAX_LENGTH = "Maximum length is of 15 characters only.";
        public const string VIN_FORMAT = "Please provide proper VIN format.";
        public const string LICENSE_PLATE_FORMAT = "Please provide proper license plate format.";
        public const string TRIP_MAX_LENGTH = "Maximum length is of 25 characters only.";
        public const string DESCRIPTION_MAX_LENGTH = "Maximum length is of 100 characters only.";
        public const string VALIDATION_FAILED = "Validation failed!";
        public const string LICENSE_NUMBER_FORMAT = "Invalid license number format!";
    }
}

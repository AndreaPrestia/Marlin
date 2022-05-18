namespace Marlin.Core
{
    public class Messages
    {
        public const string GenericFailure = "An error has occurred :(";
        public const string NotImplementedConstructor = "'{0}' has no implemented constructor";
        public const string ApiNotFound = "API '{0}' {1} not found";
        public const string ApiIsPublic = "API '{0}' {1} is public";
        public const string ApiConflict = "API '{0}' {1} is duplicated. Cannot resolve the endpoint.";
        public const string ApiBodyOnlyOne = "Only one body parameter has to be provided.";
        public const string ApiNotAuthorized = "API '{0}' {1} not authorized for user '{2}' with roles '{3}' in organization '{4}'";
        public const string ApiNotAuthorizedClaim = "API '{0}' {1} not authorized";
        public const string ApiKeySecretNotValid = "API Key secret not valid";
        public const string ApiKeyNotAvailable = "API Key not available for user '{0}' organization {1}";
        public const string ConfigurationNotProvided = "Configuration '{0}' not provided";
        public const string TokenFormat = "JWT";
        public const string TokenScheme = "Bearer";
        public const string TokenNotProvided = "Bearer";
        public const string TokenInvalid = "Invalid token";
        public const string TokenInvalidPayload = "Invalid token payload";
        public const string TokenInvalidClaims = "Invalid token claims";
        public const string TokenExpired = "Token expired";
        public const string TokenClaimNotProvided = "Claim '{0}' not provided";
        public const string TokenClaimAlreadyProvided = "Claim '{0}' already provided";
        public const string TokenInvalidIss = "Invalid token issuer";
        public const string TokenInvalidAud = "Invalid token audience";
        public const string TokenEnter = "Enter a valid token";
        public const string HeaderAuthorization = "Authorization";
        public const string HeaderOrganization = "Organization";
        public const string HeaderNotProvided = "Header '{0}' not provided";
        public const string ParameterNotProvided = "Parameter '{0}' not provided";
        public const string EntityNotProvided = "Entity '{0}' not provided";
        public const string Unauthorized = "Unauthorized";
        public const string RequestNotProvided = "Request not provided";
    }
}

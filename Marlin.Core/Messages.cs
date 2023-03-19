namespace Marlin.Core
{
    internal static class Messages
    {
        internal const string GenericFailure = "An error has occurred :(";
        internal const string NotImplementedConstructor = "'{0}' has no implemented constructor";
        internal const string ApiNotFound = "API '{0}' {1} not found";
        internal const string ApiConflict = "API '{0}' {1} is duplicated. Cannot resolve the endpoint.";
        internal const string ApiBodyOnlyOne = "Only one body parameter has to be provided.";
        internal const string ApiNotAuthorizedClaim = "API '{0}' {1} not authorized";
        internal const string TokenNotProvided = "Bearer";
        internal const string TokenInvalidPayload = "Invalid token payload";
        internal const string TokenInvalidClaims = "Invalid token claims";
        internal const string TokenExpired = "Token expired";
        internal const string TokenClaimNotProvided = "Claim '{0}' not provided";
        internal const string TokenInvalidIss = "Invalid token issuer";
        internal const string TokenInvalidAud = "Invalid token audience";
        internal const string HeaderAuthorization = "Authorization";
        internal const string HeaderNotProvided = "Header '{0}' not provided";
        internal const string ParameterNotProvided = "Parameter '{0}' not provided";
        internal const string EntityNotProvided = "Entity '{0}' not provided";
        internal const string Unauthorized = "Unauthorized";
        internal const string ContextNotLoaded = "Context is not loaded";
        internal const string InvalidContentType = "Content-Type '{0}' is not allowed. Only '{1}' allowed.";
    }
}

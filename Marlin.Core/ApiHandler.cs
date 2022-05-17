using Microsoft.AspNetCore.Http;

namespace Marlin.Core
{
    public abstract class ApiHandler
    {
        private ApiOutput GetResult(int statusCode, object content, string contentType)
        {
            return new ApiOutput() { ContentType = contentType, StatusCode = statusCode, Response = content != null ? Utility.Serialize(content) : string.Empty };
        }

        public ApiOutput Accepted(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status202Accepted, content, contentType);
        }

        public ApiOutput AlreadyReported(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status208AlreadyReported, content, contentType);
        }

        public ApiOutput AuthenticationTimeout(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status419AuthenticationTimeout, content, contentType);
        }

        public ApiOutput BadGateway(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status502BadGateway, content, contentType);
        }

        public ApiOutput BadRequest(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status400BadRequest, content, contentType);
        }

        public ApiOutput Conflict(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status409Conflict, content, contentType);
        }

        public ApiOutput Continue(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status100Continue, content, contentType);
        }

        public ApiOutput Created(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status201Created, content, contentType);
        }

        public ApiOutput Custom(int statusCode, object content = null, string contentType = "application/json")
        {
            return GetResult(statusCode, content, contentType);
        }

        public ApiOutput ExpectationFailed(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status417ExpectationFailed, content, contentType);
        }

        public ApiOutput FailedDependency(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status424FailedDependency, content, contentType);
        }

        public ApiOutput Forbidden(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status403Forbidden, content, contentType);
        }

        public ApiOutput Found(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status302Found, content, contentType);
        }

        public ApiOutput GatewayTimeout(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status504GatewayTimeout, content, contentType);
        }

        public ApiOutput Gone(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status410Gone, content, contentType);
        }

        public ApiOutput HttpVersionNotsupported(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status505HttpVersionNotsupported, content, contentType);
        }

        public ApiOutput ImATeapot(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status418ImATeapot, content, contentType);
        }

        public ApiOutput IMUsed(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status226ImUsed, content, contentType);
        }

        public ApiOutput InsufficientStorage(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status507InsufficientStorage, content, contentType);
        }

        public ApiOutput InternalServerError(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status500InternalServerError, content, contentType);
        }

        public ApiOutput LengthRequired(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status411LengthRequired, content, contentType);
        }

        public ApiOutput Locked(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status423Locked, content, contentType);
        }

        public ApiOutput LoopDetected(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status508LoopDetected, content, contentType);
        }

        public ApiOutput MethodNotAllowed(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status405MethodNotAllowed, content, contentType);
        }

        public ApiOutput MisdirectedRequest(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status421MisdirectedRequest, content, contentType);
        }

        public ApiOutput MovedPermanently(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status301MovedPermanently, content, contentType);
        }

        public ApiOutput MultipleChoices(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status300MultipleChoices, content, contentType);
        }

        public ApiOutput MultiStatus(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status207MultiStatus, content, contentType);
        }

        public ApiOutput NetworkAuthenticationRequired(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status511NetworkAuthenticationRequired, content, contentType);
        }

        public ApiOutput NoContent(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status204NoContent, content, contentType);
        }

        public ApiOutput NonAuthoritative(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status203NonAuthoritative, content, contentType);
        }

        public ApiOutput NotAcceptable(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status406NotAcceptable, content, contentType);
        }

        public ApiOutput NotExtended(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status510NotExtended, content, contentType);
        }

        public ApiOutput NotFound(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status404NotFound, content, contentType);
        }

        public ApiOutput NotImplemented(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status501NotImplemented, content, contentType);
        }

        public ApiOutput NotModified(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status304NotModified, content, contentType);
        }

        public ApiOutput Ok(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status200Ok, content, contentType);
        }

        public ApiOutput PartialContent(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status206PartialContent, content, contentType);
        }

        public ApiOutput PayloadTooLarge(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status413PayloadTooLarge, content, contentType);
        }

        public ApiOutput PaymentRequired(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status402PaymentRequired, content, contentType);
        }

        public ApiOutput PermanentRedirect(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status308PermanentRedirect, content, contentType);
        }

        public ApiOutput PreconditionFailed(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status412PreconditionFailed, content, contentType);
        }

        public ApiOutput PreconditionRequired(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status428PreconditionRequired, content, contentType);
        }

        public ApiOutput Processing(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status102Processing, content, contentType);
        }

        public ApiOutput ProxyAuthenticationRequired(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status407ProxyAuthenticationRequired, content, contentType);
        }

        public ApiOutput RequestedRangeNotSatisfiable(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status416RequestedRangeNotSatisfiable, content, contentType);
        }

        public ApiOutput RequestEntityToLarge(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status413RequestEntityTooLarge, content, contentType);
        }

        public ApiOutput RequestHeaderFieldsTooLarge(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status431RequestHeaderFieldsTooLarge, content, contentType);
        }

        public ApiOutput RequestTimeout(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status408RequestTimeout, content, contentType);
        }

        public ApiOutput RequestUriTooLong(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status414RequestUriTooLong, content, contentType);
        }

        public ApiOutput ResetContent(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status205ResetContent, content, contentType);
        }

        public ApiOutput SeeOther(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status303SeeOther, content, contentType);
        }

        public ApiOutput ServiceUnavailable(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status503ServiceUnavailable, content, contentType);
        }

        public ApiOutput SwitchingProtocols(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status101SwitchingProtocols, content, contentType);
        }

        public ApiOutput SwitchProxy(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status306SwitchProxy, content, contentType);
        }

        public ApiOutput TemporaryRedirect(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status307TemporaryRedirect, content, contentType);
        }

        public ApiOutput TooManyRequests(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status429TooManyRequests, content, contentType);
        }

        public ApiOutput Unauthorized(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status401Unauthorized, content, contentType);
        }

        public ApiOutput UnavailableForLegalReasons(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status451UnavailableForLegalReasons, content, contentType);
        }

        public ApiOutput UnprocessableEntity(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status422UnprocessableEntity, content, contentType);
        }

        public ApiOutput UnsupportedMediaType(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status415UnsupportedMediaType, content, contentType);
        }

        public ApiOutput UpgradeRequired(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status426UpgradeRequired, content, contentType);
        }

        public ApiOutput UriTooLong(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status414UriTooLong, content, contentType);
        }

        public ApiOutput UseProxy(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status305UseProxy, content, contentType);
        }

        public ApiOutput VariantAlsoNegotiates(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status506VariantAlsoNegotiates, content, contentType);
        }
    }
}

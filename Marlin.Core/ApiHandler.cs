using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Marlin.Core
{
    public abstract class ApiHandler
    {
        private static readonly JsonSerializerSettings Options = new()
            { StringEscapeHandling = StringEscapeHandling.Default };

        private static ApiOutput GetResult(int statusCode, object content, string contentType)
        {
            return new ApiOutput(content != null ? JsonConvert.SerializeObject(content, Options) : string.Empty,
                statusCode, contentType);
        }

        protected ApiOutput Accepted(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status202Accepted, content, contentType);
        }

        protected ApiOutput AlreadyReported(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status208AlreadyReported, content, contentType);
        }

        protected ApiOutput AuthenticationTimeout(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status419AuthenticationTimeout, content, contentType);
        }

        protected ApiOutput BadGateway(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status502BadGateway, content, contentType);
        }

        protected ApiOutput BadRequest(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status400BadRequest, content, contentType);
        }

        protected ApiOutput Conflict(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status409Conflict, content, contentType);
        }

        protected ApiOutput Continue(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status100Continue, content, contentType);
        }

        protected ApiOutput Created(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status201Created, content, contentType);
        }

        protected ApiOutput Custom(int statusCode, object content = null, string contentType = "application/json")
        {
            return GetResult(statusCode, content, contentType);
        }

        protected ApiOutput ExpectationFailed(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status417ExpectationFailed, content, contentType);
        }

        protected ApiOutput FailedDependency(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status424FailedDependency, content, contentType);
        }

        protected ApiOutput Forbidden(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status403Forbidden, content, contentType);
        }

        protected ApiOutput Found(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status302Found, content, contentType);
        }

        protected ApiOutput GatewayTimeout(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status504GatewayTimeout, content, contentType);
        }

        protected ApiOutput Gone(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status410Gone, content, contentType);
        }

        protected ApiOutput HttpVersionNotSupported(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status505HttpVersionNotsupported, content, contentType);
        }

        protected ApiOutput ImATeapot(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status418ImATeapot, content, contentType);
        }

        protected ApiOutput ImUsed(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status226IMUsed, content, contentType);
        }

        protected ApiOutput InsufficientStorage(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status507InsufficientStorage, content, contentType);
        }

        protected ApiOutput InternalServerError(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status500InternalServerError, content, contentType);
        }

        protected ApiOutput LengthRequired(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status411LengthRequired, content, contentType);
        }

        protected ApiOutput Locked(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status423Locked, content, contentType);
        }

        protected ApiOutput LoopDetected(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status508LoopDetected, content, contentType);
        }

        protected ApiOutput MethodNotAllowed(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status405MethodNotAllowed, content, contentType);
        }

        protected ApiOutput MisdirectedRequest(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status421MisdirectedRequest, content, contentType);
        }

        protected ApiOutput MovedPermanently(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status301MovedPermanently, content, contentType);
        }

        protected ApiOutput MultipleChoices(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status300MultipleChoices, content, contentType);
        }

        protected ApiOutput MultiStatus(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status207MultiStatus, content, contentType);
        }

        protected ApiOutput NetworkAuthenticationRequired(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status511NetworkAuthenticationRequired, content, contentType);
        }

        protected ApiOutput NoContent(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status204NoContent, content, contentType);
        }

        protected ApiOutput NonAuthoritative(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status203NonAuthoritative, content, contentType);
        }

        protected ApiOutput NotAcceptable(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status406NotAcceptable, content, contentType);
        }

        protected ApiOutput NotExtended(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status510NotExtended, content, contentType);
        }

        protected ApiOutput NotFound(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status404NotFound, content, contentType);
        }

        protected ApiOutput NotImplemented(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status501NotImplemented, content, contentType);
        }

        protected ApiOutput NotModified(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status304NotModified, content, contentType);
        }

        protected ApiOutput Ok(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status200OK, content, contentType);
        }

        protected ApiOutput PartialContent(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status206PartialContent, content, contentType);
        }

        protected ApiOutput PayloadTooLarge(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status413PayloadTooLarge, content, contentType);
        }

        protected ApiOutput PaymentRequired(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status402PaymentRequired, content, contentType);
        }

        protected ApiOutput PermanentRedirect(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status308PermanentRedirect, content, contentType);
        }

        protected ApiOutput PreconditionFailed(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status412PreconditionFailed, content, contentType);
        }

        protected ApiOutput PreconditionRequired(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status428PreconditionRequired, content, contentType);
        }

        protected ApiOutput Processing(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status102Processing, content, contentType);
        }

        protected ApiOutput ProxyAuthenticationRequired(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status407ProxyAuthenticationRequired, content, contentType);
        }

        protected ApiOutput RequestedRangeNotSatisfiable(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status416RequestedRangeNotSatisfiable, content, contentType);
        }

        protected ApiOutput RequestEntityToLarge(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status413RequestEntityTooLarge, content, contentType);
        }

        protected ApiOutput RequestHeaderFieldsTooLarge(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status431RequestHeaderFieldsTooLarge, content, contentType);
        }

        protected ApiOutput RequestTimeout(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status408RequestTimeout, content, contentType);
        }

        protected ApiOutput RequestUriTooLong(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status414RequestUriTooLong, content, contentType);
        }

        protected ApiOutput ResetContent(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status205ResetContent, content, contentType);
        }

        protected ApiOutput SeeOther(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status303SeeOther, content, contentType);
        }

        protected ApiOutput ServiceUnavailable(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status503ServiceUnavailable, content, contentType);
        }

        protected ApiOutput SwitchingProtocols(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status101SwitchingProtocols, content, contentType);
        }

        protected ApiOutput SwitchProxy(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status306SwitchProxy, content, contentType);
        }

        protected ApiOutput TemporaryRedirect(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status307TemporaryRedirect, content, contentType);
        }

        protected ApiOutput TooManyRequests(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status429TooManyRequests, content, contentType);
        }

        protected ApiOutput Unauthorized(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status401Unauthorized, content, contentType);
        }

        protected ApiOutput UnavailableForLegalReasons(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status451UnavailableForLegalReasons, content, contentType);
        }

        protected ApiOutput UnprocessableEntity(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status422UnprocessableEntity, content, contentType);
        }

        protected ApiOutput UnsupportedMediaType(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status415UnsupportedMediaType, content, contentType);
        }

        protected ApiOutput UpgradeRequired(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status426UpgradeRequired, content, contentType);
        }

        protected ApiOutput UriTooLong(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status414UriTooLong, content, contentType);
        }

        protected ApiOutput UseProxy(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status305UseProxy, content, contentType);
        }

        protected ApiOutput VariantAlsoNegotiates(object content = null, string contentType = "application/json")
        {
            return GetResult(StatusCodes.Status506VariantAlsoNegotiates, content, contentType);
        }
    }
}
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Diagnostics.CodeAnalysis;

namespace DotnetMSWorkshop.Utils.Formatters
{
    // Input Type Formatter to allow model binding to Streams
    [ExcludeFromCodeCoverage]
    public class InputFormatterStream : InputFormatter
    {
        public InputFormatterStream()
        {
            SupportedMediaTypes.Add("application/octet-stream");
            SupportedMediaTypes.Add("image/jpeg");
        }

        protected override bool CanReadType(Type type)
        {
            if (type == typeof(Stream))
            {
                return true;
            }

            return false;
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            return InputFormatterResult.SuccessAsync(context.HttpContext.Request.Body);
        }
    }
}
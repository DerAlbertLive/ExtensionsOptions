using Microsoft.Extensions.Options;

namespace Options.Tests
{
    public class MyOtherOptionsValidator : IValidateOptions<MyOtherOptions>
    {
        public ValidateOptionsResult Validate(string name, MyOtherOptions options)
        {
            if (options.Unknown == "illegal")
            {
                return ValidateOptionsResult.Fail("Alles illegal");
            }
            return ValidateOptionsResult.Success;
        }
    }
}
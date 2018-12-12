using Microsoft.Extensions.Options;

namespace Options.Tests
{
    public class MyTest
    {
        private readonly IOptions<MyOptions> _optionsAccessor;
        private readonly IOptions<MyOtherOptions> _otherAccessor;

        public MyTest(IOptions<MyOptions> optionsAccessor, IOptions<MyOtherOptions> otherAccessor)
        {
            _optionsAccessor = optionsAccessor;
            _otherAccessor = otherAccessor;
        }

        public string GetName()
        {
            return _optionsAccessor.Value.Name;
        }

        public string GetFullName()
        {
            return _otherAccessor.Value.FullName;
        }
    }
}
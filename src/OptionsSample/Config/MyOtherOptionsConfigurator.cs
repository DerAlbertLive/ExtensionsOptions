using Microsoft.Extensions.Options;

namespace Options.Tests
{
    public class MyOtherOptionsConfigurator : IConfigureOptions<MyOtherOptions>
    {
        private readonly IOptions<MyOptions> _optionsAccessor;

        public MyOtherOptionsConfigurator(IOptions<MyOptions> optionsAccessor)
        {
            _optionsAccessor = optionsAccessor;
        }
        public void Configure(MyOtherOptions options)
        {
            options.FullName = $"{_optionsAccessor.Value.Name} Weinert";
        }
    }
}
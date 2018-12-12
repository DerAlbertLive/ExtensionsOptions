using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;
using Xunit.Abstractions;

namespace Options.Tests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _outputHelper;

        public UnitTest1(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        private IServiceProvider CreateServiceProvider(Action<IServiceCollection> configureServices)
        {
            var services = new ServiceCollection();
            services.AddTransient<MyTest>();
            services.AddOptions();
            configureServices(services);
            return services.BuildServiceProvider();
        }

        [Fact]
        public void NameIsEmpty()
        {
            var sp = CreateServiceProvider(services =>
             {
             });

            var test = sp.GetRequiredService<MyTest>();

            test.GetName().Should().BeNull();
        }

        [Fact]
        public void NameShouldBeAlbert()
        {
            _outputHelper.WriteLine(nameof(NameShouldBeAlbert));

            var sp = CreateServiceProvider(services =>
            {
                _outputHelper.WriteLine(nameof(CreateServiceProvider));
                services.Configure<MyOptions>(o =>
                {
                    _outputHelper.WriteLine("Options Initialized");

                    o.Name = "Albert";
                });
            });
            _outputHelper.WriteLine("GetService");

            var test = sp.GetRequiredService<MyTest>();
            test.GetName().Should().Be("Albert");
            _outputHelper.WriteLine("Fertig");
        }

        [Fact]
        public void ConfigurationTest()
        {
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                {"Name", "Peter"},
                {"Foo:Name", "Albert"}
            });
            IConfiguration configuration = builder.Build();
            configuration = configuration.GetSection("Foo");

            var sp = CreateServiceProvider(services =>
            {
                services.Configure<MyOptions>(configuration);
            });
            var test = sp.GetRequiredService<MyTest>();
            test.GetName().Should().Be("Albert");
        }

        [Fact]
        public void ConfigureOptions()
        {
            var sp = CreateServiceProvider(services =>
            {
                services.Configure<MyOptions>(o=>o.Name ="Albert");
                services.AddTransient<IConfigureOptions<MyOtherOptions>, MyOtherOptionsConfigurator>();
            });
            var test = sp.GetRequiredService<MyTest>();
            test.GetFullName().Should().Be("Albert Weinert");
        }

        [Fact]
        public void ConfigureOptionsDirect()
        {
            var sp = CreateServiceProvider(services =>
            {
                services.Configure<MyOptions>(o=>o.Name ="");
                services.AddOptions<MyOtherOptions>()
                    .Configure<IOptions<MyOptions>>((o, optionsAccessor) =>
                    {
                        var myOptions = optionsAccessor.Value;
                        if (!string.IsNullOrWhiteSpace(myOptions.Name))
                        {
                            o.FullName = $"{myOptions.Name} Dei";
                        }
                    })
                    .PostConfigure(o =>
                    {
                        if (string.IsNullOrWhiteSpace(o.FullName))
                        {
                            o.FullName = "Default Name";
                        }

                    //    o.Unknown = "illegal";
                    })
                  //  .ValidateDataAnnotations()
                    //.Validate(o =>
                    //{
                    //    if (o.Unknown == null)
                    //    {
                    //        return false;
                    //    }

                    //    return true;
                    //}, "Unknown ist nicht vorhanden");
                    ;
                services.AddTransient<IValidateOptions<MyOtherOptions>, MyOtherOptionsValidator>();

            });
            var test = sp.GetRequiredService<MyTest>();
            test.GetFullName().Should().Be("Default Name");
        }
    }
}

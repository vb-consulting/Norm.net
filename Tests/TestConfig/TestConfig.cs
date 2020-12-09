using System;
using Microsoft.Extensions.Configuration;

namespace TestConfig
{
    public class TestConfig
    {
        public string Default { get; set; }
        public string TestDatabase { get; set; }
    }

    public static class Config
    {
        public static TestConfig Value { get; }

        static Config()
        {
            var value = new TestConfig();
            new ConfigurationBuilder()
                .AddJsonFile("testsettings.json", true, false)
                .AddJsonFile("testsettings.local.json", true, false)
                .Build()
                .Bind("db", value);
            Value = value;
        }
    }
}

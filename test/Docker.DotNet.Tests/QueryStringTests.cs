using System;
using System.Collections.Generic;
using Docker.DotNet.Models;
using Xunit;

namespace Docker.DotNet.Tests
{
    public class QueryStringTests
    {
        [Fact]
        public void ServiceListParameters_GenerateIdFilters()
        {
            var p = new ServiceListParameters
            {
                Filters = new Dictionary<string, IDictionary<string, bool>>
                {
                    ["id"] = new Dictionary<string, bool>
                    {
                        ["service-id"] = true
                    }
                }
            };

            var qs = new QueryString<ServiceListParameters>(p);

            Assert.Equal("filters={\"id\":{\"service-id\":true}}", Uri.UnescapeDataString(qs.GetQueryString()));
        }

        [Fact]
        public void ServiceListParameters_GenerateCompositeFilters()
        {
            var p = new ServiceListParameters
            {
                Filters = new Dictionary<string, IDictionary<string, bool>>
                {
                    ["id"] = new Dictionary<string, bool>
                    {
                        ["service-id"] = true
                    },
                    ["label"] = new Dictionary<string, bool>
                    {
                        ["label"] = true
                    }
                }
            };

            var qs = new QueryString<ServiceListParameters>(p);

            Assert.Equal("filters={\"id\":{\"service-id\":true},\"label\":{\"label\":true}}", Uri.UnescapeDataString(qs.GetQueryString()));
        }

        [Fact]
        public void ServiceListParameters_GenerateNullFilters()
        {
            var p = new ServiceListParameters { Filters = new Dictionary<string, IDictionary<string, bool>>() };
            var qs = new QueryString<ServiceListParameters>(p);

            Assert.Equal("filters={}", Uri.UnescapeDataString(qs.GetQueryString()));
        }

        [Fact]
        public void ServiceListParameters_GenerateNullModeFilters()
        {
            var p = new ServiceListParameters { Filters = new Dictionary<string, IDictionary<string, bool>> { ["mode"] = new Dictionary<string, bool>() } };
            var qs = new QueryString<ServiceListParameters>(p);

            Assert.Equal("filters={\"mode\":{}}", Uri.UnescapeDataString(qs.GetQueryString()));
        }
    }
}

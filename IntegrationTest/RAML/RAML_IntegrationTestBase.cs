using System;
using NUnit.Framework;

namespace IntegrationTest.RAML
{
    public class RAML_IntegrationTestBase : IntegrationWireMockTestsBase
    {
        [OneTimeSetUp]
        public void InitBase()
        {
            this.scenarioHeaderKey = "Content-Type";
            this.scenarioHeaderValue = "application/json";
        }
    }
}

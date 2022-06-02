using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WireMock.Admin.Mappings;

namespace IntegrationTest.RAML.CohesionCustomer
{
    public class CohesionCustomer_RAML_IntegrationTestBase : RAML_IntegrationTestBase
    {
        [OneTimeSetUp]
        public new void InitBase()
        {
        }
    }
}

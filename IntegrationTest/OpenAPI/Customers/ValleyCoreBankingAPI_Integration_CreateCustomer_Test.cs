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

namespace IntegrationTest.OpenAPI.Customers
{
    public class ValleyCoreBankingAPI_Integration_CreateCustomer_Test : ValleyCoreBankingAPI_IntegrationTestBase
    {
        [OneTimeSetUp]
        public void Init()
        {
            this.scenarioPath = "/core/v1/customers/";
            this.scenarioTitle = "";
            this.scenarioHeaderKey = "Content-Type";
            this.scenarioHeaderValue = "application/json";
            this.scenarioVerb = "POST";
            this.scenarioPathRegEx = new Regex($@"{this.scenarioPath}$", RegexOptions.IgnoreCase);

            methods = new string[] { this.scenarioVerb };

            bodyModel = new BodyModel()
            {
                Matcher = new MatcherModel()
                {
                    Name = "JsonMatcher",
                    IgnoreCase = true,
                    Patterns = new object[]
                    {
                        WireMockHelper.FixEndOfLineCharacterInString("{\n  \"taxIds\": [\n    {\n      \"taxIdNumber\": \"982233333\",\n      \"taxIdType\": \"SSN\"\n    }\n  ],\n  \"business\": {\n    \"name\": \"Business Name\"\n  },\n  \"name\": {\n    \"first\": \"Jairo\",\n    \"middle\": \"Alex\",\n    \"last\": \"Smith\",\n    \"maiden\": \"Adams\",\n    \"suffix\": \"Senior\"\n  },\n  \"addresses\": [\n    {\n      \"addressType\": \"HOME_ADDRESS\",\n      \"address1\": \"11 hope st\",\n      \"address2\": \"first floor\",\n      \"city\": \"Queens\",\n      \"state\": \"NY\",\n      \"postalCode\": \"11385\"\n    }\n  ],\n  \"phoneNumbers\": [\n    {\n      \"number\": \"2123221522\",\n      \"type\": \"BUSINESS\",\n      \"extension\": \"123\"\n    },\n    {\n      \"number\": \"9174444444\",\n      \"type\": \"HOME\"\n    },\n    {\n      \"number\": \"9171111111\",\n      \"type\": \"CELL\"\n    }\n  ],\n  \"identifications\": [\n    {\n      \"identificationType\": \"PASSPORT\",\n      \"identificationValue\": \"1237896458049\",\n      \"identificationIssuer\": \"CA\",\n      \"identificationIssueDate\": \"20180409\",\n      \"identificationExpDate\": \"20280408\"\n    },\n    {\n      \"identificationType\": \"DRIVERS_LICENSE\",\n      \"identificationValue\": \"1237896458046\",\n      \"identificationIssuer\": \"NJ\",\n      \"identificationIssueDate\": \"20180409\",\n      \"identificationExpDate\": \"20220408\"\n    }\n  ],\n  \"emailAddresses\": [\n    {\n      \"emailAddress\": \"foo@bar.com\"\n    },\n    {\n      \"emailAddress\": \"joe@baz.com\"\n    }\n  ],\n  \"dateOfBirth\": \"19610830\",\n  \"employer\": \"Acme\",\n  \"origBranch\": \"002\",\n  \"occupation\": \"Bus driver\",\n  \"onlineBanking\": true\n}", this.endOfLine)
                    }
                }
            };

            headerModelsAuxiliarString = new List<string>
            {
                "X-Correlation-ID"
            };

            const string configuredResponseBody = "{\"responseCode\": \"OK\",\"data\": {\"customerId\": \"0040315509\",\"name\": {\"middle\": \"ALEX\",\"last\": \"SMITH\",\"displayName\": \"JAIRO ALEX SMITH SENIOR\",\"suffix\": \"SENIOR\",\"first\": \"JAIRO\",\"maiden\": \"ADAMS\"},\"taxIds\": [{\"taxIdNumber\": \"982233333\",\"taxIdType\": \"SSN\"}],\"identifications\": [{\"identificationType\": \"PASSPORT\",\"identificationValue\": \"1237896458049\",\"identificationIssuer\": \"CA\",\"identificationIssueDate\": \"20180409\",\"identificationExpDate\": \"20280408\"},{\"identificationType\": \"DRIVERS_LICENSE\",\"identificationValue\": \"1237896458046\",\"identificationIssuer\": \"NJ\",\"identificationIssueDate\": \"20180409\",\"identificationExpDate\": \"20220408\"}],\"addresses\": [{\"address1\": \"11 HOPE ST FIRST FLOOR\",\"addressIndicator\": \"0\",\"city\": \"QUEENS\",\"state\": \"NY\",\"postalCode\": \"11385\"}],\"emailAddresses\": [{\"emailAddress\": \"foo@bar.com\"},{\"emailAddress\": \"joe@baz.com\"}],\"phoneNumbers\": [{\"number\": \"9171111111\",\"type\": \"CELL\"},{\"number\": \"9174444444\",\"type\": \"HOME\"},{\"number\": \"2123221522\",\"type\": \"BUSINESS\",\"extension\": \"000123\"}],\"origBranch\": \"002\",\"privacyIndicator\": \"N\",\"affInfoIndicator\": \"0\",\"occupation\": \"BUS DRIVER\",\"employer\": \"ACME\",\"dateOfBirth\": \"19610830\",\"custType\": \"P\",\"employeeCd\": \"0\",\"onlineBanking\": true}}";
            responseBody = JObject.Parse(configuredResponseBody);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_CreateCustomer_When_Check_Configuration_Scenario_Then_Exist()
        {
            var mapping = GetMapping();
            Assert.NotNull(mapping.Request);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_CreateCustomer_When_Check_Configuration_Scenario_Then_Exist_Once()
        {
            var mapping = GetMappings();
            Assert.AreEqual(1, mapping.Count);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_CreateCustomer_When_Check_Configuration_Path_Then_Exist()
        {
            var mapping = GetMapping();
            PathModel path = JsonConvert.DeserializeObject<PathModel>(mapping.Request.Path.ToString());
            Assert.IsTrue(path.Matchers.Any(c => c.Pattern.ToString() == this.scenarioPath));
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_CreateCustomer_When_Check_Configuration_Request_Method_Then_Is_POST()
        {
            var mapping = GetMapping();
            Assert.AreEqual(this.scenarioVerb, mapping.Request.Methods.First().ToString());
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_CreateCustomer_When_Check_Configuration_Query_Params_Then_Not_Exist()
        {
            var mapping = GetMapping();
            Assert.IsNull(mapping.Request.Params);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_CreateCustomer_When_Check_Configuration_Request_Body_Then_Is_Expected()
        {
            var mapping = GetMapping();
            Assert.AreEqual(bodyModel.Matcher.Patterns.First().ToString(), mapping.Request.Body.Matcher.Patterns.First().ToString());
        }


        [Test]
        public void Given_ValleyCoreBankingAPI_CreateCustomer_When_Check_Configuration_Headers_Then_There_Is_Not_Diffeence()
        {
            var mapping = GetMapping();
            List<string> actualRequestHeaders = new();
            foreach (HeaderModel param in mapping.Request.Headers)
            {
                actualRequestHeaders.Add(param.Name);
            }
            Assert.IsTrue(actualRequestHeaders.Except(headerModelsAuxiliarString).ToList().Count == 0);
        }


        [Test]
        public void Given_ValleyCoreBankingAPI_CreateCustomer_When_Check_Configuration_Response_Type_Then_Equal_Application_Json()
        {
            var mapping = GetMapping();
            Assert.IsTrue(mapping.Response.Headers.Any(h => h.Key == this.scenarioHeaderKey && h.Value.ToString() == this.scenarioHeaderValue));
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_CreateCustomer_When_Check_Configuration_Response_Status_Code_Then_Equal_201()
        {
            var mapping = GetMapping();
            Assert.AreEqual((long)HttpStatusCode.Created, (long)mapping.Response.StatusCode);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_CreateCustomer_When_Check_Configuration_Response_Body_Then_Is_Expected()
        {
            var mapping = GetMapping();
            Assert.AreEqual(responseBody, JObject.Parse(mapping.Response.BodyAsJson.ToString()));
        }

        [Test]
        public async Task Given_ValleyCoreBankingAPI_CreateCustomer_When_Execute_Request_Then_Response_Body_Is_Expected()
        {
            var mapping = GetMapping();

            UriBuilder requestEndpointURI = new(requestURI + this.scenarioPath[1..]);
            requestEndpointURI.Query = WireMockHelper.AppendQueryParams(mapping);

            HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, requestEndpointURI.Uri);
            WireMockHelper.AppendHeaders(mapping, ref httpRequestMessage);
            WireMockHelper.AppendRequestBody(mapping, ref httpRequestMessage);


            var httpResponse = await _client.SendAsync(httpRequestMessage);
            dynamic httResponseDeserialize = JsonConvert.DeserializeObject(await httpResponse.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.Created, httpResponse.StatusCode);
            Assert.AreEqual(mapping.Response.BodyAsJson, httResponseDeserialize);
        }
    }
}

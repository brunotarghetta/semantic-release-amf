using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using WireMock.Admin.Mappings;

namespace IntegrationTest.OpenAPI.Customers
{
    public class ValleyCoreBankingAPI_Integration_SetBeneficiary_Test : ValleyCoreBankingAPI_IntegrationTestBase
    {

        [OneTimeSetUp]
        public void Init()
        {
            this.scenarioPathRegEx = new Regex("/core/v1/customers/beneficiaries/$", RegexOptions.IgnoreCase);
            this.scenarioHeaderKey = "Content-Type";
            this.scenarioHeaderValue = "application/json";
            this.scenarioVerb = "POST";

            methods = new string[] { this.scenarioVerb };

            bodyModel = new BodyModel()
            {
                Matcher = new MatcherModel()
                {
                    Name = "JsonMatcher",
                    IgnoreCase = true,
                    Patterns = new object[]
                    {
                        WireMockHelper.FixEndOfLineCharacterInString("{\n  \"accountId\": \"500000020141\",\n  \"productCode\": \"ALL_ACCESS_SAVINGS\",\n  \"beneficiaries\": [\n    {\n      \"beneficiaryId\": \"\",\n      \"name\": {\n        \"first\": \"Jairo\",\n        \"last\": \"Smith\"\n      },\n      \"taxId\": {\n        \"taxIdNumber\": \"982233333\",\n        \"taxIdType\": \"SSN\"\n      },\n      \"addresses\": [\n        {\n          \"addressType\": \"HOME_ADDRESS\",\n          \"address1\": \"11 hope st\",\n          \"address2\": \"first floor\",\n          \"city\": \"Queens\",\n          \"state\": \"NY\",\n          \"postalCode\": \"11385\"\n        }\n      ],\n      \"birthDate\": \"19610830\",\n      \"origBranch\": \"002\"\n    },\n    {\n      \"beneficiaryId\": \"\",\n      \"name\": {\n        \"first\": \"Jairo\",\n        \"last\": \"Smith\"\n      },\n      \"taxId\": {\n        \"taxIdNumber\": \"982233331\",\n        \"taxIdType\": \"SSN\"\n      },\n      \"addresses\": [\n        {\n          \"addressType\": \"HOME_ADDRESS\",\n          \"address1\": \"11 hope st\",\n          \"address2\": \"first floor\",\n          \"city\": \"Queens\",\n          \"state\": \"NY\",\n          \"postalCode\": \"11385\"\n        }\n      ],\n      \"birthDate\": \"19870830\",\n      \"origBranch\": \"002\"\n    }\n  ]\n}",this.endOfLine)
                    }
                }
            };

            headerModelsAuxiliarString = new List<string>
            {
                "X-Correlation-ID"
            };
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_SetBeneficiary_When_Check_Configuration_Scenario_Then_Exist()
        {
            var mapping = GetMapping();
            Assert.NotNull(mapping.Request);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_SetBeneficiary_When_Check_Configuration_Scenario_Then_Exist_Once()
        {
            var mapping = GetMappings();
            Assert.AreEqual(1, mapping.Count);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_SetBeneficiary_When_Check_Configuration_Path_Then_Exist()
        {
            var mapping = GetMapping();
            PathModel path = JsonConvert.DeserializeObject<PathModel>(mapping.Request.Path.ToString());
            Assert.IsTrue(this.scenarioPathRegEx.IsMatch(path.Matchers.FirstOrDefault().Pattern.ToString()));
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_SetBeneficiary_When_Check_Configuration_Query_Params_Then_Not_Exist()
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
        public void Given_ValleyCoreBankingAPI_SetBeneficiary_When_Check_Configuration_Headers_Then_There_Is_Not_Diffeence()
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
        public void Given_ValleyCoreBankingAPI_SetBeneficiary_When_Check_Configuration_Response_Type_Then_Equal_Application_Json()
        {
            var mapping = GetMapping();
            Assert.IsTrue(mapping.Response.Headers.Any(h => h.Key == this.scenarioHeaderKey && h.Value.ToString() == this.scenarioHeaderValue));
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_SetBeneficiary_When_Check_Configuration_Response_Status_Code_Then_Equal_200()
        {
            var mapping = GetMapping();
            Assert.AreEqual((long)HttpStatusCode.OK, (long)mapping.Response.StatusCode);
        }

        [Test]
        public async Task Given_ValleyCoreBankingAPI_SetBeneficiary_When_Execute_Request_Then_Response_Body_Is_Expected()
        {
            var mapping = GetMapping();
            PathModel path = JsonConvert.DeserializeObject<PathModel>(mapping.Request.Path.ToString());

            UriBuilder requestEndpointURI = new(requestURI + path.Matchers.First().Pattern.ToString()[1..]);
            requestEndpointURI.Query = WireMockHelper.AppendQueryParams(mapping);

            HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, requestEndpointURI.Uri);
            WireMockHelper.AppendHeaders(mapping, ref httpRequestMessage);
            WireMockHelper.AppendRequestBody(mapping, ref httpRequestMessage);


            var httpResponse = await _client.SendAsync(httpRequestMessage);
            dynamic httResponseDeserialize = JsonConvert.DeserializeObject(await httpResponse.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.AreEqual(mapping.Response.BodyAsJson, httResponseDeserialize);
        }
    }
}

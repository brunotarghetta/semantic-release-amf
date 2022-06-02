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

namespace IntegrationTest.OpenAPI.Accounts
{
    public class ValleyCoreBankingAPI_GetAccountFlags_Test : ValleyCoreBankingAPI_IntegrationTestBase
    {
        [OneTimeSetUp]
        public void Init()
        {
            this.scenarioPath = "/core/v1/accounts/{accountId}/flags";
            this.scenarioTitle = "";
            this.scenarioHeaderKey = "Content-Type";
            this.scenarioHeaderValue = "application/json";
            this.scenarioVerb = "GET";
            this.scenarioPathRegEx = new Regex(@"/core/v1/accounts/([0-9]{2}[A-Z]{5}[0-9]{2})/flags$", RegexOptions.IgnoreCase);

            methods = new string[] { this.scenarioVerb };

            headerModelsAuxiliarString = new List<string>
            {
                "X-Correlation-ID"
            };

            paramModelsAuxiliarString = new List<string>()
            {
                "customerId",
                "productCode"
            };            
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GetAccountFlags_When_Check_Configuration_Scenario_Then_Exist()
        {
            var mapping = GetMapping();
            Assert.NotNull(mapping.Request);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GetAccountFlags_When_Check_Configuration_Scenario_Then_Exist_Once()
        {
            var mapping = GetMappings();
            Assert.AreEqual(1, mapping.Count);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GetAccountFlags_When_Check_Configuration_Path_Then_Exist()
        {
            var mapping = GetMapping();
            string finalPath = JsonConvert.DeserializeObject<PathModel>(mapping.Request.Path.ToString()).Matchers.First().Pattern.ToString();
            Assert.IsTrue(scenarioPathRegEx.IsMatch(finalPath));
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GetAccountFlags_When_Check_Configuration_Request_Method_Then_Is_GET()
        {
            var mapping = GetMapping();
            Assert.AreEqual(this.scenarioVerb, mapping.Request.Methods.First().ToString());
        }
        
        [Test]
        public void Given_ValleyCoreBankingAPI_GetAccountFlags_When_Check_Configuration_Query_Params_Then_There_Is_Not_Diffeence()
        {
            var mapping = GetMapping();
            List<string> actualRequestParams = new();
            foreach (ParamModel param in mapping.Request.Params)
            {
                actualRequestParams.Add(param.Name);
            }
            Assert.IsTrue(actualRequestParams.Except(paramModelsAuxiliarString).ToList().Count == 0);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GetAccountFlags_When_Check_Configuration_Headers_Then_There_Is_Not_Diffeence()
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
        public void Given_ValleyCoreBankingAPI_GetAccountFlag_When_Check_Configuration_Request_Body_Then_Does_Not_Exist()
        {
            var mapping = GetMapping();
            Assert.IsNull(mapping.Request.Body);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GetAccountFlag_When_Check_Configuration_Response_Type_Then_Equal_Application_Json()
        {
            var mapping = GetMapping();
            Assert.IsTrue(mapping.Response.Headers.Any(h => h.Key == this.scenarioHeaderKey && h.Value.ToString() == this.scenarioHeaderValue));
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GettAccountFlag_When_Check_Configuration_Response_Status_Code_Then_Equal_200()
        {
            var mapping = GetMapping();
            Assert.AreEqual((long)HttpStatusCode.OK, (long)mapping.Response.StatusCode);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GetAccountFlag_When_Check_Configuration_Response_Body_Then_Is_Expected()
        {
            var mapping = GetMapping();
            Assert.IsNotNull(mapping.Response.BodyAsJson);
        }

        [Test]
        public async Task Given_ValleyCoreBankingAPI_GetAccountFlag_When_Execute_Request_Then_Response_Body_Is_Expected()
        {
            var mapping = GetMapping();

            string finalPath = JsonConvert.DeserializeObject<PathModel>(mapping.Request.Path.ToString()).Matchers.First().Pattern.ToString()[1..];
            UriBuilder requestEndpointURI = new(requestURI + finalPath);
            requestEndpointURI.Query = WireMockHelper.AppendQueryParams(mapping);

            HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, requestEndpointURI.Uri);
            WireMockHelper.AppendHeaders(mapping, ref httpRequestMessage);
            WireMockHelper.AppendRequestBody(mapping, ref httpRequestMessage);


            var httpResponse = await _client.SendAsync(httpRequestMessage);
            dynamic httResponseDeserialize = JsonConvert.DeserializeObject(await httpResponse.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.AreEqual(mapping.Response.BodyAsJson, httResponseDeserialize);
        }
    }
}

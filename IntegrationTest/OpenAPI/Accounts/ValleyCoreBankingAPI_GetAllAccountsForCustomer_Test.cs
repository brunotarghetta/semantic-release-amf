using System;
using System.Collections.Generic;
using System.IO;
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
    public class ValleyCoreBankingAPI_GetAllAccountsForCustomer_Test : ValleyCoreBankingAPI_IntegrationTestBase
    {
        [OneTimeSetUp]
        public void Init()
        {
            this.scenarioPath = "/core/v1/accounts/getAllWithBalances";
            this.scenarioTitle = "";
            this.scenarioHeaderKey = "Content-Type";
            this.scenarioHeaderValue = "application/json";
            this.scenarioVerb = "GET";
            this.scenarioPathRegEx = new Regex($@"{this.scenarioPath}$", RegexOptions.IgnoreCase);

            methods = new string[] { this.scenarioVerb };

            headerModelsAuxiliarString = new List<string>
            {
                "X-Correlation-ID"
            };

            paramModelsAuxiliarString = new List<string>()
            {
                "customerId"
            };

            const string configuredResponseBody = "{\"responseCode\": \"OK\",\"data\": {\"accounts\": [{\"balance\": 2.15,\"account\": {\"productCode\": \"ALL_ACCESS_SAVINGS\",\"branchNumber\": 1,\"owners\": [{\"customerId\": \"0021458711\",\"relationshipCode\": \"PRIMARY\"}],\"accountNumber\": \"005066026517\",\"paymentFrequency\": \"MONTHLY\",\"paymentMethod\": \"COMPOUND\",\"statementCycle\": \"MONTHLY_LAST_DAY_OF_MONTH\",\"taxInformation\": {\"taxIdNumber\": \"999887777\",\"taxIdType\": \"SSN\"},\"alternateAddress\": {\"addressType\": \"HOME_ADDRESS\",\"address1\": \"11 hope st\",\"address2\": \"first floor\",\"city\": \"Queens\",\"state\": \"NY\",\"postalCode\": \"11385\",\"country\": \"USA\"}}},{\"balance\": \"0.00\",\"account\": {\"productCode\": \"LOYALTY_CHECKING\",\"branchNumber\": 1,\"owners\": [{\"customerId\": \"0021458711\",\"relationshipCode\": \"JOINTOWNER\"}],\"accountNumber\": \"000008651234\",\"paymentFrequency\": \"MONTHLY\",\"paymentMethod\": \"COMPOUND\",\"statementCycle\": \"MONTHLY_LAST_DAY_OF_MONTH\",\"taxInformation\": {\"taxIdNumber\": \"999887777\",\"taxIdType\": \"SSN\"},\"alternateAddress\": {\"addressType\": \"HOME_ADDRESS\",\"address1\": \"11 hope st\",\"address2\": \"first floor\",\"city\": \"Queens\",\"state\": \"NY\",\"postalCode\": \"11385\",\"country\": \"USA\"}}}]}}";

            using (JsonReader reader = new JsonTextReader(new StringReader(configuredResponseBody)))
            {
                reader.DateParseHandling = DateParseHandling.None;
                responseBody = JObject.Load(reader);
            }

        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GetAllWithBalances_When_Check_Configuration_Scenario_Then_Exist()
        {
            var mapping = GetMapping();
            Assert.NotNull(mapping.Request);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GetAllWithBalances_When_Check_Configuration_Scenario_Then_Exist_Once()
        {
            var mapping = GetMappings();
            Assert.AreEqual(1, mapping.Count);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GetAllWithBalances_When_Check_Configuration_Request_Method_Then_Is_GET()
        {
            var mapping = GetMapping();
            Assert.AreEqual(this.scenarioVerb, mapping.Request.Methods.First().ToString());
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GetAllWithBalances_When_Check_Configuration_Path_Then_Exist()
        {
            var mapping = GetMapping();
            PathModel path = JsonConvert.DeserializeObject<PathModel>(mapping.Request.Path.ToString());
            Assert.IsTrue(path.Matchers.Any(c => c.Pattern.ToString() == this.scenarioPath));
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GetAllWithBalances_When_Check_Configuration_Query_Params_Then_There_Is_Not_Diffeence()
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
        public void Given_ValleyCoreBankingAPI_GetAllWithBalances_When_Check_Configuration_Headers_Then_There_Is_Not_Diffeence()
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
        public void Given_ValleyCoreBankingAPI_GetAllWithBalances_When_Check_Configuration_Request_Body_Then_Is_Not_Configured()
        {
            var mapping = GetMapping();
            Assert.IsNull(mapping.Request.Body);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GetAllWithBalances_When_Check_Configuration_Response_Type_Then_Equal_Application_Json()
        {
            var mapping = GetMapping();
            Assert.IsTrue(mapping.Response.Headers.Any(h => h.Key == this.scenarioHeaderKey && h.Value.ToString() == this.scenarioHeaderValue));
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GetAllWithBalances_When_Check_Configuration_Response_Status_Code_Then_Equal_200()
        {
            var mapping = GetMapping();
            Assert.AreEqual((long)HttpStatusCode.OK, (long)mapping.Response.StatusCode);
        }

        [Test]
        public void Given_ValleyCoreBankingAPI_GetAllWithBalances_When_Check_Configuration_Response_Body_Then_Is_Expected()
        {
            var mapping = GetMapping();
            Assert.AreEqual(responseBody.ToString(), mapping.Response.BodyAsJson.ToString());
        }

        [Test]
        public async Task Given_ValleyCoreBankingAPI_GetAllWithBalances_When_Execute_Request_Then_Response_Body_Is_Expected()
        {
            var mapping = GetMapping();

            UriBuilder requestEndpointURI = new(requestURI + this.scenarioPath[1..]);
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

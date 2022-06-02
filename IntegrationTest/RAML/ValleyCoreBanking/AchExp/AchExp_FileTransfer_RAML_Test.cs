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

namespace IntegrationTest.RAML.Ach
{
    public class AchExp_FileTransfer_RAML_Test : ValleyCoreBankingRAML_IntegrationTestBase
    {
        [OneTimeSetUp]
        public void Init()
        {
            this.scenarioPath = "/core/raml/v1/ach/";
            this.scenarioTitle = "";
            this.scenarioVerb = "POST";
            this.scenarioPathRegEx = new Regex($@"{this.scenarioPath}$", RegexOptions.IgnoreCase);

            methods = new string[] { this.scenarioVerb };
            bodyModel = new BodyModel()
            {
                Matcher = new MatcherModel()
                {
                    Name = "JsonMatcher",
                    IgnoreCase = true,
                    Patterns = new object[] { "{\n  \"fileType\": \"txt\",\n  \"fileName\": \"FileName\",\n  \"fileContents\": \"dGVzdA==\"\n}" }
                }
            };

            headerModelsAuxiliarString = new List<string>
            {
                "X-Correlation-Id"
            };

            const string configuredResponseBody = "{\r\n   \"status\": \"Success\",\r\n   \"transactionId\": \"O123456789\"\r\n}";
            responseBody = JObject.Parse(configuredResponseBody);

        }

        [Test]
        public void Given_AchExp_FileTransfer_When_Check_Configuration_Scenario_Then_Exist()
        {
            var mapping = GetMapping();
            Assert.NotNull(mapping.Request);
        }

        [Test]
        public void Given_AchExp_FileTransfer_When_Check_Configuration_Scenario_Then_Exist_Once()
        {
            var mapping = GetMappings();
            Assert.AreEqual(1, mapping.Count);
        }

        [Test]
        public void Given_AchExp_FileTransfer_When_Check_Configuration_Path_Then_Exist()
        {
            var mapping = GetMapping();
            PathModel path = JsonConvert.DeserializeObject<PathModel>(mapping.Request.Path.ToString());
            Assert.IsTrue(path.Matchers.Any(c => c.Pattern.ToString() == this.scenarioPath));
        }

        [Test]
        public void Given_AchExp_FileTransfer_When_Check_Configuration_Request_Method_Then_Is_POST()
        {
            var mapping = GetMapping();
            Assert.AreEqual(this.scenarioVerb, mapping.Request.Methods.First().ToString());
        }

        [Test]
        public void Given_AchExp_FileTransfer_When_Check_Configuration_Query_Params_Then_Not_Exist()
        {
            var mapping = GetMapping();
            Assert.IsNull(mapping.Request.Params);
        }

        [Test]
        public void Given_AchExp_FileTransfer_When_Check_Configuration_Headers_Then_There_Is_Not_Diffeence()
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
        public void Given_AchExp_FileTransfer_When_Check_Configuration_Request_Body_Then_Is_Expected()
        {
            var mapping = GetMapping();
            Assert.IsTrue(mapping.Request.Body.Matcher.Patterns.First().ToString().Equals(bodyModel.Matcher.Patterns.First().ToString()));
        }

        [Test]
        public void Given_AchExp_FileTransfer_When_Check_Configuration_Response_Type_Then_Equal_Application_Json()
        {
            var mapping = GetMapping();
            Assert.IsTrue(mapping.Response.Headers.Any(h => h.Key == this.scenarioHeaderKey && h.Value.ToString() == this.scenarioHeaderValue));
        }


        [Test]
        public void Given_AchExp_FileTransfer_When_Check_Configuration_Response_Status_Code_Then_Equal_200()
        {
            var mapping = GetMapping();
            Assert.AreEqual((long)HttpStatusCode.OK, (long)mapping.Response.StatusCode);
        }

        [Test]
        public void Given_AchExp_FileTransfer_When_Check_Configuration_Response_Body_Then_Is_Expected()
        {
            var mapping = GetMapping();
            Assert.AreEqual(responseBody, JObject.Parse(mapping.Response.BodyAsJson.ToString()));
        }

        [Test]
        public async Task Given_AchExp_FileTransfer_When_Execute_Request_Then_Response_Body_Is_Expected()
        {
            var mapping = GetMapping();

            UriBuilder requestEndpointURI = new(requestURI + this.scenarioPath[1..]);
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

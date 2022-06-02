using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WireMock.Admin.Mappings;

namespace IntegrationTest
{
    public class IntegrationWireMockTestsBase
    {
        protected readonly string requestURI = TestContext.Parameters.Get("webApiUrl") is null ? "http://localhost:8080/" :
                                            TestContext.Parameters.Get("webApiUrl").ToString();
        protected readonly EndOfLine endOfLine = TestContext.Parameters.Get("endOfLine") is null ? EndOfLine.Mac :
                                        (EndOfLine)Enum.Parse(typeof(EndOfLine), TestContext.Parameters.Get("endOfLine"));

        public HttpClient _client;
        public MappingModel[] allMappings;
        public RequestModel[] allRequests;
        public HttpResponseMessage responseMessage;
        public string scenarioTitle = string.Empty;
        public string scenarioPath = string.Empty;
        public string scenarioVerb = string.Empty;
        public string scenarioHeaderKey = String.Empty;
        public string scenarioHeaderValue = String.Empty;
        public Regex scenarioPathRegEx;

        protected BodyModel bodyModel;
        protected IList<HeaderModel> headerModels;
        protected IList<string> headerModelsAuxiliarString;
        protected string[] methods;
        protected IList<ParamModel> paramModels;
        protected IList<string> paramModelsAuxiliarString;
        protected JToken responseBody;
        protected string responseBodyXml;
        

        [OneTimeSetUp]
        public async Task SetupAsync()
        {
            _client = new HttpClient();
            responseMessage = await _client.GetAsync(requestURI + "__admin/mappings");
            allMappings = JsonConvert.DeserializeObject<MappingModel[]>(await responseMessage.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// To identify single request model we use: body, headers, method and path
        /// </summary>
        /// <returns>Unique MappingModel</returns>
        public IEnumerable<MappingModel> BuildQuery()
        {
            return allMappings.Where(map =>
            {
                List<string> paramsString = new();
                List<string> headersString = new();
                if (map.Request.Params != null)
                {
                    foreach (ParamModel paramsModel in map.Request.Params)
                    {
                        paramsString.Add(paramsModel.Name);
                    }
                }

                if (map.Request.Headers != null)
                {
                    foreach (HeaderModel headersModel in map.Request.Headers)
                    {
                        headersString.Add(headersModel.Name);
                    }
                }

                return ((map.Request.Body == null || bodyModel == null) ||
                         ((map.Request?.Body?.Matcher?.Patterns != null &&
                            map.Request.Body.Matcher.Patterns.First().ToString().Equals(bodyModel?.Matcher?.Patterns?.First().ToString())
                          ) ||
                          (map.Request.Body?.Matcher?.Pattern != null &&
                                map.Request.Body?.Matcher?.Pattern?.ToString() == bodyModel?.Matcher?.Pattern?.ToString()
                          )
                         )
                        ) &&
                        ((map.Request.Params == null || paramModels == null) ||
                         (map.Request.Params != null &&
                          paramsString.Except(paramModelsAuxiliarString).ToList().Count == 0)
                        ) &&
                        ((map.Request.Headers == null || headerModels == null) ||
                         (map.Request.Headers != null &&
                          headersString.Except(headerModelsAuxiliarString).ToList().Count == 0)
                        ) &&
                        map.Request.Methods.First().ToString().Equals(methods.First().ToString()) &&
                        map.Request.Path != null &&
                        (scenarioPathRegEx != null &&
                         scenarioPathRegEx.IsMatch(
                         JsonConvert.DeserializeObject<PathModel>(
                         map.Request.Path.ToString()).Matchers.First().Pattern.ToString()));

            }
                                      );
        }

        public MappingModel GetMapping()
        {
            return BuildQuery().First();
        }

        public List<MappingModel> GetMappings()
        {
            return BuildQuery().ToList();
        }

        [Obsolete]
        public MappingModel GetMappingUsingVerbAndPath()
        {
            // Key to filter: Path and request method
            return allMappings.Where(map => map.Request.Methods.First().ToString().Equals(this.scenarioVerb) &&
                                                            map.Request.Path != null &&
                                                            (scenarioPathRegEx != null &&
                                                             scenarioPathRegEx.IsMatch(
                                                             JsonConvert.DeserializeObject<PathModel>(
                                                             map.Request.Path.ToString()).Matchers.First().Pattern.ToString())
                                                            )
                                                    ).First();
        }
    }
}
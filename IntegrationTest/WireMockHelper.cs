using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using WireMock.Admin.Mappings;

namespace IntegrationTest
{
    public static class WireMockHelper
    {
        public static void AppendHeaders(MappingModel mapping, ref HttpRequestMessage httpRequestMessage)
        {
            if (mapping.Request?.Headers == null)
            {
                return;
            }

            try
            {
                foreach (HeaderModel header in mapping.Request.Headers)
                {
                    httpRequestMessage.Headers.Add(header.Name, header.Matchers.First().Pattern.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public static string AppendQueryParams(MappingModel mapping)
        {
            if (mapping.Request?.Params == null)
            {
                return string.Empty;
            }
            try
            {
                StringBuilder queryParamsBuilder = new StringBuilder();
                ParamModel lastParam = mapping.Request.Params.Last();
                foreach (ParamModel item in mapping.Request.Params)
                {
                    MatcherModel matcherModel = item.Matchers.FirstOrDefault();
                    queryParamsBuilder.Append(item.Name + "=" + matcherModel.Pattern + (item.Equals(lastParam) ? "" : "&"));
                }
                return queryParamsBuilder.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return ex.Message;
            }
        }

        public static void AppendRequestBody(MappingModel mapping, ref HttpRequestMessage httpRequestMessage)
        {
            if (mapping.Request?.Body == null)
            {
                return;
            }
            try
            {
                if (String.IsNullOrEmpty(mapping.Request.Body.Matcher.PatternAsFile))
                {
                    httpRequestMessage.Content = new StringContent(mapping.Request.Body.Matcher.Patterns.First().ToString(), Encoding.UTF8, "application/json");
                }
                else
                {
                    string contentType = string.Empty;
                    string fileExtension = Path.GetExtension(mapping.Request.Body.Matcher.PatternAsFile);
                    switch (fileExtension)
                    {
                        case ".xml":
                            contentType = "application/xml";
                            break;

                        default:
                            contentType = "application/json";
                            break;
                    }
                    httpRequestMessage.Content = new StringContent(mapping.Request.Body.Matcher.Pattern.ToString(), Encoding.UTF8, contentType);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static string FixEndOfLineCharacterInString(string inputToConvert, EndOfLine key)
        {
            if (key.Equals(EndOfLine.Mac))
            {
                inputToConvert = inputToConvert.Replace("\r\n", "\n");
            }
            if (key.Equals(EndOfLine.Windows))
            {
                inputToConvert = inputToConvert.Replace("\n", "\r\n");
            }
            return inputToConvert;
        }
    }
}

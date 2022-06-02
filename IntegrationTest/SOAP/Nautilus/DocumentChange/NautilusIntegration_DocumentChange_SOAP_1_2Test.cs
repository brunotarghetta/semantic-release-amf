using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using WireMock.Admin.Mappings;
namespace IntegrationTest.SOAP.Nautilus.DocumentChange
{
    public class NautilusIntegration_DocumentChange_SOAP_1_2Test : NautilusIntegrationTestBase
    {
        [OneTimeSetUp]
        public void Init()
        {
            this.scenarioPathRegEx = new Regex("/FiAPI/PDS1450.asmx$", RegexOptions.IgnoreCase);
            this.scenarioHeaderKey = "Content-Type";
            this.scenarioHeaderValue = "application/xml; charset=UTF-8";
            this.scenarioVerb = "POST";

            methods = new string[] { this.scenarioVerb };

            bodyModel = new BodyModel()
            {
                Matcher = new MatcherModel()
                {
                    Name = "ExactMatcher",
                    IgnoreCase = true,
                    Pattern = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\">\r\n  <soap12:Body>\r\n    <ProcessRequest xmlns=\"http://tempuri.org/\">\r\n      <fiAPIPacket>\r\n           <fiAPI xmlns=\"http://integration.fiapi.com\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://integration.fiapi.com/fiDocumentChange.xsd\">\r\n           <!-- fiHeader -->\r\n           <fiHeader Version=\"1.2\">\r\n              <Service Version=\"1.0\" Name=\"Hyland.Applications.Web.PDS1450\">\r\n                 <DateTime>2005-02-03T11:35:49-06:00</DateTime>\r\n                 <UUID>575cf92c-526a-4051-b00b-a331470dc545</UUID>\r\n              </Service>\r\n              <Security>\r\n                 <AuthenticationMaterial>\r\n                    <MessageDigest algorithm=\"RFC2104 SHA-1&gt;\" />\r\n                    <PrincipalPWD>password</PrincipalPWD>\r\n                 </AuthenticationMaterial>\r\n                 <PrincipalID>manager</PrincipalID>\r\n              </Security>\r\n              <Client Version=\"5.0\">\r\n                 <VendorID>Vendor</VendorID>\r\n                 <AppID>AppId</AppID>\r\n                 <OrgID>100</OrgID>\r\n                 <SessionID>123456789</SessionID>\r\n              </Client>\r\n              <DataSource>Nautilus</DataSource>\r\n           </fiHeader>\r\n           <!-- Request -->\r\n           <Request TypeOfRequest=\"DocumentChangeRq\" RequestID=\"1\" Echo=\"0\">\r\n              <!-- classification info -->\r\n              <Type Type=\"Name\">Statements</Type>\r\n              <!-- keyword details to add -->\r\n              <Detail>\r\n                 <Name>Account</Name>\r\n                 <Value>1234567</Value>\r\n              </Detail>\r\n              <Detail>\r\n                 <Name>Field2</Name>\r\n                 <Value>12333</Value>\r\n              </Detail>\r\n              <!-- page data for new file -->\r\n              <Page Number=\"1\" Format=\"PDF\">\r\n                 <Value>[Base 64 Document]</Value>\r\n              </Page>\r\n           </Request>\r\n        </fiAPI>\r\n      </fiAPIPacket>\r\n    </ProcessRequest>\r\n  </soap12:Body>\r\n</soap12:Envelope>"
                }
            };
        }


        [Test]
        public void Given_Nautilus_document_change_SOAP_1_2_When_Check_Configuration_Scenario_Then_Exist()
        {
            var mapping = GetMapping();
            Assert.NotNull(mapping.Request);
        }

        [Test]
        public void Given_Nautilus_document_change_SOAP_1_2_When_Check_Configuration_Scenario_Then_Exist_Once()
        {
            var mapping = GetMappings();
            Assert.AreEqual(1, mapping.Count());
        }

        [Test]
        public void Given_Nautilus_document_change_SOAP_1_2_When_Check_Configuration_Method_POST_Then_Exist()
        {
            var mapping = GetMapping();
            Assert.NotNull(mapping.Request.Methods.Any(c => c == this.scenarioVerb));
        }

        [Test]
        public void Given_Nautilus_document_change_SOAP_1_2_When_Check_Configuration_Path_Then_Exist()
        {
            var mapping = GetMapping();
            PathModel path = JsonConvert.DeserializeObject<PathModel>(mapping.Request.Path.ToString());
            Assert.NotNull(path.Matchers.Any(c => c.Pattern.ToString() == this.scenarioPath));
        }

        [Test]
        public void Given_Nautilus_document_change_SOAP_1_2_When_Check_Configuration_Query_Params_Then_Not_Exist()
        {
            var mapping = GetMapping();
            Assert.IsNull(mapping.Request.Params);
        }

        [Test]
        public void Given_Nautilus_document_change_SOAP_1_2_When_Check_Configuration_Request_Body_Then_Is_Expected()
        {
            var mapping = GetMapping();
            Assert.AreEqual(bodyModel.Matcher.Pattern.ToString(), mapping.Request.Body.Matcher.Pattern.ToString());
        }

        [Test]
        public void Given_Nautilus_document_change_SOAP_1_2_When_Check_Configuration_Response_Type_Then_Equal_Application_Xml_And_CharSet_Utf8()
        {
            var mapping = GetMapping();
            Assert.IsNotNull(mapping.Response.Headers.Any(h => h.Key == this.scenarioHeaderKey && h.Value == (object)this.scenarioHeaderValue));
        }

        [Test]
        public void Given_Nautilus_document_change_SOAP_1_2_When_Check_Configuration_Response_Status_Code_Then_Equal_200()
        {
            var mapping = GetMapping();
            Assert.IsNotNull(mapping.Response.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Test]
        public async Task Given_Nautilus_document_change_SOAP_1_2_When_Execute_Request_Then_Response_Body_Is_Expected()
        {
            var mapping = GetMapping();

            UriBuilder requestEndpointURI = new(requestURI + this.scenarioPath);
            requestEndpointURI.Query = WireMockHelper.AppendQueryParams(mapping);

            HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, requestEndpointURI.Uri);
            WireMockHelper.AppendHeaders(mapping, ref httpRequestMessage);
            WireMockHelper.AppendRequestBody(mapping, ref httpRequestMessage);

            var httResponse = await _client.SendAsync(httpRequestMessage);
            string xmlHttpResponse = await httResponse.Content.ReadAsStringAsync();


            if (mapping.Response.BodyAsBytes is null)
            {
                await SetupAsync();
                mapping = GetMapping();
            }

            string xmlConfigurationResponse = Encoding.UTF8.GetString(mapping.Response.BodyAsBytes, 0, mapping.Response.BodyAsBytes.Length);


            Assert.AreEqual(HttpStatusCode.OK, httResponse.StatusCode);
            Assert.AreEqual(xmlConfigurationResponse, xmlHttpResponse);
        }
    }
}

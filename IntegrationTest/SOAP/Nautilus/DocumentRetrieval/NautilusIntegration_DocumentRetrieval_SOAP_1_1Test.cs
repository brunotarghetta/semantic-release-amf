using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using WireMock.Admin.Mappings;

namespace IntegrationTest.SOAP.Nautilus.DocumentRetrieval
{
    public class NautilusIntegration_DocumentRetrieval_SOAP_1_1Test : NautilusIntegrationTestBase
    {
        [OneTimeSetUp]
        public void Init()
        {
            this.scenarioPathRegEx = new Regex("/FiAPI/pds1450.asmx$", RegexOptions.IgnoreCase);
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
                    Pattern = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">\r\n  <soap:Body>\r\n    <ProcessRequest xmlns=\"http://tempuri.org/\">\r\n      <fiAPIPacket>\r\n        <fiAPI xmlns=\"http://integration.fiapi.com\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://integration.fiapi.com/fiDocumentInquiry.xsd\">\r\n          <fiHeader Version=\"\">\r\n            <Service Version=\"\" Name=\"\">\r\n              <DateTime>2011-02-14T19:00:00</DateTime>\r\n              <UUID></UUID>\r\n            </Service>\r\n            <Security>\r\n              <AuthenticationMaterial>\r\n                <PrincipalPWD>password</PrincipalPWD>\r\n              </AuthenticationMaterial>\r\n              <PrincipalID>manager</PrincipalID>\r\n            </Security>\r\n            <Client Version=\"\">\r\n              <VendorID></VendorID>\r\n              <AppID></AppID>\r\n              <OrgID>100</OrgID>\r\n              <SessionID></SessionID>\r\n            </Client>\r\n            <DataSource>Nautilus</DataSource>\r\n          </fiHeader>\r\n          <Request TypeOfRequest=\"DocumentInquiryRq\" RequestID=\"1\" Echo=\"0\" MaxRows=\"1\">\r\n            <ID>524690</ID>\r\n            <RequestData ID=\"1\" Name=\"0\" Date=\"1\" Class=\"0\" Type=\"0\">\r\n              <Detail MultiReplace=\"*\">*</Detail>\r\n              <PageRange Format=\"TIFF\">\r\n                <Start>1</Start>\r\n                <End>2</End>\r\n              </PageRange>\r\n            </RequestData>\r\n            <RequestMetaData SearchFields=\"0\" Formats=\"1\" PageTotal=\"0\"></RequestMetaData>\r\n          </Request>\r\n        </fiAPI>\r\n      </fiAPIPacket>\r\n    </ProcessRequest>\r\n  </soap:Body>\r\n</soap:Envelope>"
                }
            };            
            responseBodyXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">\r\n  <soap:Body>\r\n    <ProcessRequestResponse xmlns=\"http://tempuri.org/\">\r\n      <ProcessRequestResult>\r\n        <fiAPI xmlns=\"http://integration.fiapi.com\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://integration.fiapi.com\r\nC:\\MessageSet\\DocumentServer\\fiDocumentInquiry.xsd\">\r\n          <fiHeader Version=\"1.1\">\r\n            <Service Version=\"1.0\" Name=\"DocumentServer\">\r\n              <DateTime>2011-03-16T16:54:06-04:00:00</DateTime>\r\n              <UUID></UUID>\r\n            </Service>\r\n            <Security>\r\n              <AuthenticationMaterial>\r\n                <MessageDigest algorithm=\"RFC2104 SHA-1\"></MessageDigest>\r\n                <PrincipalPWD />\r\n              </AuthenticationMaterial>\r\n            </Security>\r\n            <SessionID> nd30bxygavdpvwrydry1uba5 </SessionID>\r\n            <DataSource>Nautilus</DataSource>\r\n          </fiHeader>\r\n          <Response TypeOfResponse=\"DocumentInquiryRs\" More=\"0\" Cursor=\"0\" TotalRows=\"1\" ResponseID=\"1\">\r\n            <Status>\r\n              <ServerStatusCode>0</ServerStatusCode>\r\n              <StatusCode>0</StatusCode>\r\n              <Severity />\r\n              <version>\r\n                <![CDATA[9,2,1,140]]>\r\n              </version>\r\n            </Status>\r\n            <DocumentInquiryRs>\r\n              <Document>\r\n                <ID>524690</ID>\r\n                <Date>2/11/2011 12:00:00 AM</Date>\r\n                <Detail Type=\"Data\">\r\n                  <Name>Account #</Name>\r\n                  <ID>51</ID>\r\n                  <Value>123456789</Value>\r\n                </Detail>\r\n                <Detail Type=\"Data\">\r\n                  <Name>Check Serial #</Name>\r\n                  <ID>52</ID>\r\n                  <Value>123</Value>\r\n                </Detail>\r\n                <Detail Type=\"Data\">\r\n                  <Name>Check Amount</Name>\r\n                  <ID>53</ID>\r\n                  <Value>$10.00</Value>\r\n                </Detail>\r\n                <Page Number=\"1\" Format=\"TIFF\">\r\n                  <Value>Image-Here</Value>\r\n                </Page>\r\n                <Page Number=\"2\" Format=\"TIFF\">\r\n                  <Value>Image-Here</Value>\r\n                </Page>\r\n              </Document>\r\n            </DocumentInquiryRs>\r\n          </Response>\r\n        </fiAPI>\r\n      </ProcessRequestResult>\r\n    </ProcessRequestResponse>\r\n  </soap:Body>\r\n</soap:Envelope>";

        }

        [Test]
        public void Given_Nautilus_DocumentRetrieval_SOAP_1_1_When_Check_Configuration_Scenario_Then_Exist()
        {
            var mapping = GetMapping();
            Assert.NotNull(mapping.Request);
        }

        [Test]
        public void Given_Nautilus_DocumentRetrieval_SOAP_1_1_When_Check_Configuration_Scenario_Then_Exist_Once()
        {
            var mapping = GetMappings();
            Assert.AreEqual(1, mapping.Count());
        }

        [Test]
        public void Given_Nautilus_Nautilus_DocumentRetrieval_SOAP_1_1_When_Check_Configuration_Method_POST_Then_Exist()
        {
            var mapping = GetMapping();
            Assert.NotNull(mapping.Request.Methods.Any(c => c == this.scenarioVerb));
        }

        [Test]
        public void Given_Nautilus_DocumentRetrieval_SOAP_1_1_When_Check_Configuration_Path_Then_Exist()
        {
            var mapping = GetMapping();
            PathModel path = JsonConvert.DeserializeObject<PathModel>(mapping.Request.Path.ToString());
            Assert.NotNull(path.Matchers.Any(c => c.Pattern.ToString() == this.scenarioPath));
        }

        [Test]
        public void Given_Nautilus_DocumentRetrieval_SOAP_1_1_When_Check_Configuration_Query_Params_Then_Not_Exist()
        {
            var mapping = GetMapping();
            Assert.IsNull(mapping.Request.Params);
        }

        [Test]
        public void Given_Nautilus_DocumentRetrieval_SOAP_1_1_When_Check_Configuration_Request_Headers_Then_Is_Null()
        {
            var mapping = GetMapping();
            Assert.IsNull(mapping.Request.Headers);
        }

        [Test]
        public void Given_Nautilus_DocumentRetrieval_SOAP_1_1_When_Check_Configuration_Request_Body_Then_Is_Expected()
        {
            var mapping = GetMapping();
            Assert.AreEqual(bodyModel.Matcher.Pattern.ToString(), mapping.Request.Body.Matcher.Pattern.ToString());
        }

        [Test]
        public void Given_Nautilus_DocumentRetrieval_SOAP_1_1_When_Check_Configuration_Response_Type_Then_Equal_Application_Xml_And_CharSet_Utf8()
        {
            var mapping = GetMapping();
            Assert.IsNotNull(mapping.Response.Headers.Any(h => h.Key == this.scenarioHeaderKey && h.Value == (object)this.scenarioHeaderValue));
        }

        [Test]
        public void Given_Nautilus_DocumentRetrieval_SOAP_1_1_When_Check_Configuration_Response_Status_Code_Then_Equal_200()
        {
            var mapping = GetMapping();
            Assert.AreEqual((long)HttpStatusCode.OK, (long)mapping.Response.StatusCode);
        }

        [Test]
        // [Ignore("Ignore when target is virtual machine on Azure")]
        public async Task Given_Nautilus_DocumentRetrieval_SOAP_1_1_When_Check_Configuration_Response_Body_Then_Is_ExpectedAsync()
        {
            var mapping = GetMapping();

            UriBuilder requestEndpointURI = new(requestURI + this.scenarioPath);
            requestEndpointURI.Query = WireMockHelper.AppendQueryParams(mapping);

            HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, requestEndpointURI.Uri);
            WireMockHelper.AppendHeaders(mapping, ref httpRequestMessage);
            WireMockHelper.AppendRequestBody(mapping, ref httpRequestMessage);

            await _client.SendAsync(httpRequestMessage);

            if (mapping.Response.BodyAsBytes is null)
            {
                await SetupAsync();
                mapping = GetMapping();
            }

            string xmlConfigurationResponse = Encoding.UTF8.GetString(mapping.Response.BodyAsBytes, 0, mapping.Response.BodyAsBytes.Length);

            Assert.AreEqual(responseBodyXml, xmlConfigurationResponse);
        }

        [Test]
        // [Ignore("Ignore when target is virtual machine on Azure")]
        public async Task Given_Nautilus_DocumentRetrieval_SOAP_1_1_When__Execute_Request_Then_Response_Body_Is_Expected()
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

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

namespace IntegrationTest.SOAP.Nautilus.CheckItem
{
    public class NautilusIntegration_CheckItem_SOAP_1_1Test : NautilusIntegrationTestBase
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
                    Pattern = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">\r\n  <soap:Body>\r\n    <ProcessRequest xmlns=\"http://tempuri.org/\">\r\n      <fiAPIPacket>\r\n        <fiAPI xmlns=\"http://integration.fiapi.com\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://integration.fiapi.com/fiDocumentInquiry.xsd\">\r\n          <fiHeader Version=\"\">\r\n            <Service Version=\"\" Name=\"\">\r\n              <DateTime>2011-02-14T19:00:00</DateTime>\r\n              <UUID></UUID>\r\n            </Service>\r\n            <Security>\r\n              <AuthenticationMaterial>\r\n                <PrincipalPWD>password</PrincipalPWD>\r\n              </AuthenticationMaterial>\r\n              <PrincipalID>manager</PrincipalID>\r\n            </Security>\r\n            <Client Version=\"\">\r\n              <VendorID></VendorID>\r\n              <AppID></AppID>\r\n              <OrgID>100</OrgID>\r\n              <SessionID></SessionID>\r\n            </Client>\r\n            <DataSource>Nautilus</DataSource>\r\n          </fiHeader>\r\n          <Request TypeOfRequest=\"DocumentInquiryRq\" RequestID=\"1\" Echo=\"0\" MaxRows=\"1\">\r\n            <CustomQuery Type=\"ID\">123</CustomQuery>\r\n            <DateRange>\r\n              <Start>01-01-2000</Start>\r\n              <End>12-31-2011</End>\r\n            </DateRange>\r\n            <Condition>\r\n              <Detail>Account #</Detail>\r\n              <Operator>EQ</Operator>\r\n              <Value>123456789</Value>\r\n              <Connection>AND</Connection>\r\n            </Condition>\r\n            <Condition>\r\n              <Detail>Check Amount</Detail>\r\n              <Operator>EQ</Operator>\r\n              <Value>10.00</Value>\r\n              <Connection>AND</Connection>\r\n            </Condition>\r\n            <Condition>\r\n              <Detail>Check Serial #</Detail>\r\n              <Operator>EQ</Operator>\r\n              <Value>123</Value>\r\n              <Connection>AND</Connection>\r\n            </Condition>\r\n            <Condition>\r\n              <Detail>Institution #</Detail>\r\n              <Operator>EQ</Operator>\r\n              <Value>146</Value>\r\n              <Connection>AND</Connection>\r\n            </Condition>\r\n            <RequestData ID=\"1\" Name=\"0\" Date=\"1\" Class=\"0\" Type=\"0\">\r\n              <Detail MultiReplace=\"*\">*</Detail>\r\n              <PageRange Format=\"TIFF\">\r\n                <Start>1</Start>\r\n                <End>2</End>\r\n              </PageRange>\r\n            </RequestData>\r\n            <RequestMetaData SearchFields=\"0\" Views=\"*\" Formats=\"1\" PageTotal=\"0\"></RequestMetaData>\r\n          </Request>\r\n        </fiAPI>\r\n      </fiAPIPacket>\r\n    </ProcessRequest>\r\n  </soap:Body>\r\n</soap:Envelope>"
                }
            };
        }


        [Test]
        public void Given_Nautilus_check_item_SOAP_1_1_When_Check_Configuration_Scenario_Then_Exist()
        {
            var mapping = GetMapping();
            Assert.NotNull(mapping.Request);
        }

        [Test]
        public void Given_Nautilus_check_item_SOAP_1_1_When_Check_Configuration_Scenario_Then_Exist_Once()
        {
            var mapping = GetMappings();
            Assert.AreEqual(1, mapping.Count());
        }

        [Test]
        public void Given_Nautilus_check_item_SOAP_1_1_When_Check_Configuration_Method_POST_Then_Exist()
        {
            var mapping = GetMapping();
            Assert.NotNull(mapping.Request.Methods.Any(c => c == this.scenarioVerb));
        }

        [Test]
        public void Given_Nautilus_check_item_SOAP_1_1_When_Check_Configuration_Path_Then_Exist()
        {
            var mapping = GetMapping();
            PathModel path = JsonConvert.DeserializeObject<PathModel>(mapping.Request.Path.ToString());
            Assert.NotNull(path.Matchers.Any(c => c.Pattern.ToString() == this.scenarioPath));
        }

        [Test]
        public void Given_Nautilus_check_item_SOAP_1_1_When_Check_Configuration_Query_Params_Then_Not_Exist()
        {
            var mapping = GetMapping();
            Assert.IsNull(mapping.Request.Params);
        }

        [Test]
        public void Given_Nautilus_check_item_SOAP_1_1_When_Check_Configuration_Request_Body_Then_Is_Expected()
        {
            var mapping = GetMapping();
            Assert.AreEqual(bodyModel.Matcher.Pattern.ToString(), mapping.Request.Body.Matcher.Pattern.ToString());
        }

        [Test]
        public void Given_Nautilus_check_item_SOAP_1_1_When_Check_Configuration_Response_Type_Then_Equal_Application_Xml_And_CharSet_Utf8()
        {
            var mapping = GetMapping();
            Assert.IsNotNull(mapping.Response.Headers.Any(h => h.Key == this.scenarioHeaderKey && h.Value == (object)this.scenarioHeaderValue));
        }

        [Test]
        public void Given_Nautilus_check_item_SOAP_1_1_When_Check_Configuration_Response_Status_Code_Then_Equal_200()
        {
            var mapping = GetMapping();
            Assert.IsNotNull(mapping.Response.StatusCode.Equals(HttpStatusCode.OK));
        }
        
        [Test]
        // [Ignore("Ignore when target is virtual machine on Azure")]
        public async Task Given_Nautilus_check_item_SOAP_1_1_When_Execute_Request_Then_Response_Body_Is_Expected()
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

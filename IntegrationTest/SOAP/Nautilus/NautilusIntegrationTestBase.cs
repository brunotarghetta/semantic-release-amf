using System;
﻿using NUnit.Framework;

namespace IntegrationTest.SOAP.Nautilus
{
    public class NautilusIntegrationTestBase : IntegrationWireMockTestsBase
    {
        [OneTimeSetUp]
        public void InitBase()
        {
            this.scenarioPath = "FiAPI/pds1450.asmx";
            this.scenarioHeaderKey = "Content-Type";
            this.scenarioHeaderValue = "application/xml; charset=UTF-8";
        }       
    }
}

using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace POC.Microsoft.Recognizers.Text
{
    internal class InvalidDateParseTests : DefaultCultureFixture
    {
        public InvalidDateParseTests(string culture) : base(culture) { }

        [TestCase("202x-06-05")]
        public void ISO8601Date_BadDateOnly_ReturnsNoDate(string date)
        {
            List<ModelResult> results =
                    DateTimeRecognizer.RecognizeDateTime(date, _culture);

            TestContext.WriteLine(JsonConvert.SerializeObject(results, Formatting.Indented));

            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.EqualTo(0));
        }
    }
}

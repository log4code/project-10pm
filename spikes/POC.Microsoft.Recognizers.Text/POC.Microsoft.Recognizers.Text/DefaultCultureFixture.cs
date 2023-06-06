using Microsoft.Recognizers.Text;

namespace POC.Microsoft.Recognizers.Text
{
    [TestFixture(Culture.English)]
    [TestFixture(Culture.German)]
    public class DefaultCultureFixture
    {
        protected string _culture;
        public DefaultCultureFixture(string culture) 
        { 
            _culture = culture;
        }

        [OneTimeSetUp]
        public void Setup()
        {
            //_culture = Culture.English;
        }
    }
}
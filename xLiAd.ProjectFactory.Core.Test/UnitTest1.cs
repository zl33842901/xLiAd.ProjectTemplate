using NUnit.Framework;

namespace xLiAd.ProjectFactory.Core.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            CodeLoader codeLoader = new CodeLoader("./", "xLiAdProjectTemplate");
            IConvertService convertService = new ConvertService(codeLoader);
            var fileItems = convertService.Convert("SampCorp.Abc");
            Assert.Pass();
        }
    }
}
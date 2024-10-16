using MontiorWPFBlend;

namespace TestMonitorSistema
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
            Config.GetInstance().Load();
            string originalLang = Config.GetInstance().lang;
            string originalColor = Config.GetInstance().color;

            Config.GetInstance().InitDefaults();

            const string testLang = "_testlang_";
            const string testColor = "_testcolor_";
            Config.GetInstance().lang = testLang;
            Config.GetInstance().color = testColor;
            Config.GetInstance().Save();

            Config.GetInstance().InitDefaults();

            Config.GetInstance().Load();

            Assert.AreEqual(testLang, Config.GetInstance().lang);
            Assert.AreEqual(testColor, Config.GetInstance().color);

            if(originalColor != null || originalLang != null)
            {
                Config.GetInstance().lang = originalLang;
                Config.GetInstance().color = originalColor;
            } 
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using System.Threading;
using System;

namespace CalculatorTest
{
    [TestClass]
    public class ScenarioStandard : CalculatorSession
    {
        private static WindowsElement header;
        private static WindowsElement calculatorResult;

        [TestMethod]
        public void Addition()
        {
            session.FindElementByName("One").Click();
            session.FindElementByName("Plus").Click();
            session.FindElementByName("Seven").Click();
            session.FindElementByName("Equals").Click();
            Assert.AreEqual("8", GetCalculatorResultText());
        }

        public void Templatized(string input1, string operation, string input2, string expectedResult)
        {
            session.FindElementByName(input1).Click();
            session.FindElementByName(operation).Click();
            session.FindElementByName(input2).Click();
            session.FindElementByName("Equals").Click();
            Assert.AreEqual(expectedResult, GetCalculatorResultText());
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);

            try
            {
                header = session.FindElementByAccessibilityId("Header");
            }
            catch
            {
                header = session.FindElementByAccessibilityId("ContentPresenter");
            }            

            if (!header.Text.Equals("Standard", StringComparison.OrdinalIgnoreCase))
            {
                session.FindElementByAccessibilityId("TogglePaneButton").Click();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                var splitViewPane = session.FindElementByClassName("SplitViewPane");
                splitViewPane.FindElementByName("Standard Calculator").Click();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.IsTrue(header.Text.Equals("Standard", StringComparison.OrdinalIgnoreCase));
            }

            calculatorResult = session.FindElementByAccessibilityId("CalculatorResults");
            Assert.IsNotNull(calculatorResult);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestInitialize]
        public void Clear()
        {
            session.FindElementByName("Clear").Click();
            Assert.AreEqual("0", GetCalculatorResultText());
        }

        private string GetCalculatorResultText()
        {
            return calculatorResult.Text.Replace("Display is", string.Empty).Trim();
        }
    }
}

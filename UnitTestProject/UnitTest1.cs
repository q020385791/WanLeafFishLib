using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WanLibrary;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestGetEnumValueFromDescription()
        {
            string Description=DataFormat.GetEnumValueFromDescription<EnumCnEn>("姓名");
            string Target = "Name";
            Assert.AreEqual(Target, Description);
        }
        [TestMethod]
        public void TestGetDescriptionFromValue()
        {
            string Value = DataFormat.GetDescriptionFromValue<EnumCnEn>("Name");
            string Target = "姓名";
            Assert.AreEqual(Target, Value);

        }
        [TestMethod]
        public void TestStringPaddingRight()
        {
           string Value= DataFormat.StringPadRight("Test",8,'0');
            string Target = "Test0000";
            Assert.AreEqual(Target, Value);
        }
        [TestMethod]
        public void TestStringPaddingLeft()
        {
            string Value = DataFormat.StringPadLeft("Test", 8, '0');
            string Target = "0000Test";
            Assert.AreEqual(Target, Value);
        }
        //[TestMethod]
        public void TestCreateXmlFile()
        {
            //存檔預設檔名為TargetPath上的檔名，測試時請依照自己的需求變更TargetPath內容
            string Value = "<?xml version=\"1.0\" encoding=\"utf-8\"?><ColonQCFile><Contact_QC><Unit>UUnittt</Unit><Name>eee</Name><Tel>0963298268</Tel><Email>q020385791@gmail.com</Email></Contact_QC><Colon_QC><Patient_sex>1</Patient_sex><LesionData><Lesion>A</Lesion><LesionA>01</LesionA></LesionData></Colon_QC></ColonQCFile>";
            string TargetPath= @"D:\tet.xml";
            CreateFile.CreateXmlFile(Value);
            XmlDocument x = new XmlDocument();
            x.Load(TargetPath);
            string Target = x.OuterXml;
            Assert.AreEqual(Target, Value);
        }
        [TestMethod]
        public void TestADToROCYear()
        {
            string Value = DateFormat.ADToROCYear("2020") ;
            string Target = "109";
            Assert.AreEqual(Target, Value);
        }
        [TestMethod]
        public void TestROCYearToAD()
        {
            string Value = DateFormat.RocYearToAD("109");
            string Target = "2020";
            Assert.AreEqual(Target, Value);
        }
        [TestMethod]
        public void TestIsNumeric()
        {
            bool Value = false;
            Value=DataCheck.IsNumeric("0123456789");
            bool Target = true;
            Assert.AreEqual(Target, Value);
        }

    }
}

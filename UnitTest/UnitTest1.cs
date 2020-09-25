using Microsoft.VisualStudio.TestTools.UnitTesting;
using CurrencyLibrary;
using System.Data;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly Class1 fromXmlToXml=new Class1();
        
        

        [TestMethod]
        public void TestMethoda()
        {
            string[] _orderBy = { "Isim;ASC" };
            string[] _where = { "ForexBuying;>0" };
            
           var data= fromXmlToXml.ReadXml("https://www.tcmb.gov.tr/kurlar/today.xml");
            dt = data;
            dv = new DataView(dt);
            var sonuc=false;
            if (dt.Rows.Count> 0)
            {
                sonuc = true;
                
                
               
                
            }

            Assert.IsTrue(sonuc, "dt is ok");
            




        }
        public  DataView dv;
        public  DataTable dt;
        [TestMethod]
        [DataRow]
        public void  TestMethodFiltering()
        {

            //dv = new DataView(dt);
            var data = fromXmlToXml.ReadXml("https://www.tcmb.gov.tr/kurlar/today.xml");
            dt = data;
            var dw = fromXmlToXml.FilteringDataTable(dt, "Isim = 'ABD DOLARI'");
            
             Assert.IsNotNull(dw);
           
            


        }
        [TestMethod]
        public void TestMethodSorting()
        {
            var dview = dv;

            //dv = new DataView(dt);
            var data = fromXmlToXml.ReadXml("https://www.tcmb.gov.tr/kurlar/today.xml");
            dt = data;
            var dw = fromXmlToXml.SortingDataTable(dt, "Isim asc,ForexBuying desc");

            Assert.IsNotNull(dw,"Datatable not null");



        }
        [TestMethod]
        public void TestMethodCreateXMLContent()
        {
            var dview = dv;

            //dv = new DataView(dt);
            var data = fromXmlToXml.ReadXml("https://www.tcmb.gov.tr/kurlar/today.xml");
            dt = data;
            var content = fromXmlToXml.DataTableToCSV(dt);

            Assert.AreNotEqual(content, "");



        }
        [TestMethod]
        public void TestMethodCreateJSONContent()
        {
            var dview = dv;

            //dv = new DataView(dt);
            var data = fromXmlToXml.ReadXml("https://www.tcmb.gov.tr/kurlar/today.xml");
            dt = data;
            var content = fromXmlToXml.DataTableToJSONWithStringBuilder(dt);

            Assert.AreNotEqual(content, "");

        }
    }
}

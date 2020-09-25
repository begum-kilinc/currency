using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Globalization;

namespace CurrencyLibrary
{
    public class Class1
    {
        //private string[] orderBy;

        public Class1()
        {
           
            //ReadXml(orderBy,where);
        }

        public  DataTable  ReadXml(string _file_uri)
        {
            DataTable dt=new DataTable();
            try
            {
                XmlDocument xmldoc = new XmlDocument();
           // DataTable dt;
            String URLString = _file_uri;

            //XmlTextReader reader = new XmlTextReader(URLString);

            XElement root = XElement.Load(URLString);
            IEnumerable<XElement> orderedtabs = root.Elements();
            List<XElement> _list = new List<XElement>();
                string orderByString = "";
                /*if (_orderby.Length > 0)
                {
                    
                       // orderByString += "orderby";
                    for (int i = 0; i < _orderby.Length; i++)
                    {
                        string[] item = _orderby[i].Split(';');
                        orderByString +=" m.Element("+ item[0].ToString()+").Value "+item[1];
        
                }
                }
                string whereString = "";
                if(_where.Length>0)
                {
                    whereString += "where ";
                    for(int i=0;i<_where.Length;i++)
                    {
                        string[] item = _where[i].Split(";");
                        whereString = " m.Element(" + item[0].ToString() + ").Value " + item[1];
                    }

                }*/
                        
            var result = from el in root.Elements() select el;
                /*var a = @"from m in result 
                     "+ whereString+
                     @"orderby 
Math.Round(Convert.ToDecimal(m.Element('ForexBuying').IsEmpty ? 0 : Convert.ToDecimal(m.Element('ForexBuying').Value)) /
Convert.ToDecimal(m.Element('ForexSelling').IsEmpty ? 1 : Convert.ToDecimal(m.Element('ForexSelling').Value)), 4) descending, "
+ orderByString+@"   select m; ";*/
            var ss = from m in result
                       
                     orderby
Math.Round(Convert.ToDecimal(m.Element("ForexBuying").IsEmpty ? 0 : Convert.ToDecimal(m.Element("ForexBuying").Value)) / Convert.ToDecimal(m.Element("ForexSelling").IsEmpty ? 1 : Convert.ToDecimal(m.Element("ForexSelling").Value)), 4) descending
, orderByString 
                     select m;
            root.ReplaceAll(ss);
                 dt = new DataTable();
                var elements=root.Elements().First();
                foreach (XElement el in elements.Elements())
                {
                    try
                    {
                        dt.Columns.Add(el.Name.LocalName);
                    }
                    catch(Exception ex)
                    {
                        break;
                    }
                }
                
            //var son= result.OrderBy(k=> Convert.ToDecimal(k.Element("ForexBuying").Value)/ Convert.ToDecimal(k.Element("ForexSelling").Value));
            foreach (XElement el in root.Elements())
            {

                decimal sonuc = 0;
                try
                {

                      DataRow dr=  dt.NewRow();
                      for(int i=0;i<  dt.Columns.Count;i++)
                        {
                            dr[i] = el.Element(dt.Columns[i].ColumnName).Value;
                        }
                        dt.Rows.Add(dr);
                    sonuc = (decimal)el.Element("ForexBuying") / (decimal)el.Element("ForexSelling");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                }
                
            }
            
                return dt;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return dt;
            }
            
           


        }
        public DataTable FilteringDataTable(DataTable dt,string filter)
        {
            //DataView dw = new DataView(dt);
            DataView dw= new DataView(dt);
            dw.RowFilter = filter;
            
            dw.ToTable(true);
            DataTable newdt = new DataTable();
            //dt.AsDataView();
            DataTable d = dw.ToTable();
            d.TableName = "CurrencyTable";
            return d;
        }
        public DataTable SortingDataTable(DataTable dt, string sort)
        {
            //DataView dw = new DataView(dt);
            DataView dw = new DataView(dt);
            dw.Sort = sort;
            DataTable d = dw.ToTable();
            d.TableName = "CurrencyTable";
            //dt.AcceptChanges();
            return d;
        }
        public bool CreateFile(DataTable dt,string _file)
        {
            try
            {
                string _format = _file.Split('.')[1];
                if (_format == "xml")
                {
                    dt.WriteXml(_file);
                   /* string xml = File.ReadAllText("curr.xml");

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);*/
                   // string json = JsonSerializer.Serialize(doc.InnerXml);

                    
                    return true;
                }
                else if (_format == "csv")
                {
                   string csvstring= DataTableToCSV(dt);

                    File.WriteAllText(_file,csvstring);
                    return true;
                }
                else if(_format=="json")
                { 
                  string jsonstring=  DataTableToJSONWithStringBuilder(dt);
                    File.WriteAllText(_file, jsonstring);
                    return true; }
                else
                {
                    return false;
                }
                }
                catch (Exception ex)
                {
                    return false;
                }
            
        }
        public string DataTableToCSV(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }
            return sb.ToString();
        }
        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }
        }
}

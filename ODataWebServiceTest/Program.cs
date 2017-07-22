using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace ODataWebServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //readUnitOfMeasure();
            //readCustomer();
            readTest();
            writeTest();
            Console.ReadKey();
        }

        static void readUnitOfMeasure()
        {
            string baseUrl = getBaseUrl();
            var json = GET(getBaseUrl() + "/JsonUnitOfMeasure");

            var unitOfMeasures = JsonConvert.DeserializeObject<ODataUnitOfMeasure<UnitOfMeasure>>(json);
            foreach (var unitOfMeasure in unitOfMeasures.Value)
                Console.WriteLine(unitOfMeasure.UOM);
        }
        
        static void readCustomer()
        {
            string baseUrl = getBaseUrl();
            var json = GET(getBaseUrl() + "/JsonCustomer");

            var customers = JsonConvert.DeserializeObject<ODataCustomer<Customer>>(json);
            foreach (var customer in customers.Value)
                Console.WriteLine(customer.Name);
        }

        static void readTest()
        {
            var json = GET(getBaseUrl() + "/JsonTestPost");

            var tests = JsonConvert.DeserializeObject<ODataTest<TestPost>>(json);
            foreach (var test in tests.Value)
                Console.WriteLine(test.Test);
        }
        static void writeTest()
        {

            TestPost test = new TestPost();
            test.Test = "New Record";
            test.Entry_No = 10;
            Console.WriteLine(test.ToString());
            var json = JsonConvert.SerializeObject(test);
            Console.WriteLine(json);
            POST(getBaseUrl() + "/JsonTestPost", json);

        }
        static string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UseDefaultCredentials = true;
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }

        static void POST(string url, string jsonContent)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UseDefaultCredentials = true;
            request.Method = "POST";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    length = response.ContentLength;
                }
            }
            catch (WebException ex)
            {
                
                // Log exception and throw as for GET example above
            }
        }

        private static string getBaseUrl()
        {
            return "http://desktop-ahmvit0:7048/DynamicsNAV100/ODataV4/Company('CRONUS%20International%20Ltd.')";
        }

        internal class ODataUnitOfMeasure<T>
        {
            public List<UnitOfMeasure> Value { get; set; }
        }

        internal class ODataCustomer<T>
        {
            public List<Customer> Value { get; set; }
        }

        internal class ODataTest<T>
        {
            public List<TestPost> Value { get; set; }
        }

        internal class UnitOfMeasure
        {
            public string UOM { get; set; }
            public string Length { get; set; }
            public string Width { get; set; }
        }

        internal class Customer
        {
            public string No { get; set; }
            public string Name { get; set; }
            public string E_Mail { get; set; }
            public string Phone_No { get; set; }
        }

        internal class TestPost
        {
            public int Entry_No { get; set; }
            public string Test { get; set; }
        }
    }
}

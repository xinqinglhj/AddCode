using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
//using MongoDB.Driver;

namespace CommitBug
{
    internal static class Program
    {
        //这是一个测试类库，用来测试项目
        private static void Main(string[] args)
        {
            //MongodbHelper h = new MongodbHelper();
            //var collection = h.GetCollection<BugModel>();
            //var resual6 = collection.Find(x => x.Id == "cdcea4bf-4a32-4d9e-86eb-0250a69f93e9").FirstAsync();
            //var resual7 = collection.Find(x => x.Id == "cdcea4bf-4a32-4d9e-86eb-0250a69f93e9").SingleAsync();
            //var aaa = resual6.Result;

            //bug.GetData("cdcea4bf-4a32-4d9e-86eb-0250a69f93e9");

            //var bug = new CommitBugBase();
            //var item = bug.GetBugModel("dc83acac-4db5-4858-a3ad-7f3a6b82d390");
            //bug.SetData(new BugModel());
            /*
            var lift = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 7, 9, 10 };

            var newOrderby = lift.OrderBy(x => x);
            Console.WriteLine(string.Join(",", newOrderby));

            newOrderby.Aggregate("", (x, y) => x + "," + y);*/

            //var item = Getlamda((x, y) => (x + y).ToString());
            //Console.WriteLine(item(1, 2));

            //var item1 = ConvertBase64(item, Converts.ReConvert);
            OpenReadWithHttps("http://localhost:6935/api/CommitBug/setBug", "model=");


        }


        /// 采用https协议访问网络
        /// <param name="url">url地址</param>
        /// <param name="strPostdata">发送的数据</param>
        /// <param name="strEncoding"></param>
        /// <returns></returns>
        public static string OpenReadWithHttps(string url, string strPostdata)
        {
            var encoding = Encoding.UTF8;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "post";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/x-www-form-urlencoded";
            var buffer = encoding.GetBytes(strPostdata);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            var response = (HttpWebResponse)request.GetResponse();
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }

        }


        /// <summary>
        /// Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetUrltoHtml(string url, string type)
        {
            try
            {
                var wReq = System.Net.WebRequest.Create(url);
                // Get the response instance.
                var wResp = wReq.GetResponse();
                var respStream = wResp.GetResponseStream();
                // Dim reader As StreamReader = New StreamReader(respStream)
                if (respStream != null)
                    using (var reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding(type)))
                    {
                        return reader.ReadToEnd();
                    }
            }
            catch (Exception)
            {
                //errorMsg = ex.Message;
            }
            return "";
        }


        public static string ConvertBase64(string code, Converts conv)
        {
            if (conv == Converts.Convert)
            {
                var bytes = Encoding.Default.GetBytes(code);
                return Convert.ToBase64String(bytes);
            }
            else
            {
                //解码：
                var outputb = Convert.FromBase64String(code);
                return Encoding.Default.GetString(outputb);

            }
        }

        public enum Converts
        {
            Convert,
            ReConvert
        }

        public static Func<int, int, string> Getlamda(Func<int, int, string> func)
        {
            return func;
        }
    }
}

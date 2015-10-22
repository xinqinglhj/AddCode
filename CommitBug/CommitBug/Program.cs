using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using CommitBugLab;
using CommitBugLab.Base;
using CommitBugLab.Help;
using CommitBugLab.Interface;
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

            var bug = new CommitBugBase();
            var item = bug.GetBugModel("dc83acac-4db5-4858-a3ad-7f3a6b82d390");
            //bug.SetData(new BugModel());

            var lift = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 7, 9, 10 };

            var newOrderby = lift.OrderBy(x => x);
            Console.WriteLine(string.Join(",", newOrderby));

            newOrderby.Aggregate("", (x, y) => x + "," + y);
        }
    }
}

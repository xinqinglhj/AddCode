using System;
using System.Collections.Generic;
using CommitBugLab;
using CommitBugLab.Base;

namespace CommitBug
{
    class Program
    {

        //这是一个测试类库，用来测试项目
        static void Main(string[] args)
        {
            BugModel model = new BugModel();
            //model.Exce = ex;
            model.Ip = "192.168.31.2";
            IBugControllerInterface b = new CommitBugBase();
            var guid = b.SaveBug(model);

        }
    }
}

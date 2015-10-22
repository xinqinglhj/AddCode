using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddCode
{
    class Program
    {

        //第一个参数是代码  //必选
        //第二个参数是该页面路径或该页面的名称，作为该页的唯一的标示   //可选
        //第三个参数是日志存放路径 //可选
        //New 第一个是项目地址
        static void Main(string[] args)
        {
            switch (args.Length)
            {
                case 1:
                    var read = new StreamReader(args[0]);
                    var text = read.ReadToEnd();
                    var codes = new[] { text, args[0] };
                    Save(codes);
                    break;
                case 2:
                    Output(args[1]);
                    break;
                default:
                    Console.WriteLine("you not input parements ?");
                    break;
            }
        }

        public static void Output(string path)
        {
            first:
            var list = GetCode().FindAll(p => p.CodeName == path);

            for (var index = 0; index < list.Count; index++)
            {
                Console.WriteLine(index + 1 + " " + list[index].CodeCreateTime + " " + list[index].CodeName);
            }
            Console.WriteLine((list.Count + 1) + " other");

            var re = Convert.ToInt32(Console.ReadLine());

            if (re != list.Count + 1)
            {
                select:
                Console.WriteLine("Selected " + list[re - 1].CodeCreateTime + " [Yes=Cover | No=return | Preview] (y/n/p)");
                var readLine = Console.ReadLine();
                if (readLine != null)
                {
                    var input = readLine.ToLower();

                    switch (input)
                    {
                        case "y":
                            var w = new StreamWriter(path);
                            w.Write(list[re - 1].CodeText);
                            w.Close();
                            break;
                        case "p":
                            Console.WriteLine(list[re - 1].CodeText);
                            goto select;
                        default:
                            goto first;
                    }
                }
            }

        }


        //将所有的bug存储下来
        public static void Save(string[] code)
        {
            var c = new DataController();
            var m = new Model();
            if (code.Length > 2)
            {
                DataController.PathAddress = code[2];
            }

            if (code.Length > 1)
            {
                m.CodeName = code[1];
            }
            m.CodeText = code[0];
            c.Save(m);
            Console.WriteLine("OK");
        }


        public static List<Model> GetCode()
        {

            var arrayList = DataController.GetAllCode();
            var list = new List<Model>();

            foreach (var item in arrayList)
            {
                if (item.Length < 4) continue;
                var m = new Model();
                var thisItem = item.Split(' ');
                var date = thisItem[0] + " " + thisItem[1];
                m.CodeCreateTime = Convert.ToDateTime(date);
                m.CodeName = DataController.ConvertBase64(thisItem[2], DataController.Converts.ReConvert);
                m.CodeText = DataController.ConvertBase64(thisItem[3], DataController.Converts.ReConvert);
                list.Add(m);
            }
            return list;
        }
    }
}

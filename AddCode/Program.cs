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

            //args = new string[1] { "D:\\Projects\\AddCode\\AddCode\\Program.cs" };
            // args = new string[2];
            if (args.Length == 1)
            {
                StreamReader read = new StreamReader(args[0]);
                var text = read.ReadToEnd();
                var codes = new string[] { text, args[0] };
                Save(codes);
            }
            //还原版本
            else if (args.Length == 2)
            {
                output(args[1]);
                //Console.WriteLine(args[1]);
                //Console.ReadKey();
                //output("e:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\VC\\include\\ConcurrencySal.h");
            }
            else
            {
                Console.WriteLine("you not input parements ?");
            }
        }

        public static void output(string path)
        {
            first:
            List<Model> list = getCode().FindAll(p => p.CodeName == path);

            for (int index = 0; index < list.Count; index++)
            {
                Console.WriteLine(index + 1 + " " + list[index].CodeCreateTime + " " + list[index].CodeName);
            }
            Console.WriteLine((list.Count + 1) + " other");

            int re = Convert.ToInt32(Console.ReadLine());

            if (re != list.Count + 1)
            {
                select:
                Console.WriteLine("Selected " + list[re - 1].CodeCreateTime + " [Yes=Cover | No=return | Preview] (y/n/p)");
                var input = Console.ReadLine().ToLower();

                if (input == "y")
                {
                    StreamWriter w = new StreamWriter(path);
                    w.Write(list[re - 1].CodeText);
                    w.Close();
                }
                else if (input == "p")
                {
                    Console.WriteLine(list[re - 1].CodeText);
                    goto select;
                }
                else
                {
                    goto first;
                }
            }

        }


        //将所有的bug存储下来
        public static void Save(string[] code)
        {
            DataController c = new DataController();
            Model m = new Model();
            if (code.Length > 2)
            {
                DataController.pathAddress = code[2];
            }

            if (code.Length > 1)
            {
                m.CodeName = code[1];
            }
            m.CodeText = code[0];
            c.Save(m);
            Console.WriteLine("OK");
        }


        public static List<Model> getCode()
        {

            var ArrayList = DataController.GetAllCode();
            List<Model> list = new List<Model>();

            foreach (var item in ArrayList)
            {
                if (item.Length < 4) continue;
                Model m = new Model();
                var thisItem = item.Split(' ');
                var date = thisItem[0] + " " + thisItem[1];
                m.CodeCreateTime = Convert.ToDateTime(date);
                m.CodeName = DataController.convertBase64(thisItem[2], DataController.Converts.ReConvert);
                m.CodeText = DataController.convertBase64(thisItem[3], DataController.Converts.ReConvert);
                list.Add(m);
            }
            return list;
        }
    }
}

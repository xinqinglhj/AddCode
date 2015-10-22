using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddCode
{
    public class DataController
    {
        public static string PathAddress = "D:\\code.txt";
        public bool Save(Model model)
        {
            var base64Code = ConvertBase64(model.CodeText, Converts.Convert);
            var address = ConvertBase64(model.CodeName, Converts.Convert);


            var w = new StreamWriter(PathAddress, true);
            w.WriteLine(model.CodeCreateTime + " " + address + " " + base64Code);
            w.Close();
            return true;
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

        public static string[] GetAllCode()
        {
            var read = new StreamReader(PathAddress);
            if (!File.Exists(PathAddress))
            {
                return null;
            }
            else
            {
                var list = new List<string>();

                var test = read.ReadToEnd().Replace("\r\n", "$").Split('$');
                return test;
            }

        }

        public enum Converts
        {
            Convert,
            ReConvert
        }
    }
}

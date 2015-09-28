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
        public static string pathAddress = "D:\\code.txt";
        public bool Save(Model model)
        {
            string base64Code = convertBase64(model.CodeText, Converts.Convert);
            string address = convertBase64(model.CodeName, Converts.Convert);


            StreamWriter w = new StreamWriter(pathAddress, true);
            w.WriteLine(model.CodeCreateTime + " " + address + " " + base64Code);
            w.Close();
            return true;
        }
        public static string convertBase64(string code, Converts conv)
        {
            if (conv == Converts.Convert)
            {
                byte[] bytes = Encoding.Default.GetBytes(code);
                return Convert.ToBase64String(bytes);
            }
            else
            {
                //解码：
                byte[] outputb = Convert.FromBase64String(code);
                return Encoding.Default.GetString(outputb);

            }
        }

        public static string[] GetAllCode()
        {
            StreamReader read = new StreamReader(pathAddress);
            if (!File.Exists(pathAddress))
            {
                return null;
            }
            else
            {
                List<string> list = new List<string>();

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

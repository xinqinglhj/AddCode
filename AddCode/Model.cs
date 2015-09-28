using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddCode
{
    public class Model
    {
        public string CodeName { get; set; }
        public string CodeText { get; set; }
        public DateTime CodeCreateTime { get; set; }
        public Model() {
            this.CodeCreateTime = DateTime.Now;
        }
    }
}

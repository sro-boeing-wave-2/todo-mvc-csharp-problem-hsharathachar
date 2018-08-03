using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_api.Models
{
    public class CheckList
    {
        public int ID { get; set; }
        public string CheckListData { get; set; }
        public bool Status { get; set; }
    }
}

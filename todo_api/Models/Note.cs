using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_api.Models
{
    public class Note
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string PlainText { get; set; }

        internal static object Include(Func<object, object> p)
        {
            throw new NotImplementedException();
        }

        public bool Pinned { get; set; }

        public List<CheckList> CheckLists { get; set; }
        public List<Label> Labels { get; set; }
    }
}

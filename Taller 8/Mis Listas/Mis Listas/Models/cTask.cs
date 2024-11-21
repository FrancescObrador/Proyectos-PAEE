using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mis_Listas.Models
{
    class cTask
    {
        public string text { get; set; }
        public DateTime date { get; set; }
        public DateTime? expired { get; set; }
        public bool visible { get; set; }
        public bool finished { get; set; }
        
        public cTask()
        {
            text = "";
            date = DateTime.Now;
            expired = null;
            visible = true;
            finished = false;
        }
    }
}

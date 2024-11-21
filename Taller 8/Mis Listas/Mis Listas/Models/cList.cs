using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mis_Listas.Models
{
    class cList
    {
        public string name {  get; set; }
        public DateTime date {  get; set; }
        public bool visible { get; set; }
        public string color { get; set; }
        public List<cTask> tasks { get; set; }
        
        public cList() 
        {
            name = "";
            date = DateTime.Now;
            visible = true;
            color = "";
            tasks = new List<cTask>();
        }
    }
}

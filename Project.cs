using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo
{
    internal class Project : Functions
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public bool Status { get; set; }
        public List<Task> Tasks = new List<Task> ();
        public string CompletedWhen { get; set; }

        //constructors
        public Project() {}
        public Project(string title, DateTime date, bool status)
        {
            Title = title;
            DueDate = date;
            Status = status;
        }
        public Project(string title, DateTime date, bool status, List<Task> tasks)
        {
            Title = title;
            DueDate = date;
            Status = status;
            Tasks = tasks;
        }
    }
}

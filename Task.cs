using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo
{
    internal class Task : Functions
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public bool Status { get; set; }
        public Project Project { get; set; }
        public string CompletedWhen { get; set; }

        //constructors
        public Task() { }
        public Task(string title, DateTime duedate, bool status)
        {
            Title = title;
            DueDate = duedate;
            Status = status;
        }
        public Task(string title, DateTime duedate, bool status, Project project) 
        {
            Title = title;
            DueDate = duedate;
            Status = status;
            Project = project;
        }
    }
}

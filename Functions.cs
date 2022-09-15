using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo
{
    internal class Functions
    {
        //color shortcuts
        public void Red()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        public void Green()
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        public void Yellow()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        public void Reset()
        {
            Console.ResetColor();
        }
        
        public string CheckNullOrEmpty(Functions func)
        {
            //checking and forcing user to input something
            string input = "";
            while (true)
            {
                Console.Write("> ");
                input = Console.ReadLine().Trim();
                if (String.IsNullOrEmpty(input))
                {
                    func.Red();
                    Console.WriteLine("Please input something...");
                    func.Reset();
                    continue;
                }
                else { break; }
            }
            return input;
        }
        public List<Task> TaskTemplate()
        {
            //just add/change contents of array to modify the created tasks
            string[] startingTasks = new string[] { "Specify goals", "Make a plan", "Make goals and plan into tasks" };
            DateTime today = DateTime.Today;
            bool status = false;
            List<Task> tasks = new List<Task>();
            
            //creates and adds 3 "starter-tasks" with incrementing dueDates
            int counter = 1;
            foreach (string title in startingTasks)
            {
                Task newTask = new Task(title, today.AddDays(counter), status);
                tasks.Add(newTask);
                counter++;
            }
            return tasks;
        }
        public User ProjectTemplate(User user)
        {
            Random roll = new Random();
            DateTime manyDays = DateTime.Today.AddDays(roll.Next(10, 101));
            bool status = false; 
            
            //chooses a random title for the new project
            Project project = new Project(user.randomProjects[roll.Next(0, user.randomProjects.Count())], manyDays, status);
            user.randomProjects.Remove(project.Title);
            user.Projects.Add(project);

            return user;
        }
        public User AutoFill(User user, Functions func, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (user.randomProjects.Count() == 0)
                {
                    Console.WriteLine("We ran out of ideas. You have to make up your own projects from now on.");
                    Console.Write("(press enter to continue) ");
                    Console.ReadLine();
                }
                else
                {
                    //creates a random project
                    func.ProjectTemplate(user);

                    //assigns tasks to that project
                    user.Projects[user.Projects.Count() - 1].Tasks = func.TaskTemplate();

                    //assigns the project to each of those tasks
                    foreach (var task in user.Projects[user.Projects.Count() - 1].Tasks)
                    {
                        task.Project = user.Projects[user.Projects.Count() - 1];
                    }
                }
            }
            return user;
        }
    }
}

// See https://aka.ms/new-console-template for more information
using ToDo;

User user = new User();
Functions func = new Functions();

Load(user);
if (String.IsNullOrEmpty(user.Name)) { GetName(user, func); }
if (user.Projects.Count() == 0) { AskForAutoFill(user, func); }
MainMenu(user, func);
Save(user);

Console.WriteLine("--Program end");


static void MainMenu(User user, Functions func)
{
    bool projectsDateSorted = false;
    while (true)
    {
        //printing all projects and a menu
        PrintProjects(user, func, projectsDateSorted);

        //verifying input for the menu choice
        string input = func.CheckNullOrEmpty(func);
        bool isInt = int.TryParse(input, out int chosenProject);
        int chosenProjectIndex = chosenProject - 1;


        if (input.ToUpper() == "Q") { break; }
        else if (input.ToUpper() == "C") { CreateProject(user, func); continue; }
        else if (input.ToUpper() == "D") { DeleteProject(user, func); continue; }
        else if (input.ToUpper() == "M") { ChangeProjectStatus(user, func); continue; }
        else if (chosenProjectIndex <= user.Projects.Count() && chosenProjectIndex > -1) 
        {
            //displays chosen project and prints its tasks 
            MenuForTasks(user, func, chosenProjectIndex); 
            continue; 
        } 
        //sorting by date or alphabetical (toggle)
        else if (input.ToUpper() == "S" && !projectsDateSorted)
        {
            user.Projects.Sort((a, b) => a.DueDate.CompareTo(b.DueDate));
            projectsDateSorted = true;
            continue;
        }
        else if (input.ToUpper() == "S" && projectsDateSorted)
        {
            user.Projects.Sort((a, b) => a.Title.CompareTo(b.Title));
            projectsDateSorted = false;
            continue;
        }
        else { Stop(); continue; }
    }
}

static User GetName(User user, Functions func)
{
    Console.WriteLine("Please start by entering your name");
    user.Name = func.CheckNullOrEmpty(func);
    func.Green();
    Console.WriteLine($"Thank you, {user.Name}.");
    func.Reset();
    return user;
}

static void AskForAutoFill(User user, Functions func)
{
    Console.WriteLine("\nAutofill program with random projects to get started? (Y/N)");
    while (true)
    {
        string YN = func.CheckNullOrEmpty(func);
        if (YN.ToUpper() == "N") { break; }
        //adds 10 random projects 
        else if (YN.ToUpper() == "Y") { func.AutoFill(user, func, 10); break; }
        else
        {
            func.Red();
            Console.WriteLine("Please input Y or N...");
            func.Reset();
            continue;
        }
    }
}

static void PrintProjects(User user, Functions func, bool projectsDateSorted)
{
    Console.Clear();
    Console.WriteLine("-PROJECTS-");
    Console.WriteLine("Input corresponding number to view and edit your project, or choose from the menu with letters. \n");

    int counter = 1;
    foreach (Project project in user.Projects)
    {
        //var diff = project.DueDate - DateTime.Today;
        //DateTime limit = DateTime.Today.AddDays(30);
        //if (diff < limit) { func.Yellow() }

        //coloring projects by date or status
        if (project.Status) { func.Green(); }
        else if ((project.DueDate - DateTime.Today).TotalDays < 30) { func.Red(); } //https://stackoverflow.com/questions/528368/datetime-compare-how-to-check-if-a-date-is-less-than-30-days-old
        else if ((project.DueDate - DateTime.Today).TotalDays < 60) { func.Yellow(); }
        
        if (String.IsNullOrEmpty(project.CompletedWhen))
        {
            Console.WriteLine($"'{counter}' {project.Title}".PadRight(35) + $"| due: {project.DueDate.ToString("dd/MM-yyyy")}");
        }
        else
        {
            Console.WriteLine($"'{counter}' {project.Title}".PadRight(35) + $"| completed on: {project.CompletedWhen}");
        }
        counter++;
        func.Reset();
    }
    if (user.Projects.Count() == 0) { Console.WriteLine("(no projects created yet)"); }
    Console.WriteLine();

    if (!projectsDateSorted) { Console.WriteLine("'S' to sort projects based on date"); }
    else { Console.WriteLine("'S' to sort projects in alphabetical order"); }
    Console.WriteLine("'C' to Create new project.");
    Console.WriteLine("'D' to Delete a project.");
    Console.WriteLine("'M' to Mark a project as 'completed' (or back to 'not completed')");
    Console.WriteLine("'Q' to Quit program (and save data)");
    Console.WriteLine();
}

static User CreateProject(User user, Functions func)
{
    Console.Clear();
    while (true)
    {
        Console.WriteLine("-CREATE PROJECT-");
        Console.WriteLine("Input 'R' for a random project, or input your own project title.");

        string inputTitle = func.CheckNullOrEmpty(func);
        if (inputTitle.ToUpper() == "R")
        {
            //creates and adds a random project automatically
            user.ProjectTemplate(user);
            func.Green();
            Console.WriteLine($"'{user.Projects[user.Projects.Count() - 1].Title}' was created.");
            Console.WriteLine($"complete before '{user.Projects[user.Projects.Count() - 1].DueDate.ToString("dd/MM-yyyy")}'");
            func.Reset();
            Console.Write("(press enter to continue) ");
            Console.ReadLine();
            break;
        }

        //verifying input for manually created project 
        Console.WriteLine("In how many days should the project be completed? ");
        int numberOfDays = 0;
        while (true)
        {
            string inputDays = func.CheckNullOrEmpty(func);
            bool isInt = int.TryParse(inputDays, out numberOfDays);
            if (!isInt)
            {
                func.Red();
                Console.WriteLine("Please input a number. ");
                func.Reset();
                continue;
            }
            else { break; }
        }

        //creates and adds the manually inputed project
        DateTime dueDate = DateTime.Today.AddDays(numberOfDays);
        bool status = false;
        Project newProject = new Project(inputTitle, dueDate, status);
        user.Projects.Add(newProject);

        func.Green();
        Console.WriteLine($"'{newProject.Title}' was created.");
        Console.WriteLine($"complete before '{newProject.DueDate.ToString("dd/MM-yyyy")}'");
        func.Reset();
        Console.Write("(press enter to continue) ");
        Console.ReadLine();
        return user;
    }
    return user;
}

static User DeleteProject(User user, Functions func)
{
    while (true)
    {
        if (user.Projects.Count() == 0)
        {
            Console.WriteLine("There are no projects to be deleted...");
            Console.Write("(press enter to continue) ");
            Console.ReadLine();
            break;
        }
        Console.WriteLine("Please input the corresponding number for the project you wish to delete. ('Q' to cancel)");
        string input = func.CheckNullOrEmpty(func);
        if (input.ToUpper() == "Q") { break; }

        bool isInt = int.TryParse(input, out int menuChoice);
        if (!isInt)
        {
            func.Red();
            continue;
        }
        //deletes the chosen project after the input is verified
        else if (menuChoice <= user.Projects.Count() && menuChoice > 0)
        {
            string deleted = user.Projects[menuChoice-1].Title;
            user.Projects.RemoveAt(menuChoice-1);
            func.Green();
            Console.WriteLine($"'{deleted}' was deleted");
            func.Reset();
            Console.Write("(press enter to continue) ");
            Console.ReadLine();
            break;
        }
        else
        {
            func.Red();
            continue;
        }
    }
    return user;
}

static User ChangeProjectStatus(User user, Functions func)
{
    while (true)
    {
        if (user.Projects.Count() == 0)
        {
            Console.WriteLine("There are no projects to change status on.");
            Console.Write("(press enter to continue) ");
            Console.ReadLine();
            break;
        }
        Console.WriteLine("Please input the corresponding number for the project you wish to change status on. ('Q' to cancel)");
        string input = func.CheckNullOrEmpty(func);
        if (input.ToUpper() == "Q") { break; }

        bool isInt = int.TryParse(input, out int menuChoice);
        if (!isInt)
        {
            func.Red();
            continue;
        }
        else if (menuChoice <= user.Projects.Count() && menuChoice > 0)
        {
            func.Green();
            if (!user.Projects[menuChoice - 1].Status)
            {
                user.Projects[menuChoice - 1].Status = true;
                user.Projects[menuChoice - 1].CompletedWhen = DateTime.Now.ToString("dd/MM-yyyy, HH:mm:ss");
                Console.WriteLine($"'{user.Projects[menuChoice - 1].Title}' is now marked as 'completed', congratulations!");
            }
            else
            {
                user.Projects[menuChoice - 1].Status = false;
                user.Projects[menuChoice - 1].CompletedWhen = "";
                Console.WriteLine($"'{user.Projects[menuChoice - 1].Title}' is now marked as 'not completed', aaww....");
            }
            func.Reset();
            Console.Write("(press enter to continue) ");
            Console.ReadLine();
            break;
        }
        else
        {
            func.Red();
            continue;
        }
    }
    return user;
}

static void Stop()
{
    string stop = "Stop trying to break the program please...\n(press enter to continue) ";
    foreach (char c in stop)
    {
        Console.Write(c);
        Thread.Sleep(20); //knock knock neo
    }
    Console.ReadLine();
}

static User MenuForTasks(User user, Functions func, int chosenProjectIndex)
{
    
    bool taskDateSorted = false;
    while (true)
    {
        //prints the tasks for chosen project and a menu
        PrintTasks(user, func, chosenProjectIndex, taskDateSorted);

        //verifying input for the menu choice
        string input = func.CheckNullOrEmpty(func);
        bool isInt = int.TryParse(input, out int chosenTask);
        int chosenTaskIndex = chosenTask - 1;

        if (input.ToUpper() == "Q") { break; }
        else if (input.ToUpper() == "C") { CreateTask(user, func, chosenProjectIndex); continue; }
        else if (input.ToUpper() == "D") { DeleteTask(user, func, chosenProjectIndex); continue; }
        else if (input.ToUpper() == "M") { ChangeTaskStatus(user, func, chosenProjectIndex); continue; }
        else if (chosenTaskIndex <= user.Projects.Count() && chosenTaskIndex > -1)
        {
            //displays chosen project and prints its tasks 
            MenuForTasks(user, func, chosenTaskIndex);
            continue;
        }
        //sorting by date or alphabetical (toggle)
        else if (input.ToUpper() == "S" && !taskDateSorted)
        {
            user.Projects[chosenProjectIndex].Tasks.Sort((a, b) => a.DueDate.CompareTo(b.DueDate));
            taskDateSorted = true;
            continue;
        }
        else if (input.ToUpper() == "S" && taskDateSorted)
        {
            user.Projects[chosenProjectIndex].Tasks.Sort((a, b) => a.Title.CompareTo(b.Title));
            taskDateSorted = false;
            continue;
        }
        else { Stop(); continue; }
    }
    return user;
}

static void PrintTasks(User user, Functions func, int chosenProjectIndex, bool taskDateSorted)
{
    Console.Clear();
    Console.WriteLine($"PROJECT: {user.Projects[chosenProjectIndex].Title}");
    Console.WriteLine();
    Console.WriteLine("TASKS: ");
    for (int i = 0; i < user.Projects[chosenProjectIndex].Tasks.Count(); i++)
    {
        if (user.Projects[chosenProjectIndex].Tasks[i].Status) { func.Green(); }
        else if ((user.Projects[chosenProjectIndex].Tasks[i].DueDate - DateTime.Today).TotalDays < 30) { func.Red(); }
        else if ((user.Projects[chosenProjectIndex].Tasks[i].DueDate - DateTime.Today).TotalDays < 60) { func.Yellow(); }

        if (String.IsNullOrEmpty(user.Projects[chosenProjectIndex].Tasks[i].CompletedWhen))
        {
            Console.WriteLine($"'{i + 1}' {user.Projects[chosenProjectIndex].Tasks[i].Title}".PadRight(35) + $"| due: {user.Projects[chosenProjectIndex].Tasks[i].DueDate.ToString("dd/MM-yyyy")}");
        }
        else
        {
            Console.WriteLine($"'{i + 1}' {user.Projects[chosenProjectIndex].Tasks[i].Title}".PadRight(35) + $"| completed on: {user.Projects[chosenProjectIndex].Tasks[i].CompletedWhen}");
        }
        func.Reset();
    }
    if (user.Projects[chosenProjectIndex].Tasks.Count() == 0) { Console.WriteLine("(no tasks created yet)"); }
    Console.WriteLine();

    if (!taskDateSorted) { Console.WriteLine("'S' to sort tasks based on date"); }
    else { Console.WriteLine("'S' to sort tasks in alphabetical order"); }
    Console.WriteLine("'C' to Create new task.");
    Console.WriteLine("'D' to Delete a task.");
    Console.WriteLine("'M' to Mark a task as 'completed' (or back to 'not completed').");
    Console.WriteLine("'Q' to go back to main menu.");
    Console.WriteLine();
}

static User CreateTask(User user, Functions func, int chosenProjectIndex)
{
    Console.Clear();
    while (true)
    {
        string inputTitle = "";
        Console.WriteLine("-CREATE TASK-");
        if (user.Projects[chosenProjectIndex].Tasks.Count() == 0)
        {
            Console.WriteLine("Input 'X' to add 3 starter-tasks, or input a title for your new task.");

            inputTitle = func.CheckNullOrEmpty(func);
            if (inputTitle.ToUpper() == "X")
            {
                //creates and adds 3 starter tasks automatically
                user.Projects[chosenProjectIndex].Tasks = func.TaskTemplate();

                //assigns the project to each of those tasks
                foreach (var task in user.Projects[chosenProjectIndex].Tasks)
                {
                    task.Project = user.Projects[chosenProjectIndex];
                }

                func.Green();
                Console.WriteLine($"Added 3 tasks to project: '{user.Projects[chosenProjectIndex].Title}'");
                func.Reset();
                Console.Write("(press enter to continue) ");
                Console.ReadLine();
                break;
            }
        }
        else
        {
            Console.WriteLine("Input the title for your new task.");
            inputTitle = func.CheckNullOrEmpty(func);
        }

        //verifying input for manually created project 
        Console.WriteLine("In how many days should the project be completed? ");
        int numberOfDays = 0;
        while (true)
        {
            string inputDays = func.CheckNullOrEmpty(func);
            bool isInt = int.TryParse(inputDays, out numberOfDays);
            if (!isInt)
            {
                func.Red();
                Console.WriteLine("Please input a number. ");
                func.Reset();
                continue;
            }
            else { break; }
        }

        //creates and adds the manually inputed project
        DateTime dueDate = DateTime.Today.AddDays(numberOfDays);
        bool status = false;
        ToDo.Task newTask = new ToDo.Task(inputTitle, dueDate, status); //if i dont have ToDo. i get ambigous error because of system.threading
        //assigns a project to the new task
        newTask.Project = user.Projects[chosenProjectIndex];
        //adds task to chosen project
        user.Projects[chosenProjectIndex].Tasks.Add(newTask);

        func.Green();
        Console.WriteLine($"'{newTask.Title}' was created.");
        Console.WriteLine($"complete before '{newTask.DueDate.ToString("dd/MM-yyyy")}'");
        func.Reset();
        Console.Write("(press enter to continue) ");
        Console.ReadLine();
        return user;
    }
    return user;
}

static User DeleteTask(User user, Functions func, int chosenProjectIndex)
{
    while(true)
    {
        if (user.Projects[chosenProjectIndex].Tasks.Count() == 0)
        {
            Console.WriteLine("There are no tasks to be deleted...");
            Console.Write("(press enter to continue) ");
            Console.ReadLine();
            break;
        }
        Console.WriteLine("Please input the corresponding number for the task you wish to delete. ('Q' to cancel)");
        string input = func.CheckNullOrEmpty(func);

        bool isInt = int.TryParse(input, out int menuChoice);
        if (!isInt)
        {
            func.Red();
            continue;
        }
        //deletes the chosen project after the input is verified
        else if (menuChoice > 0 && menuChoice <= user.Projects[chosenProjectIndex].Tasks.Count())
        {
            string deleted = user.Projects[chosenProjectIndex].Tasks[menuChoice -1].Title;
            user.Projects[chosenProjectIndex].Tasks.RemoveAt(menuChoice - 1);
            func.Green();
            Console.WriteLine($"'{deleted}' was deleted");
            func.Reset();
            Console.Write("(press enter to continue) ");
            Console.ReadLine();
            break;
        }
        else
        {
            func.Red();
            continue;
        }
    }
    return user;
}

static User ChangeTaskStatus(User user, Functions func, int chosenProjectIndex)
{
    while (true)
    {
        if (user.Projects[chosenProjectIndex].Tasks.Count() == 0)
        {
            Console.WriteLine("There are no tasks to change status on.");
            Console.Write("(press enter to continue) ");
            Console.ReadLine();
            break;
        }
        Console.WriteLine("Please input the corresponding number for the task you wish to change status on. ('Q' to cancel)");
        string input = func.CheckNullOrEmpty(func);
        if (input.ToUpper() == "Q") { break; }

        bool isInt = int.TryParse(input, out int menuChoice);
        if (!isInt)
        {
            func.Red();
            continue;
        }
        else if (menuChoice <= user.Projects[chosenProjectIndex].Tasks.Count() && menuChoice > 0)
        {
            func.Green();
            if (!user.Projects[chosenProjectIndex].Tasks[menuChoice - 1].Status)
            {
                user.Projects[chosenProjectIndex].Tasks[menuChoice - 1].Status = true;
                user.Projects[chosenProjectIndex].Tasks[menuChoice - 1].CompletedWhen = DateTime.Now.ToString("dd/MM-yyyy, HH:mm:ss");
                Console.WriteLine($"'{user.Projects[chosenProjectIndex].Tasks[menuChoice - 1].Title}' is now marked as 'completed', congratulations!");
            }
            else
            {
                user.Projects[chosenProjectIndex].Tasks[menuChoice - 1].Status = false;
                user.Projects[chosenProjectIndex].Tasks[menuChoice - 1].CompletedWhen = "";
                Console.WriteLine($"'{user.Projects[chosenProjectIndex].Tasks[menuChoice - 1].Title}' is now marked as 'not completed', aaww....");
            }
            func.Reset();
            Console.Write("(press enter to continue) ");
            Console.ReadLine();
            break;
        }
        else
        {
            func.Red();
            continue;
        }
    }
    return user;
}

static User Save(User user)
{
    //empty so far
    return user;
}

static User Load(User user)
{
    //empty so far
    return user;
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo
{
    internal class User : Functions
    {
        public string Name { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();
        public List<string> randomProjects = new List<string> { "Improve coding skills", "Find internship", "Learn stocktrading", "Learn cryptotrading", "Find a better girlfriend", "Learn a new language", "Renovate house", "Get a million dollars", "Loose weight", "Find a new hobby", "Eat healthier", "Improve cooking skills", "Learn to hack", "Swim to denmark", "Run to Haparanda", "Build a car", "Fast for 30 days", "Reconnect with an old friend", "Sober october", "Benchpress 100kg", "Run marathon", "Visit Texas" };
    }
}

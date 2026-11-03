using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice
{
    public class Student
    {

        public Student() { }

        public Student(string name, int age)  : this (name)
        {
           Age = age;
        }


        public Student(string name)
        {
            Name = name;
        }

        public string Name;    
        public int Age;
      /*  public int Group;*/

        public void Display()
        {
            Console.WriteLine($"name is: {Name}");
        }
    }

    
}

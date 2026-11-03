using Practice;
using System.Text;


internal class Program
{
    private static void Main()
    {
        /*Student student = new Student {
            Name = "Test",
            Age = 30,
        };

        string name = student.Name;
        student.Display();

        StringBuilder builder = new StringBuilder();
        {
            for (int i = 0; i < 100; i++)
            {
                builder.Append(name);
            }
        }

        string b = builder.ToString();
        Console.WriteLine(b);
*/
        /*        int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, };

                Array.Sort(nums);
                foreach (int num in nums)
                {
                    Console.WriteLine(num);
                }

                Array.Reverse(nums);
                foreach (int num in nums) {
                    Console.WriteLine(num);
                    }


                Student[] students = new Student[4];
                students[0] = new Student("Karlos", 27);
                students[1] = new Student("Rafo", 52);
                students[2] = new Student("Lyub", 29);
                students[3] = new Student("Anush", 50);

                foreach (Student student in students)
                {
                    Console.WriteLine($"{student.Name} is {student.Age} years old");
                }

                Student[] group = new Student[4];
                students.CopyTo(group, 0);

                foreach (Student student in group)
                {
                    Console.WriteLine($"{student.Name} is {student.Age} years old");
                }*/

        /*List<int> numbers = new List<int>();
        numbers.Add(1); 
        numbers.Add(2);
        numbers.Add(3);
        numbers.Add(4);
        numbers.Add(5);
        numbers.Add(6);
        numbers.Add(7);
        foreach (int i in numbers)      
            Console.WriteLine(i);*/

        int[] ints = new int[5];

        try
        {
            Console.WriteLine("Try:");
            ints[333] = 1;
            Console.WriteLine("Worked");
        }
        catch (Exception)
        {
            Console.WriteLine("Didnt work:");
        
        }
        
        // ints[0] = "Gago";
    }
}
using ParallelPrograming;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using static ParallelPrograming.ParallelMethodArchive;

class Program
{
    static async Task Main()
    {
        Student[] students = new Student[]
        {
           new ("Gago" , 45),
           new ("Lilit" , 95),
           new ("Vardan" , 85),
           new ("Mariam" , 88),
           new ("Gaspar" , 67),
           new ("Mushegh" , 77),
        };

        Task<double> averageTask =
        CalculateAverageAsync(students);

        Task<int> goodStudentsTask =
            GoodStudentsCountAsync(students);

        await Task.WhenAll(averageTask, goodStudentsTask);

        double averageGrade = await averageTask;
        int goodStudents = await goodStudentsTask;

        await ProcessStudentsAsync(students);
        //double averageGrade = await CalculateAverageAsync(students);
        Console.WriteLine($"Average grade: {averageGrade}");

        //int goodStudents = await GoodStudentsCountAsync(students);
        Console.WriteLine($"60+ students count: {goodStudents}");

        await TestCodeLauncher.Launch();

    }
    
}
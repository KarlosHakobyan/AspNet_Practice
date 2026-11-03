using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ParallelPrograming
{
    internal class ParallelMethodArchive
    {
        public static async Task ProcessStudentsAsync(Student[] students)
        {
            var tasks = students.Select(async student =>
            {
                await Task.Delay(100);
                Console.WriteLine($"Student Name: {student.Name} , Student Grade: {student.Grade}");
            }).ToArray();

            await Task.WhenAll(tasks);
        }

        /* public static async Task<double> CalculateAverageAsync(Student[] students)
            {
                if (students == null || students.Length == 0)
                    return Task.FromResult(0.0);

                double average = students.Average(s => s.Grade);

                return Task.FromResult(average);
            }*/

        public async static Task<double> CalculateAverageAsync(Student[] students)
        {
            if (students.Length == 0) return 0;

            return await Task.Run(() =>
            {
                return students.Average(s => s.Grade);
            });
        }

        /* public async static Task<double> GoodStudentsCountAsync(Student[] students)
         {
             if (students.Length == 0) return 0;

             return await Task.Run(() =>
             {
                 return students.Where(e => e.Grade >= 60).Count();
             });
         }*/

        public async static Task<int> GoodStudentsCountAsync(Student[] students)
        {
            if (students == null || students.Length == 0) return 0;

            int count = 0;

            await Task.Run(() =>
            {
                Parallel.ForEach(students, student =>
                {
                    if (student.Grade >= 60)
                    {
                        Interlocked.Increment(ref count);
                    }
                });
            });

            return count;
        }
    }
}

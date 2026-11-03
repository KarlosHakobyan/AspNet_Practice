using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelPrograming
{
    internal class TestCodeLauncher
    {
        public static async Task Launch()
        {
            Task task1 = Task.Run(() =>
            {
                Console.WriteLine("Task 1");
            });

            Task task2 = Task.Run(() =>
            {
                Console.WriteLine("Task 2");
            });

            await Task.WhenAll(task1, task2);

            Console.WriteLine("All tasks completed");

            Task<int> task = Task.Run(() =>
            {
                return 10 * 20;
            });

            int result = await task;

            Console.WriteLine(result);

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Value: {i}, Thread: {Environment.CurrentManagedThreadId}");
            }

            Parallel.For(0, 10, i =>
            {
                Console.WriteLine(
                    $"Value: {i}, Thread: {Environment.CurrentManagedThreadId}"
                );
            });

            int[] userIds = { 1, 2, 3, 4, 5 };

            await Parallel.ForEachAsync(
                userIds,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = 1
                },
                async (id, cancellationToken) =>
                {
                    await Task.Delay(500, cancellationToken);

                    Console.WriteLine($"Processed user {id}");
                }
            );

        }
    }
}

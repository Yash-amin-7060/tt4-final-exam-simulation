using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;

namespace TaskManagerApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TaskDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any tasks
            if (context.Tasks.Any())
            {
                return;   // DB has been seeded
            }

            var tasks = new Models.Task[]
            {
                new Models.Task
                {
                    Title = "Complete project documentation",
                    Description = "Write thorough documentation for the Task Manager project",
                    Completed = false
                },
                new Models.Task
                {
                    Title = "Fix UI bugs",
                    Description = "Address UI issues reported in the Task Manager interface",
                    Completed = false
                },
                new Models.Task
                {
                    Title = "Buy groceries",
                    Description = "Eggs, milk, bread, fruits, and vegetables",
                    Completed = true
                },
                new Models.Task
                {
                    Title = "Read book chapter",
                    Description = "Finish reading chapter 5 of 'Clean Code'",
                    Completed = false
                }
            };

            context.Tasks.AddRange(tasks);
            context.SaveChanges();
        }
    }
} 
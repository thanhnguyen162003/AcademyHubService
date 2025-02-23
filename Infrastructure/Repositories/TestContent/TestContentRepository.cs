using Domain.Entity;
using Infrastructure.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json;

namespace Infrastructure.Repositories
{
    public class TestContentRepository : SqlRepository<TestContent>, ITestContentRepository
    {
        private readonly DbContext _context;
        public TestContentRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task CreateTestContent(List<TestContent> test)
        {
            await _dbSet.AddRangeAsync(test);
        }
        public async Task Delete(IEnumerable<TestContent> testContents)
        {
            _dbSet.RemoveRange(testContents);

            await Task.CompletedTask;
        }

        public async Task<Dictionary<Guid, string>> SubmitTest(List<TestContent> test)
        {
            var testIds = test.Select(t => t.Id).ToList();
            var testAnswers = test.Select(_ => _.Answers).ToList();


            var existingEntities = await _dbSet.AsNoTracking()
                .Where(x => testIds.Contains(x.Id))
                .ToListAsync();
            Dictionary<Guid, string> wrongAnswer = new Dictionary<Guid, string>();

            if (testAnswers.Count != existingEntities.Count) return null;

            for (int i=0; i < testAnswers.Count; i++)
            {
                var correctAnswer = DeserializeAnswers(existingEntities[i].Answers);
                if (!testAnswers[i].Equals(correctAnswer[existingEntities[i].CorrectAnswer.Value]))
                {
                    wrongAnswer[existingEntities[i].Id] = testAnswers[i];
                }
            }

           
            return wrongAnswer;
        }
        private static List<string>? DeserializeAnswers(string? answersJson)
        {
            if (string.IsNullOrEmpty(answersJson)) return null; // Handle null or empty JSON string

            try
            {
                return JsonSerializer.Deserialize<List<string>>(answersJson); // Or JsonConvert.DeserializeObject<List<string>>(answersJson);
            }
            catch (JsonException ex) // Handle potential JSON parsing errors
            {
                // Log the exception (recommended)
                Console.WriteLine($"Error deserializing answers: {ex.Message}");
                // Or throw the exception if you want it to propagate
                // throw;

                return null; // Or return an empty list: return new List<string>(); if that's more appropriate for your use case
            }
        }
    }

}

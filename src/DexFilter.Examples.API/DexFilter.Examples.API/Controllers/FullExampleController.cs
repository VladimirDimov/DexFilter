using DexFilter.AGGrid.Interfaces;
using DexFilter.AGGrid.Models;
using DexFilter.Core.Enumerations;
using DexFilter.Examples.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace DexFilter.Examples.API.Controllers
{
    [ApiController]
    [Route("students/full-example")]
    public class FullExampleController : ControllerBase
    {
        private readonly StudentsData studentsData;
        private readonly IAGFilterProcessorFactory agFilterProcessorFactory;

        public FullExampleController(StudentsData studentsData, IAGFilterProcessorFactory agFilterProcessorFactory)
        {
            this.studentsData = studentsData;
            this.agFilterProcessorFactory = agFilterProcessorFactory;
        }

        [HttpPost]
        public IActionResult Index([FromBody] AGGridRequest agGridRequest)
        {
            // Get IQueryable data
            IQueryable<Student> data = studentsData.Students;

            // Create instance of AG Grid processor
            IAGServerSideProcessor<Student> processor = agFilterProcessorFactory.New<Student>(config =>
            {
                // Read the time offset from header
                int.TryParse(HttpContext.Request.Headers["offset"], out var timeOffset);
                // Set time offset in configuration
                config.SetTimeOffset(timeOffset);

                // Add custom logic for "contains" filter on Subjects property
                // Find all students with any subject containing provided substring
                config.AddCustomFilter(nameof(Student.Subjects), FilterType.Contains, (s, filter)
                    => s.Subjects.Any(s => s.Name.ToLower().Contains(filter.Values.First().ToLower())));

                // Add custom logic for "notContains" filter on Subjects property
                // Find all students with none of the subjects containing provided substring
                config.AddCustomFilter(nameof(Student.Subjects), FilterType.NotContains, (s, filter)
                    => s.Subjects.Any(s => !s.Name.ToLower().Contains(filter.Values.First().ToLower())));

                // Add custom logic for "equals" filter on Subjects property
                // Find all students with any subject equal to provided substring
                config.AddCustomFilter(nameof(Student.Subjects), FilterType.Equals, (s, filter)
                    => s.Subjects.Any(s => s.Name.ToLower() == filter.Values.First().ToLower()));

                // Add new custom filter behavior. This filter returns students filtered
                // by their number of subjects
                config.AddCustomFilter(nameof(Student.Subjects), "count", (filter) =>
                {
                    if (!int.TryParse(filter.Values.FirstOrDefault(), out var numberOfSubjects))
                    {
                        return s => true;
                    }

                    return s => s.Subjects.Count() == numberOfSubjects;
                });

                // Add custom "orderBy" asc on Subjects property logic (order by number of suvjects)
                config.AddCustomOrderBy(s => s.Subjects, OrderByType.Asc, (collection, sort) =>
                {
                    return collection.OrderBy(x => x.Subjects.Count());
                });

                // Add custom "orderBy" desc on Subjects property logic (order by number of suvjects)
                config.AddCustomOrderBy(s => s.Subjects, OrderByType.Desc, (collection, sort) =>
                {
                    return collection.OrderByDescending(x => x.Subjects.Count());
                });
            });

            // Process Data
            AGFilterResult<Student> processedData = processor.Process(data, agGridRequest);

            // Return result
            return Ok(processedData);
        }
    }
}

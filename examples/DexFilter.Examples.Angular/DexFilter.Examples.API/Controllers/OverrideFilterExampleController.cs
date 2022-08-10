namespace DexFilter.Examples.API.Controllers
{
    using DexFilter.AGGrid.Interfaces;
    using DexFilter.AGGrid.Models;
    using DexFilter.Core.Enumerations;
    using DexFilter.Examples.API.Data;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("students/override-filter-example")]
    public class OverrideFilterExampleController : ControllerBase
    {
        private readonly StudentsData studentsData;
        private readonly IAGFilterProcessorFactory agFilterProcessorFactory;

        public OverrideFilterExampleController(StudentsData studentsData, IAGFilterProcessorFactory agFilterProcessorFactory)
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
                // Add custom logic for "contains" filter on Subjects property
                // Find all students with any subject containing provided substring
                config.AddCustomFilter(nameof(Student.Subjects), FilterType.Contains, (student, filter)
                    => student.Subjects.Any(s => s.Name.ToLower().Contains(filter.Values.First().ToLower())));
            });

            // Process Data
            AGFilterResult<Student> processedData = processor.Process(data, agGridRequest);

            // Return result
            return Ok(processedData);
        }
    }
}

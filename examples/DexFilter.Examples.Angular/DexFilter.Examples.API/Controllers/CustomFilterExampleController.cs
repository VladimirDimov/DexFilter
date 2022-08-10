namespace DexFilter.Examples.API.Controllers
{
    using DexFilter.AGGrid.Interfaces;
    using DexFilter.AGGrid.Models;
    using DexFilter.Examples.API.Data;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("students/custom-filter-example")]
    public class CustomFilterExampleController : ControllerBase
    {
        private readonly StudentsData studentsData;
        private readonly IAGFilterProcessorFactory agFilterProcessorFactory;

        public CustomFilterExampleController(StudentsData studentsData, IAGFilterProcessorFactory agFilterProcessorFactory)
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
                // Add new custom filter behavior. This filter returns students filtered
                // by their number of subjects
                config.AddCustomFilter(nameof(Student.Subjects), "count", (filter) =>
                {
                    if (!int.TryParse(filter.Values.FirstOrDefault(), out var numberOfSubjects))
                    {
                        throw new ArgumentException();
                    }

                    return s => s.Subjects.Count() == numberOfSubjects;
                });
            });

            // Process Data
            AGFilterResult<Student> processedData = processor.Process(data, agGridRequest);

            // Return result
            return Ok(processedData);
        }
    }
}

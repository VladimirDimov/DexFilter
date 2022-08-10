namespace DexFilter.Examples.API.Controllers
{
    using DexFilter.AGGrid.Interfaces;
    using DexFilter.AGGrid.Models;
    using DexFilter.Examples.API.Data;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("students/basic-example")]
    public class BasicExampleController : ControllerBase
    {
        private readonly StudentsData studentsData;
        private readonly IAGFilterProcessorFactory agFilterProcessorFactory;

        public BasicExampleController(
            StudentsData studentsData,
            IAGFilterProcessorFactory agFilterProcessorFactory)
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
            IAGServerSideProcessor<Student> processor = agFilterProcessorFactory.New<Student>();

            // Process Data
            AGFilterResult<Student> processedData = processor.Process(data, agGridRequest);

            // Return result
            return Ok(processedData);
        }
    }
}

using DexFilter.AGGrid.Interfaces;
using DexFilter.AGGrid.Models;
using DexFilter.Core.Enumerations;
using DexFilter.Examples.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace DexFilter.Examples.API.Controllers
{
    [ApiController]
    [Route("students/custom-orderby-example")]
    public class CustomOrderByExampleController : ControllerBase
    {
        private readonly StudentsData studentsData;
        private readonly IAGFilterProcessorFactory agFilterProcessorFactory;

        public CustomOrderByExampleController(StudentsData studentsData, IAGFilterProcessorFactory agFilterProcessorFactory)
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

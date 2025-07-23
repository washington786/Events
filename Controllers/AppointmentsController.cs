using Microsoft.AspNetCore.Mvc;

namespace Events.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {

        [HttpPost]
        public void Create(string parameter)
        {

        }

        [HttpGet()]
        public void GetAll()
        {

        }

        [HttpGet("{Id}")]
        public void GetId(int Id)
        {

        }

        [HttpPut("{Id}")]
        public void Update(int Id)
        {

        }

        [HttpDelete("{Id}")]
        public void Remove(int Id)
        {

        }
    }
}

using Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace BoscoStay.ApartmentsManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status500InternalServerError)]
    public class BaseController : ControllerBase
    {
        protected ActionResult GetApiResponse<T>(ApiResponse<T> obj) where T : class
        {
            if (obj.IsSuccessStatusCode && obj.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                return StatusCode(obj.StatusCode);

            if (obj.IsSuccessStatusCode)
                return StatusCode(obj.StatusCode, obj.Data);

            if (!obj.IsSuccessStatusCode && obj.HttpStatusCode == System.Net.HttpStatusCode.Conflict)
                return StatusCode(obj.StatusCode, obj.Data);

            return StatusCode(obj.StatusCode, obj.Errors);
        }

        protected ActionResult GetDownloadResponse<T>(ApiResponse<T> obj) where T : DownloadableResponse
        {
            return File(obj.Data.Data, obj.Data.ContentType, obj.Data.Name);
        }
    }
}

using Application.Features.Apartments.Commands.CreateApartment;
using Application.Features.Apartments.Commands.DeleteApartment;
using Application.Features.Apartments.Commands.UpdateApartment;
using Application.Features.Apartments.Queries.GetApartment;
using Application.Features.Apartments.Queries.GetCategorization;
using Application.Features.Apartments.Queries.SearchApartments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoscoStay.ApartmentsManagement.Controllers
{
    public class ApartmentsController(IMediator _mediator) : BaseController
    {
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(CreateApartmentCommandResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult<CreateApartmentCommandResponse>> CreateApartment(CreateApartmentCommand command)
        {
            var result = await _mediator.Send(command);

            return GetApiResponse(result);
        }

        [AllowAnonymous]
        [HttpPut]
        [ProducesResponseType(typeof(UpdateApartmentCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpdateApartmentCommandResponse>> UpdateApartment(UpdateApartmentCommand command)
        {
            var result = await _mediator.Send(command);

            return GetApiResponse(result);
        }

        [AllowAnonymous]
        [HttpDelete]
        [ProducesResponseType(typeof(DeleteApartmentCommandResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DeleteApartmentCommandResponse>> DeleteApartment(DeleteApartmentCommand command)
        {
            var result = await _mediator.Send(command);

            return GetApiResponse(result);
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(GetApartmentQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetApartmentQueryResponse>> GetApartment([FromQuery] GetApartmentQuery query)
        {
            var result = await _mediator.Send(query);

            return GetApiResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(typeof(SearchApartmentsQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SearchApartmentsQueryResponse>> SearchApartment([FromQuery] SearchApartmentsQuery query)
        {
            var result = await _mediator.Send(query);

            return GetApiResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("CategorizedApartments")]
        [ProducesResponseType(typeof(GetCategorizationQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetCategorizationQueryResponse>> GetCategorizedApartments([FromQuery] GetCategorizationQuery query)
        {
            var result = await _mediator.Send(query);

            return GetApiResponse(result);
        }
    }
}

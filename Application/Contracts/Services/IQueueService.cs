using Domain.Entities;
using Domain.Enums;

namespace Application.Contracts.Services
{
    public interface IQueueService
    {
        Task PublishChange(Apartment apartment, ApartmentChangeEnum apartmentChange);
    }
}

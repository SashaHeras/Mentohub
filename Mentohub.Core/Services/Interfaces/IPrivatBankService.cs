using Mentohub.Domain.PayMentAlla;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IPrivatBankService
    {
        Task<PrivatBankResponse> MakePaymentAsync(PrivatBankRequest request);
    }
}


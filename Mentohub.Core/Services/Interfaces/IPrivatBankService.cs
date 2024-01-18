using Mentohub.Domain.PayMent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IPrivatBankService
    {
        Task<PrivatBankResponse> MakePaymentAsync(PrivatBankRequest request);
    }
}


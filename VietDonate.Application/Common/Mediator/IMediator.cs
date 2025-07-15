using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietDonate.Application.Common.Mediator;

namespace VietDonate.Application.Common.Mediator
{
    public interface IMediator : ISender, IPublisher
    {
    }
}

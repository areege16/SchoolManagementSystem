using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.Account.Commands
{
    class LoginCommand:IRequest
    {

    }
    public class LoginCommandHandler : IRequestHandler<LoginCommand>
    {
        Task IRequestHandler<LoginCommand>.Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

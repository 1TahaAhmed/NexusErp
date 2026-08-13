using MediatR;
using NexusErp.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Identity.Commands.Login
{
    public record LoginCommand(
        string Email,
        string Password
        ) : IRequest<AuthResponse>;
}

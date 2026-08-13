using MediatR;
using NexusErp.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Identity.Commands.Registeration
{
    public record RegisterCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password
        ) : IRequest<AuthResponse>;
}

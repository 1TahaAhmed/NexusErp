using MediatR;
using NexusErp.Application.Common.Models;
using NexusErp.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Identity.Commands.Registeration
{
    public record RegisterCommand(
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        string Password,
        string RoleName
        ) : IRequest<Result<AuthResponse>>;
}

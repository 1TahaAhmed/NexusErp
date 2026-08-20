using MediatR;
using NexusErp.Application.Common.Models;
using NexusErp.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Identity.Commands.Login
{
    public record LoginCommand(
        string Email,
        string Password
        ) : IRequest<Result<AuthResponse>>;
}

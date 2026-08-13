using MediatR;
using NexusErp.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Identity.Commands.RefreshToken
{
    public record RefreshTokenCommand(
        string Token,
        string RefreshToken
        ) : IRequest<AuthResponse>;
}

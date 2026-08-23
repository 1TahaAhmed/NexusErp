using MediatR;
using NexusErp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Queries
{
    public class UserResponseDto
    {
        public Guid id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty; 
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
    }
    public record GetAllUsersQuery : IRequest<IEnumerable<UserResponseDto>>;
}

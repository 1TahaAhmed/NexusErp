using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nexus.Erp.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Queries
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IEnumerable<UserResponseDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync(cancellationToken);
            var userList = new List<UserResponseDto>();

            foreach (var user in users) 
            {
                var roles = await _userManager.GetRolesAsync(user);

                userList.Add(new UserResponseDto
                {
                    id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Password = user.PasswordHash ?? string.Empty,
                    Roles = roles
                });
            }
            return userList;
        }
    }
}

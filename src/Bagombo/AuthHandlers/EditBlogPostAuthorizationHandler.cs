﻿using System.Threading.Tasks;
using Bagombo.Data.Query;
using Bagombo.Data.Query.Queries;
using Bagombo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Bagombo.AuthHandlers
{
  public class EditBlogPostAuthorizationHandler : AuthorizationHandler<SameAuthorRequirement, BlogPost>
  {
    private readonly IQueryProcessorAsync _qpa;
    private readonly UserManager<ApplicationUser> _userManager;

    public EditBlogPostAuthorizationHandler(IQueryProcessorAsync qpa,
      UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
      _qpa = qpa;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
      SameAuthorRequirement requirement, BlogPost resource)
    {
      var user = await _userManager.GetUserAsync(context.User);

      var blogPostUserId = await _qpa.ProcessAsync(
        new GetUserIdFromBlogPost
        {
          blogpost = resource
        }
      );

      if (!string.IsNullOrEmpty(blogPostUserId) &&
          blogPostUserId == user.Id)
        context.Succeed(requirement);
    }
  }

  public class SameAuthorRequirement : IAuthorizationRequirement
  {
  }
}
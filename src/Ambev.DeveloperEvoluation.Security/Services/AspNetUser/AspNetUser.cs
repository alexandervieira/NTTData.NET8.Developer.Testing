﻿using Ambev.DeveloperEvoluation.Security.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Ambev.DeveloperEvoluation.Security.Services.AspNetUser;
public class AspNetUser : IAspNetUser
{
    private readonly IHttpContextAccessor _accessor;

    public AspNetUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string Name => _accessor.HttpContext.User.Identity.Name;

    public IEnumerable<Claim> GetClaims()
    {
        return _accessor.HttpContext.User.Claims;
    }

    public HttpContext GetHttpContext()
    {
        return _accessor.HttpContext;
    }

    public string GetUserEmail()
    {
        return IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";
    }

    public Guid GetUserId()
    {
        return IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
    }

    public string GetUserRefreshToken()
    {
        return IsAuthenticated() ? _accessor.HttpContext.User.GetUserRefreshToken() : "";
    }

    public string GetUserToken()
    {
        return IsAuthenticated() ? _accessor.HttpContext.User.GetUserToken() : "";
    }

    public bool HasRole(string role)
    {
        return _accessor.HttpContext.User.IsInRole(role);
    }

    public bool IsAuthenticated()
    {
        return _accessor.HttpContext.User.Identity.IsAuthenticated;
    }
}

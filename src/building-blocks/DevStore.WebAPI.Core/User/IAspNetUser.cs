using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DevStore.WebAPI.Core.User {
	public interface IAspNetUser {
		string Name { get; }
		Guid GetUserId();
		string GetUserEmail();
		string GetUserToken();
		string GetUserRefreshToken();
		bool IsAuthenticated();
		bool IsInRole(string role);
		IEnumerable<Claim> GetClaims();
		HttpContext GetHttpContext();
	}
}

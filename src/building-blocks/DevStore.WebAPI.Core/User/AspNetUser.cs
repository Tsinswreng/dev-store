using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DevStore.WebAPI.Core.User {
	public class AspNetUser : IAspNetUser {
		//# 見ask
		private readonly IHttpContextAccessor _accessor;

		public AspNetUser(IHttpContextAccessor accessor) {
			_accessor = accessor;
		}

		public string Name => _accessor.HttpContext.User.Identity.Name;

		public Guid GetUserId() {
			return IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
		}

		public string GetUserEmail() {
			return IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";
		}

		public string GetUserToken() {
			return IsAuthenticated() ? _accessor.HttpContext.User.GetUserToken() : "";
		}

		public string GetUserRefreshToken() {
			return IsAuthenticated() ? _accessor.HttpContext.User.GetUserRefreshToken() : "";
		}

		public bool IsAuthenticated() {
			return _accessor.HttpContext.User.Identity.IsAuthenticated;
		}

		public bool IsInRole(string role) {
			return _accessor.HttpContext.User.IsInRole(role);
		}

		public IEnumerable<Claim> GetClaims() {
			return _accessor.HttpContext.User.Claims;
		}

		public HttpContext GetHttpContext() {
			return _accessor.HttpContext;
		}
	}
}
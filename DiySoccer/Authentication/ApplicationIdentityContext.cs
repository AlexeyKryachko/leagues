using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNet.Identity.MongoDB;
using Core;
using Interfaces.Users.DataAccess;
using MongoDB.Driver;

namespace Authentication
{
    public class ApplicationIdentityContext : IDisposable
	{
		public static ApplicationIdentityContext Create()
		{
			// todo add settings where appropriate to switch server & database in your own application
			var client = new MongoClient(MongoConnetcionString.ConnectionString);
			var database = client.GetDatabase(MongoConnetcionString.Database);
			var users = database.GetCollection<ApplicationUser>("authUsers");
			var roles = database.GetCollection<UserRoleDb>("authRoles");
			return new ApplicationIdentityContext(users, roles);
		}

		private ApplicationIdentityContext(IMongoCollection<ApplicationUser> users, IMongoCollection<UserRoleDb> roles)
		{
			Users = users;
			Roles = roles;
		}

		public IMongoCollection<UserRoleDb> Roles { get; set; }

		public IMongoCollection<ApplicationUser> Users { get; set; }

		public Task<List<UserRoleDb>> AllRolesAsync()
		{
			return Roles.Find(r => true).ToListAsync();
		}

		public void Dispose()
		{
		}
	}
}
using System;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;

namespace Witcher.Core.Notifications
{
	public class Notification : EntityBase
	{
		protected User _user;
		public const string UserField = nameof(_user); 
		public Guid UserId { get; protected set; }

		public string Message { get; set; }

		public bool IsReaded { get; set; }

		public User User 
		{
			get => _user;
			set
			{
				_user = value ?? throw new EntityNotIncludedException<Notification>(nameof(User));
				UserId = value.Id;
			}
		}
	}
}

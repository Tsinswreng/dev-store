using System;

namespace DevStore.Core.Messages.Integration {
	public class UserRegisteredIntegrationEvent : IntegrationEvent {
		public Guid Id { get; private set; }
		public string Name { get; private set; }
		public string Email { get; private set; }
		public string SocialNumber { get; private set; }

		public UserRegisteredIntegrationEvent(Guid id, string name, string email, string socialNumber) {
			Id = id;
			Name = name;
			Email = email;
			SocialNumber = socialNumber;
		}
	}
}
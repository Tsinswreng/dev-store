using FluentValidation.Results;

namespace DevStore.Core.Messages.Integration {
	public class ResponseMessage : Message {
		public ValidationResult ValidationResult { get; set; }

		public ResponseMessage(ValidationResult validationResult) {
			ValidationResult = validationResult;
		}
	}
}
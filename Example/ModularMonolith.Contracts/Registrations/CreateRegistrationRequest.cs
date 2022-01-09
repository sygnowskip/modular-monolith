using System;

namespace ModularMonolith.Contracts.Registrations
{
    public class CreateRegistrationRequest
    {
        public CreateRegistrationRequest(string firstName, string lastName, DateTime dateOfBirth, long examId,
            CreateRegistrationRequestInvoiceData buyer)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            ExamId = examId;
            Buyer = buyer;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public DateTime DateOfBirth { get; }
        public long ExamId { get; }
        public CreateRegistrationRequestInvoiceData Buyer { get; }
    }

    public class CreateRegistrationRequestInvoiceData
    {
        public CreateRegistrationRequestInvoiceData(string name, string streetAddress, string city, string zipCode)
        {
            Name = name;
            StreetAddress = streetAddress;
            City = city;
            ZipCode = zipCode;
        }

        public string Name { get; }
        public string StreetAddress { get; }
        public string City { get; }
        public string ZipCode { get; }
    }
}
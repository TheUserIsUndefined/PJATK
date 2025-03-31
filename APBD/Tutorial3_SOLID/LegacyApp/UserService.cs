using System;
using LegacyApp.Implementations;
using LegacyApp.Interfaces;
using LegacyApp.Validators;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IUserValidator _userValidator;
        private readonly IClientRepository _clientRepository;
        private readonly IUserCreditLimit _userCreditLimit;
        private readonly IUserDataAccessProvider _userDataAccessProvider;
        
        public UserService() : this(new UserValidator(), 
            new ClientRepository(), 
            new UserCreditLimit(),
            new UserDataAccessProvider()) { }

        public UserService(IUserValidator userValidator, 
            IClientRepository clientRepository,
            IUserCreditLimit userCreditLimit,
            IUserDataAccessProvider userDataAccessProvider)
        {
            _userValidator = userValidator;
            _clientRepository = clientRepository;
            _userCreditLimit = userCreditLimit;
            _userDataAccessProvider = userDataAccessProvider;
        }
        
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            var user = new User
            {
                Client = null,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
            
            if (!_userValidator.ValidatePersonalData(user)) return false;
            
            user.Client = _clientRepository.GetById(clientId);
            _userCreditLimit.applyLimit(user);

            if(!_userValidator.ValidateCreditLimit(user)) return false;

            _userDataAccessProvider.AddUser(user);
            return true;
        }
    }
}

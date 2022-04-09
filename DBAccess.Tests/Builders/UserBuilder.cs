using System;
using DbAccess.Data.POCO;
using DbAccess.Repositories.UnitOfWork;
using DbAccess.Repositories.User;

namespace DBAccess.Tests.Builders
{
    public class UserBuilder
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private string _userName;
        private string _emailAddress;

        public UserBuilder(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public UserBuilder WithUserName(string userName)
        {
            _userName = userName;
            return this;
        }

        public UserBuilder WithEmailAddress(string emailAddress)
        {
            _emailAddress = emailAddress;
            return this;
        }

        public User Build()
        {
            var testUser = new User()
            {
                Email = Guid.NewGuid().ToString("N") + "@test.com",
                UserName = Guid.NewGuid().ToString()[..20],
                Password = "testPassword"
            };
            if (!string.IsNullOrEmpty(_userName))
                testUser.UserName = _userName;
            if (!string.IsNullOrEmpty(_emailAddress))
                testUser.Email = _emailAddress;
            _userRepository.Add(testUser);
            _unitOfWork.Save();
            return testUser;
        }
    }
}

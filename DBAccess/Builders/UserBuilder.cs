using DBAccess.Data;

namespace DBAccess.Builders
{
    public class UserBuilder
    {
        private string _email;
        private string _username;
        private string _userDescription;

        public UserBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public UserBuilder WithDescription(string userDescription)
        {
            _userDescription = userDescription;
            return this;
        }

        public UserBuilder WithUsername(string username)
        {
            _username = username;
            return this;
        }

        public User Build()
        {
            return new User
            {
                Email = _email,
                UserName = _username,
                UserDescription = _userDescription
            };
        }
    }
}

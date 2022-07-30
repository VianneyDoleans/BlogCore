using System;
using System.Data.Common;

namespace DBAccess.Builders
{
    public enum SslMode
    {
        Require,
        Disable,
        Prefer
    }

    /// <summary>
    /// parser needed for Heroku 
    /// https://stackoverflow.com/a/51663858
    /// </summary>
    public class PostgreSqlConnectionStringBuilder : DbConnectionStringBuilder
    {
        private string _database;
        private string _host;
        private string _password;
        private bool _pooling;
        private int _port;
        private string _username;
        private bool _trustServerCertificate;
        private SslMode _sslMode;

        public PostgreSqlConnectionStringBuilder(string uriString)
        {
            ParseUri(uriString);
        }

        public string Database
        {
            get => _database;
            set
            {
                base["database"] = value;
                _database = value;
            }
        }

        public string Host
        {
            get => _host;
            set
            {
                base["host"] = value;
                _host = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                base["password"] = value;
                _password = value;
            }
        }

        public bool Pooling
        {
            get => _pooling;
            set
            {
                base["pooling"] = value;
                _pooling = value;
            }
        }

        public int Port
        {
            get => _port;
            set
            {
                base["port"] = value;
                _port = value;
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                base["username"] = value;
                _username = value;
            }
        }

        public bool TrustServerCertificate
        {
            get => _trustServerCertificate;
            set
            {
                base["trust server certificate"] = value;
                _trustServerCertificate = value;
            }
        }

        public SslMode SslMode
        {
            get => _sslMode;
            set
            {
                base["ssl mode"] = value.ToString();
                _sslMode = value;
            }
        }

        public override object this[string keyword]
        {
            get
            {
                if (keyword == null) throw new ArgumentNullException(nameof(keyword));
                return base[keyword.ToLower()];
            }
            set
            {
                if (keyword == null) throw new ArgumentNullException(nameof(keyword));

                switch (keyword.ToLower())
                {
                    case "host":
                        Host = (string)value;
                        break;

                    case "port":
                        Port = Convert.ToInt32(value);
                        break;

                    case "database":
                        Database = (string)value;
                        break;

                    case "username":
                        Username = (string)value;
                        break;

                    case "password":
                        Password = (string)value;
                        break;

                    case "pooling":
                        Pooling = Convert.ToBoolean(value);
                        break;

                    case "trust server certificate":
                        TrustServerCertificate = Convert.ToBoolean(value);
                        break;

                    case "sslmode":
                        SslMode = (SslMode)value;
                        break;

                    default:
                        throw new ArgumentException($"Invalid keyword '{keyword}'.");
                }
            }
        }

        public override bool ContainsKey(string keyword)
        {
            return base.ContainsKey(keyword.ToLower());
        }

        private void ParseUri(string uriString)
        {
            var isUri = Uri.TryCreate(uriString, UriKind.Absolute, out var uri);

            if (!isUri) throw new FormatException($"'{uriString}' is not a valid URI.");

            Host = uri.Host;
            Port = uri.Port;
            Database = uri.LocalPath[1..];
            Username = uri.UserInfo.Split(':')[0];
            Password = uri.UserInfo.Split(':')[1];
        }
    }
}

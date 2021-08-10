using System;
using System.Collections.Generic;
using System.Text;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasEmailAddress
    {
        public string EmailAddress { get; set; }
    }
}

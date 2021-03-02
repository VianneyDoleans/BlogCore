using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlogAPI.DTO
{
    public abstract class ADto : IDto, IEquatable<IDto>
    {
        public int Id { get; set; }


        protected bool Equals(ADto other)
        {
            return other != null && Id == other.Id;
        }

        public bool Equals(IDto other)
        {
            return other != null && Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ADto) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}

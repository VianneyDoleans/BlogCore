using System;

namespace MyBlogAPI.DTO
{
    /// <summary>
    /// Abstract of <see cref="IDto"/>. This class implement the mandatory properties and methods of all resources classified as Dto.
    /// </summary>
    public abstract class ADto : IDto, IEquatable<IDto>
    {
        /// <inheritdoc />
        public int Id { get; set; }


        private bool Equals(ADto other)
        {
            return other != null && Id == other.Id;
        }

        /// <inheritdoc />
        public bool Equals(IDto other)
        {
            return other != null && Id == other.Id;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ADto) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id;
        }
    }
}

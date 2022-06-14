using System;

namespace BlogCoreAPI.DTOs
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
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ADto) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id;
        }
    }
}

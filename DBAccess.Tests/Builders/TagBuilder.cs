using System;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Tag;
using DbAccess.Repositories.UnitOfWork;

namespace DBAccess.Tests.Builders
{
    public class TagBuilder
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TagBuilder(ITagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
        }

        public Tag Build()
        {
            var testTag = new Tag()
            {
                Name = Guid.NewGuid().ToString()[..50]
            };
            _tagRepository.Add(testTag);
            _unitOfWork.Save();
            return testTag;
        }
    }
}

﻿using System;
using DBAccess.Data;
using DBAccess.Repositories.Tag;
using DBAccess.Repositories.UnitOfWork;

namespace DBAccess.Tests.Builders
{
    public class TagBuilder
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private string _name;

        public TagBuilder(ITagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
        }

        public TagBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public Tag Build()
        {
            var testTag = new Tag()
            {
                Name = Guid.NewGuid().ToString()
            };
            if (!string.IsNullOrEmpty(_name))
                testTag.Name = _name;
            _tagRepository.Add(testTag);
            _unitOfWork.Save();
            return testTag;
        }
    }
}

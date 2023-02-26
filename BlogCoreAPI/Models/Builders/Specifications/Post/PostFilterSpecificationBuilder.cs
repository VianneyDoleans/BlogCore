using System;
using System.Collections.Generic;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.FilterSpecifications.Filters;

namespace BlogCoreAPI.Models.Builders.Specifications.Post
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="Post"/>.
    /// </summary>
    public class PostFilterSpecificationBuilder
    {

        private string _inName;
        private string _inContent;
        private DateTime? _toPublishedAt;
        private DateTime? _fromPublishedAt;
        private int? _minimumLikeCount;
        private int? _maximumLikeCount;
        private List<string> _tags;

        public PostFilterSpecificationBuilder WithInContent(string inContent)
        {
            _inContent = inContent;
            return this;
        }

        public PostFilterSpecificationBuilder WithInName(string inName)
        {
            _inName = inName;
            return this;
        }

        public PostFilterSpecificationBuilder WithToPublishedAt(DateTime? toPublishedAt)
        {
            _toPublishedAt = toPublishedAt;
            return this;
        }

        public PostFilterSpecificationBuilder WithFromPublishedAt(DateTime? fromPublishedAt)
        {
            _fromPublishedAt = fromPublishedAt;
            return this;
        }

        public PostFilterSpecificationBuilder WithMinimumLikeCount(int? minimumLikeCount)
        {
            _minimumLikeCount = minimumLikeCount;
            return this;
        }

        public PostFilterSpecificationBuilder WithMaximumLikeCount(int? maximumLikeCount)
        {
            _maximumLikeCount = maximumLikeCount;
            return this;
        }

        public PostFilterSpecificationBuilder WithTags(List<string> tags)
        {
            _tags = tags;
            return this;
        }

        /// <summary>
        /// Get filter specification of <see cref="Post"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DBAccess.Data.Post> Build()
        {

            FilterSpecification<DBAccess.Data.Post> filter = null;

            if (_inContent != null)
                filter = new ContentContainsSpecification<DBAccess.Data.Post>(_inContent);
            if (_inName != null)
            {
                filter = filter == null ?
                    new NameContainsSpecification<DBAccess.Data.Post>(_inName)
                    : filter & new NameContainsSpecification<DBAccess.Data.Post>(_inName);
            }
            if (_toPublishedAt != null)
            {
                filter = filter == null ?
                    new PublishedBeforeDateSpecification<DBAccess.Data.Post>(_toPublishedAt.Value)
                    : filter & new PublishedBeforeDateSpecification<DBAccess.Data.Post>(_toPublishedAt.Value);
            }
            if (_fromPublishedAt != null)
            {
                filter = filter == null ?
                    new PublishedAfterDateSpecification<DBAccess.Data.Post>(_fromPublishedAt.Value)
                    : filter & new PublishedAfterDateSpecification<DBAccess.Data.Post>(_fromPublishedAt.Value);
            }
            if (_minimumLikeCount != null)
            {
                filter = filter == null ?
                    new MinimumLikeCountSpecification<DBAccess.Data.Post>(_minimumLikeCount.Value)
                    : filter & new MinimumLikeCountSpecification<DBAccess.Data.Post>(_minimumLikeCount.Value);
            }
            if (_maximumLikeCount != null)
            {
                filter = filter == null ?
                    new MaximumLikeCountSpecification<DBAccess.Data.Post>(_maximumLikeCount.Value)
                    : filter & new MaximumLikeCountSpecification<DBAccess.Data.Post>(_maximumLikeCount.Value);
            }
            if (_tags != null)
            {
                foreach (var tag in _tags)
                {
                    filter = filter == null
                        ? new TagSpecification<DBAccess.Data.Post>(tag)
                        : filter & new TagSpecification<DBAccess.Data.Post>(tag);
                }
            }

            return filter;
        }
    }
}

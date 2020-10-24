using System;
using System.Collections.Generic;

namespace Board.Core.Entities
{

    public class Job : BaseEntity
    {
        private Job()
        {
            Tags = new HashSet<string>();
        }

        public Job(string position, Company company, string applyUrl) : this()
        {
            Position = position;
            Company = company;
            ApplyUrl = applyUrl;
        }

        public string Position { get; private set; }

        public Company Company { get; private set; }

        public string ApplyUrl { get; private set; }


        public string Description { get; set; }

        public bool Remote { get; private set; }

        public string Location { get; private set; }

        public DateTime? PublishedOn { get; private set; }

        public PublicationType PublicationType { get; private set; }

        public HashSet<string> Tags { get; private set; }



        public void AddTags(List<string> tags)
        {
            Tags.UnionWith(tags);
        }

        public void SetLocation(bool remote, string location)
        {
            if (!remote && string.IsNullOrWhiteSpace(location))
                throw new Exception("Location is required for non-remote jobs");

            Remote = remote;
            Location = location;
        }

        // NB. A sanitizer is required when setting the description to ensure
        // that Description is never set without being properly sanitized.
        public void SetDescription(string description, Func<string, string> sanitizer)
        {
            Description = sanitizer(description);
        }


        public void Publish(PublicationType publicationType)
        {
            PublishedOn = DateTime.UtcNow;
            PublicationType = publicationType;
        }
    }
}

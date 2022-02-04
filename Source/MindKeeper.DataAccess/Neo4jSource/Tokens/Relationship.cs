namespace MindKeeper.DataAccess.Neo4jSource.Tokens
{
    internal static class Relationship
    {
        // User
        public const string CREATED_IDEA = nameof(CREATED_IDEA);
        public const string ADDED_TO_FAVORITES = nameof(ADDED_TO_FAVORITES);
        public const string CREATED_IDEA_UPDATE = nameof(CREATED_IDEA_UPDATE);
        public const string UPVOTED_IDEA_UPDATE = nameof(UPVOTED_IDEA_UPDATE);
        public const string DOWNVOTED_IDEA_UPDATE = nameof(DOWNVOTED_IDEA_UPDATE);
        public const string INTERESTED_IN_SUBDOMAIN = nameof(INTERESTED_IN_SUBDOMAIN);
        public const string HAS_ACHIEVEMENT = nameof(HAS_ACHIEVEMENT);

        // Idea
        public const string COPY_OF = nameof(COPY_OF);


        // IdeaUpdate
        public const string UPDATE_OF = nameof(UPDATE_OF);
        public const string UPDATED = nameof(UPDATED);

        // Country
        public const string COUNTRY_OF = nameof(COUNTRY_OF);

        // Domain
        public const string CONTAINS_SUBDOMAIN = nameof(CONTAINS_SUBDOMAIN);

        // Subdomain
        public const string CONTAINS_IDEA = nameof(CONTAINS_IDEA);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;

namespace BusinessLogic.Helpers
{
    public class Rating
    {
        public double RatingValue { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();

        public static Rating Create(IEnumerable<RatingLog> ratingLogs)
        {
            if (!(ratingLogs.Any())) { return new Rating(); }
            return new Rating
            {
                RatingValue = ratingLogs.Select(x => x.Rating).Average()
            };
        }

        public static Rating CreateWithComments(IEnumerable<RatingLog> ratingLogs)
        {
            var ratingLogsArray = ratingLogs as RatingLog[] ?? ratingLogs.ToArray();
            return new Rating
            {
                RatingValue = ratingLogsArray.Select(x => x.Rating).Average(),
                Comments = ratingLogsArray.Select(x => new Comment
                {
                    CommentText = x.Comment,
                    UserName = x.User.UserName
                }).ToList()
            };
        }
    }

    public class Comment
    {
        public string CommentText { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}

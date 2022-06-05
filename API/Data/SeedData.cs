using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class SeedData
    {
        public static async Task SeedUsers(DataContext context) 
        {
            
            if (!(await context.Users.AnyAsync())) 
            {
                var random = new Random();

                using var hmac = new HMACSHA512();

                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("P@ssw0rd"));
                var passwordSalt = hmac.Key;

                var users = new List<User>
                {
                    new User 
                    {
                        Username = "afzal",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        CreatedAt = DateTime.Now.AddMonths(-2)
                    },
                    new User 
                    {
                        Username = "saiful",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        CreatedAt = DateTime.Now.AddMonths(-2)
                    },
                    new User 
                    {
                        Username = "sadek",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        CreatedAt = DateTime.Now.AddMonths(-1)
                    },
                    new User 
                    {
                        Username = "tabin",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        CreatedAt = DateTime.Now.AddMonths(-1)
                    },
                    new User 
                    {
                        Username = "mostofa",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        CreatedAt = DateTime.Now.AddMonths(-1)
                    },
                    new User 
                    {
                        Username = "zakir",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        CreatedAt = DateTime.Now.AddMonths(-1)
                    },
                    new User 
                    {
                        Username = "abbas",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        CreatedAt = DateTime.Now.AddMonths(-1)
                    },
                    new User 
                    {
                        Username = "masud",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        CreatedAt = DateTime.Now.AddMonths(-1)
                    },
                };

                context.Users.AddRange(users);
                await context.SaveChangesAsync();

                var posts = new List<Post>();
                foreach (var user in users) 
                {
                    for (var i = 0; i < 4; i++) 
                    {
                        var post = new Post
                        {
                            Title = $"{user.Username}'s Post No - {i}",
                            Content = $"{user.Username} says: Here is tons of Content for Post No - {i} which was created by me",
                            CreatedAt = DateTime.Now.AddDays(-random.Next(29)),
                            CreatedBy = user
                        };
                        posts.Add(post);
                    }
                }

                context.Posts.AddRange(posts);
                await context.SaveChangesAsync();

                var coolWords = new List<string>
                {
                    "this is cool",
                    "splendid words you have said",
                    "amazed...... wow!"
                };

                var postComments = new List<PostComment>();
                foreach (var post in posts)
                {
                    foreach (var user in users)
                    {
                        if (user != post.CreatedBy)
                        {
                            for (var i = 0; i < 3; i++)
                            {
                                var dateNow = DateTime.Now;
                                var dateCreatedAt = post.CreatedAt;
                                var minutesDiff = Convert.ToInt32(dateNow.Subtract(dateCreatedAt).TotalMinutes+1);
                                var r = random.Next(1, minutesDiff); 
                                var comment = new PostComment
                                {
                                    Content = coolWords[i],
                                    Post = post,
                                    User = user,
                                    CreatedAt = DateTime.Now.AddMinutes(-r)
                                };
                                postComments.Add(comment);
                            }
                        }
                    }
                }

                context.PostComments.AddRange(postComments);
                await context.SaveChangesAsync();
            }


        }
    }
}
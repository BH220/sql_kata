using sql_kata.Data;
using sql_kata.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql_kata
{
    public class EF
    {
        public EF()
        {

        }   

        public void Select()
        {
            using(MyDbContext db = new MyDbContext())
            {
                List<User> users = db.Users.ToList();
                foreach(User user in users)
                {
                    Console.WriteLine($"UserId: {user.UserId}, Name: {user.Name}, Age: {user.Age}, TeamId: {user.TeamId}");
                }
            }
        }

        public void Insert()
        {
            using(MyDbContext db = new MyDbContext())
            {
                List<Team> team = db.Teams.ToList();
                if(!(team != null && team.Count>0))
                {
                    Team tm = new Team
                    {
                        Name = "보안사업2팀",
                        TeamId = 1
                    };
                    db.Teams.Add(tm);
                    db.SaveChanges();
                }
                User user = new User
                {
                    Name = $"{DateTime.Now.ToString("ssfff")} User",
                    Age = 30,
                    TeamId = 1
                };
                db.Users.Add(user);
                db.SaveChanges();
                Console.WriteLine("User inserted successfully.");
            }
        }   

        public void Update()
        {
            using(MyDbContext db = new MyDbContext())
            {
                User? user = db.Users.FirstOrDefault();
                if(user != null)
                {
                    user.Name = "Updated Name";
                    db.SaveChanges();
                    Console.WriteLine("User updated successfully.");
                }
                else
                {
                    Console.WriteLine("User not found.");
                }
            }
        }   

        public void delete()
        {
            using(MyDbContext db = new MyDbContext())
            {
                User? user = db.Users.FirstOrDefault();
                if(user != null)
                {
                    db.Users.Remove(user);
                    //db.SaveChanges();
                    Console.WriteLine("User deleted successfully.");
                }
                else
                {
                    Console.WriteLine("User not found.");
                }
            }
        }

        internal void Aquery()
        {
            using var db = new MyDbContext();

            var query = db.Teams
                .Where(t => t.Name.Contains("개발"))
                .Select(t => new
                {
                    TeamName = t.Name,
                    UserCount = t.Users
                        .Where(u => u.Age >= 25)
                        .Count(), 
                    TaskCount = t.Users
                        .SelectMany(u => u.Tasks)
                        .Where(task => task.Status == "Done"
                                       && task.Project.StartDate >= new DateTime(2024, 1, 1)
                                       && task.Project.EndDate <= new DateTime(2024, 12, 31)
                                       && task.Comments.Count >= 2)
                        .Count(),
                    CommentCount = t.Users
                        .SelectMany(u => u.Tasks)
                        .Where(task => task.Status == "Done")
                        .SelectMany(task => task.Comments)
                        .Count()
                });
        }
    }
}

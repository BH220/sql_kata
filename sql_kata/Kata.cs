using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using sql_kata.Data;
using sql_kata.Models;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace sql_kata
{
    public class Kata
    {
        MySqlConnection connection;

        public Kata()
        {
            connection = new MySqlConnection("Server=127.0.0.1;Port=3306;Database=kata;Uid=root;Pwd=kac1004;");
        }
        public void Select()
        {
            Query query = new();
            query.From("user");//.Select("*");
            query.Where("age", ">", 1);

            SqlResult result = new SqlKata.Compilers.MySqlCompiler().Compile(query);
            //Console.WriteLine("SELECT * FROM user WHERE age > 25;");
            //Console.WriteLine(result.Sql);
            //Console.WriteLine(JsonSerializer.Serialize(result.NamedBindings));

            
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);
            var users = db.Get(query).ToList();
             
            if(users.Count<=0)
            {
                Console.WriteLine("no user");
            }
            foreach (var user in users)
            {
                Console.WriteLine($"TeamID: {user.team_id} / UserID: {user.user_id} / Name: {user.name}");
            }
        }
        public void Insert()
        {
            var newUser = new
            {
                team_id = 1,
                name = "홍길동",
                age = 30
            };

            Query query = new();
            query.AsInsert(newUser);
            query.From("user");

            var compiler = new MySqlCompiler();
            compiler.Compile(query);
            var db = new QueryFactory(connection, compiler);
            db.Execute(query);

            Select();
        }
        public void Update()
        {
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);

            int affected = db.Query("user")
                             .Where("user_id", 7)
                             .Update(new
                             {
                                 Name = "김철수",
                                 Age = 35
                             });

            Select();
        }

        public void delete()
        {
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);
             
            int affected = db.Query("user")
                             .Where("user_iD", 16)
                             .Delete();

            Select();
        }

        internal void Aquery()
        {
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);


            // 1) 공통 베이스 쿼리 정의
            var baseQuery = new Query("team as t")
                .Join("user as u", "u.team_id", "t.team_id")
                .Join("task as ta", "ta.user_id", "u.user_id")
                .Join("project as p", "p.project_id", "ta.project_id")
                .LeftJoin("comment as c", "c.task_id", "ta.task_id")
                .Where("u.age", ">=", 25)
                .WhereLike("t.name", "%개발%")
                .Where("ta.status", "Done")
                .Where("p.start_date", ">=", new DateTime(2024, 1, 1))
                .Where("p.end_date", "<=", new DateTime(2024, 12, 31));

            // 2) 서브쿼리 조건 추가
            var withSubQuery = baseQuery
                .WhereIn("ta.task_id", q => q.From("comment")
                                              .Select("task_id")
                                              .GroupBy("task_id")
                                              .HavingRaw("COUNT(*) >= 2"));

            // 3) 최종 SELECT (집계/그룹화)
            var finalQuery = withSubQuery
                .Select("t.name as team_name")
                .SelectRaw("COUNT(DISTINCT u.user_id) as user_count")
                .SelectRaw("COUNT(DISTINCT ta.task_id) as task_count")
                .SelectRaw("COUNT(c.comment_id) as comment_count")
                .GroupBy("t.team_id", "t.name");

            var compiled = compiler.Compile(finalQuery);
            Console.WriteLine("=== SQL Preview ===");
            Console.WriteLine(compiled.Sql);
            Console.WriteLine(string.Join(", ", compiled.NamedBindings));

            // ────────────────
            // 실행
            // ────────────────
            var results = db.Get(finalQuery);

            foreach (var row in results)
            {
                Console.WriteLine($"{row.team_name} | Users={row.user_count} | AvgAge={row.avg_age} | Tasks={row.task_count} | Comments={row.comment_count}");
            }
        }


public void KataSelect()
{
    Query query = new();
    query.From("user");
    query.Where("age", ">", 1);

    SqlResult result = new SqlKata.Compilers.MySqlCompiler().Compile(query);

    var compiler = new MySqlCompiler();
    var db = new QueryFactory(connection, compiler);
    var users = db.Get(query).ToList();

    foreach (var user in users)
    {
        Console.WriteLine(
            $"TeamID: {user.team_id} / UserID: {user.user_id} / Name: {user.name}"
        );
    }
}


    }
}

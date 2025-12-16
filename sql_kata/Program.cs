
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace sql_kata
{
    internal class Program
    {
        private void B()
        {
            Console.WriteLine("B");
        }
        static void Main(string[] args)
        {
            EF ef = new EF();
            Kata kata = new Kata();

            ef.Select();
            ef.Insert();
            ef.Update();
            ef.delete();

            kata.Select();
            kata.Insert();
            kata.Update();
            kata.delete();


            ef.Aquery();
            kata.Aquery();

            //240101 ~ 241231 기간동안의 개발팀 프로젝트 중 업무가 종료되었고, 그 업무중 코멘트가 2개 이상 달린 것에 대한 통계
            //팀명, 해당 업무를 수행한 유저수, 해당 업무의 수, 해당 업무에 달린 코멘트 수
            /*
SELECT t.name AS team_name,
       COUNT(DISTINCT u.user_id) AS user_count,
       COUNT(DISTINCT ta.task_id) AS task_count,
       COUNT(c.comment_id) AS comment_count
FROM team t
JOIN user u ON u.team_id = t.team_id
JOIN task ta ON ta.user_id = u.user_id
JOIN project p ON p.project_id = ta.project_id
LEFT JOIN comment c ON c.task_id = ta.task_id
WHERE t.name LIKE '%개발%'
  AND ta.status = 'Done'
  AND p.start_date >= '2024-01-01'
  AND p.end_date <= '2024-12-31'
  AND ta.task_id IN (
        SELECT task_id
        FROM comment
        GROUP BY task_id
        HAVING COUNT(*) >= 2
  )
GROUP BY t.team_id, t.name;
            */
        }
    }
}

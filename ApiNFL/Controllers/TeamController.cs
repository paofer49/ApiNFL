using ApiNFL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApiNFL.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly string _connectionString;

        public TeamController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        [HttpGet]
        public ActionResult<IEnumerable<Team>> GetTeams()
        {
            var teams = new List<Team>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Teams", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    teams.Add(new Team
                    {
                        TeamID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        City = reader.GetString(2),
                        State = reader.GetString(3),
                        Stadium = reader.GetString(4),
                        FoundationYear = reader.GetInt32(5),
                        Conference = reader.GetString(6)
                    });
                }
            }
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public ActionResult<Team> GetTeamById(int id)
        {
            Team? team = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Teams WHERE TeamID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    team = new Team
                    {
                        TeamID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        City = reader.GetString(2),
                        State = reader.GetString(3),
                        Stadium = reader.GetString(4),
                        FoundationYear = reader.GetInt32(5),
                        Conference = reader.GetString(6)
                    };
                }
            }
            return team != null ? Ok(team) : NotFound();
        }

        [HttpPost]
        public IActionResult CreateTeam([FromBody] Team team)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Teams (Name, City, State, Stadium, FoundationYear, Conference) VALUES (@name, @city, @state, @stadium, @foundationYear, @conference)", conn);
                cmd.Parameters.AddWithValue("@name", team.Name);
                cmd.Parameters.AddWithValue("@city", team.City);
                cmd.Parameters.AddWithValue("@state", team.State);
                cmd.Parameters.AddWithValue("@stadium", team.Stadium);
                cmd.Parameters.AddWithValue("@foundationYear", team.FoundationYear);
                cmd.Parameters.AddWithValue("@conference", team.Conference);
                cmd.ExecuteNonQuery();
            }
            return CreatedAtAction(nameof(GetTeams), new { team.Name }, team);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTeam(int id, [FromBody] Team team)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Teams SET Name = @name, City = @city, State = @state, Stadium = @stadium, FoundationYear = @foundationYear, Conference = @conference WHERE TeamID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", team.Name);
                cmd.Parameters.AddWithValue("@city", team.City);
                cmd.Parameters.AddWithValue("@state", team.State);
                cmd.Parameters.AddWithValue("@stadium", team.Stadium);
                cmd.Parameters.AddWithValue("@foundationYear", team.FoundationYear);
                cmd.Parameters.AddWithValue("@conference", team.Conference);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0) return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTeam(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Teams WHERE TeamID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0) return NotFound();
            }
            return NoContent();
        }

    }
}

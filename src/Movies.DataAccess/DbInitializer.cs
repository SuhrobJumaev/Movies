using Dapper;

namespace Movies.DataAccess;

public class DbInitializer 
{
    private readonly IDbConnectionFactory _db;

    public DbInitializer(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task InitializeAsync()
    {
        using var conn = await _db.CreateConnectionAsync();

         await conn.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS role(
                id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                name VARCHAR(20) NOT NULL,
                created_date TIMESTAMP  DEFAULT now()
            );
         """);

         await conn.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS "user" (
                id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                name VARCHAR(20) NOT NULL,
                last_name VARCHAR(20) NOT NULL,
                age SMALLINT NOT NULL,
                gender SMALLINT NOT NULL,
                phone VARCHAR(20) NOT NULL,
                email VARCHAR(30) NOT NULL,
                password VARCHAR(100) NOT NULL,
                created_date TIMESTAMP DEFAULT now(),
                role_id SMALLINT NOT NULL,
  
                CONSTRAINT fk_role
                    FOREIGN KEY(role_id)
                        REFERENCES role(id));  
         """);

        await conn.ExecuteAsync("""
            CREATE UNIQUE INDEX CONCURRENTLY IF NOT EXISTS user_email
            ON user
            USING BTREE(email);
        """);
    }
}
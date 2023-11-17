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
            CREATE TABLE IF NOT EXISTS "movie" (
            id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
            slug TEXT NOT NULL, 
            title TEXT NOT NULL,
            year_of_release INT NOT NULL);
         """);

        await conn.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS "genre" (
            id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
            name VARCHAR(20) NOT NULL);

            
         """);
        //INSERT INTO genre(name) VALUES ('Комедия'),('Боевик'),('Детектив'),('Детский'),('Анимация');

        await conn.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS "movie_genre"(
            movie_id INT,
            genre_id INT,
            
            CONSTRAINT fk_movie
                FOREIGN KEY(movie_id)
                    REFERENCES movie(id)
                        ON DELETE CASCADE,  

            CONSTRAINT fk_genre
                FOREIGN KEY(genre_id)
                    REFERENCES genre(id)
                        ON DELETE CASCADE, 

            CONSTRAINT pk_movie_gender
                PRIMARY KEY(movie_id, genre_id));        
         """);


        await conn.ExecuteAsync("""
            CREATE UNIQUE INDEX CONCURRENTLY IF NOT EXISTS email
            ON "user"
            USING BTREE(email);
        """);

        
    }
}
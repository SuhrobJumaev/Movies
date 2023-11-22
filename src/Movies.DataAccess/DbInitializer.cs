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
                DO $$ 
            BEGIN
                IF NOT EXISTS (SELECT FROM pg_database WHERE datname = 'movie') THEN
                    CREATE DATABASE movie
                        WITH
                        OWNER = postgres
                        ENCODING = 'UTF8'
                        LC_COLLATE = 'ru_RU.UTF-8'
                        LC_CTYPE = 'ru_RU.UTF-8'
                        CONNECTION LIMIT = -1;
                END IF;
            END $$;
            
            """);

        await conn.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS role(
                id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                name VARCHAR(20) NOT NULL,
                created_date TIMESTAMP  DEFAULT now()
            );

             DO $$ 
                BEGIN
                    -- Check if the table is empty
                    IF NOT EXISTS (SELECT 1 FROM role LIMIT 1) THEN
                 
                    INSERT INTO public.role(name) VALUES ( 'Admin'), ( 'User');
                    END IF;
             END $$;
         

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
                role_id SMALLINT DEFAULT 0 ,
  
                CONSTRAINT fk_role
                    FOREIGN KEY(role_id)
                        REFERENCES role(id)
                             ON DELETE SET DEFAULT);

                 DO $$ 
                    BEGIN
                     -- Check if the table is empty
                     IF NOT EXISTS (SELECT 1 FROM "user" LIMIT 1) THEN
                 
                         INSERT INTO public."user"(name, last_name, age, gender, phone, email, password, role_id)
                        VALUES ( 'test', 'test', 24, 1, '992900090909', 'test1@gmail.com', '94d8309a5ff754a542089b3faa045d6a173060b83301995fe4ee788218b6666e', 1);
                     END IF;
                 END $$;

                

                
         """);

        await conn.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS "movie" (
            id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
            slug TEXT NOT NULL, 
            title TEXT NOT NULL,
            year_of_release INT NOT NULL,
            video_name varchar(100) NOT NULL);
         """);

        await conn.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS "genre" (
            id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
            name VARCHAR(20) NOT NULL) ;            
         """);
        

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

        await conn.ExecuteAsync("""
            CREATE INDEX IF NOT EXISTS user_name_surname
            ON "user" (name, last_name);
            """);

        await conn.ExecuteAsync("""
            CREATE INDEX IF NOT EXISTS movie_title  
            ON "movie" (title);
            """);

        await conn.ExecuteAsync("""
            CREATE INDEX IF NOT EXISTS movie_year_of_release  
            ON "movie" (year_of_release);
            """);

        await conn.ExecuteAsync("""
            CREATE INDEX IF NOT EXISTS movie_title_year_of_release  
            ON "movie" (title,year_of_release);
            """);
    }
}
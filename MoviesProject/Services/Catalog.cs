using System;
using System.Collections.Generic;
using System.Linq;

namespace MoviesProject
{
    internal class Catalog
    {
        private readonly MoviesFile _moviesFile;
        private List<Movie> _movies;

        public Catalog(string fileName)
        {
            _moviesFile = new MoviesFile(fileName);
            _movies = _moviesFile.ReadFile().ToList();
        }

        public int Count()
        {
            return _movies.Count();
        }

        public IEnumerable<Movie> AllMovies()
        {
            return _movies;
        }


        //Returns all the movies that contain 'term' as part of the title
        public IEnumerable<Movie> SearchByTitle(string term)
        {
            return _movies.Where(m => m.Title.ToUpper().Contains(term.ToUpper()));
        }

        //Finds a movie by the code
        public Movie SearchByCode(string code)
        {
            if (ValidateCode(code))
            {
                return _movies.FirstOrDefault(m => m.Code.ToUpper().Contains(code.ToUpper()));
            }
            return null;            
        }

        //Resturns the number of movies by year
        public IEnumerable<MoviesByYear> MoviesByYear()
        {
            var movies = _movies
                .GroupBy(m => m.Year)
                .Select(m => new MoviesByYear
                {
                        Year = m.Key,
                        Movies = m.Count()
                    }
                )
                .OrderByDescending(m => m.Movies)
                .ToList();

            return movies;
        }

        //Adds a new movie
        //Saves the changes into the file
        public Movie AddMovie(Movie movie)
        {    
            if (ValidateMovie(movie))
            {
                var newMovie = new Movie()
                {
                    Code = CreateCode(movie),
                    Title = movie.Title,
                    Year = movie.Year,
                    Cast = movie.Cast,
                    Genres = movie.Genres
                };
                _movies.Add(newMovie);
                _moviesFile.WriteFile(_movies);
                return newMovie;
            }
            return null;            
        }

        // *** Helpers ***
        // This method returns a random upper letter.
        // Between 'A' and 'Z' inclusize.
        private char GetLetter()
        {
            
            Random _random = new Random();
            int num = _random.Next(0, 26); // Zero to 25
            char letter = (char)('A' + num);
            return letter;
        }


        //Creates a unique code for a new movie
        //Format <year> + 3 letters
        //Validates that the code does not exit already 
        private string CreateCode(Movie movie)
        {
            var code = string.Empty;
            List<Movie> movies = null;
            code = movie.Year.ToString();
            Random _random = new Random();
            while (true)
            {
                for (int i = 1; i <= 3; i++)
                { 
                    var num = _random.Next(0, 26); // Zero to 25
                    var letter = (char)('A' + num);
                    code += letter.ToString().ToUpper();
                }
                movies = _movies.Where(m => m.Code == code).ToList();
                if ( movies != null && movies.Count() > 0)
                {
                    code = movie.Year.ToString();
                }
                else
                {
                    break;
                }
            }

            return code;
        }

        //Validates all required properties and their format
        private bool ValidateMovie(Movie movie)
        {
            if (movie.Year.ToString().Length != 4)
            {
                throw new Exception("Year must be a 4 digit number");
            }
            if (string.IsNullOrEmpty(movie.Title))
            {
                throw new Exception("Title is required");
            }
            else if (movie.Title.Length > 40)
            {
                throw new Exception("Title must have between 1 and 40 characters");
            }
            return true;
        }

        private bool ValidateCode(string code)
        {
            if (code.Length != 7)
            {
                throw new Exception("Code must be 7 characters long");
            }
            if (!int.TryParse(code.Substring(0,4), out int number))
            {
                throw new Exception("First 4 characters must be digits");
            }
            foreach (var item in code.Substring(4).ToUpper())
            {
                if (item < 65 || item > 90)
                {
                    throw new Exception("Last 3 characters must be letters [A..Z]");
                }
            }
            return true;
        }
    }
}

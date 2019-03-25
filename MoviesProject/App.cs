using System;
using System.Collections.Generic;
using System.Linq;

namespace MoviesProject
{
    public class App
    {

        private Catalog _catalog;

        public App(string fileName)
        {
            _catalog = new Catalog(fileName);
        }

        public void Run()
        {
            while (true)
            {
                PrintMainMenu();
                Console.WriteLine();
                var option = Console.ReadKey();

                if (option.Key.ToString() != "Escape")
                {
                    ExecuteMainMenuOption(option);
                }
                else
                {
                    break;
                }
            }
        }

        private void PrintMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to movies catalog");
            Console.WriteLine();
            Console.WriteLine("Choose an option:");
            Console.WriteLine();
            Console.WriteLine("[1] : List all movies");
            Console.WriteLine("[2] : Search by title");
            Console.WriteLine("[3] : Search by code");
            Console.WriteLine("[4] : Add movie");
            Console.WriteLine("[5] : Number of movies by year");
            Console.WriteLine("<Esc> to exit.");
        }

        private void ExecuteMainMenuOption(ConsoleKeyInfo option)
        {
            switch (option.KeyChar)
            {
                case '1':
                    PrintMovies(_catalog.AllMovies().ToList(), "OPTION <1> LIST ALL MOVIES");
                    break;
                case '2':
                    SearchByTitle();
                    break;
                case '3':
                    SearchByCode();
                    break;
                case '4':
                    AddMovie();
                    break;
                case '5':
                    MoviesByYearReport();
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                    break;
            }
        }

        private void PrintMovies(List<Movie> movies, string menuItem)
        {
            int current = 1;
            int size = 10;
            int pages = movies.Count() / size;
            if (movies.Count() % size != 0)
            {
                pages += 1;
            }
            int sort = 0;
            string sortingValue = "No sorting";

            char[] sortOptions = { '1', '2', '3', '4' };

            Console.Clear();
            Console.WriteLine("Choose sorting order.");
            Console.WriteLine();
            Console.WriteLine("[1] By <YEAR> ASC");
            Console.WriteLine("[2] By <YEAR> DESC");
            Console.WriteLine("[3] By <TITLE> ASC");
            Console.WriteLine("[4] By <TITLE> DESC");
            Console.WriteLine("[Any other key] NO SORT");

            var sortOption = Console.ReadKey();

            if (sortOptions.Contains(sortOption.KeyChar))
            {
                sort = int.Parse(sortOption.KeyChar.ToString());
            }

            while (true)
            {
                var skip = current * size - size;
                var items = movies.Skip(skip).Take(size).ToList();

                switch (sort)
                {
                    case 1:
                        items = movies.OrderBy(m => m.Year)
                            .ThenBy(m => m.Title)
                            .Skip(skip)
                            .Take(size)
                            .ToList();
                        sortingValue = "Year ASC";
                        break;
                    case 2:
                        items = movies
                            .OrderByDescending(m => m.Year)
                            .ThenBy(m => m.Title)
                            .Skip(skip)
                            .Take(size)
                            .ToList();
                        sortingValue = "Year DESC";
                        break;
                    case 3:
                        items = movies
                            .OrderBy(m => m.Title)
                            .Skip(skip)
                            .Take(size)
                            .ToList();
                        sortingValue = "Title Asc";
                        break;
                    case 4:
                        items = movies
                            .OrderByDescending(m => m.Title)
                            .Skip(skip)
                            .Take(size)
                            .ToList();
                        sortingValue = "Title Desc";
                        break;
                    default:
                        break;
                }

                Console.Clear();
                Console.WriteLine(menuItem);
                Console.WriteLine();
                Console.WriteLine("TOTAL MOVIES: \t" + movies.Count());
                Console.WriteLine("TOTAL PAGES: \t" + pages);
                Console.WriteLine("CURRENT PAGE: \t" + current);
                Console.WriteLine("SORT: \t\t" + sortingValue);
                Console.WriteLine();
                Console.WriteLine(string.Format("{0,-5} | {1,-7} | {2,-4} | {3,-60} | {4,-20} | {5,-30}", "", "CODE", "YEAR", "TITLE", "GENRES", "CAST"));
                Console.WriteLine("----- | ------- | ---- | ------------------------------------------------------------ | -------------------- | ----------------------------------------");

                for (int i = 0; i < items.Count(); i++)
                {
                    var genres = items[i].Genres;
                    var cast = items[i].Cast;
                    var genresCount = genres != null ? genres.Count() : 0;
                    var castCount = cast != null ? cast.Count() : 0;
                    int genresCastNumber = genresCount >= castCount ? genresCount : castCount;
                    var firstGenre = genres.FirstOrDefault();
                    var firstCast = cast.FirstOrDefault();
                    Console.WriteLine(string.Format("{0,-5} | {1,-7} | {2,-4} | {3,-60} | {4,-20} | {5,-30}", i + 1 + (current - 1) * size, items[i].Code, items[i].Year, items[i].Title, firstGenre, firstCast));
                    if (genresCastNumber > 1)
                    {
                        for (int j = 1; j < genresCastNumber; j++)
                        {
                            var genreItem = genresCount >= j + 1 ? genres[j] : "";
                            var castItem = castCount >= j + 1 ? cast[j] : "";
                            Console.WriteLine(string.Format("{0,-5} | {1,-7} | {2,-4} | {3,-60} | {4,-20} | {5,-30}", "", "", "", "", genreItem, castItem));
                        }
                    }
                    if (i < items.Count() - 1)
                    {
                        Console.WriteLine("      |         |      |                                                              |                      |                                         ");
                        Console.WriteLine("----- | ------- | ---- | ------------------------------------------------------------ | -------------------- | ----------------------------------------");
                    }
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("PAGE: " + current);
                Console.WriteLine();
                if (current < pages)
                {
                    Console.WriteLine("<PgUp> NEXT PAGE.");
                }
                if (current > 1)
                {
                    Console.WriteLine("<PgDn> PREVIOUS PAGE.");
                }
                Console.WriteLine("<Esc> RETURN TO MAIN MENU.");
                Console.WriteLine();

                var option = Console.ReadKey();

                if (option.Key.ToString() == "PageUp" && current < pages)
                {
                    current += 1;
                }
                else if (option.Key.ToString() == "PageDown" && current > 1)
                {
                    current -= 1;

                }
                else if (option.Key.ToString() == "Escape")
                {
                    break;
                }
            }

        }

        private void SearchByTitle()
        {
            Console.Clear();
            Console.WriteLine("OPTION <2> SEARCH BY TITLE");
            Console.WriteLine();
            Console.WriteLine("Enter search term");
            string term = Console.ReadLine();
            var result = _catalog.SearchByTitle(term).ToList();
            PrintMovies(result, "OPTION <2> SEARCH BY TITLE");
        }

        private void SearchByCode()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("OPTION <3> SEARCH BY TITLE");
                Console.WriteLine();
                Console.WriteLine("Enter Code");
                string code = Console.ReadLine();
                try
                {
                    var result = _catalog.SearchByCode(code);
                    if (result != null)
                    {
                        Console.WriteLine();
                        Console.WriteLine(result.ToString());
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\nMovie not found.");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\n\nPress any key to continue... ");
                    Console.ReadKey();
                    break;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nError message");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(ex.Message);
                    Console.Write("\n\nTry again? Y/N:  ");
                    var option = Console.ReadKey();
                    if (option.Key.ToString().ToUpper() != "Y")
                    {
                        break;
                    }

                }
            }
            
        }

        private void AddMovie()
        {
            while (true)
            {
                var movie = new Movie
                {
                    Genres = new List<string>(),
                    Cast = new List<string>()
                };

                Console.Clear();
                Console.WriteLine("OPTION <4> ADD MOVIE");
                Console.WriteLine();

                Console.WriteLine("Enter title <1 to 40 characters>");
                movie.Title = Console.ReadLine();

                while (true)
                {
                    Console.WriteLine("Enter Year <4 digits>");
                    string year = Console.ReadLine();
                    if (int.TryParse(year.Trim(), out int number))
                    {
                        movie.Year = number;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid number");
                    }
                }

                string genre = string.Empty;
                while (true)
                {
                    Console.WriteLine("Enter genre");
                    genre = Console.ReadLine();
                    if (!string.IsNullOrEmpty(genre.Trim()))
                    {
                        movie.Genres.Add(genre);
                    }
                    Console.Write("Add more genres? Y/N:  ");
                    var option = Console.ReadKey();
                    Console.WriteLine();
                    if (option.Key.ToString().ToUpper() != "Y")
                    {
                        break;
                    }
                }

                string character = string.Empty;                
                while (true)
                {
                    Console.WriteLine("Enter Cast");
                    character = Console.ReadLine();
                    if (!string.IsNullOrEmpty(character.Trim()))
                    {
                        movie.Cast.Add(character);
                    }
                    Console.Write("Add more characters? Y/N:  ");
                    var option = Console.ReadKey();
                    Console.WriteLine();
                    if (option.Key.ToString().ToUpper() != "Y")
                    {
                        break;
                    }
                }
                try
                {
                    var newMovie = _catalog.AddMovie(movie);
                    if (newMovie != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n\nMovie added successfully");                        
                        Console.WriteLine();
                        Console.WriteLine(newMovie.ToString());
                        
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\nSomethin were wrong.");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\n\nPress any key to continue... ");
                    Console.ReadKey();
                    break;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nError message");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(ex.Message);
                    Console.Write("\n\nTry again? Y/N:  ");
                    var option = Console.ReadKey();
                    if (option.Key.ToString().ToUpper() != "Y")
                    {
                        break;
                    }
                }
            }
        }

        private void MoviesByYearReport()
        {

            string sortingValue = "<AMOUNT> DESC";
            char[] sortOptions = { '1', '2', '3', };
            var sort = 4;

            Console.Clear();
            Console.WriteLine("OPTION <5> NUMBER OF MOVIES BY YEAR");
            Console.WriteLine();

            Console.WriteLine("Choose sorting order.");
            Console.WriteLine();
            Console.WriteLine("[1] By <YEAR> ASC");
            Console.WriteLine("[2] By <YEAR> DESC");
            Console.WriteLine("[3] By <AMOUNT> ASC");
            Console.WriteLine("[Any other key] DEFAULT ORDER (<AMOUNT> DESC)");

            var sortOption = Console.ReadKey();
            if (sortOptions.Contains(sortOption.KeyChar))
            {
                sort = int.Parse(sortOption.KeyChar.ToString());
            }

            var items = _catalog.MoviesByYear();

            switch (sort)
            {
                case 1:
                    items = items.OrderBy(m => m.Year);
                    sortingValue = "<YEAR> DESC";
                    break;
                case 2:
                    items = items.OrderByDescending(m => m.Year);
                    sortingValue = "<YEAR> DESC";
                    break;
                case 3:
                    items = items.OrderBy(m => m.Movies);
                    sortingValue = "<AMOUNT> ASC";
                    break;
                default:
                    break;
            }

            Console.Clear();
            Console.WriteLine("OPTION <5> NUMBER OF MOVIES BY YEAR");

            Console.WriteLine("\nSorting: " + sortingValue);

            Console.WriteLine("\nYEAR\tAMOUNT");
            Console.WriteLine("---\t-----------");

            foreach (var item in items)
            {
                Console.WriteLine(item.Year + "\t" + item.Movies);
            }

            Console.WriteLine("\n\nTOTAL:\t" + items.Sum(m => m.Movies));

            Console.WriteLine("\n\nPress any key to go back.");
            Console.ReadKey();
        }
    }
}

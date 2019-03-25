using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace MoviesProject
{
    class MoviesFile
    {
        private readonly string _path;

        public MoviesFile(string fileName)
        {
            _path = GetFilePath(fileName);
        }

        //Reads records on the Json file into the movies array
        public IEnumerable<Movie> ReadFile()
        {
            var movies = new List<Movie>();
            var serializer = new JsonSerializer();
            using (var reader = new StreamReader(_path))
            using (var jsonReader = new JsonTextReader(reader))
            {
                movies = serializer.Deserialize<List<Movie>>(jsonReader);
            }
            return movies;
        }

        //Writes the movies array into the Json file
        public void WriteFile(IEnumerable<Movie> movies)
        {
            var serializer = new JsonSerializer();
            using (var writer = new StreamWriter(_path))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, movies);
            }
        }

        //Get the full path of the Json file
        private string GetFilePath(string fileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            return Path.Combine(directory.FullName, fileName);
        }
    }
}

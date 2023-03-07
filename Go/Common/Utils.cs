using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Go.Models;

namespace Go.Common
{
    public static class Utils
    {
        /// <summary>
        /// Method at convert Image to binary
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static byte[] ImageToBinary(string imagePath)
        {
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }

        public static List<TimeRange> BuildRanges()
        {
            return new List<TimeRange>()
            {
                new TimeRange()
                {
                    Id = 1,
                    Name = "Todos"
                },
                new TimeRange()
                {
                    Id = 2,
                    Name = "Hoy"
                },
                new TimeRange()
                {
                    Id = 3,
                    Name = "Mañana"
                },
                new TimeRange()
                {
                    Id = 4,
                    Name = "Esta semana"
                },
                new TimeRange()
                {
                    Id = 5,
                    Name = "Este fin de semana"
                },
                new TimeRange()
                {
                    Id = 6,
                    Name = "Próxima semana"
                },
                new TimeRange()
                {
                    Id = 7,
                    Name = "Este mes"
                }
            };
        }

        public static List<Category> BuildCategories()
        {
            return new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Fiesta",
                    Icon = "party.svg"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Tecnología y ciencia",
                    Icon = "computer.svg"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Arte y cultura",
                    Icon = "theatre.svg"
                },
                new Category()
                {
                    Id = 4,
                    Name = "Entrenimiento",
                    Icon = "cinema.svg"
                },
                new Category()
                {
                    Id = 5,
                    Name = "Deportes",
                    Icon = "futbol.svg"
                },
                new Category()
                {
                    Id = 6,
                    Name = "Restaurantes y bares",
                    Icon = "bar.svg"
                },
                new Category()
                {
                    Id = 7,
                    Name = "Conferencia",
                    Icon = "education.svg"
                },
                new Category()
                {
                    Id = 8,
                    Name = "Música",
                    Icon = "music.svg"
                },
                new Category()
                {
                    Id = 9,
                    Name = "Otros",
                    Icon = "other.svg"
                }
            };
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
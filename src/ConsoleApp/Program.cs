using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using Bogus;

namespace ConsoleApp2
{
    internal class Program
    {
        private static readonly List<Person> listOfPerson = new List<Person>();
        private static readonly List<dynamic> listOfDynamicPerson = new List<dynamic>();
        private static readonly List<dynamic> listOfDynamicExpandoObjects = new List<dynamic>();

        private static readonly int readListCount = 1000;
        private static int readCount = 50_000_000;

        private static readonly int writeListCount = 5_000_000;

        private static readonly List<object> dump = new List<object>();

        private static void Main(string[] args)
        {
            for (int i = 0; i < 4; i++)
            {
                GenerateExpandoObjects(writeListCount);
                GenerateStronglyTypedPeople(writeListCount);
            }

            GeneratePeople(readListCount);
            for (int i = 0; i < 4; i++)
            {
                AccessRandomPeople1(readCount, listOfPerson, "strongly typed objects in a strongly typed list");
                AccessRandomPeople2(readCount, listOfDynamicPerson, "strongly typed objects in a list of dynamic");
                AccessRandomPeople2(readCount, listOfDynamicExpandoObjects, "expando objects in a list of dynamic");
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private static void GenerateStronglyTypedPeople(int count)
        {
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < count; i++)
            {
                var person = new Person
                {
                    Address = "address",
                    DateOfBirth = DateTime.Now,
                    Id = 123,
                    Name = "name"
                };
                dump.Add(person);
            }
            dump.Clear();
            Console.WriteLine("Took {0} ms to generate {1} strongly typed objects", sw.ElapsedMilliseconds, count);
        }

        private static void GenerateExpandoObjects(int count)
        {
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < count; i++)
            {
                dynamic weakPerson = new ExpandoObject();
                weakPerson.Address = "address";
                weakPerson.DateOfBirth = DateTime.Now;
                weakPerson.Id = 123;
                weakPerson.Name = "name";
                dump.Add(weakPerson);
            }
            dump.Clear();
            Console.WriteLine("Took {0} ms to generate {1} expando objects", sw.ElapsedMilliseconds, count);
        }

        private static void AccessRandomPeople1(int count, List<Person> list, string listDetails)
        {
            var sw = new Stopwatch();
            sw.Start();
            var rand = new Random();
            for (var i = 0; i < count; i++)
            {
                var person = list[rand.Next(readListCount)];
                var a = person.Name.Length;
                var b1 = person.DateOfBirth.Day;
                var b2 = person.DateOfBirth.Hour;
                var b3 = person.DateOfBirth.Millisecond;
                var c = person.Id + 123;
                var d = string.IsNullOrWhiteSpace(person.Address);
            }

            Console.WriteLine("Took {0} ms to access {1} {2}", sw.ElapsedMilliseconds, count, listDetails);
        }

        private static void AccessRandomPeople2(int count, List<dynamic> list, string listDetails)
        {
            var sw = new Stopwatch();
            sw.Start();
            var rand = new Random();
            for (var i = 0; i < count; i++)
            {
                var person = list[rand.Next(readListCount)];
                var a = person.Name.Length;
                var b1 = person.DateOfBirth.Day;
                var b2 = person.DateOfBirth.Hour;
                var b3 = person.DateOfBirth.Millisecond;
                var c = person.Id + 123;
                var d = string.IsNullOrWhiteSpace(person.Address);
            }

            Console.WriteLine("Took {0} ms to access {1} {2}", sw.ElapsedMilliseconds, count, listDetails);
        }

        private static void GeneratePeople(int count)
        {
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < count; i++)
            {
                var bogus = new Faker();

                var person = new Person
                {
                    Address = bogus.Address.StreetAddress(),
                    DateOfBirth = bogus.Date.Past(),
                    Id = bogus.Random.Int(1),
                    Name = bogus.Person.FirstName
                };
                listOfPerson.Add(person);

                listOfDynamicPerson.Add(person);

                dynamic weakPerson = new ExpandoObject();
                weakPerson.Address = person.Address;
                weakPerson.DateOfBirth = person.DateOfBirth;
                weakPerson.Id = person.Id;
                weakPerson.Name = person.Name;
                listOfDynamicExpandoObjects.Add(weakPerson);
            }

            Console.WriteLine("Took {0} ms to generate {1} objects", sw.ElapsedMilliseconds, count);
        }
    }

    
    internal class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
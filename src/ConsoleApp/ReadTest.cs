using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using Bogus;

namespace ConsoleApp
{
    public class ReadTest
    {
        private readonly List<dynamic> _listOfDynamicExpandoObjects = new List<dynamic>();
        private readonly List<dynamic> _listOfDynamicPerson = new List<dynamic>();
        private readonly List<Person> _listOfPerson = new List<Person>();

        public void RunReadTest(int readListCount, int readCount, int testIterations)
        {
            GeneratePeople(readListCount);
            for (var i = 0; i < testIterations; i++)
            {
                AccessRandomPeople1(readListCount, readCount, _listOfPerson,
                    "strongly typed objects in a strongly typed list");
                AccessRandomPeople2(readListCount, readCount, _listOfDynamicPerson,
                    "strongly typed objects in a list of dynamic");
                AccessRandomPeople2(readListCount, readCount, _listOfDynamicExpandoObjects,
                    "expando objects in a list of dynamic");
                Console.WriteLine();
            }
        }

        private void AccessRandomPeople1(int readListCount, int readCount, List<Person> list, string listDetails)
        {
            var sw = new Stopwatch();
            sw.Start();
            var rand = new Random();
            for (var i = 0; i < readCount; i++)
            {
                var person = list[rand.Next(readListCount)];
                var a = person.Name.Length;
                var b1 = person.DateOfBirth.Day;
                var b2 = person.DateOfBirth.Hour;
                var b3 = person.DateOfBirth.Millisecond;
                var c = person.Id + 123;
                var d = string.IsNullOrWhiteSpace(person.Address);
            }

            Console.WriteLine("Took {0} ms to access {1} {2}", sw.ElapsedMilliseconds, readCount, listDetails);
        }

        private void AccessRandomPeople2(int readListCount, int readCount, List<dynamic> list, string listDetails)
        {
            var sw = new Stopwatch();
            sw.Start();
            var rand = new Random();
            for (var i = 0; i < readCount; i++)
            {
                var person = list[rand.Next(readListCount)];
                var a = person.Name.Length;
                var b1 = person.DateOfBirth.Day;
                var b2 = person.DateOfBirth.Hour;
                var b3 = person.DateOfBirth.Millisecond;
                var c = person.Id + 123;
                var d = string.IsNullOrWhiteSpace(person.Address);
            }

            Console.WriteLine("Took {0} ms to access {1} {2}", sw.ElapsedMilliseconds, readCount, listDetails);
        }

        private void GeneratePeople(int count)
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
                _listOfPerson.Add(person);

                _listOfDynamicPerson.Add(person);

                dynamic weakPerson = new ExpandoObject();
                weakPerson.Address = person.Address;
                weakPerson.DateOfBirth = person.DateOfBirth;
                weakPerson.Id = person.Id;
                weakPerson.Name = person.Name;
                _listOfDynamicExpandoObjects.Add(weakPerson);
            }
        }
    }
}
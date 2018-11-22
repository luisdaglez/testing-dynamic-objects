using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;

namespace ConsoleApp
{
    public class WriteTest
    {
        private readonly List<object> dump = new List<object>();

        public void RunWriteTest(int writeListCount, int testIterations)
        {
            for (var i = 0; i < testIterations; i++)
            {
                GenerateExpandoObjects(writeListCount);
                GenerateStronglyTypedPeople(writeListCount);
            }
        }

        private void GenerateStronglyTypedPeople(int count)
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

        private void GenerateExpandoObjects(int count)
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
    }
}
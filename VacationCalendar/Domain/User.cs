using System;

namespace VacationCalendar.Domain
{
    public class User
    {
        public int Id { get; }
        public string Name { get; }

        public User(int id, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Name can not be empty");

            Id = id;
            Name = name;
        }
    }
}
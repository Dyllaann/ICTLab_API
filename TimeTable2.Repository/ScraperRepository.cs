using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using TimeTable2.Engine;
using TimeTable2.Repository.Interfaces;

namespace TimeTable2.Repository
{
    public class ScraperRepository : DbRepository, IScraperRepository
    {
        public ScraperRepository(DbContext context) : base(context) { }


        public void AddOrUpdateClassrooms(List<Classroom> classrooms)
        {
            Context.Set<Classroom>().AddOrUpdate(classrooms.ToArray());
            Context.SaveChanges();
        }

        public Class GetClassByName(string className)
        {
            var schoolClass = Context.Set<Class>().FirstOrDefault(c => c.Name == className);
            if (schoolClass != null) return schoolClass;

            //else
            var newClass = new Class
            {
                Id = Guid.NewGuid(),
                Name = className
            };
            Context.Set<Class>().Add(newClass);
            return newClass;

        }

        public Course GetExistingCourseByRoomAndClassCode(string courseCode, string description, int week, int weekday, int startBlock, int endBlock)
        {
            var lesson = Context.Set<Course>().FirstOrDefault(c => c.Week == week
                                                                   && c.CourseCode == courseCode
                                                                   && c.Description == description
                                                                   && c.WeekDay == weekday
                                                                   && c.StartBlock == startBlock
                                                                   && c.EndBlock == endBlock);
            return lesson;
        }

        public Course AddOrUpdateCourse(Course course)
        {
            Context.Set<Course>().AddOrUpdate(course);
            Context.SaveChanges();
            return course;
        }
    }
}

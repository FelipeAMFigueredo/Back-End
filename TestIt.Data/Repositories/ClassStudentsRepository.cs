﻿using TestIt.Data.Abstract;
using TestIt.Model.Entities;

namespace TestIt.Data.Repositories
{
    public class ClassStudentsRepository : EntityBaseRepository<ClassStudents>, IClassStudentsRepository
    {
        public ClassStudentsRepository(TestItContext context)
            : base(context)
        { }
    }
}

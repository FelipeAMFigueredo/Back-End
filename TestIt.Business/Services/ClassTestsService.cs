﻿using TestIt.Data.Abstract;

namespace TestIt.Business.Services
{
    public class ClassTestsService : IClassTestsService
    {
        private IClassTestsRepository classTestsRepository;

        public ClassTestsService(IClassTestsRepository classTestsRepository)
        {
            this.classTestsRepository = classTestsRepository;
        }
    }
}
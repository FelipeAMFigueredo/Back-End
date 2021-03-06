﻿using System;
using System.Collections.Generic;

namespace TestIt.Model.Entities
{
    public class Question : IEntityBase
    {
        public Question()
        {
            DateCreated = DateTime.Now;
            DateUpdated = DateTime.Now;

            AnsweredQuestions = new List<AnsweredQuestion>();
        }
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public string Description { get; set; }
        public double Value { get; set; }
        public int Order { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }

        public virtual EssayQuestion EssayQuestion { get; set; }
        public virtual AlternativeQuestion AlternativeQuestion { get; set; }
        public virtual IEnumerable<AnsweredQuestion> AnsweredQuestions { get; set; }
    }
}

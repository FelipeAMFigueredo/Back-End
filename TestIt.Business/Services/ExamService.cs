﻿using System;
using System.Collections.Generic;
using TestIt.Data.Abstract;
using TestIt.Model.DTO;
using TestIt.Model.Entities;
using TestIt.Model;

namespace TestIt.Business.Services
{
    public class ExamService : IExamService
    {
        private IExamRepository _examRepository;
        private IAnsweredQuestionRepository _answeredQuestionRepository;


        public ExamService(IExamRepository examRepository, IAnsweredQuestionRepository answeredQuestionRepository)
        {
            _examRepository = examRepository;
            _answeredQuestionRepository = answeredQuestionRepository;
        }

        public void Save(Exam e)
        {
            _examRepository.Add(e);
            _examRepository.Commit();
        }
        public IEnumerable<ExamDTO> GetStudentExams(int id)
        {
            var exams = _examRepository.GetExams(id);
            return exams;
        }

        public bool EndExam(int id, List<AnsweredQuestion> answerdQuestions)
        {
            var e = _examRepository.GetSingle(id);

            if (e != null)
            {
                e.DateUpdated = DateTime.Now;
                e.Status = (int)EnumStatus.Finished;
                e.EndDate = DateTime.Now;

                _answeredQuestionRepository.AddOrUpdateMultiple(answerdQuestions);

                _examRepository.Commit();

                return true;
            }
            else
                return false;
        }

        public bool SaveExam(int id, List<AnsweredQuestion> answerdQuestions)
        {
            var e = _examRepository.GetSingle(id);

            if (e != null)
            {
                e.DateUpdated = DateTime.Now;

                _answeredQuestionRepository.AddOrUpdateMultiple(answerdQuestions);

                _examRepository.Commit();

                return true;
            }
            else
                return false;
        }

        public ExamInformationsDTO Get (int id)
        {
            var exam = _examRepository.GetFull(id);
            
            return exam;
        }
        
    }
}

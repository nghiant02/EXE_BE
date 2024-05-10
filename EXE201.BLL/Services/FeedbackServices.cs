using AutoMapper;
using EXE201.BLL.Interfaces;
using EXE201.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Services
{
    public class FeedbackServices : IFeedbackServices
    {
        private readonly IFeedbacksRepository _feedbackRepository;
        private readonly IMapper _mapper;

        public FeedbackServices(IFeedbacksRepository feedbackRepository, IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
        }
    }
}

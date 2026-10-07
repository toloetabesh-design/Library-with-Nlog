using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Application.DTOs;
using Library.Domain.Entities;

namespace Library.Application.Profiles
{
    public class MemberProfile : AutoMapper.Profile
    {
        public MemberProfile()
        {
            CreateMap<Member, MemberDto>();
            CreateMap<MemberDto, Member>();
        }
    }
}
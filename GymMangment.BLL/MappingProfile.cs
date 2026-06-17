using AutoMapper;
using GymMangment.BLL.ViewModels.MemberViewModels;
using GymMangment.BLL.ViewModels.PlanViewModel;
using GymMangment.BLL.ViewModels.SessionViewModel;
using GymMangment.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapMember();
            MapSession();
            MapPlan();

        }
        private void MapMember()
        {
            CreateMap<Member, MemberViewModel>().
              ForMember(des => des.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}")).
              ForMember(des => des.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()));
            CreateMap<HealthRecord, HealthRecordViewModel>();
            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(des => des.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(des => des.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(des => des.City, opt => opt.MapFrom(src => src.Address.City));
            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(des => des.Name, opt => opt.Ignore())
                .ForMember(des => des.Phone, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                    dest.Address.BuildingNumber = src.BuildingNumber;
                });
            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(des => des.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City,
                }))
                .ForMember(des => des.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));
        }

        private void MapSession()
        {
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, SessionViewModel>();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Catagory,CatagorySelectViewModel>();
        }

        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, EditPlanViewModel>();
            CreateMap<EditPlanViewModel, Plan>();
        }
    }
}

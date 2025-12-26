using AutoMapper;
using ExpenseTracker.Api.DTOs;
using ExpenseTracker.Api.Models;

namespace ExpenseTracker.Api.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, IdNameDto>().ForMember(d => d.Name, o => o.MapFrom(s => s.Title));
            CreateMap<Category, IdNameDto>();
            CreateMap<PaymentType, IdNameDto>();
            CreateMap<TransactionType, IdNameDto>();


            CreateMap<Book, BookDto>();
            CreateMap<Book, BookCreateResponseDto>();
            CreateMap<CreateBookDto, Book>();
            CreateMap<BookUpdateDto, Book>();

            CreateMap<Expense, ExpenseItemDto>()
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category))
                .ForMember(d => d.PaymentType, o => o.MapFrom(s => s.PaymentType))
                .ForMember(d => d.TransactionType, o => o.MapFrom(s => s.TransactionType));

            CreateMap<Expense, ExpenseCreateResponseDto>().ForMember(d => d.Category, o => o.MapFrom(s => s.Category))
                .ForMember(d => d.PaymentType, o => o.MapFrom(s => s.PaymentType))
                .ForMember(d => d.TransactionType, o => o.MapFrom(s => s.TransactionType))
                .ForMember(d => d.Book, o => o.MapFrom(s => s.Book));

            CreateMap<CreateExpenseDto, Expense>();
            CreateMap<ExpenseUpdateDto, Expense>();

            CreateMap<Category, CategoryDto>();
            CreateMap<Category, ExpenseItemCategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
        }
    }
}
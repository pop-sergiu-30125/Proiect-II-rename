using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProiectII.DTO.Reviews;
using ProiectII.Interfaces;
using ProiectII.Models;

namespace ProiectII.Services.CoreDomain;

public class AppReviewService : IAppReviewService
{
    private readonly IAppReviewRepository _repo;
    private readonly IMapper _mapper;

    public AppReviewService(IAppReviewRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    // =========================
    // GET ALL
    // =========================
    public async Task<List<AppReviewListItemDto>> GetAllAsync()
    {
        var reviews = await _repo.GetVisibleAsync();
        return _mapper.Map<List<AppReviewListItemDto>>(reviews);
    }

    
    public async Task CreateAsync(CreateAppReviewDto dto, string? userId)
    {
        var review = _mapper.Map<AppReview>(dto);
        review.UserId = userId;

        await _repo.AddAsync(review);
        await _repo.SaveAsync();
    }

   
    public async Task<bool> HideReviewAsync(uint id, string reason)
    {
        var review = await _repo.GetByIdAsync(id);

        if (review == null)
            return false;

        review.IsHidden = true;
        review.HiddenReason = reason;

        await _repo.SaveAsync();

        return true;
    }

    
    public async Task<AppReviewPageDto > GetStatsAsync()
    {
        var query = _repo.GetVisibleQuery();

        var total = await query.CountAsync();
        var avg = total == 0 ? 0 : await query.AverageAsync(r => r.Rating);

        return new AppReviewPageDto 
        {
            AverageRating = avg,
            TotalReviews = total
        };
    }
}